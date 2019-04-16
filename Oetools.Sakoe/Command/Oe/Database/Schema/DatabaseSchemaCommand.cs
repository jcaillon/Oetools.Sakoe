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
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "schema", "sc",
        Description = "Operate on database schema."
    )]
    [Subcommand(typeof(DatabaseSchemaLoadCommand))]
    [Subcommand(typeof(DatabaseSchemaDumpCommand))]
    [Subcommand(typeof(DatabaseSchemaDumpCustomCommand))]
    [Subcommand(typeof(DatabaseSchemaDumpSqlCommand))]
    [Subcommand(typeof(DatabaseSchemaDumpIncrementalCommand))]
    [Subcommand(typeof(DatabaseSchemaDumpIncrementalFromDfCommand))]
    internal class DatabaseSchemaCommand : AExpectSubCommand {
    }

    [Command(
        "load", "lo",
        Description = "Load a schema definition (.df) to a database."
    )]
    internal class DatabaseSchemaLoadCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [FileExists]
        [Argument(0, "<df-file>", "Path to the " + UoeDatabaseLocation.SchemaDefinitionExtension + " file that contains the schema definition (or partial schema) of the database.")]
        public string LoadFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSchemaDefinition(GetSingleDatabaseConnection(), LoadFilePath);
            }
            return 0;
        }
    }

    [Command(
        "dump", "du",
        Description = "Dump the schema definition (" + UoeDatabaseLocation.SchemaDefinitionExtension + ") of a database."
    )]
    internal class DatabaseSchemaDumpCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "Path to the output " + UoeDatabaseLocation.SchemaDefinitionExtension + " file that will contain the schema definition of the database (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-t|--table <tables>", "A list of comma separated table names for which we need a schema definition dump. Defaults to `ALL` which dumps every table and sequence.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSchemaDefinition(GetSingleDatabaseConnection(), DumpFilePath.AddFileExtention(UoeDatabaseLocation.SchemaDefinitionExtension), TableName);
            }
            return 0;
        }
    }

    [Command(
        "dump-custom", "dc",
        Description = "Dump the schema definition in a custom format."
    )]
    internal class DatabaseSchemaDumpCustomCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "Path to the output " + UoeDatabaseLocation.SchemaDefinitionExtension + " file that will contain the schema definition of the database.")]
        public string DumpFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.GetDatabaseDefinition(GetSingleDatabaseConnection().Yield(), DumpFilePath);
            }
            return 0;
        }
    }

    [Command(
        "dump-sql", "ds",
        Description = "Dump the SQL-92 schema of a database."
    )]
    internal class DatabaseSchemaDumpSqlCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "Path to the output " + UoeDatabaseLocation.SqlSchemaDefinitionExtension + " file that will contain the sql-92 definition of the database (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-op|--options <args>", @"Use options for the sqlschema utility (see the documentation online). Defaults to dumping everything.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-f %.% -g %.% -G %.% -n %.% -p %.% -q %.% -Q %.% -s %.% -t %.% -T %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSqlSchema(GetSingleDatabaseConnection(), DumpFilePath.AddFileExtention(UoeDatabaseLocation.SqlSchemaDefinitionExtension), new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "dump-inc", "di",
        Description = "Dump an incremental schema definition (.df) by comparing 2 databases.",
        ExtendedHelpText = "Two databases schema definition are compared and the difference is written in a 'delta' `.df`. This `.df` file then allows to upgrade an older database schema to the new schema."
    )]
    internal class DatabaseSchemaDumpIncrementalCommand : ADatabaseCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<old-db-path>", "The path to the 'old' database. The .db extension is optional.")]
        public string OldDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(1, "<new-db-path>", "The path to the 'new' database. The .db extension is optional.")]
        public string NewDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(2, "<dump-file>", "Path to the output incremental " + UoeDatabaseLocation.SchemaDefinitionExtension + " file (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-rf|--rename-file <file>", @"Path to the 'rename file'.
It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.

The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT):
- T,old-table-name,new-table-name
- F,table-name,old-field-name,new-field-name
- S,old-sequence-name,new-sequence-name

Missing entries or entries with an empty new name are considered to have been deleted.
", CommandOptionType.SingleValue)]
        [FileExists]
        public string RenameFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpIncrementalSchemaDefinitionFromDatabases(new [] { ope.GetDatabaseConnection(new UoeDatabaseLocation(NewDatabasePath)), ope.GetDatabaseConnection(new UoeDatabaseLocation(OldDatabasePath)) }, DumpFilePath.AddFileExtention(UoeDatabaseLocation.SchemaDefinitionExtension), RenameFilePath);
            }
            return 0;
        }
    }

    [Command(
        "dump-inc-df", "dd",
        Description = "Dump an incremental schema definition (.df) by comparing 2 .df files."
    )]
    internal class DatabaseSchemaDumpIncrementalFromDfCommand : ADatabaseCommand {

        [Required]
        [FileExists]
        [Argument(0, "<old df path>", "The path to the 'old' database.")]
        public string OldDfPath { get; set; }

        [Required]
        [FileExists]
        [Argument(1, "<new df path>", "The path to the 'new' database.")]
        public string NewDfPath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(2, "<dump-file>", "Path to the output incremental " + UoeDatabaseLocation.SchemaDefinitionExtension + " file (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-rf|--rename-file <file>", @"Path to the 'rename file'.
It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.

The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT):
- T,old-table-name,new-table-name
- F,table-name,old-field-name,new-field-name
- S,old-sequence-name,new-sequence-name

Missing entries or entries with an empty new name are considered to have been deleted.
", CommandOptionType.SingleValue)]
        [FileExists]
        public string RenameFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpIncrementalSchemaDefinition(OldDfPath?.ToAbsolutePath(), NewDfPath?.ToAbsolutePath(), DumpFilePath.AddFileExtention(UoeDatabaseLocation.SchemaDefinitionExtension), RenameFilePath);
            }
            return 0;
        }
    }
}
