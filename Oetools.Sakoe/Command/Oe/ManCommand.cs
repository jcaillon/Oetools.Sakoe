using System;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "man", "ma",
        Description = "Read the (f*ing) manual for this tool",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(FiltersHelpCommand))]
    internal class ManCommand : BaseCommand {
        public const string Name = "manual";
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine("Write the manual.");
            Out.WriteResultOnNewLine(null);
            Out.WriteResultOnNewLine("Topics :");
            foreach (var command in app.Commands) {
                Out.WriteResultOnNewLine($"  {command.Name} {command.Description}.");
            }
            
            return 0;
        }
    }
    
    [Command(
        Name, "fi",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class FiltersHelpCommand : BaseCommand {
        public const string Name = "filters";
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine("Write something about the filters.");
            return 0;
        }
    }
    
}