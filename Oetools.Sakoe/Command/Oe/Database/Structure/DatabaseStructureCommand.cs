using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge.Database;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "structure", "st",
        Description = "Operate on a database structure file (" + UoeDatabaseLocation.StructureFileExtension + ")."
    )]
    [Subcommand(typeof(DatabaseStructureValidateCommand))]
    [Subcommand(typeof(DatabaseStructureUpdateCommand))]
    [Subcommand(typeof(DatabaseStructureAddCommand))]
    [Subcommand(typeof(DatabaseStructureRemoveCommand))]
    [Subcommand(typeof(DatabaseStructureGenerateCommand))]
    internal class DatabaseStructureCommand : ABaseParentCommand {
    }

    [Command(
        "validate", "va",
        Description = "Validate a structure file (" + UoeDatabaseLocation.StructureFileExtension + ") against a given database.",
        ExtendedHelpText = "Validates whether or not the structure is valid to use either for creation (if the database does not exist) or for an update."
    )]
    internal class DatabaseStructureValidateCommand : ADatabaseSingleLocationCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<st-file>", "Path to the structure file (" + UoeDatabaseLocation.StructureFileExtension + ") to validate against the database. The path can be relative to the current directory.")]
        public string StructureFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().ValidateStructureFile(GetSingleDatabaseLocation(), StructureFilePath);
            return 0;
        }
    }

    [Command(
        "update", "up",
        Description = "Create or update a structure file (" + UoeDatabaseLocation.StructureFileExtension + ") from the database " + UoeDatabaseLocation.Extension + " file."
    )]
    internal class DatabaseStructureUpdateCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Option("-rp|--relative-path", "Use relative path in the structure file.", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;

        [Option("-dp|--directory-path", "By default, listing will output file path to each extent, this option allows to output directory path instead.", CommandOptionType.NoValue)]
        public bool ExtentPathAsDirectories { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().UpdateStructureFile(GetSingleDatabaseLocation(), RelativePath, ExtentPathAsDirectories, new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters));
            return 0;
        }
    }

    [Command(
        "add", "ad",
        Description = "Append the extents from a structure file (" + UoeDatabaseLocation.StructureFileExtension + ") to a database."
    )]
    internal class DatabaseStructureAddCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<st-file>", "Path to the structure file (" + UoeDatabaseLocation.StructureFileExtension + ") to add. The path can be relative to the current directory.")]
        public string StructureFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().AddStructureDefinition(GetSingleDatabaseLocation(), StructureFilePath, false, new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters));
            return 0;
        }
    }

    [Command(
        "remove", "rm",
        Description = "Remove storage areas or extents."
    )]
    internal class DatabaseStructureRemoveCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Required]
        [Argument(0, "<extent-token>", "Indicates the type of extent to remove. Specify one of the following: d, bi, ai, tl.")]
        public string ExtentToken { get; set; }

        [Required]
        [Argument(1, "<storage-area>", "Specifies the name of the storage area to remove.")]
        public string StorageArea { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().RemoveStructureDefinition(GetSingleDatabaseLocation(), ExtentToken, StorageArea, new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters));
            return 0;
        }
    }

    [Command(
        "generate", "ge",
        Description = "Generate the structure file (" + UoeDatabaseLocation.StructureFileExtension + ") from a definition file (" + UoeDatabaseLocation.SchemaDefinitionExtension + ").",
        ExtendedHelpText = "Create all the needed AREA found in the given schema definition file (" + UoeDatabaseLocation.SchemaDefinitionExtension + ")."
    )]
    internal class DatabaseStructureGenerateCommand : ADatabaseSingleLocationCommand {

        [Required]
        [FileExists]
        [Argument(0, "<df-file>", "Path to the " + UoeDatabaseLocation.SchemaDefinitionExtension + " file containing the database schema definition. The path can be relative to the current directory.")]
        public string SchemaDefinitionFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().GenerateStructureFileFromDf(GetSingleDatabaseLocation(), SchemaDefinitionFilePath);
            return 0;
        }
    }


}
