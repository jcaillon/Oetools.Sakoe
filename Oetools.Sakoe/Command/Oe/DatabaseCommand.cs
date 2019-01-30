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
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "database", "db",
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
    [Subcommand(typeof(DatabaseDataDiggerCommand))]
    // TODO: project database manipulation!
//    [Subcommand(typeof(ProjectDatabaseCommand))]
    internal class DatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "datadigger", "dd",
        Description = "Open a new DataDigger instance.",
        ExtendedHelpText = "Please note that when running DataDigger, the DataDigger.pf file of the installation path is used."
    )]
    internal class DatabaseDataDiggerCommand : ADatabaseToolCommand {

        [Option("-ro|--read-only", "Start DataDigger in read-only mode (records will not modifiable).", CommandOptionType.NoValue)]
        public bool ReadOnly { get; set; } = false;

        protected override string ToolArguments() => $"{DataDiggerCommand.DataDiggerStartUpParameters(ReadOnly)}";

        protected override string ExecutionWorkingDirectory => DataDiggerCommand.DataDiggerInstallationDirectory;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (!Utils.IsRuntimeWindowsPlatform) {
                throw new CommandException("DataDigger can only run on windows platform.");
            }
            if (!DataDiggerCommand.IsDataDiggerInstalled) {
                throw new CommandException($"DataDigger is not installed yet, use the command {typeof(DataDiggerInstallCommand).GetFullCommandLine().PrettyQuote()}.");
            }
            return base.ExecuteCommand(app, console);
        }
    }

    [Command(
        "dictionary", "di", "dic",
        Description = "Open the database dictionary tool."
    )]
    internal class DatabaseDictionaryCommand : ADatabaseToolCommand {

        protected override string ToolArguments() => "-p _dict.p";

    }

    [Command(
        "admin", "ad",
        Description = "Open the database administration tool."
    )]
    internal class DatabaseAdminCommand : ADatabaseToolCommand {

        protected override string ToolArguments() => "-p _admin.p";
    }

    [Command(
        "connect", "co",
        Description = "Get the connection string to use to connect to a database."
    )]
    internal class ConnectDatabaseCommand : ADatabaseSingleLocationCommand {

        [Option("-l|--logical-name", "The logical name to use for the database in the connection string.", CommandOptionType.SingleValue)]
        public string LogicalName { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine(GetOperator().GetDatabaseConnection(GetSingleDatabaseLocation(), LogicalName).ToString());
            return 0;
        }
    }

    [Command(
        "repair", "re",
        Description = "Repair the structure of a database.",
        ExtendedHelpText = "Update the database control information, usually done after an extent has been moved or renamed."
    )]
    internal class RepairDatabaseCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().RepairDatabaseControlInfo(GetSingleDatabaseLocation(), DatabaseAccessStartupParameters);
            return 0;
        }
    }

    [Command(
        "delete", "de",
        Description = "Delete a database.",
        ExtendedHelpText = "All the files composing the database are deleted without confirmation."
    )]
    internal class DeleteDatabaseCommand : ADatabaseSingleLocationCommand {

        [Required]
        [Option("-y|--yes", "Mandatory option to force the deletion and avoid bad manipulation.", CommandOptionType.NoValue)]
        public bool ForceDelete { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (ForceDelete) {
                GetOperator().Delete(GetSingleDatabaseLocation());
            }
            return 0;
        }
    }

    [Command(
        "stop", "sp",
        Description = "Stop a database that was started for multi-users."
    )]
    internal class StopDatabaseCommand : ADatabaseSingleLocationCommand {

        [Description("[-- <extra proshut args>...]")]
        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().Shutdown(GetSingleDatabaseLocation(), GetRemainingArgsAsProArgs(RemainingArgs));
            return 0;
        }
    }

    [Command(
        "copy", "cp",
        Description = "Copy a database."
    )]
    internal class CopyDatabaseCommand : ADatabaseCommand {

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
        "start", "st",
        Description = "Start a database in order to use it in multi-users mode."
    )]
    internal class StartDatabaseCommand : ADatabaseSingleLocationCommand {

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

            if (NbUsers.HasValue && NbUsers.value <= 0) {
                throw new CommandException("The number of users can only be strictly superior to zero.");
            }

            var db = GetSingleDatabaseLocation();
            GetOperator().Start(db, HostName, ServiceName, NbUsers.HasValue ? (int?) NbUsers.value : null, GetRemainingArgsAsProArgs(RemainingArgs));

            Log.Info("Multi-user connection string:");
            Out.WriteResultOnNewLine(UoeDatabaseConnection.NewMultiUserConnection(db, HostName, ServiceName).ToString());

            return 0;
        }
    }

    [Command(
        "create", "cr",
        Description = "Creates a new database."
    )]
    internal class CreateDatabaseCommand : ADatabaseCommand {

        [LegalFilePath]
        [Option("-f|--file",  "File name (physical name) of the database to create (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory. Defaults to the name of the current directory.", CommandOptionType.SingleValue)]
        public string DatabasePhysicalName { get; set; }

        [FileExists]
        [Option("-df|--df", "Path to the " + UoeDatabaseLocation.SchemaDefinitionExtension + " file containing the database schema definition. The path can be relative to the current directory.", CommandOptionType.SingleValue)]
        public string SchemaDefinitionFilePath { get; set; }

        [FileExists]
        [Option("-st|--st", "Path to the structure file (" + UoeDatabaseLocation.StructureFileExtension + " file) containing the database physical structure. The path can be relative to the current directory.", CommandOptionType.SingleValue)]
        public string StuctureFilePath { get; set; }

        [Option("-bs|--block-size", "The block-size to use when creating the database. Defaults to the default block-size for the current platform (linux or windows).", CommandOptionType.SingleValue)]
        public DatabaseBlockSize BlockSize { get; } = DatabaseBlockSize.DefaultForCurrentPlatform;

        [Option("-lg|--lang", "The codepage/lang to use. Creates the database using an empty database located in the openedge installation $DLC/prolang/(lang).", CommandOptionType.SingleValue)]
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
}
