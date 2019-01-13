using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {

    internal abstract class ADatabaseCommand : AOeDlcCommand {

        private const string DbPathArgumentName = "<db path>";

        [LegalFilePath]
        [Argument(0, "[" + DbPathArgumentName + "]", "Path to the database. The .db extension is optional. Defaults to the first " + UoeConstants.DatabaseFileExtension + " file found in the current directory.")]
        public string TargetDatabasePath { get; set; }

        protected void SetTargetDatabasePath() {
            TargetDatabasePath = TargetDatabasePath?.MakePathAbsolute();
            if (string.IsNullOrEmpty(TargetDatabasePath)) {
                var list = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{UoeConstants.DatabaseFileExtension}", SearchOption.TopDirectoryOnly).ToList();
                if (list.Count == 0) {
                    throw new CommandException($"No database file {UoeConstants.DatabaseFileExtension.PrettyQuote()} found in the current folder: {Directory.GetCurrentDirectory().PrettyQuote()}. Initialize a new database using the command: {typeof(CreateDatabaseCommand).GetFullCommandLine().PrettyQuote()}.");
                }
                if (list.Count > 1) {
                    throw new CommandException($"Ambiguous database, found {list.Count} databases in the current folder, specify the database to use in the command line with the argument {DbPathArgumentName.PrettyQuote()}.");
                }
                TargetDatabasePath = list.First();
            }
        }
    }

    internal abstract class ADatabaseCsCommand : ADatabaseCommand {


    }
}
