using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "structure", "st",
        Description = "Operations involving a database structure file (" + UoeDatabaseLocation.StructureFileExtension + ")."
    )]
    [Subcommand(typeof(ValidateStructureDatabaseCommand))]
    internal class StructureDatabaseCommand : AExpectSubCommand {
    }

    [Command(
        "validate", "va",
        Description = "Validate the structure file (" + UoeDatabaseLocation.StructureFileExtension + ") of a database.",
        ExtendedHelpText = "Update the database control information, usually done after an extent has been moved or renamed."
    )]
    internal class ValidateStructureDatabaseCommand : ADatabaseSingleLocationCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<st-file>", "Path to the structure file (" + UoeDatabaseLocation.StructureFileExtension + ") to validate against the given database.")]
        public string StructureFilePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().ValidateStructureFile(GetSingleDatabaseLocation(), StructureFilePath);
            return 0;
        }
    }
}
