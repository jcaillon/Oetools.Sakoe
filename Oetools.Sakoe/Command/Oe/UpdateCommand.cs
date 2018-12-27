using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using GithubUpdater;
using GithubUpdater.GitHub;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
       
    [Command(
        "update", "up",
        Description = "Update this tool with the latest release found on github.",
        ExtendedHelpText = ""
    )]
    internal class UpdateCommand : ABaseCommand {
        
        private const string RepoOwner = "jcaillon";
        private const string RepoName = "Oetools.Sakoe";
        private const string GitHubToken = "MmViMDJlNWVlYWZlMTIzNGIxN2VmOTkxMGQ1NzljMTRkM2E1ZDEyMw==";
        
        [Option("-b|--get-beta", "Accept to update from new 'beta' (i.e. pre-release) versions of the tool.\nThis option will be used by default if the current version of the tool is a beta version. Otherwise, only stable releases will be used for updates. ", CommandOptionType.NoValue)]
        public bool GetBeta { get; set; }
        
        [Option("-p|--proxy", "The http proxy to use for this update. Useful if you are behind a corporate firewall.\nThe expected format is: 'http(s)://[user:password@]host[:port]'.\nIt is also possible to use the environment variable HTTP_PROXY to set this value.", CommandOptionType.SingleValue)]
        public string HttpProxy { get; set; }
        
        [Option("-c|--check-only", "Check for new releases but exit the command before actually updating the tool.", CommandOptionType.NoValue)]
        public bool CheckOnly { get; set; }
        
        [Option("-u|--override-github-url", "Use an alternative url for the github api. This option is here to allow updates from a different location (a private server for instance) but should not be used in most cases.", CommandOptionType.SingleValue)]
        public string OverrideGithubUrl { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var usePreRelease = GetBeta || RunningAssembly.IsPreRelease;
            Log.Debug(usePreRelease ? "Getting pre-releases." : "Skipping pre-releases.");
            
            var httpProxyFromEnv = Environment.GetEnvironmentVariable("HTTP_PROXY");

            if (string.IsNullOrEmpty(HttpProxy) && !string.IsNullOrEmpty(httpProxyFromEnv)) {
                Log.Debug($"Using the proxy found in the HTTP_PROXY environment variable: {httpProxyFromEnv.PrettyQuote()}.");
                HttpProxy = httpProxyFromEnv;
            }
            
            var updater = new GitHubUpdater();

            if (!string.IsNullOrEmpty(OverrideGithubUrl)) {
                Log.Debug($"Using alternative base url for github api: {OverrideGithubUrl.PrettyQuote()}.");
                updater.UseAlternativeBaseUrl(OverrideGithubUrl);
            }

            updater.UseAuthorizationToken(Encoding.ASCII.GetString(Convert.FromBase64String(GitHubToken)));
            updater.SetRepo(RepoOwner, RepoName);
            updater.UseCancellationToken(CancelToken);
            updater.UseMaxNumberOfReleasesToFetch(10);

            if (!string.IsNullOrEmpty(HttpProxy) && HttpProxy.ParseHttpAddress(out var userName, out var password, out var host, out var port)) {
                Log.Debug($"Using http proxy: {HttpProxy.PrettyQuote()}.");
                updater.UseProxy($"{host}{(port > 0 ? $":{port}" : "")}", userName, password);
            }
            
            var currentVersion = UpdaterHelper.StringToVersion(RunningAssembly.FileVersion);
            Log.Info($"Current local version: {currentVersion}.");

            var releases = updater.FetchNewReleases(currentVersion);

            GitHubRelease first;
            while (!usePreRelease && (first = releases.FirstOrDefault()) != null && (first.Draft || first.Prerelease)) {
                Log.Debug($"Removing pre-release: {first.TagName}.");
                releases.RemoveAt(0);
            }
            
            if (releases.Count == 0) {
                Log.Done("Your version is up to date.");
                return 0;
            }

            var latestRelease = releases[0];
            
            Log.Info($"{releases.Count} new releases found on github:");
            foreach (var release in releases) {
                Log.Info($"- {release.TagName}, {release.Name.PrettyQuote()}: {release.HtmlUrl}");
            }

            if (CheckOnly) {
                Log.Warn("Option 'check only' activated, no updates done.");
                return 1;
            }

            var latestAsset = latestRelease.Assets.FirstOrDefault(asset => {
                if (Utils.IsNetFrameworkBuild) {
                    return !asset.Name.Contains("core");
                }
                return asset.Name.Contains("core");
            });

            if (latestAsset == null) {
                throw new CommandException("Could not find a matching asset in the latest github release.");
            }
            
            Log.Debug($"Downloading the latest release asset: {latestAsset.BrowserDownloadUrl}.");
            var tempFilePath = updater.DownloadToTempFile(latestAsset.BrowserDownloadUrl, progress => {
                Log.ReportProgress(100, (int) Math.Round((decimal) progress.NumberOfBytesDoneTotal / progress.NumberOfBytesTotal * 100), "Downloading update.");
            });

            var tempExtractionDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempExtractionDir);
            using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Read)) {
                zip.ExtractToDirectory(tempExtractionDir);
            }
            
            var fileUpdater = SimpleFileUpdater.Instance;
            var toolDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(toolDirectory)) {
                throw new CommandException("Could not find the directory in which this tool is installed.");
            }
            foreach (var file in Utils.EnumerateAllFiles(tempExtractionDir)) {
                fileUpdater.AddFileToMove(file, Path.Combine(toolDirectory, file.Replace(tempExtractionDir, "").TrimStartDirectorySeparator()));
            }
            
            File.Delete(tempFilePath);

            if (fileUpdater.IsAdminRightsNeeded) {
                Log.Warn("The update will require you to accept the execution permission for the updater.exe.");
            }

            fileUpdater.Start();
            Log.Done("Update successful.");
            
            return 0;
        }
    }

}