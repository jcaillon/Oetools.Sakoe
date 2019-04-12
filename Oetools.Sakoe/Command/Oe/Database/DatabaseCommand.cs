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

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.ConLog;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge.Database;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "database", "db",
        Description = "Database manipulation tools."
    )]
    [Subcommand(typeof(DatabaseStructureCommand))]
    [Subcommand(typeof(DatabaseAnalysisCommand))]
    [Subcommand(typeof(DatabaseLogCommand))]
    [Subcommand(typeof(DatabaseBiCommand))]
    [Subcommand(typeof(DatabaseIndexCommand))]
    [Subcommand(typeof(DatabaseAdminCommand))]
    [Subcommand(typeof(DatabaseDictionaryCommand))]
    [Subcommand(typeof(DataDiggerCommand))]
    [Subcommand(typeof(DatabaseProjectCommand))]
    [Subcommand(typeof(DatabaseDataCommand))]
    [Subcommand(typeof(DatabaseSchemaCommand))]

    [Subcommand(typeof(DatabaseCreateCommand))]
    [Subcommand(typeof(DatabaseStartCommand))]
    [Subcommand(typeof(DatabaseStopCommand))]
    [Subcommand(typeof(DatabaseKillCommand))]
    [Subcommand(typeof(DatabaseKillAllCommand))]
    [Subcommand(typeof(DatabaseDeleteCommand))]
    [Subcommand(typeof(DatabaseRepairCommand))]
    [Subcommand(typeof(DatabaseConnectCommand))]
    [Subcommand(typeof(DatabaseCopyCommand))]
    [Subcommand(typeof(DatabaseGetBusyCommand))]
    [Subcommand(typeof(DatabaseBackUpCommand))]
    [Subcommand(typeof(DatabaseRestoreCommand))]
    internal class DatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "connect", "co",
        Description = "Get the connection string to use to connect to a database."
    )]
    internal class DatabaseConnectCommand : ADatabaseSingleLocationCommand {

        [Option("-l|--logical-name <name>", "The logical name to use for the database in the connection string.", CommandOptionType.SingleValue)]
        public string LogicalName { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine(GetOperator().GetDatabaseConnection(GetSingleDatabaseLocation(), LogicalName).ToString());
            return 0;
        }
    }

    [Command(
        "repair", "re",
        Description = "Repair/update the database control information file (" + UoeDatabaseLocation.Extension + ").",
        ExtendedHelpText = "Update the database control information, usually done after an extent has been moved or renamed."
    )]
    internal class DatabaseRepairCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().RepairDatabaseControlInfo(GetSingleDatabaseLocation(), new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters));
            return 0;
        }
    }

    [Command(
        "delete", "de",
        Description = "Delete a database.",
        ExtendedHelpText = "All the files composing the database are deleted."
    )]
    internal class DatabaseDeleteCommand : ADatabaseSingleLocationCommand {

        [Option("-y|--yes", "Automatically answer yes on deletion confirmation.", CommandOptionType.NoValue)]
        public bool ForceDelete { get; set; } = false;

        [Option("-st|--delete-st", "Also delete the structure file (" + UoeDatabaseLocation.StructureFileExtension + ").", CommandOptionType.NoValue)]
        public bool DeleteStructureFile { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var loc = GetSingleDatabaseLocation();

            if (!ForceDelete) {
                Out.WriteOnNewLine(null);
                var args = Prompt.GetString($"Please confirm the definitive deletion of {loc.PhysicalName.PrettyQuote()} (y/n): ");

                if (string.IsNullOrEmpty(args) || !args.Equals("y", StringComparison.CurrentCultureIgnoreCase)) {
                    return 1;
                }
            }

            GetOperator().Delete(loc);

            if (DeleteStructureFile) {
                Log?.Debug($"Deleting: {loc.StructureFileFullPath.PrettyQuote()}.");
                File.Delete(loc.StructureFileFullPath);
            }

            return 0;
        }
    }

    [Command(
        "stop", "so",
        Description = "Stop a database that was started for multi-users."
    )]
    internal class DatabaseStopCommand : ADatabaseSingleLocationCommand {

        [Description("[-- <extra proshut args>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Stop(GetSingleDatabaseLocation(), new ProcessArgs().Append(RemainingArgs));
            return 0;
        }
    }

    [Command(
        "kill", "ki",
        Description = "Kill the broker/server processes running for a particular a database."
    )]
    internal class DatabaseKillCommand : ADatabaseSingleLocationCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Kill(GetSingleDatabaseLocation());
            return 0;
        }
    }

    [Command(
        "copy", "cp",
        Description = "Copy a database."
    )]
    internal class DatabaseCopyCommand : ADatabaseCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<source db path>", "Path to the source database (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory.")]
        public string SourceDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(1, "<target db path>", "Path to the target database (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory.")]
        public string TargetDatabasePath { get; set; }

        [Option("-ni|--new-instance", "Specifies that a new GUID be created for the target database.", CommandOptionType.NoValue)]
        public bool NewInstance { get; } = false;

        [Option("-rp|--relative-path", "Use relative path in the structure file.", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Copy(new UoeDatabaseLocation(TargetDatabasePath), new UoeDatabaseLocation(SourceDatabasePath), NewInstance, RelativePath);
            return 0;
        }
    }

    [Command(
        "kill-all", "ka",
        Description = "Kill all the broker/server processes running on this machine.",
        ExtendedHelpText = "A broker process is generally named: `_mprosrv`."
    )]
    internal class DatabaseKillAllCommand : ABaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            UoeDatabaseOperator.KillAllMproSrv();
            return 0;
        }
    }

    [Command(
        "start", "sa",
        Description = "Start a database in order to use it in multi-users mode."
    )]
    internal class DatabaseStartCommand : ADatabaseSingleLocationCommand {

        [Option("-s|--service <port>", "Service name that will be used by this database. Usually a port number or a service name declared in /etc/services.", CommandOptionType.SingleValue)]
        public string ServiceName { get; set; }

        private const string HostNameLongName = "--hostname";
        [Option("-h|" + HostNameLongName + " <host>", "The hostname on which to start the database. Defaults to the current machine.", CommandOptionType.SingleValue)]
        public string HostName { get; set; }

        private const string NextPortAvailableLongName = "--next-port";
        [Option("-np|" + NextPortAvailableLongName + " <port>", "Port number, the next available port after this number will be used to start the database.", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) NextPortAvailable { get; set; }

        [Description("[-- <extra proserve args>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            // service or next port
            if (!string.IsNullOrEmpty(ServiceName) && NextPortAvailable.HasValue) {
                throw new CommandException($"Both {HostNameLongName} and {NextPortAvailableLongName} are defined but they are mutually exclusive options, choose only one.");
            }

            if (NextPortAvailable.HasValue) {
                ServiceName = UoeDatabaseOperator.GetNextAvailablePort(NextPortAvailable.value).ToString();
            }

            var db = GetSingleDatabaseLocation();
            var connection = GetOperator().Start(db, HostName, ServiceName, new UoeProcessArgs().Append(RemainingArgs) as UoeProcessArgs);

            Log.Info("Multi-user connection string:");
            Out.WriteResultOnNewLine(connection.ToString());

            return 0;
        }
    }

    [Command(
        "create", "cr",
        Description = "Creates a new database."
    )]
    internal class DatabaseCreateCommand : ADatabaseCommand {

        [LegalFilePath]
        [Option("-f|--file <file>",  "File name (physical name) of the database to create (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory. Defaults to the name of the current directory.", CommandOptionType.SingleValue)]
        public string DatabasePhysicalName { get; set; }

        [FileExists]
        [Option("-df|--df <file>", "Path to the " + UoeDatabaseLocation.SchemaDefinitionExtension + " file containing the database schema definition. The path can be relative to the current directory.", CommandOptionType.SingleValue)]
        public string SchemaDefinitionFilePath { get; set; }

        [FileExists]
        [Option("-st|--st <file>", "Path to the structure file (" + UoeDatabaseLocation.StructureFileExtension + " file) containing the database physical structure. The path can be relative to the current directory.", CommandOptionType.SingleValue)]
        public string StuctureFilePath { get; set; }

        [Option("-bs|--block-size <size>", "The block-size to use when creating the database. Defaults to the default block-size for the current platform (linux or windows).", CommandOptionType.SingleValue)]
        public DatabaseBlockSize BlockSize { get; } = DatabaseBlockSize.DefaultForCurrentPlatform;

        [Option("-lg|--lang <lang>", "The codepage/lang to use. Creates the database using an empty database located in the openedge installation $DLC/prolang/(lang).", CommandOptionType.SingleValue)]
        public string Codepage { get; } = null;

        [Option("-ni|--new-instance", "Specifies that a new GUID be created for the target database.", CommandOptionType.NoValue)]
        public bool NewInstance { get; } = false;

        [Option("-rp|--relative-path", "Use relative path in the structure file.", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (string.IsNullOrEmpty(DatabasePhysicalName)) {
                DatabasePhysicalName = UoeDatabaseLocation.GetValidPhysicalName(Path.GetFileName(Directory.GetCurrentDirectory()));
                Log.Info($"Using directory name for the database name: {DatabasePhysicalName.PrettyQuote()}.");
            }

            using (var ope = GetAdministrator()) {
                var db = new UoeDatabaseLocation(DatabasePhysicalName);

                ope.CreateWithDf(db, SchemaDefinitionFilePath?.ToAbsolutePath(), StuctureFilePath?.ToAbsolutePath(), BlockSize, Codepage, NewInstance, RelativePath);

                Log.Info("Single user connection string:");
                Out.WriteOnNewLine(UoeDatabaseConnection.NewSingleUserConnection(db).ToString());
            }

            return 0;
        }
    }

    [Command(
        "busy", "bu",
        Description = "Fetch the busy mode of a database, indicating if the database is used in single/multi user mode (or not busy at all)."
    )]
    internal class DatabaseGetBusyCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var busyMode = GetOperator().GetBusyMode(GetSingleDatabaseLocation(), new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters));

            Log.Info("Database busy mode:");
            Out.WriteOnNewLine(busyMode.ToString());

            return 0;
        }
    }

    [Command(
        "backup", "ba",
        Description = "Backup a database into a single file for a future restore."
    )]
    internal class DatabaseBackUpCommand : ADatabaseSingleLocationCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<backup-file>", "File path that will contain the database backup.")]
        public string DumpFilePath { get; set; }

        [Option("-ns|--no-scan", "Prevents the tool from performing an initial scan of the database to display the number of blocks that will be backed up and the amount of media the backup requires.", CommandOptionType.NoValue)]
        public bool NoScan { get; } = false;

        [Option("-nc|--no-compress", "Prevents the tool from compressing the backup file.", CommandOptionType.NoValue)]
        public bool NoCompressed { get; } = false;

        [Option("-op|--options <args>", @"Use options for the probkup utility (see the documentation online).", CommandOptionType.SingleValue)]
        public string Options { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Backup(GetSingleDatabaseLocation(), DumpFilePath, Verbosity == ConsoleLogThreshold.Debug, !NoScan, !NoCompressed, new UoeProcessArgs().AppendFromQuotedArgs(Options));
            return 0;
        }
    }

    [Command(
        "restore", "rs",
        Description = "Restore a database from a backup file."
    )]
    internal class DatabaseRestoreCommand : ADatabaseSingleLocationCommand {

        [Required]
        [FileExists]
        [Argument(0, "<backup-file>", "File path that contains the database backup.")]
        public string BackupFilePath { get; set; }

        [Option("-op|--options <args>", @"Use options for the prorest utility (see the documentation online).", CommandOptionType.SingleValue)]
        public string Options { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Restore(GetSingleDatabaseLocation(), BackupFilePath, new UoeProcessArgs().AppendFromQuotedArgs(Options));
            return 0;
        }
    }
}
