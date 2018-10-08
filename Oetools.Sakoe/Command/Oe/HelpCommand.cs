using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "he",
        Description = "Help for the use of this tool",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(FiltersHelpCommand))]
    internal class HelpCommand : OeBaseCommand {
        public const string Name = "help";
    }
    
    
    [Command(
        Name, "fi",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class FiltersHelpCommand : OeBaseCommand {
        public const string Name = "filters";
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteLineOutput("Write something about the filters");
            return 0;
        }
    }
    
}