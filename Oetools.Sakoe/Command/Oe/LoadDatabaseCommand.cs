using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {
    [Command(
        "load", "lo",
        Description = "Load data or schema definition to a database."
    )]
    [Subcommand(typeof(LoadSeqDatabaseCommand))]
    [Subcommand(typeof(LoadDataDatabaseCommand))]
    [Subcommand(typeof(LoadSqlDataDatabaseCommand))]
    [Subcommand(typeof(LoadDfDatabaseCommand))]
    internal class LoadDatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "seq", "se",
        Description = "Load the database sequence values from a plain text file (.d)."
    )]
    internal class LoadSeqDatabaseCommand : ADatabaseSingleConnectionCommand {

        [FileExists]
        [LegalFilePath]
        [Argument(0, "<data-file>", "File path that contains the sequence data to load (usually has the .d extension).")]
        public string LoadFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSequenceData(GetSingleDatabaseConnection(), LoadFilePath);
            }
            return 0;
        }
    }

    [Command(
        "data", "da",
        Description = "Load the database data from plain text files (.d)."
    )]
    internal class LoadDataDatabaseCommand : ADatabaseSingleConnectionCommand {

        [DirectoryExists]
        [LegalFilePath]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual .d file named like the table.")]
        public string LoadDirectoryPath { get; set; }

        [Option("-t|--table", "TA list of comma separated table names to load. Defaults to `ALL` which loads every table.", CommandOptionType.SingleValue)]
        public string TableName { get; set; } = "ALL";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadData(GetSingleDatabaseConnection(), LoadDirectoryPath, TableName);
            }
            return 0;
        }
    }

    [Command(
        "sql-data", "sd",
        Description = "Load the database data from SQL-92 format files (" + UoeDatabaseLocation.SqlExtension + ")."
    )]
    internal class LoadSqlDataDatabaseCommand : ADatabaseSingleConnectionCommand {

        [DirectoryExists]
        [LegalFilePath]
        [Argument(0, "<data-directory>", "Directory path that contain the data to load. Each table of the database should be stored as an individual " + UoeDatabaseLocation.SqlExtension + " file named like the `owner.table`.")]
        public string LoadDirectoryPath { get; set; }

        [Option("-op|--options", @"Use options for the sqlload utility (see the documentation online). Defaults to loading every table.", CommandOptionType.SingleValue)]
        public string Options { get; set; } = "-t %.%";

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSqlData(GetSingleDatabaseConnection(), LoadDirectoryPath, Options);
            }
            return 0;
        }
    }

    [Command(
        "schema", "df",
        Description = "Load the schema definition (.df) to a database."
    )]
    internal class LoadDfDatabaseCommand : ADatabaseSingleConnectionCommand {

        [FileExists]
        [LegalFilePath]
        [Argument(0, "<df-file>", "Path to the " + UoeDatabaseLocation.SchemaDefinitionExtension + " file that contains the schema definition (or partial schema) of the database.")]
        public string LoadFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ope.LoadSchemaDefinition(GetSingleDatabaseConnection(), LoadFilePath);
            }
            return 0;
        }
    }

}
