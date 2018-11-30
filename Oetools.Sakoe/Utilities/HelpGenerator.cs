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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Oetools.Sakoe.Utilities.Extension;

namespace Oetools.Sakoe.Utilities {
    
    public class HelpGenerator : IHelpTextGenerator {
        
        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static HelpGenerator Singleton => _instance ?? (_instance = new HelpGenerator(ConsoleIo.Singleton));

        /// <summary>
        /// Initializes a new instance of <see cref="HelpGenerator"/>.
        /// </summary>
        protected HelpGenerator(IConsoleOutput console) {
            _console = console;
        }
        
        private static HelpGenerator _instance;

        private IConsoleOutput _console;

        /// <inheritdoc />
        public void Generate(CommandLineApplication application, TextWriter output) {
            try {
                _console.OutputTextWriter = output;
                GenerateCommandHelp(application);
            } finally {
                _console.OutputTextWriter = null;
            }
        }

        private void GenerateCommandHelp(CommandLineApplication application) {
            var arguments = application.Arguments.Where(a => a.ShowInHelpText).ToList();
            var options = application.GetOptions().Where(o => o.ShowInHelpText).ToList();
            var commands = application.Commands.Where(c => c.ShowInHelpText).ToList();

            var optionShortNameColumnWidth = options.Max(o => string.IsNullOrEmpty(o.ShortName) ? 0 : o.ShortName.Length);
            if (optionShortNameColumnWidth > 0) {
                optionShortNameColumnWidth += 3; // -name,_
            }
            var optionLongNameColumnWidth = options.Max(o => {
                var lgt = string.IsNullOrEmpty(o.LongName) ? 0 : o.LongName.Length;
                if (!string.IsNullOrEmpty(o.ValueName)) {
                    lgt += 3 + o.ValueName.Length; // space and <>
                }
                return lgt;
            });
            if (optionLongNameColumnWidth > 0) {
                optionLongNameColumnWidth += 2; // --name
            }
            var firstColumnWidth = Math.Max(Math.Max(arguments.Count > 0 ? arguments.Max(a => a.Name.IndexOf('[') < 0 ? a.Name.Length : a.Name.Length - 2) : 0, Math.Max(optionShortNameColumnWidth + optionLongNameColumnWidth, commands.Count > 0 ? commands.Max(c => c.Name?.Length ?? 0) : 0)), 20);
            firstColumnWidth = Math.Min(firstColumnWidth, 40);
            
            if (firstColumnWidth != optionShortNameColumnWidth + optionLongNameColumnWidth) {
                optionLongNameColumnWidth = firstColumnWidth - optionShortNameColumnWidth;
            }

            var fullCommandLine = application.GetFullCommandLine();

            if (!string.IsNullOrEmpty(application.Description)) {
                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine("SYNOPSIS", ConsoleColor.Cyan, 1);
                _console.WriteOnNewLine(application.Description, padding: 3);
            }
            
            var commandType = application.GetTypeFromCommandLine();
            
            GenerateUsage(fullCommandLine, arguments, options, commands, commandType.GetProperty("RemainingArgs"));
            GenerateArguments(arguments, firstColumnWidth);
            GenerateOptions(options, optionShortNameColumnWidth, optionLongNameColumnWidth);
            GenerateCommands(application, fullCommandLine, commands, firstColumnWidth);
            
            var methodInfo = commandType.GetMethod("GetAdditionalHelpText", BindingFlags.Public | BindingFlags.Static);
            if (methodInfo != null) {
                methodInfo.Invoke(null, new object[]{ _console, application, firstColumnWidth });
            }
            
            if (!string.IsNullOrEmpty(application.ExtendedHelpText)) {
                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine("DESCRIPTION", ConsoleColor.Cyan, 1);
                _console.WriteOnNewLine(application.ExtendedHelpText, padding: 3);
            }
            
            _console.WriteOnNewLine(null);
        }

        /// <summary>
        /// Generate the line that shows usage
        /// </summary>
        protected virtual void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            _console.WriteOnNewLine(null);
            _console.WriteOnNewLine("USAGE", ConsoleColor.Cyan, 1);
            _console.WriteOnNewLine(thisCommandLine, padding: 3);
            foreach (var argument in visibleArguments) {
                Console.Write($" {argument.Name}");
            }
            if (visibleOptions.Any()) {
                _console.Write(" [options]");
            }
            if (visibleCommands.Any()) {
                _console.Write(" [command]");
            }
            if (remainingArgs != null) {
                if (Attribute.GetCustomAttribute(remainingArgs, typeof(DescriptionAttribute), true) is DescriptionAttribute description) {
                    _console.Write($" {description.Description}");
                } else {
                    _console.Write(" [[--] <arg>...]");
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about arguments
        /// </summary>
        protected virtual void GenerateArguments(IReadOnlyList<CommandArgument> visibleArguments, int firstColumnWidth) {
            if (visibleArguments.Any()) {
                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine("ARGUMENTS", ConsoleColor.Cyan, 1);
                
                foreach (var arg in visibleArguments) {
                    var name = arg.Name.Replace("[", "").Replace("]", "");
                    _console.WriteOnNewLine(name.PadRight(firstColumnWidth + 2), padding: 3);
                    if (name.Length > firstColumnWidth) {
                        _console.WriteOnNewLine(arg.Description, padding: firstColumnWidth + 5);
                    } else {
                        _console.Write(arg.Description, padding: firstColumnWidth + 5);
                    }
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about options
        /// </summary>
        protected virtual void GenerateOptions(IReadOnlyList<CommandOption> visibleOptions, int optionShortNameColumnWidth, int optionLongNameColumnWidth) {
            var firstColumnWidth = optionShortNameColumnWidth + optionLongNameColumnWidth;
            if (visibleOptions.Any()) {
                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine("OPTIONS", ConsoleColor.Cyan, 1);
                
                foreach (var opt in visibleOptions) {
                    var shortName = string.IsNullOrEmpty(opt.SymbolName) ? string.IsNullOrEmpty(opt.ShortName) ? "" : $"-{opt.ShortName}, " : $"-{opt.SymbolName}, ";
                    var longName = string.IsNullOrEmpty(opt.LongName) ? "" : $"--{opt.LongName}";
                    if (!string.IsNullOrEmpty(opt.ValueName)) {
                        longName = string.IsNullOrEmpty(longName) ? opt.ValueName : $"{longName} <{opt.ValueName.Replace("_", " ")}>";
                    }
                    var firstColumn = $"{shortName.PadRight(optionShortNameColumnWidth)}{longName.PadRight(optionLongNameColumnWidth)}";
                    _console.WriteOnNewLine(firstColumn.PadRight(firstColumnWidth + 2), padding: 3);
                    if (firstColumn.Length > firstColumnWidth) {
                        _console.WriteOnNewLine(opt.Description, padding: firstColumnWidth + 5);
                    } else {
                        _console.Write(opt.Description, padding: firstColumnWidth + 5);
                    }
                    if (opt.OptionType == CommandOptionType.MultipleValue) {
                        _console.WriteOnNewLine("This option can be used multiple times.", padding: firstColumnWidth + 5);
                    }
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about subcommands
        /// </summary>
        protected virtual void GenerateCommands(CommandLineApplication application, string thisCommandLine, IReadOnlyList<CommandLineApplication> visibleCommands, int firstColumnWidth) {
            if (visibleCommands.Any()) {
                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine("COMMANDS", ConsoleColor.Cyan, 1);

                foreach (var cmd in visibleCommands.OrderBy(c => c.Name)) {
                    _console.WriteOnNewLine(cmd.Name.PadRight(firstColumnWidth + 2), padding: 3);
                    if (cmd.Name.Length > firstColumnWidth) {
                        _console.WriteOnNewLine(cmd.Description, padding: firstColumnWidth + 5);
                    } else {
                        _console.Write(cmd.Description, padding: firstColumnWidth + 5);
                    }
                }

                _console.WriteOnNewLine(null);
                _console.WriteOnNewLine($"Run '{thisCommandLine} [command] --{application.OptionHelp.LongName}' for more information about a command.", padding: 3);
            }
        }

    }
}