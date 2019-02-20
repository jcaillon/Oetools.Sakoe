#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (DataDiggerCommand.cs) is part of Oetools.Sakoe.
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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using GithubUpdater.GitHub;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "datadigger", "dd",
        Description = "The DataDigger is a database explorer tool, fully written in Progress, that enables you to handle your data in a very natural way.",
        ExtendedHelpText = @"If DataDigger is already installed on your computer, you can use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify the installation location so sakoe knows where to find the tool. Otherwise, simply let sakoe install it in the default location.

DataDigger is maintained by Patrick Tingen: https://github.com/patrickTingen/DataDigger.

Learn more here: https://datadigger.wordpress.com."
    )]
    [Subcommand(typeof(DataDiggerInstallCommand))]
    [Subcommand(typeof(DataDiggerRemoveCommand))]
    [Subcommand(typeof(DataDiggerRunCommand))]
    internal class DataDiggerCommand : AExpectSubCommand {

        /// <summary>
        /// DataDigger installation directory.
        /// </summary>
        public static string DataDiggerInstallationDirectory {
            get {
                var path = Environment.GetEnvironmentVariable("OE_DATADIGGER_INSTALL_PATH");
                if (string.IsNullOrEmpty(path)) {
                    path = Path.Combine(ExternalTools.ExternalToolInstallationDirectory, "DataDigger");
                }

                return path;
            }
        }

        /// <summary>
        /// Returns true if DataDigger is installed.
        /// </summary>
        public static bool IsDataDiggerInstalled => Directory.Exists(DataDiggerInstallationDirectory) && Directory.EnumerateFiles(DataDiggerInstallationDirectory, "*", SearchOption.TopDirectoryOnly).Any();

        /// <summary>
        /// Returns the prowin parameters to use for DataDigger.
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public static UoeProcessArgs DataDiggerStartUpParameters(bool readOnly) => new UoeProcessArgs().Append("-pf").Append("DataDigger.pf").Append("-p").Append(readOnly ? "DataReader.p" : "DataDigger.p").Append("-T").Append(Utils.CreateTempDirectory(Utils.GetRandomName())) as UoeProcessArgs;
    }

    [Command(
        "run", "ru",
        Description = "Run a new DataDigger instance.",
        ExtendedHelpText = "Please note that when running DataDigger, the DataDigger.pf file of the installation path is used."
    )]
    internal class DataDiggerRunCommand : DatabaseDataDiggerCommand { }

    [Command(
        "remove", "rm",
        Description = "Remove DataDigger from the installation path."
    )]
    internal class DataDiggerRemoveCommand : ABaseCommand {

        [Required]
        [Option("-f|--force", "Mandatory option to force the removal and avoid bad manipulation.", CommandOptionType.NoValue)]
        public bool Force { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (!DataDiggerCommand.IsDataDiggerInstalled) {
                Log.Warn("DataDigger is not installed.");
                return 1;
            }

            Directory.Delete(DataDiggerCommand.DataDiggerInstallationDirectory, true);

            Log.Done($"DataDigger removed from: {DataDiggerCommand.DataDiggerInstallationDirectory.PrettyQuote()}.");

            return 0;
        }
    }

    [Command(
        "install", "in",
        Description = "Install DataDigger in the default installation path. Use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify a different location."
    )]
    internal class DataDiggerInstallCommand : ABaseCommand {
        private const string RepoOwner = "patrickTingen";
        private const string RepoName = "DataDigger";
        private const string GitHubToken = UpdateCommand.GitHubToken;

        [Option("-b|--get-pre-release", "Accept to install pre-release (i.e. 'beta') versions of the tool.", CommandOptionType.NoValue)]
        public bool GetPreRelease { get; set; } = false;

        [Option("-p|--proxy", "The http proxy to use for this update. Useful if you are behind a corporate firewall.\nThe expected format is: 'http(s)://[user:password@]host[:port]'.\nIt is also possible to use the environment variables OE_HTTP_PROXY or http_proxy to set this value.", CommandOptionType.SingleValue)]
        public string HttpProxy { get; set; }

        [Option("-f|--force", "Force the installation even if the tool is already installed.", CommandOptionType.NoValue)]
        public bool Force { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (!Force && DataDiggerCommand.IsDataDiggerInstalled) {
                Log.Warn($"DataDigger is already installed on path: {DataDiggerCommand.DataDiggerInstallationDirectory.PrettyQuote()}.");
                return 1;
            }

            Log.Debug(GetPreRelease ? "Getting pre-releases." : "Skipping pre-releases.");

            var updater = new GitHubUpdater();

            UpdateCommand.SetUpdaterProxy(updater, HttpProxy, Log);

            updater.UseAuthorizationToken(Encoding.ASCII.GetString(Convert.FromBase64String(GitHubToken)));
            updater.SetRepo(RepoOwner, RepoName);
            updater.UseCancellationToken(CancelToken);
            updater.UseMaxNumberOfReleasesToFetch(10);

            Log.Debug("Fetching latest releases from github api.");
            var latestRelease = updater.FetchNewReleases(release => !release.Prerelease || GetPreRelease)?.FirstOrDefault();
            if (latestRelease == null) {
                throw new CommandException("Could not find a github release for datadigger.");
            }

            Log.Info($"Latest release: {latestRelease.Name.PrettyQuote()}: {latestRelease.HtmlUrl}");

            Log.Debug($"Downloading the latest release asset: {latestRelease.ZipballUrl}.");
            var tempFilePath = updater.DownloadToTempFile(latestRelease.ZipballUrl, progress => {
                Log.ReportProgress(100, (int) Math.Round((decimal) progress.NumberOfBytesDoneTotal / progress.NumberOfBytesTotal * 100), "Downloading the release.");
            });

            var destinationDir = DataDiggerCommand.DataDiggerInstallationDirectory;
            Utils.CreateDirectoryIfNeeded(destinationDir);

            using (var zip = ZipFile.Open(tempFilePath, ZipArchiveMode.Read)) {
                var count = zip.Entries.Count;
                var i = 0;
                foreach (var zipEntry in zip.Entries) {
                    if (zipEntry.Length == 0 && string.IsNullOrEmpty(zipEntry.Name)) {
                        continue; // folder
                    }
                    var correctedPath = zipEntry.FullName.ToCleanRelativePathUnix();
                    var idx = correctedPath.IndexOf("/", StringComparison.Ordinal);
                    if (idx < 0 || idx + 1 > correctedPath.Length) {
                        continue;
                    }
                    correctedPath = correctedPath.Substring(correctedPath.IndexOf("/", StringComparison.Ordinal) + 1);
                    var extractionPath = Path.Combine(destinationDir, correctedPath);
                    Utils.CreateDirectoryIfNeeded(Path.GetDirectoryName(extractionPath));
                    Log.ReportProgress(count, i, $"Extracting file to: {extractionPath.PrettyQuote()}.");
                    zipEntry.ExtractToFile(extractionPath, true);
                    i++;
                }
            }

            File.Delete(tempFilePath);

            Log.Done($"DataDigger has been installed on path: {destinationDir.PrettyQuote()}.");

            return 0;
        }
    }
}
