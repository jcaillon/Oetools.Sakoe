#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (MarkdownHelpGenerator.cs) is part of Oetools.Sakoe.
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.ConLog;
using Oetools.Sakoe.Utilities.Extension;

namespace Oetools.Sakoe.Utilities {
    
    public class MarkdownHelpGenerator : IHelpFormatter {

        private readonly StreamWriter _writer;
        
        /// <summary>
        /// Initializes a new instance of <see cref="HelpGenerator"/>.
        /// </summary>
        public MarkdownHelpGenerator(StreamWriter writer) {
            _writer = writer;
        }

        public virtual void GenerateCommandHelp(CommandLineApplication application) {
            
            var arguments = application.Arguments.Where(a => a.ShowInHelpText).ToList();
            var options = application.GetOptions().Where(o => o.ShowInHelpText).ToList();
            var commands = application.Commands.Where(c => c.ShowInHelpText).ToList();

            var fullCommandLine = application.GetFullCommandLine();

            if (!string.IsNullOrEmpty(application.Description)) {
                WriteOnNewLine(null);
                WriteSectionTitle("Synopsis");
                WriteOnNewLine(application.Description);
            }
            
            var commandType = application.GetTypeFromCommandLine();
            
            GenerateUsage(fullCommandLine, arguments, options, commands, commandType.GetProperty("RemainingArgs"));
            GenerateArguments(arguments);
            GenerateOptions(options);
            GenerateCommands(application, fullCommandLine, commands);

            var additionalHelpTextAttribute = (CommandAdditionalHelpTextAttribute) Attribute.GetCustomAttribute(commandType, typeof(CommandAdditionalHelpTextAttribute), true);
            if (additionalHelpTextAttribute != null) {
            var methodInfo = commandType.GetMethod(additionalHelpTextAttribute.MethodName, BindingFlags.Public | BindingFlags.Static);
                if (methodInfo != null) {
                    methodInfo.Invoke(null, new object[]{ this, application, 0 });
                }
            }
            
            if (!string.IsNullOrEmpty(application.ExtendedHelpText)) {
                WriteOnNewLine(null);
                WriteSectionTitle("Description");
                WriteOnNewLine(application.ExtendedHelpText);
            }
            
            WriteOnNewLine(null);
        }

        /// <summary>
        /// Generate the line that shows usage
        /// </summary>
        protected virtual void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            WriteOnNewLine(null);
            WriteSectionTitle("Usage");
            
            WriteOnNewLine($"`{thisCommandLine}");
            
            foreach (var argument in visibleArguments) {
                Write($" {argument.Name}");
            }
            if (visibleOptions.Any()) {
                Write(" [options]");
            }
            if (visibleCommands.Any()) {
                Write(" [command]");
            }
            if (remainingArgs != null) {
                if (Attribute.GetCustomAttribute(remainingArgs, typeof(DescriptionAttribute), true) is DescriptionAttribute description) {
                    Write($" {description.Description}");
                } else {
                    Write(" [[--] <arg>...]");
                }
            }
            Write("`");
        }

        /// <summary>
        /// Generate the lines that show information about arguments
        /// </summary>
        protected virtual void GenerateArguments(IReadOnlyList<CommandArgument> visibleArguments) {
            if (visibleArguments.Any()) {
                WriteOnNewLine(null);
                WriteSectionTitle("Arguments");
                WriteOnNewLine("| Argument | Description |");
                WriteOnNewLine("| --- | --- |");
                
                foreach (var arg in visibleArguments) {
                    WriteOnNewLine($"| {arg.Name.Replace("[", "").Replace("]", "").Replace("<", "\\<").Replace(">", "\\>")} | {ReplaceNewLines(arg.Description)} |");
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about options
        /// </summary>
        protected virtual void GenerateOptions(IReadOnlyList<CommandOption> visibleOptions) {
            if (visibleOptions.Any()) {
                WriteOnNewLine(null);
                WriteSectionTitle("Options");
                WriteOnNewLine("| Short name | Long name | Description |");
                WriteOnNewLine("| --- | --- | --- |");
                
                foreach (var opt in visibleOptions) {
                    var text = opt.Description;
                    if (opt.OptionType == CommandOptionType.MultipleValue) {
                        text = $"(Can be used multiple times) {text}";
                    }
                    WriteOnNewLine($"| {opt.ShortName?.PadLeft(opt.ShortName.Length + 1, '-')} | {opt.LongName?.PadLeft(opt.LongName.Length + 2, '-')} | {ReplaceNewLines(text)} |");
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about subcommands
        /// </summary>
        protected virtual void GenerateCommands(CommandLineApplication application, string thisCommandLine, IReadOnlyList<CommandLineApplication> visibleCommands) {
            if (visibleCommands.Any()) {
                WriteOnNewLine(null);
                WriteSectionTitle("Commands");
                WriteOnNewLine("| Short name | Long name | Description |");
                WriteOnNewLine("| --- | --- | --- |");

                foreach (var cmd in visibleCommands.OrderBy(c => c.Name)) {
                    WriteOnNewLine($"| {(cmd.Names != null && cmd.Names.Count() > 1 ? cmd.Names.ElementAt(1) : "")} | {cmd.Name} | {ReplaceNewLines(cmd.Description)} |");
                }
            }
        }

        private string ReplaceNewLines(string text) => text?.Replace("\n", " ").Replace("\r", "");
        
        private string ReplaceBulletList(string text) => text?.Replace("├─ ", "- ").Replace("└─ ", "- ").Replace("│  ", "  ");
        
        /// <inheritdoc cref="IConsoleOutput.WriteOnNewLine"/>
        public virtual void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0, string prefixForNewLines = null) {
            _writer.WriteLine();
            _writer.Write(ReplaceBulletList(result));
        }
        
        /// <inheritdoc cref="IConsoleOutput.Write"/>
        public virtual void Write(string result, ConsoleColor? color = null, int padding = 0, string prefixForNewLines = null) {
            _writer.Write(ReplaceBulletList(result));
        }
        
        public virtual void WriteSectionTitle(string result, int padding = 0) {
            if (!string.IsNullOrEmpty(result) && result.Length > 1) {
                _writer.WriteLine();
                _writer.Write($"### {result[0].ToString().ToUpper()}{result.Substring(1).ToLower()}");
                _writer.WriteLine();
            }
        }
        
        /// <inheritdoc cref="IConsoleOutput.Write"/>
        /// <summary>Write a tip.</summary>
        public virtual void WriteTip(string result, int padding = 0, string prefixForNewLines = null) {
            _writer.WriteLine();
            _writer.Write($"> {result}");
        }

    }
    
}