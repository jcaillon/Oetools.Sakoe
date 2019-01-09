#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (DatabaseCommand.cs) is part of Oetools.Sakoe.
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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Database;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe {


    [Command(
        "database", "da", "db",
        Description = "Database manipulation tools."
    )]
    [Subcommand(typeof(CreateDatabaseCommand))]
    [Subcommand(typeof(StartDatabaseCommand))]
    [Subcommand(typeof(StopDatabaseCommand))]
    [Subcommand(typeof(KillAllDatabaseCommand))]
    [Subcommand(typeof(DeleteDatabaseCommand))]
    [Subcommand(typeof(RepairDatabaseCommand))]
    [Subcommand(typeof(ConnectDatabaseCommand))]
    [Subcommand(typeof(CopyDatabaseCommand))]
    [Subcommand(typeof(DumpDatabaseCommand))]
    [Subcommand(typeof(LoadDatabaseCommand))]
    [Subcommand(typeof(DatabaseAdminCommand))]
    [Subcommand(typeof(DatabaseDictionaryCommand))]
    // TODO: project database manipulation!
//    [Subcommand(typeof(ProjectDatabaseCommand))]
    internal class DatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "dictionary", "di", "dic",
        Description = "Open the database dictionary tool."
    )]
    internal class DatabaseDictionaryCommand : DatabaseAdminCommand {

        protected override string ProgramName => "_dict.p";

        protected override string ExtraParameters => "";
    }

    [Command(
        "admin", "ad",
        Description = "Open the database administration tool."
    )]
    internal class DatabaseAdminCommand : ADatabaseCommand {

        [Description("[[--] <connection string>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dlcPath = GetDlcPath();
            var connectionString = RemainingArgs != null ? string.Join(" ", RemainingArgs) : null;
            if (!string.IsNullOrEmpty(connectionString)) {
                Log.Info($"Connection string used: {connectionString.PrettyQuote()}.");
            } else {
                SetTargetDatabasePath();
                connectionString = new UoeDatabaseOperator(dlcPath) {
                    Log = Log
                }.GetConnectionString(TargetDatabasePath);
            }

            if (UoeUtilities.CanProVersionUseNoSplashParameter(UoeUtilities.GetProVersionFromDlc(dlcPath))) {
                connectionString = $"{connectionString ?? ""} -nosplash";
            }

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = UoeUtilities.GetProExecutableFromDlc(dlcPath),
                    Arguments = $"-p {ProgramName} {connectionString} {ExtraParameters}"
                }
            };
            process.Start();

            return 0;
        }

        protected virtual string ProgramName => "_admin.p";

        protected virtual string ExtraParameters => "";
    }

    [Command(
        "connect", "co",
        Description = "Get the connection string to use to connect to a database."
    )]
    internal class ConnectDatabaseCommand : ADatabaseCommand {

        [Option("-l|--logical-name", "The logical name to use for the database in the connection string.", CommandOptionType.SingleValue)]
        public string LogicalName { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            SetTargetDatabasePath();
            var res = new UoeDatabaseOperator(GetDlcPath()) {
                Log = Log
            }.GetConnectionString(TargetDatabasePath, LogicalName);
            Out.WriteResultOnNewLine(res);
            return 0;
        }
    }

    [Command(
        "repair", "re",
        Description = "Repair the structure of a database.",
        ExtendedHelpText = "Update the database control information, usually done after an extent has been moved or renamed."
    )]
    internal class RepairDatabaseCommand : ADatabaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            SetTargetDatabasePath();
            new UoeDatabaseOperator(GetDlcPath()) {
                Log = Log
            }.ProstrctRepair(TargetDatabasePath);
            return 0;
        }
    }

    [Command(
        "delete", "de",
        Description = "Delete a database.",
        ExtendedHelpText = "All the files composing the database are deleted without confirmation."
    )]
    internal class DeleteDatabaseCommand : ADatabaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            SetTargetDatabasePath();
            new UoeDatabaseOperator(GetDlcPath()) {
                Log = Log
            }.Delete(TargetDatabasePath);
            return 0;
        }
    }

    [Command(
        "stop", "proshut", "sp",
        Description = "Stop a database that was started for multi-users.",
        AllowArgumentSeparator = true
    )]
    internal class StopDatabaseCommand : ADatabaseCommand {

        [Description("[[--] <extra proshut parameters>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            SetTargetDatabasePath();
            var extra = RemainingArgs != null ? string.Join(" ", RemainingArgs) : null;
            if (!string.IsNullOrEmpty(extra)) {
                Log.Info($"Extra parameters: {extra.PrettyQuote()}.");
            }

            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) {
                Log = Log
            };
            dbOperator.Proshut(TargetDatabasePath, extra);
            Log.Debug(dbOperator.LastOperationOutput);

            return 0;
        }
    }

    [Command(
        "copy", "cp",
        Description = "Copy a database."
    )]
    internal class CopyDatabaseCommand : AOeDlcCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<source db path>", "The path to the 'source' database. The .db extension is optional.")]
        public string SourceDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(1, "<target db path>", "The path to the 'target' database. The .db extension is optional.")]
        public string TargetDatabasePath { get; set; }

        [Option("-ni|--newinstance", "Use -newinstance in the procopy command.", CommandOptionType.NoValue)]
        public bool NewInstance { get; } = false;

        [Option("-rp|--relativepath", "Use -relativepath in the procopy command.", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) {
                Log = Log
            };
            dbOperator.Procopy(TargetDatabasePath, SourceDatabasePath, NewInstance, RelativePath);
            return 0;
        }
    }

    [Command(
        "kill", "ki",
        Description = "Kill all the broker processes running on this machine.",
        ExtendedHelpText = "A broker process is generally named: `_mprosrv`."
    )]
    internal class KillAllDatabaseCommand : ABaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            UoeDatabaseOperator.KillAllMproSrv();
            return 0;
        }
    }

    [Command(
        "start", "proserve", "st",
        Description = "Start a database in order to use it in multi-users mode.",
        AllowArgumentSeparator = true
    )]
    internal class StartDatabaseCommand : ADatabaseCommand {

        [Option("-s|--service", "Service name that will be used by this database. Usually a port number or a service name declared in /etc/services.", CommandOptionType.SingleValue)]
        public string ServiceName { get; set; }

        private const string HostNameLongName = "--hostname";
        [Option("-h|" + HostNameLongName, "The hostname on which to start the database. Defaults to the current machine.", CommandOptionType.SingleValue)]
        public string HostName { get; set; }

        private const string NextPortAvailableLongName = "--next-port";
        [Option("-np|" + NextPortAvailableLongName, "Port number, the next available port after this number will be used to start the database.", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) NextPortAvailable { get; set; }

        [Option("-nu|--nb-users", "Number of users that should be able to connect to this database simultaneously.", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) NbUsers { get; set; }

        [Description("[[--] <extra proserve parameters>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            SetTargetDatabasePath();
            var extra = RemainingArgs != null ? string.Join(" ", RemainingArgs) : null;
            if (!string.IsNullOrEmpty(extra)) {
                Log.Info($"Extra parameters: {extra.PrettyQuote()}.");
            }

            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) { Log = Log };

            // service or next port
            if (!string.IsNullOrEmpty(ServiceName) && NextPortAvailable.HasValue) {
                throw new CommandException($"Both {HostNameLongName} and {NextPortAvailableLongName} are defined but they are mutually exclusive options, choose only one.");
            }

            if (NextPortAvailable.HasValue) {
                ServiceName = UoeDatabaseOperator.GetNextAvailablePort(NextPortAvailable.value).ToString();
            }

            if (NbUsers.HasValue && NbUsers.value <= 0) {
                throw new CommandException("The number of users can only be strictly superior to zero.");
            }

            dbOperator.ProServe(TargetDatabasePath, HostName, ServiceName, NbUsers.value > 0 ? (int?) NbUsers.value : null, extra);

            Log.Info("Multi-user connection string:");
            Out.WriteResultOnNewLine($"{UoeDatabaseOperator.GetMultiUserConnectionString(TargetDatabasePath, HostName, ServiceName)}");
            Log.Info("Database started successfully.");

            return 0;
        }
    }

    [Command(
        "create", "cr",
        Description = "Creates a new database."
    )]
    internal class CreateDatabaseCommand : AOeDlcCommand {

        [LegalFilePath]
        [Argument(0, "<db path>", "Path to the database to create. The .db extension is optional. Defaults to the name of the current directory.")]
        public string TargetDatabasePath { get; set; }

        [Option("-df|--df", "Path to the .df file containing the database schema definition.", CommandOptionType.SingleValue)]
        [FileExists]
        public string SchemaDefinitionFilePath { get; set; }

        [Option("-st|--st", "Path to the .st file containing the database physical structure.", CommandOptionType.SingleValue)]
        [FileExists]
        public string StuctureFilePath { get; set; }

        [Option("-bs|--blocksize", "The blocksize to use when creating the database.", CommandOptionType.SingleValue)]
        public DatabaseBlockSize BlockSize { get; } = DatabaseBlockSize.DefaultForCurrentPlatform;

        [Option("-cp|--codepage", "Existing codepage in the openedge installation $DLC/prolang/(codepage).", CommandOptionType.SingleValue)]
        public string Codepage { get; } = null;

        [Option("-ni|--newinstance", "Use -newinstance in the procopy command.", CommandOptionType.NoValue)]
        public bool NewInstance { get; } = false;

        [Option("-rp|--relativepath", "Use -relativepath in the procopy command.", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (string.IsNullOrEmpty(TargetDatabasePath)) {
                TargetDatabasePath = Path.GetFileName(Directory.GetCurrentDirectory().ToCleanPath());
                Log.Info($"Using directory name for the database name: {TargetDatabasePath.PrettyQuote()}.");
            }

            using (var dbAdministrator = new UoeDatabaseAdministrator(GetDlcPath())) {
                dbAdministrator.Log = Log;

                dbAdministrator.CreateDatabase(TargetDatabasePath, StuctureFilePath?.MakePathAbsolute(), BlockSize, Codepage, NewInstance, RelativePath, SchemaDefinitionFilePath?.MakePathAbsolute());

                Log.Info("Single user connection string:");
                Out.WriteOnNewLine(UoeDatabaseOperator.GetSingleUserConnectionString(TargetDatabasePath));
                Log.Info($"Database created successfully: {TargetDatabasePath.PrettyQuote()}.");
            }

            return 0;
        }
    }
}
