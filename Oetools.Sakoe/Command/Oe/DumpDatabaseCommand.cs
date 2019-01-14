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
    internal class DumpSeqDatabaseCommand : ADatabaseWithProwinCommand {

        [Required]
        [LegalFilePath]
        [Option("-d|--dump-file", "File path that will contain the data dumped.", CommandOptionType.SingleValue)]
        public string DumpFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dlcPath = GetDlcPath();
            using (var dbAdministrator = new UoeDatabaseAdministrator(dlcPath)) {
                dbAdministrator.Log = Log;
                dbAdministrator.DumpSequenceData(GetConnectionString(dlcPath, app), DumpFilePath);
            }
            return 0;
        }
    }

    [Command(
        "data", "da",
        Description = "Dump the database data in plain text files (.d)."
    )]
    internal class DumpDataDatabaseCommand : ADatabaseWithProwinCommand {

        [Required]
        [LegalFilePath]
        [Option("-d|--dump-directory", "Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .d file named like the table.", CommandOptionType.SingleValue)]
        public string DumpFilePath { get; set; }

        [Option("-t|--table", "A list of comma separated table names to dump. Defaults to `ALL` which dumps every table.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dlcPath = GetDlcPath();
            using (var dbAdministrator = new UoeDatabaseAdministrator(dlcPath)) {
                dbAdministrator.Log = Log;
                dbAdministrator.DumpData(GetConnectionString(dlcPath, app), DumpFilePath, TableName);
            }
            return 0;
        }
    }

    [Command(
        "schema", "df",
        Description = "Dump the schema definition (.df) of a database."
    )]
    internal class DumpDfDatabaseCommand : ADatabaseWithProwinCommand {

        [Required]
        [LegalFilePath]
        [Option("-df|--df", "Path to the output .df file that will contain the schema definition of the database.", CommandOptionType.SingleValue)]
        public string DumpFilePath { get; set; }

        [Option("-t|--table", "A list of comma separated table names for which we need a schema definition dump. Defaults to `ALL` which dumps every table and sequence.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dlcPath = GetDlcPath();
            using (var dbAdministrator = new UoeDatabaseAdministrator(dlcPath)) {
                dbAdministrator.Log = Log;
                dbAdministrator.DumpSchemaDefinition(GetConnectionString(dlcPath, app), DumpFilePath, TableName);
            }
            return 0;
        }
    }

    [Command(
        "incremental", "inc", "in",
        Description = "Dump an incremental schema definition (.df) allowing to update a database schema definition.",
        ExtendedHelpText = "Two databases schema definition are compared and the difference is written in a 'delta' `.df`. This `.df` file then allows to upgrade an older database schema to the new schema."
    )]
    internal class DumpIncrementalDfDatabaseCommand : AOeDlcCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<old db path>", "The path to the 'old' database. The .db extension is optional.")]
        public string OldDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Argument(1, "<new db path>", "The path to the 'new' database. The .db extension is optional.")]
        public string NewDatabasePath { get; set; }

        [Required]
        [LegalFilePath]
        [Option("-df|--df", "Path to the output incremental .df file.", CommandOptionType.SingleValue)]
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
            OldDatabasePath = OldDatabasePath?.MakePathAbsolute();
            NewDatabasePath = NewDatabasePath?.MakePathAbsolute();
            var dlcPath = GetDlcPath();
            using (var dbAdministrator = new UoeDatabaseAdministrator(dlcPath)) {
                dbAdministrator.Log = Log;
                dbAdministrator.DumpIncrementalSchemaDefinitionFromDatabases($"{dbAdministrator.GetConnectionString(NewDatabasePath)} {dbAdministrator.GetConnectionString(OldDatabasePath)}", DumpFilePath, RenameFilePath);
            }
            return 0;
        }
    }

    [Command(
        "incremental-df", "id",
        Description = "Dump an incremental schema definition (.df) by comparing .df files."
    )]
    internal class DumpIncrementalDfFromDfDatabaseCommand : AOeDlcCommand {

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
        [Option("-df|--df", "Path to the output incremental .df file.", CommandOptionType.SingleValue)]
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
            OldDfPath = OldDfPath?.MakePathAbsolute();
            NewDfPath = NewDfPath?.MakePathAbsolute();
            using (var dbAdministrator = new UoeDatabaseAdministrator(GetDlcPath())) {
                dbAdministrator.Log = Log;
                dbAdministrator.DumpIncrementalSchemaDefinition(OldDfPath, NewDfPath, DumpFilePath, RenameFilePath);
            }
            return 0;
        }
    }
}
