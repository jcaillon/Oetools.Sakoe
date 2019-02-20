using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "dump", "du",
        Description = "Dump data or schema definition from a database."
    )]
    [Subcommand(typeof(DumpDfDatabaseCommand))]
    [Subcommand(typeof(DumpSqlDatabaseCommand))]
    [Subcommand(typeof(DumpSqlDataDatabaseCommand))]
    [Subcommand(typeof(DumpIncrementalDfDatabaseCommand))]
    [Subcommand(typeof(DumpIncrementalDfFromDfDatabaseCommand))]
    [Subcommand(typeof(DumpDataDatabaseCommand))]
    [Subcommand(typeof(DumpSeqDatabaseCommand))]
    //TODO: binary dump
    internal class DumpDatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "seq", "se",
        Description = "Dump the database sequence values in a plain text file (.d)."
    )]
    internal class DumpSeqDatabaseCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "File path that will contain the data dumped (usually has the .d extension).")]
        public string DumpFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSequenceData(GetSingleDatabaseConnection(), DumpFilePath);
            }
            return 0;
        }
    }

    [Command(
        "schema", "df",
        Description = "Dump the schema definition (" + UoeDatabaseLocation.SchemaDefinitionExtension + ") of a database."
    )]
    internal class DumpDfDatabaseCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "Path to the output " + UoeDatabaseLocation.SchemaDefinitionExtension + " file that will contain the schema definition of the database (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-t|--table", "A list of comma separated table names for which we need a schema definition dump. Defaults to `ALL` which dumps every table and sequence.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSchemaDefinition(GetSingleDatabaseConnection(), DumpFilePath.AddFileExtention(UoeDatabaseLocation.SchemaDefinitionExtension), TableName);
            }
            return 0;
        }
    }

    [Command(
        "sql-schema", "ss",
        Description = "Dump the SQL-92 schema of a database."
    )]
    internal class DumpSqlDatabaseCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-file>", "Path to the output " + UoeDatabaseLocation.SqlSchemaDefinitionExtension + " file that will contain the sql-92 definition of the database (extension is optional).")]
        public string DumpFilePath { get; set; }

        [Option("-op|--options", @"Use options for the sqlschema utility (see the documentation online). Defaults to dumping everything.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-f %.% -g %.% -G %.% -n %.% -p %.% -q %.% -Q %.% -s %.% -t %.% -T %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSqlSchema(GetSingleDatabaseConnection(), DumpFilePath.AddFileExtention(UoeDatabaseLocation.SqlSchemaDefinitionExtension), new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "sql-data", "sd",
        Description = "Dump data in SQL-92 format from a database."
    )]
    internal class DumpSqlDataDatabaseCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-directory>", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual " + UoeDatabaseLocation.SqlDataExtension + " file named like the table.")]
        public string DumpDirectoryPath { get; set; }

        [Option("-op|--options", @"Use options for the sqldump utility (see the documentation online). Defaults to dumping every table.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-t %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpSqlData(GetSingleDatabaseConnection(), DumpDirectoryPath, new ProcessArgs().AppendFromQuotedArgs(Options));
            }
            return 0;
        }
    }

    [Command(
        "incremental", "inc", "in",
        Description = "Dump an incremental schema definition (.df) allowing to update a database schema definition.",
        ExtendedHelpText = "Two databases schema definition are compared and the difference is written in a 'delta' `.df`. This `.df` file then allows to upgrade an older database schema to the new schema."
    )]
    internal class DumpIncrementalDfDatabaseCommand : ADatabaseCommand {

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

        [Option("-rf|--rename-file", @"Path to the 'rename file'.
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
        "incremental-df", "id",
        Description = "Dump an incremental schema definition (.df) by comparing .df files."
    )]
    internal class DumpIncrementalDfFromDfDatabaseCommand : ADatabaseCommand {

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

        [Option("-rf|--rename-file", @"Path to the 'rename file'.
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

    [Command(
        "data", "da",
        Description = "Dump the database data in plain text files (.d)."
    )]
    internal class DumpDataDatabaseCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<dump-directory>", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .d file named like the table.")]
        public string DumpDirectoryPath { get; set; }


        [Option("-t|--table", "A list of comma separated table names to dump. Defaults to `ALL` which dumps every table (extension is optional).", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.DumpData(GetSingleDatabaseConnection(), DumpDirectoryPath, TableName);
            }
            return 0;
        }
    }
}
