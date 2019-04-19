#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (DataDiggerCommand.cs) is part of Oetools.Sakoe.
//
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Project.Properties;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "command", "cm", "cmd",
        Description = "Manual for the commands of sakoe."
    )]
    [Subcommand(typeof(BuildManCommand))]
    [Subcommand(typeof(ManCommandListCommand))]
    internal class ManCommandCommand : AExpectSubCommand {
    }

    [Command(
        "build", "bu",
        Description = "The build command, what is a build and how to configure it."
    )]
    [CommandAdditionalHelpText(nameof(GetAdditionalHelpText))]
    internal class BuildManCommand {

        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("OVERVIEW");
            formatter.WriteOnNewLine(@"With sakoe, you can 'build' your project. A build process is a succession of tasks that (typically) transform your source files into a deliverable format, usually called a release or package.

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

    [Command(
        "list", "li",
        Description = "List all the commands of this tool."
    )]
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class ManCommandListCommand {

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

}
