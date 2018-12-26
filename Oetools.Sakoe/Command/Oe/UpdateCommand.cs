using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using GithubUpdater;
using GithubUpdater.GitHub;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
       
    [Command(
        "update", "up",
        Description = "Update this tool with the latest release found on github.",
        ExtendedHelpText = ""
    )]
    internal class UpdateCommand : ABaseCommand {

        private const string RepoOwner = "3pUser";
        private const string RepoName = "yolo";
        
        [Option("-p|--proxy", "The http proxy to use for this update. Useful if you are behind a corporate firewall.\nThe expected format is: 'http(s)://[user:password@]host[:port]'.\nIt is also possible to use the environment variable HTTP_PROXY to set this value.", CommandOptionType.SingleValue)]
        public string HttpProxy { get; set; }
        
        [Option("-u|--override-github-url", "Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists.", CommandOptionType.SingleValue)]
        public string OverrideGithubUrl { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

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
            
            updater.SetRepo(RepoOwner, RepoName);
            updater.UseCancellationToken(CancelToken);
            updater.UseMaxNumberOfReleasesToFetch(10);

            if (!string.IsNullOrEmpty(HttpProxy) && HttpProxy.ParseHttpAddress(out var userName, out var password, out var host, out var port)) {
                Log.Debug($"Using http proxy: {HttpProxy.PrettyQuote()}.");
                updater.UseProxy($"{host}{(port > 0 ? $":{port}" : "")}", userName, password);
            }
            
            var currentVersion = UpdaterHelper.StringToVersion(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            Log.Info($"Current local version: {currentVersion}.");

            var releases = updater.FetchNewReleases(currentVersion);
            if (releases == null || releases.Count == 0) {
                Log.Done("Your version is up to date.");
                return 0;
            }

            var latestRelease = releases[0];
            
            Log.Debug($"{releases.Count} new releases found on github, the latest release is {latestRelease.TagName}.");

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
            var tempFilePath = updater.DownloadToTempFile(releases[0].Assets[0].BrowserDownloadUrl, progress => {
                Log.ReportProgress(100, (int) Math.Round((decimal) progress.NumberOfBytesDoneTotal / progress.NumberOfBytesDoneTotal * 100), "Downloading update.");
            });

            var tempExtractionDir = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            Directory.CreateDirectory(tempExtractionDir);
            using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Read)) {
                zip.ExtractToDirectory(tempExtractionDir);
            }
            
            var fileUpdater = SimpleFileUpdater.Instance;
            foreach (var file in Utils.EnumerateAllFiles(tempExtractionDir)) {
                fileUpdater.AddFileToMove(file, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), file.Replace(tempExtractionDir, "")));
            }
            
            File.Delete(tempFilePath);

            if (fileUpdater.IsAdminRightsNeeded) {
                Log.Warn("The update will require you to accept the execution permission for the updater.exe.");
            }

            Log.Done("The update will be done after this command has ended.");
            fileUpdater.Start();
            
            return 1;
        }
    }

}