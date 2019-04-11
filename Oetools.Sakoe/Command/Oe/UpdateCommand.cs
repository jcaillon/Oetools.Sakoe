#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (UpdateCommand.cs) is part of Oetools.Sakoe.
//
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        private const string RepoName = "battle-code"; // Oetools.Sakoe
        public const string GitHubToken = "MmViMDJlNWVlYWZlMTIzNGIxN2VmOTkxMGQ1NzljMTRkM2E1ZDEyMw==";

        [Option("-b|--get-pre-release", "Accept to update from new pre-release (i.e. 'beta') versions of the tool.\nThis option will be used by default if the current version of the tool is a pre-release version. Otherwise, only stable releases will be used for updates. ", CommandOptionType.NoValue)]
        public bool GetPreRelease { get; set; }

        [Option("-p|--proxy", "The http proxy to use for this update. Useful if you are behind a corporate firewall.\nThe expected format is: 'http(s)://[user:password@]host[:port]'.\nIt is also possible to use the environment variables OE_HTTP_PROXY or http_proxy to set this value.", CommandOptionType.SingleValue)]
        public string HttpProxy { get; set; }

        [Option("-c|--check-only", "Check for new releases but exit the command before actually updating the tool.", CommandOptionType.NoValue)]
        public bool CheckOnly { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var usePreRelease = GetPreRelease || RunningAssembly.Info.IsPreRelease;
            Log.Debug(usePreRelease ? "Getting pre-releases." : "Skipping pre-releases.");

            var updater = new GitHubUpdater();

            SetUpdaterProxy(updater, HttpProxy, Log);

            updater.UseAuthorizationToken(Encoding.ASCII.GetString(Convert.FromBase64String(GitHubToken)));
            updater.SetRepo(RepoOwner, RepoName);
            updater.UseCancellationToken(CancelToken);
            updater.UseMaxNumberOfReleasesToFetch(10);

            var currentVersion = RunningAssembly.Info.AssemblyVersion;
            Log.Info($"Current local version: {currentVersion}.");

            Log.Debug("Fetching latest releases from github api.");
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
                    return asset.Name.Contains("win") && asset.Name.Contains(Environment.Is64BitProcess ? "x64" : "x86");
                }
                #if !SELFCONTAINEDWITHEXE
                    return asset.Name.Contains("no-runtime");
                #else
                    return asset.Name.Contains("core");
                #endif
            });

            if (latestAsset == null) {
                throw new CommandException("Could not find a matching asset in the latest github release.");
            }

            Log.Debug($"Downloading the latest release asset {latestAsset.Name} from {latestAsset.BrowserDownloadUrl}.");
            var tempFilePath = updater.DownloadToTempFile(latestAsset.BrowserDownloadUrl, progress => {
                Log.ReportProgress(100, (int) Math.Round((decimal) progress.NumberOfBytesDoneTotal / progress.NumberOfBytesTotal * 100), "Downloading update.");
            });

            var tempExtractionDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempExtractionDir);
            using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Read)) {
                zip.ExtractToDirectory(tempExtractionDir);
            }

            var fileUpdater = SimpleFileUpdater.Instance;
            var toolDirectory = Path.GetDirectoryName(RunningAssembly.Info.Location);
            if (string.IsNullOrEmpty(toolDirectory)) {
                throw new CommandException("Could not find the directory in which this tool is installed.");
            }
            foreach (var file in Utils.EnumerateAllFiles(Path.Combine(tempExtractionDir, "sakoe"))) {
                if (file != null) {
                    fileUpdater.AddFileToMove(file, Path.Combine(toolDirectory, Path.GetFileName(file)));
                }
            }

            File.Delete(tempFilePath);

            if (fileUpdater.IsAdminRightsNeeded) {
                Log.Warn("The update will require you to accept the execution permission for the updater.exe.");
            }

            fileUpdater.Start();
            Log.Done("Update successful.");

            return 0;
        }

        public static void SetUpdaterProxy(GitHubUpdater updater, string httpProxy, ILog log) {
            var httpProxyFromEnv = Environment.GetEnvironmentVariable("OE_HTTP_PROXY");

            if (string.IsNullOrEmpty(httpProxy) && !string.IsNullOrEmpty(httpProxyFromEnv)) {
                log.Debug($"Using the proxy found in the environment variable OE_HTTP_PROXY: {httpProxyFromEnv.PrettyQuote()}.");
                httpProxy = httpProxyFromEnv;
            }

            httpProxyFromEnv = Environment.GetEnvironmentVariable("http_proxy");

            if (string.IsNullOrEmpty(httpProxy) && !string.IsNullOrEmpty(httpProxyFromEnv)) {
                log.Debug($"Using the proxy found in the environment variable HTTP_PROXY: {httpProxyFromEnv.PrettyQuote()}.");
                httpProxy = httpProxyFromEnv;
            }

            if (!string.IsNullOrEmpty(httpProxy) && httpProxy.ParseWebProxy(out var host, out var port, out var user, out var password)) {
                log.Debug($"Using http proxy: {httpProxy.PrettyQuote()}.");
                updater.UseProxy($"{host}:{port}", user, password);
            }
        }
    }

}
