#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ManCommand.cs) is part of Oetools.Sakoe.
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using CommandLineUtilsPlus;
using CommandLineUtilsPlus.Extension;
using DotUtilities.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        Name, "ma", "man",
        Description = "The manual of this tool. Learn about the usage and key concepts of sakoe."
    )]
    [Subcommand(typeof(ManCommandCommand))]
    [Subcommand(typeof(CompleteManCommand))]
    [Subcommand(typeof(MarkdownManCommand))]
    [CommandAdditionalHelpText(nameof(GetAdditionalHelpText))]
    internal class ManCommand {

        public const string Name = "manual";

        public static void GetAdditionalHelpText(IHelpWriter formatter, CommandLineApplication app, int firstColumnWidth) {
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
  - If you need to use a double quote within a double quote, you can do so by double quoting the double quotes (i.e. ""my """"special"""" value"").
  - If an extra layer is needed, just double the doubling (i.e. -opt ""-mysubopt """"my special """"""""value"""""""""""""").
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
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kinds of warnings.
  - 9 : used when a command does not complete and ends up in error.");

            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("WEBSITE");
            formatter.WriteOnNewLine(@"The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

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
            GetAdditionalHelpText(app.HelpTextGenerator as IHelpWriter, app, 0);
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

        public static void GetAdditionalHelpText(IHelpWriter formatter, CommandLineApplication app, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            app.Parent.Commands.Remove(app);
            var rootCommand = app;
            while (rootCommand.Parent != null) {
                rootCommand = rootCommand.Parent;
            }
            ListCommands(formatter, rootCommand.Commands);
            formatter.WriteOnNewLine(null);
        }

        private static void ListCommands(IHelpWriter formatter, List<CommandLineApplication> subCommands) {
            var i = 0;
            foreach (var subCommand in subCommands.OrderBy(c => c.Name)) {
                subCommand.ShowHelp();
                if (subCommand.Commands != null && subCommand.Commands.Count > 0) {
                    ListCommands(formatter, subCommand.Commands);
                }
                i++;
            }
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            GetAdditionalHelpText(app.HelpTextGenerator as IHelpWriter, app, 0);
            return 0;
        }
    }


    [Command(
        "export-md", "md",
        Description = "Export the documentation of this tool to a markdown file."
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
                    writer.WriteLine($"> This markdown can be generated using the command: {app.GetFullCommandLine().PrettyQuote()}.");
                    writer.WriteLine("> ");
                    writer.WriteLine($"> This version has been generated on {DateTime.Now:yy-MM-dd} at {DateTime.Now:HH:mm:ss}.");
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


}
