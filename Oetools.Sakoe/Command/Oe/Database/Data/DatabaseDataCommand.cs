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

using System.ComponentModel.DataAnnotations;
using DotUtilities;
using DotUtilities.Process;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "data", "da",
        Description = "Operate on database data."
    )]
    [Subcommand(typeof(DatabaseDataDumpCommand))]
    [Subcommand(typeof(DatabaseDataDumpSequenceCommand))]
    [Subcommand(typeof(DatabaseDataDumpSqlCommand))]
    [Subcommand(typeof(DatabaseDataDumpBinaryCommand))]
    [Subcommand(typeof(DatabaseDataLoadCommand))]
    [Subcommand(typeof(DatabaseDataLoadSequenceCommand))]
    [Subcommand(typeof(DatabaseDataLoadSqlCommand))]
    [Subcommand(typeof(DatabaseDataLoadBinaryCommand))]
    [Subcommand(typeof(DatabaseDataLoadBulkCommand))]
    [Subcommand(typeof(DatabaseDataTruncateTableCommand))]
    internal class DatabaseDataCommand : ABaseParentCommand {
    }

    [Command(
        "dump", "du",
        Description = "Dump the database data in plain text files (" + UoeDatabaseLocation.ProgressDumpFileExtention + ")."
    )]
    internal class DatabaseDataDumpCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-directory>", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual " + UoeDatabaseLocation.ProgressDumpFileExtention + " file named like the table.")]
        public string DumpDirectoryPath { get; set; }


        [Option("-t|--table <tables>", "A list of comma separated table names to dump. Defaults to `ALL` which dumps every table.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpData(GetSingleDatabaseConnection(), DumpDirectoryPath, TableName);
            }
            return 0;
        }
    }

    [Command(
        "dump-sequence", "dq",
        Description = "Dump the database sequence data in a plain text file (" + UoeDatabaseLocation.ProgressDumpFileExtention + ").",
        ExtendedHelpText = "The data of each sequence of the database is dumped in the output file."
    )]
    internal class DatabaseDataDumpSequenceCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "File path that will contain the data dumped (usually has the " + UoeDatabaseLocation.ProgressDumpFileExtention + " extension).")]
        public string DumpFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSequenceData(GetSingleDatabaseConnection(), DumpFilePath);
            }
            return 0;
        }
    }

    [Command(
        "dump-sql", "ds",
        Description = "Dump data in SQL-92 format from a database."
    )]
    internal class DatabaseDataDumpSqlCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-directory>", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual " + UoeDatabaseLocation.SqlDataExtension + " file named like the table.")]
        public string DumpDirectoryPath { get; set; }

        [Option("-op|--options <args>", @"Use options for the sqldump utility (see the documentation online). Defaults to dumping every table.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-t %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSqlData(GetSingleDatabaseConnection(), DumpDirectoryPath, new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "dump-binary", "db",
        Description = "Dump data in binary format from a database."
    )]
    internal class DatabaseDataDumpBinaryCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-directory>", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual " + UoeDatabaseLocation.BinaryDataExtension + " file named like the table.")]
        public string DumpDirectoryPath { get; set; }

        [Option("-t|--table <tables>", "A list of comma separated table names to dump. Defaults to dumping every table.", CommandOptionType.SingleValue)]
        public string TableName { get; set; }

        [Option("-op|--options <args>", @"Use options for the proutil dump utility (see the documentation online).", CommandOptionType.SingleValue)]
        public string Options { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            // TODO: handle the comma separated list of tables
            GetOperator().DumpBinaryData(GetSingleDatabaseLocation(), TableName, DumpDirectoryPath, new ProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters), new ProcessArgs().AppendFromQuotedArgs(Options));
            return 0;
        }
    }

    [Command(
        "load", "lo",
        Description = "Load the database data from plain text files (" + UoeDatabaseLocation.ProgressDumpFileExtention + ")."
    )]
    internal class DatabaseDataLoadCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [DirectoryExists]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual " + UoeDatabaseLocation.ProgressDumpFileExtention + " file named like the table.")]
        public string LoadDirectoryPath { get; set; }

        [Option("-t|--table <tables>", "A list of comma separated table names to load. Defaults to `ALL` which loads every table.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadData(GetSingleDatabaseConnection(), LoadDirectoryPath, TableName);
            }
            return 0;
        }
    }

    [Command(
        "load-sequence", "lq",
        Description = "Load the database sequence data from a plain text file (" + UoeDatabaseLocation.ProgressDumpFileExtention + ")."
    )]
    internal class DatabaseDataLoadSequenceCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [FileExists]
        [Argument(0, "<data-file>", "File path that contains the sequence data to load (usually has the " + UoeDatabaseLocation.ProgressDumpFileExtention + " extension).")]
        public string LoadFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSequenceData(GetSingleDatabaseConnection(), LoadFilePath);
            }
            return 0;
        }
    }

    [Command(
        "load-sql", "ls",
        Description = "Load the database data from SQL-92 format files (" + UoeDatabaseLocation.SqlDataExtension + ")."
    )]
    internal class DatabaseDataLoadSqlCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [DirectoryExists]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual " + UoeDatabaseLocation.SqlDataExtension + " file named like the `owner.table` (e.g. PUB.table1).")]
        public string LoadDirectoryPath { get; set; }

        [Option("-op|--options <args>", @"Use options for the sqlload utility (see the documentation online). Defaults to loading every table.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-t %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSqlData(GetSingleDatabaseConnection(), LoadDirectoryPath, new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "load-binary", "lb",
        Description = "Load the database data from binary format files."
    )]
    internal class DatabaseDataLoadBinaryCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Required]
        [DirectoryExists]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual " + UoeDatabaseLocation.BinaryDataExtension + " file named like the table.")]
        public string LoadDirectoryPath { get; set; }

        [Option("-op|--options <args>", @"Use options for the proutil load utility (see the documentation online).", CommandOptionType.SingleValue)]
        public string Options { get; set; }

        [Option("-nr|--no-rebuild", "Do not rebuild indexes when loading the data.", CommandOptionType.NoValue)]
        public bool NoIndexRebuild { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var ope = GetOperator();
            foreach (var dumpFile in Utils.EnumerateAllFiles(LoadDirectoryPath)) {
                ope.LoadBinaryData(GetSingleDatabaseLocation(), dumpFile, !NoIndexRebuild, new ProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters), new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "load-bulk", "lk",
        Description = "Load the database data from plain text files (.d)."
    )]
    internal class DatabaseDataLoadBulkCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Required]
        [DirectoryExists]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual " + UoeDatabaseLocation.BinaryDataExtension + " file named like the table.")]
        public string LoadDirectoryPath { get; set; }

        [Option("-op|--options <args>", @"Use options for the proutil load utility (see the documentation online).", CommandOptionType.SingleValue)]
        public string Options { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            //TODO: fix this
            GetOperator().BulkLoad(GetSingleDatabaseLocation(), null, LoadDirectoryPath, new ProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters), new ProcessArgs().AppendFromQuotedArgs(Options));
            return 0;
        }
    }

    [Command(
        "truncate", "tc",
        Description = "Deletes all the data of a given table."
    )]
    internal class DatabaseDataTruncateTableCommand : ADatabaseSingleLocationCommand {

        [Required]
        [Argument(0, "<tables>", "A list of comma separated table names to truncate.")]
        public string TableNames { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                var connection = GetSingleDatabaseConnection(true);
                foreach (var tableName in TableNames.Split(',')) {
                    ope.TruncateTableData(connection, tableName);
                }
            }
            return 0;
        }
    }
}
