using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "man", "ma",
        Description = "Read the (f*ing) manual for this tool",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(FiltersHelpCommand))]
    internal class ManCommand : OeBaseCommand {
        public const string Name = "manual";
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