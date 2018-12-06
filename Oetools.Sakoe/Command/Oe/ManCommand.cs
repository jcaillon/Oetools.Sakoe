using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "man", "ma",
        Description = "The manual of this tool. Learn about its usage and about key concepts of sakoe."
    )]
    [Subcommand(typeof(FiltersHelpCommand))]
    [Subcommand(typeof(BuildManCommand))]
    [Subcommand(typeof(ListAllCommandsManCommand))]
    internal class ManCommand : ABaseCommand {
        
        public const string Name = "manual";
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("WHAT IS THIS TOOL");
            HelpFormatter.WriteOnNewLine(app.Parent?.Description);

            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("ABOUT THIS MANUAL");
            HelpFormatter.WriteOnNewLine(@"The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, use the " + MainCommand.HelpLongName + " option abundantly.");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("COMMAND LINE USAGE");
            HelpFormatter.WriteOnNewLine(@"How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. ""my value"")
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. ""my """"special"""" value"")
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("RESPONSE FILE PARSING");
            HelpFormatter.WriteOnNewLine(@"Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.
Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.
  sakoe @responsefile.txt");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("EXIT CODE");
            HelpFormatter.WriteOnNewLine(@"The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("WEBSITE");
            HelpFormatter.WriteOnNewLine(@"The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/

If you want to help, you are welcome to contribute to the github repo. 
You are invited to STAR the project on github to increase its visibility!");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("LEARN MORE");
            HelpFormatter.WriteOnNewLine("Learn more about specific topics using the command:");
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteOnNewLine($"{app.GetFullCommandLine()} <TOPIC>");

            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("TOPICS");
            foreach (var command in app.Commands.ToList().OrderBy(c => c.Name)) {
                HelpFormatter.WriteOnNewLine(command.Name.PadRight(30));
                HelpFormatter.Write(command.Description, padding: 30);
            }
            
            HelpFormatter.WriteOnNewLine(null);
            return 0;
        }
    }
    
    [Command(
        Name, "fi",
        Description = "How to use filters."
    )]
    internal class FiltersHelpCommand : ABaseCommand {
        public const string Name = "filters";
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            HelpFormatter.WriteOnNewLine(null);
            Out.WriteResultOnNewLine("Write something about the filters.");
            HelpFormatter.WriteOnNewLine(null);
            return 0;
        }
    }
    
    [Command(
        "allcmd", "all", "al",
        Description = "List all the commands of this tool."
    )]
    internal class ListAllCommandsManCommand : ABaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("LIST OF ALL THE COMMANDS");
            var rootCommand = app;
            while (rootCommand.Parent != null) {
                rootCommand = rootCommand.Parent;
            }
            HelpFormatter.WriteOnNewLine(rootCommand.Name);
            ListCommands(rootCommand.Commands, "");
            HelpFormatter.WriteOnNewLine(null);
            return 0;
        }    

        private void ListCommands(List<CommandLineApplication> subCommands, string linePrefix) {
            var i = 0;
            foreach (var subCommand in subCommands) {
                HelpFormatter.WriteOnNewLine($"{linePrefix}{(i == subCommands.Count - 1 ? "└─ " : "├─ ")}{subCommand.Name}".PadRight(30));
                var linePrefixForNewLine = $"{linePrefix}{(i == subCommands.Count - 1 ? "   " : "│  ")}";
                HelpFormatter.Write(subCommand.Description, padding: 30, prefixForNewLines: linePrefixForNewLine);
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    ListCommands(subCommand.Commands, linePrefixForNewLine);
                }
                i++;
            }
        }
    }
    
    [Command(
        "build", "bu",
        Description = "What is a build and how to configure it."
    )]
    internal class BuildManCommand : ABaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            HelpFormatter.WriteOnNewLine(null);
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("OVERVIEW");
            HelpFormatter.WriteOnNewLine(@"With sakoe, you can 'build' your application. A build process is a succession of tasks that (typically) transform your source files into a deliverable format, usually called a release or package.

In sakoe, you describe a build process using a 'build configuration'. A build configuration holds 'properties' of the build (for instance, the path to the openedge installation directory $DLC). It also holds the list of 'tasks' that will be executed successively during the build process.

To illustrate this, here is a possible build process:
  - Task 1: compile all your .p files to a 'procedures' directory.
  - Task 2: compile all your .w files into a pro-library 'client.pl'.
  - Task 3: zip the 'procedures' and 'client.pl' together into an archive file 'release.zip'.

In order to store these build configurations, sakoe uses project files: " + OeBuilderConstants.OeProjectExtension + @". You can create them with the command: " + typeof(ProjectInitCommand).GetFullCommandLine().PrettyQuote() + @".

The chapters below contain more details about a project, build configuration, properties and tasks. 
");
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("PROJECT");
            HelpFormatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeProject).GetXmlName()));
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("BUILD CONFIGURATION");
            HelpFormatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeProject).GetXmlName(nameof(OeProject.BuildConfigurations))));
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("BUILD CONFIGURATION VARIABLES");
            HelpFormatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.Variables))));
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("BUILD CONFIGURATION PROPERTIES");
            HelpFormatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.Properties))));
            
            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("BUILD STEPS");
            HelpFormatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.BuildSteps))));
            
            HelpFormatter.WriteOnNewLine(null);
            return 0;
        }
    }
    
}