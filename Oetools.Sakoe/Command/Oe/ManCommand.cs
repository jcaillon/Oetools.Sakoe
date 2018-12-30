using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Project.Properties;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "man", "ma",
        Description = "The manual of this tool. Learn about its usage and about key concepts of sakoe."
    )]
    [Subcommand(typeof(ListAllCommandsManCommand))]
    [Subcommand(typeof(BuildManCommand))]
    [Subcommand(typeof(CompleteManCommand))]
    [Subcommand(typeof(MarkdownManCommand))]
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class ManCommand {
        
        public const string Name = "manual";
        
        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("WHAT IS THIS TOOL");
            formatter.WriteOnNewLine(app.Parent?.Description);

            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("ABOUT THIS MANUAL");
            formatter.WriteOnNewLine(@"The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, don't be afraid to use the " + MainCommand.HelpLongName.PrettyQuote() + " option.");
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("COMMAND LINE USAGE");
            formatter.WriteOnNewLine(@"How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. ""my value"").
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. ""my """"special"""" value"").
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.");
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("RESPONSE FILE PARSING");
            formatter.WriteOnNewLine(@"Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.

Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.

  `sakoe @responsefile.txt`");
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("EXIT CODE");
            formatter.WriteOnNewLine(@"The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.");
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("WEBSITE");
            formatter.WriteOnNewLine(@"The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

If you want to help, you are welcome to contribute to the github repo. 
You are invited to STAR the project on github to increase its visibility!");

            if (app.Commands != null && app.Commands.Count > 0) {
                formatter.WriteOnNewLine(null);
                formatter.WriteSectionTitle("LEARN MORE");
                formatter.WriteOnNewLine("Learn more about specific topics using the command:");
                formatter.WriteOnNewLine(null);
                formatter.WriteOnNewLine($"{app.GetFullCommandLine()} <TOPIC>".PrettyQuote());

                formatter.WriteOnNewLine(null);
                formatter.WriteSectionTitle("TOPICS");
                foreach (var command in app.Commands.ToList().OrderBy(c => c.Name)) {
                    formatter.WriteOnNewLine(command.Name.PadRight(30));
                    formatter.Write(command.Description, padding: 30);
                }
            }

            formatter.WriteOnNewLine(null);
        }
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            GetAdditionalHelpText(HelpGenerator.Singleton, app, 0);
            return 0;
        }
        
    }
    
    [Command(
        "list", "li",
        Description = "List all the commands of this tool."
    )]
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class ListAllCommandsManCommand {
        
        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("LIST OF ALL THE COMMANDS");
            var rootCommand = app;
            while (rootCommand.Parent != null) {
                rootCommand = rootCommand.Parent;
            }
            formatter.WriteOnNewLine(rootCommand.Name);
            ListCommands(formatter, rootCommand.Commands, "");
            formatter.WriteOnNewLine(null);
        }

        private static void ListCommands(IHelpFormatter formatter, List<CommandLineApplication> subCommands, string linePrefix) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                formatter.WriteOnNewLine($"{linePrefix}{(i == subCommands.Count - 1 ? "└─ " : "├─ ")}{subCommand.Name}".PadRight(30));
                var linePrefixForNewLine = $"{linePrefix}{(i == subCommands.Count - 1 ? "   " : "│  ")}";
                formatter.Write(subCommand.Description, padding: 30, prefixForNewLines: linePrefixForNewLine);
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    ListCommands(formatter, subCommand.Commands, linePrefixForNewLine);
                }
                i++;
            }
        }
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            GetAdditionalHelpText(HelpGenerator.Singleton, app, 0);
            return 0;
        }
    }
    
    [Command(
        Name, "co",
        Description = "Print the help of each command of this tool ."
    )]
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class CompleteManCommand {
        
        public const string Name = "complete";
        
        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            app.Parent.Commands.Remove(app);
            var rootCommand = app;
            while (rootCommand.Parent != null) {
                rootCommand = rootCommand.Parent;
            }
            ListCommands(formatter, rootCommand.Commands);
            formatter.WriteOnNewLine(null);
        }

        private static void ListCommands(IHelpFormatter formatter, List<CommandLineApplication> subCommands) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                formatter.WriteSectionTitle("================");
                formatter.WriteSectionTitle(subCommand.GetFullCommandLine());
                formatter.WriteSectionTitle("================");
                subCommand.ShowHelp();
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    ListCommands(formatter, subCommand.Commands);
                }
                i++;
            }
        }
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            GetAdditionalHelpText(HelpGenerator.Singleton, app, 0);
            return 0;
        }
    }
    
    
    [Command(
        "markdown-file", "mf",
        Description = "Print the documentation of this tool in a markdown file."
    )]
    internal class MarkdownManCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "<file>", "The file in which to print this markdown manual.")]
        public string OutputFile { get; set; }
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            app.Parent.Commands.Clear();
            
            using (var stream = new FileStream(OutputFile, FileMode.Create)) {
                using (var writer = new StreamWriter(stream, Encoding.UTF8)) {
                    writer.WriteLine("# SAKOE");
                    writer.WriteLine();
                    
                    var rootCommand = app;
                    while (rootCommand.Parent != null) {
                        rootCommand = rootCommand.Parent;
                    }

                    var mdGenerator = new MarkdownHelpGenerator(writer);
                    
                    writer.WriteLine("## ABOUT");
                    writer.WriteLine();
                    ManCommand.GetAdditionalHelpText(mdGenerator, app.Parent, 0);
                    writer.WriteLine();
                    
                    writer.WriteLine("## TABLE OF CONTENT");
                    writer.WriteLine();
                    writer.WriteLine("- [Commands overview](#commands-overview)");
                    writer.WriteLine("- [About the build command](#about-the-build-command)");
                    WriteToc(writer, rootCommand.Commands, "");
                    writer.WriteLine();
                    
                    writer.WriteLine("## COMMANDS OVERVIEW");
                    writer.WriteLine();
                    writer.WriteLine("| Full command line | Short description |");
                    writer.WriteLine("| --- | --- |");
                    WriteAllCommands(writer, rootCommand.Commands);
                    writer.WriteLine();
                    
                    writer.WriteLine("## ABOUT THE BUILD COMMAND");
                    writer.WriteLine();
                    BuildManCommand.GetAdditionalHelpText(mdGenerator, app.Parent, 0);
                    writer.WriteLine();
                    
                    ListCommands(mdGenerator, rootCommand.Commands);
                }
            }
            return 0;
        }
        
        private static void WriteToc(StreamWriter writer, List<CommandLineApplication> subCommands, string linePrefix) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                writer.WriteLine($"{linePrefix}- [{subCommand.Name}](#{subCommand.GetFullCommandLine().Replace(" ", "-")})");
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    WriteToc(writer, subCommand.Commands, $"{linePrefix}  ");
                }
                i++;
            }
        }
        
        private static void WriteAllCommands(StreamWriter writer, List<CommandLineApplication> subCommands) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                writer.WriteLine($"| [{subCommand.GetFullCommandLine()}](#{subCommand.GetFullCommandLine().Replace(" ", "-")}) | {subCommand.Description?.Replace("\n", " ").Replace("\r", "")} |");
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    WriteAllCommands(writer, subCommand.Commands);
                }
                i++;
            }
        }
        
        private static void ListCommands(MarkdownHelpGenerator mdGenerator, List<CommandLineApplication> subCommands) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                mdGenerator.WriteOnNewLine($"## {subCommand.GetFullCommandLine().ToUpper()}");
                mdGenerator.GenerateCommandHelp(subCommand);
                mdGenerator.WriteOnNewLine("**[\\[Go back to the table of content\\].](#table-of-content)**");
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    ListCommands(mdGenerator, subCommand.Commands);
                }
                i++;
            }
        }
    }
    
    [Command(
        "build", "bu",
        Description = "What is a build and how to configure it."
    )]
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class BuildManCommand {
        
        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("OVERVIEW");
            formatter.WriteOnNewLine(@"With sakoe, you can 'build' your application. A build process is a succession of tasks that (typically) transform your source files into a deliverable format, usually called a release or package.

In sakoe, you describe a build process using a 'build configuration'. A build configuration holds 'properties' of the build (for instance, the path to the openedge installation directory $DLC). It also holds the list of 'tasks' that will be executed successively during the build process.

To illustrate this, here is a possible build process:
  - Task 1: compile all your .p files to a `procedures` directory.
  - Task 2: compile all your .w files into a pro-library `client.pl`.
  - Task 3: zip the `procedures` and `client.pl` together into an archive file `release.zip`.

In order to store these build configurations, sakoe uses project files: " + OeBuilderConstants.OeProjectExtension.PrettyQuote() + @".
You can create them with the command: " + typeof(ProjectInitCommand).GetFullCommandLine().PrettyQuote() + @".");
            formatter.WriteOnNewLine(null);
            formatter.WriteOnNewLine(OeIncrementalBuildOptions.GetDefaultEnabledIncrementalBuild() ? "By default, a build is 'incremental'." : "A build can be 'incremental'.");
            formatter.WriteOnNewLine("An incremental build is the opposite of a full build. In incremental mode, only the files that were added/modified/deleted since the previous build are taken into account. Unchanged files are simply not rebuilt.");
            formatter.WriteOnNewLine(null);
            formatter.WriteOnNewLine("The chapters below contain more details about a project, build configuration, properties and tasks. ");
            
            // TODO: list all the node and their documentation, use a tree
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("PROJECT");
            formatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeProject).GetXmlName()));
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("BUILD CONFIGURATION");
            formatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeProject).GetXmlName(nameof(OeProject.BuildConfigurations))));
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("BUILD CONFIGURATION VARIABLES");
            formatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.Variables))));
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("BUILD CONFIGURATION PROPERTIES");
            formatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.Properties))));
            
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("BUILD STEPS");
            formatter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(typeof(OeBuildConfiguration).GetXmlName(nameof(OeBuildConfiguration.BuildSteps))));
            
            formatter.WriteOnNewLine(null);
        }
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            GetAdditionalHelpText(HelpGenerator.Singleton, app, 0);
            return 0;
        }
    }
    
}