#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (HelpTextGenerator.cs) is part of Oetools.Sakoe.
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
using System.IO;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Utilities {
    public class HelpGenerator : TextWriterWordWrap, IHelpTextGenerator {
        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static HelpGenerator Singleton { get; } = new HelpGenerator();

        /// <summary>
        /// Initializes a new instance of <see cref="HelpGenerator"/>.
        /// </summary>
        protected HelpGenerator() { }

        /// <summary>
        /// Determines if commands are ordered by name in generated help text
        /// </summary>
        public bool SortCommandsByName { get; set; } = true;
        
        public  IConsole Console { get; set; }

        /// <inheritdoc />
        public virtual void Generate(CommandLineApplication application, TextWriter output) {
            GenerateHeader(application, output);
            GenerateBody(application, output);
            GenerateFooter(application, output);
        }

        /// <summary>
        /// Draw the logo of this tool
        /// </summary>
        /// <param name="output"></param>
        public static void DrawLogo(TextWriter output) {
            output.WriteLine();
            output.WriteLine(@"                '`.        ");
            output.WriteLine(@" '`.    .^      \  \       == SAKOE ==");
            output.WriteLine(@"  \ \  /;/       \ \\      A Swiss Army Knife for OpenEdge.");
            output.WriteLine(@"   \ \/_/_________\  \     " + $"Version {typeof(HelpGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}.");
            output.WriteLine(@"    `/ .           _  \    " + $"Running with {(Utils.IsNetFrameworkBuild ? ".netframework" : $".netcore-{(Utils.IsRuntimeWindowsPlatform ? "win" : "unix")}")}.");
            output.WriteLine(@"     \________________/    " + $"Session started on {DateTime.Now:yy-MM-dd} at {DateTime.Now:HH:mm:ss}.");
            output.WriteLine();
        }
        
        /// <summary>
        /// Generate the first few lines of help output text
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        protected virtual void GenerateHeader(CommandLineApplication application, TextWriter output) {
            if (!string.IsNullOrEmpty(application.Description)) {
                output.WriteLine(application.Description);
                output.WriteLine();
            }
        }

        /// <summary>
        /// Generate detailed help information
        /// </summary>
        /// <param name="application">The application</param>
        /// <param name="output">Help text output</param>
        protected virtual void GenerateBody(CommandLineApplication application, TextWriter output) {
            var arguments = application.Arguments.Where(a => a.ShowInHelpText).ToList();
            var options = application.GetOptions().Where(o => o.ShowInHelpText).ToList();
            var commands = application.Commands.Where(c => c.ShowInHelpText).ToList();

            var firstColumnWidth = 2 + Math.Max(arguments.Count > 0 ? arguments.Max(a => a.Name.Length) : 0, Math.Max(options.Count > 0 ? options.Max(o => o.Template?.Length ?? 0) : 0, commands.Count > 0 ? commands.Max(c => c.Name?.Length ?? 0) : 0));

            GenerateUsage(application, output, arguments, options, commands);
            GenerateArguments(application, output, arguments, firstColumnWidth);
            GenerateOptions(application, output, options, firstColumnWidth);
            GenerateCommands(application, output, commands, firstColumnWidth);
        }

        /// <summary>
        /// Generate the line that shows usage
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        /// <param name="visibleArguments">Arguments not hidden from help text</param>
        /// <param name="visibleOptions">Options not hidden from help text</param>
        /// <param name="visibleCommands">Commands not hidden from help text</param>
        protected virtual void GenerateUsage(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands) {
            output.Write("Usage:");
            var stack = new Stack<string>();
            for (var cmd = application; cmd != null; cmd = cmd.Parent) {
                stack.Push(cmd.Name);
            }

            while (stack.Count > 0) {
                output.Write(' ');
                output.Write(stack.Pop());
            }

            if (visibleArguments.Any()) {
                output.Write(" [arguments]");
            }

            if (visibleOptions.Any()) {
                output.Write(" [options]");
            }

            if (visibleCommands.Any()) {
                output.Write(" [command]");
            }

            if (application.AllowArgumentSeparator) {
                output.Write(" [[--] <arg>...]");
            }

            output.WriteLine();
        }

        /// <summary>
        /// Generate the lines that show information about arguments
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        /// <param name="visibleArguments">Arguments not hidden from help text</param>
        /// <param name="firstColumnWidth">The width of the first column of commands, arguments, and options</param>
        protected virtual void GenerateArguments(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandArgument> visibleArguments, int firstColumnWidth) {
            if (visibleArguments.Any()) {
                output.WriteLine();
                output.WriteLine("Arguments:");
                var outputFormat = string.Format("  {{0, -{0}}}{{1}}", firstColumnWidth);

                var newLineWithMessagePadding = Environment.NewLine + new string(' ', firstColumnWidth + 2);

                foreach (var arg in visibleArguments) {
                    var message = string.Format(outputFormat, arg.Name, arg.Description);
                    message = message.Replace(Environment.NewLine, newLineWithMessagePadding);

                    output.Write(message);
                    output.WriteLine();
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about options
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        /// <param name="visibleOptions">Options not hidden from help text</param>
        /// <param name="firstColumnWidth">The width of the first column of commands, arguments, and options</param>
        protected virtual void GenerateOptions(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandOption> visibleOptions, int firstColumnWidth) {
            if (visibleOptions.Any()) {
                output.WriteLine();
                output.WriteLine("Options:");
                var outputFormat = string.Format("  {{0, -{0}}}{{1}}", firstColumnWidth);

                var newLineWithMessagePadding = Environment.NewLine + new string(' ', firstColumnWidth + 2);

                foreach (var opt in visibleOptions) {
                    var message = string.Format(outputFormat, opt.Template, opt.Description);
                    message = message.Replace(Environment.NewLine, newLineWithMessagePadding);

                    output.Write(message);
                    output.WriteLine();
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about subcommands
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        /// <param name="visibleCommands">Commands not hidden from help text</param>
        /// <param name="firstColumnWidth">The width of the first column of commands, arguments, and options</param>
        protected virtual void GenerateCommands(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandLineApplication> visibleCommands, int firstColumnWidth) {
            if (visibleCommands.Any()) {
                output.WriteLine();
                output.WriteLine("Commands:");
                var outputFormat = string.Format("  {{0, -{0}}}{{1}}", firstColumnWidth);

                var newLineWithMessagePadding = Environment.NewLine + new string(' ', firstColumnWidth + 2);

                var orderedCommands = SortCommandsByName ? visibleCommands.OrderBy(c => c.Name).ToList() : visibleCommands;
                foreach (var cmd in orderedCommands) {
                    var message = string.Format(outputFormat, cmd.Name, cmd.Description);
                    message = message.Replace(Environment.NewLine, newLineWithMessagePadding);

                    output.Write(message);
                    output.WriteLine();
                }

                if (application.OptionHelp != null) {
                    output.WriteLine();
                    output.WriteLine($"Run '{application.Name} [command] --{application.OptionHelp.LongName}' for more information about a command.");
                }
            }
        }

        /// <summary>
        /// Generate the last lines of help text output
        /// </summary>
        /// <param name="application">The app</param>
        /// <param name="output">Help text output</param>
        protected virtual void GenerateFooter(CommandLineApplication application, TextWriter output) {
            output.WriteLine();
            output.WriteLine("Extended help:");
            WriteToConsoleWithWordWrap(output, application.ExtendedHelpText, false, 5);
            output.WriteLine();
            output.WriteLine();
        }
    }
}