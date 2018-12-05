using System;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "man", "ma",
        Description = "The manual of this tool. Learn",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(FiltersHelpCommand))]
    [Subcommand(typeof(BuildManCommand))]
    internal class ManCommand : ABaseCommand {
        
        public const string Name = "manual";
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteOnNewLine(@"USAGE
  - You can escape white spaces in argument and option values by using double quotes (i.e. "")
  - In the 'USAGE' help, arguments between brackets (i.e. []) are optionals

EXIT CODE
  The convention followed by this tool is the following.
  0 : used when a command completed successfully, without errors nor warnings.
  1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  9 : used when a command does not complete and ends up in error.

RESPONSE FILE PARSING
   Instead of using a long command line, you can use a response file that contains each argument/option that should be used.
   Everything that is usually separated by a space in the command line should be separated by a new line in the file.
   In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.
   sakoe @responsefile.txt

WEBSITE 
  https://jcaillon.github.io/Oetools.Sakoe/", padding: 1);
            Out.WriteOnNewLine(null);
            Out.WriteOnNewLine("Topics :");
            foreach (var command in app.Commands) {
                Out.WriteOnNewLine($"  {command.Name} {command.Description}.");
            }
            
            return 0;
        }
    }
    
    [Command(
        Name, "fi",
        Description = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class FiltersHelpCommand : ABaseCommand {
        public const string Name = "filters";
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine("Write something about the filters.");
            return 0;
        }
    }
    
    [Command(
        "build", "bu",
        Description = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class BuildManCommand : ABaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine("Write something about the build.");
            return 0;
        }
    }
    
}