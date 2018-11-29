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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Utilities {
    public class HelpGenerator : IHelpTextGenerator {
        
        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static HelpGenerator Singleton => _helpGenerator ?? (_helpGenerator = new HelpGenerator());

        /// <summary>
        /// Initializes a new instance of <see cref="HelpGenerator"/>.
        /// </summary>
        protected HelpGenerator() {}

        public IConsoleOutput Console {
            get => _console ?? (_console = new ConsoleOutput(PhysicalConsole.Singleton));
            set => _console = value;
        }

        private static HelpGenerator _helpGenerator;
        private IConsoleOutput _console;

        /// <inheritdoc />
        public void Generate(CommandLineApplication application, TextWriter output) {
            /*
            var commandType = application.GetTypeFromCommandLine();

            var arguments = new List<ArgumentAttribute>();
            var options = new List<OptionAttribute>();
            
            foreach (var propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if (Attribute.GetCustomAttribute(propertyInfo, typeof(ArgumentAttribute), true) is ArgumentAttribute argument) {
                    if (!argument.ShowInHelpText) {
                        continue;
                    }
                    if (string.IsNullOrEmpty(argument.Name)) {
                        argument.Name = $"<{propertyInfo.Name.CamelCaseToSeparator(" ")}>";
                    }
                    if (Attribute.GetCustomAttribute(propertyInfo, typeof(RequiredAttribute), true) != null && argument.Name.IndexOf('[') < 0) {
                        argument.Name = $"[{argument.Name}]";
                    }
                    arguments.Add(argument);
                } else if (Attribute.GetCustomAttribute(propertyInfo, typeof(OptionAttribute), true) is OptionAttribute option) {
                    if (!option.ShowInHelpText) {
                        continue;
                    }
                    if (string.IsNullOrEmpty(option.Template)) {
                        option.Template = $"--{propertyInfo.Name.CamelCaseToSeparator("-")}";
                    }
                    if (option.Template.IndexOf('<') < 0 && option.OptionType != CommandOptionType.NoValue) {
                        var valueName = $"<{option.ValueName ?? propertyInfo.Name.CamelCaseToSeparator(" ")}>";
                        if (option.OptionType == CommandOptionType.SingleOrNoValue) {
                            valueName = $"[{valueName}]";
                        }
                        option.Template = $"{option.Template} {valueName}";
                    }
                    if (option.OptionType == CommandOptionType.MultipleValue) {
                        option.Description = $"{option.Description ?? ""}\nThis option can be used several times.";
                    }
                    option.Template = option.Template.Replace("|", ", ");
                    options.Add(option);
                }
            }
            */
            var arguments = application.Arguments.Where(a => a.ShowInHelpText).ToList();
            var options = application.GetOptions().Where(o => o.ShowInHelpText).ToList();
            var commands = application.Commands.Where(c => c.ShowInHelpText).ToList();

            var optionShortNameColumnWidth = options.Max(o => string.IsNullOrEmpty(o.ShortName) ? 0 : o.ShortName.Length);
            if (optionShortNameColumnWidth > 0) {
                optionShortNameColumnWidth += 3;
            }
            var optionLongNameColumnWidth = options.Max(o => string.IsNullOrEmpty(o.LongName) ? 0 : o.LongName.Length);
            if (optionLongNameColumnWidth > 0) {
                optionLongNameColumnWidth += 2;
            }
            var firstColumnWidth = Math.Max(Math.Max(arguments.Count > 0 ? arguments.Max(a => a.Name.IndexOf('[') < 0 ? a.Name.Length : a.Name.Length - 2) : 0, Math.Max(optionShortNameColumnWidth + optionLongNameColumnWidth, commands.Count > 0 ? commands.Max(c => c.Name?.Length ?? 0) : 0)), 20);
            if (firstColumnWidth > optionShortNameColumnWidth + optionLongNameColumnWidth) {
                optionLongNameColumnWidth = firstColumnWidth - optionShortNameColumnWidth;
            }

            var fullCommandLine = application.GetFullCommandLine();

            if (!string.IsNullOrEmpty(application.Description)) {
                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine("SYNOPSIS", ConsoleColor.Cyan, 1);
                Console.WriteOnNewLine(application.Description, padding: 3);
            }
            
            var commandType = application.GetTypeFromCommandLine();
            
            GenerateUsage(fullCommandLine, arguments, options, commands, commandType.GetProperty("RemainingArgs"));
            GenerateArguments(arguments, firstColumnWidth);
            GenerateOptions(options, optionShortNameColumnWidth, optionLongNameColumnWidth);
            GenerateCommands(application, fullCommandLine, commands, firstColumnWidth);
            
            if (!string.IsNullOrEmpty(application.ExtendedHelpText)) {
                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine("DESCRIPTION", ConsoleColor.Cyan, 1);
                Console.WriteOnNewLine(application.ExtendedHelpText, padding: 3);
            }
            
            Console.WriteOnNewLine(null);
        }

        /// <summary>
        /// Generate the line that shows usage
        /// </summary>
        protected virtual void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            Console.WriteOnNewLine(null);
            Console.WriteOnNewLine("USAGE", ConsoleColor.Cyan, 1);
            Console.WriteOnNewLine(thisCommandLine, padding: 3);
            foreach (var argument in visibleArguments) {
                Console.Write($" {argument.Name}");
            }
            if (visibleOptions.Any()) {
                Console.Write(" [options]");
            }
            if (visibleCommands.Any()) {
                Console.Write(" [command]");
            }
            if (remainingArgs != null) {
                if (Attribute.GetCustomAttribute(remainingArgs, typeof(DescriptionAttribute), true) is DescriptionAttribute description) {
                    Console.Write($" {description.Description}");
                } else {
                    Console.Write(" [[--] <arg>...]");
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about arguments
        /// </summary>
        /// <param name="visibleArguments">Arguments not hidden from help text</param>
        protected virtual void GenerateArguments(IReadOnlyList<CommandArgument> visibleArguments, int firstColumnWidth) {
            if (visibleArguments.Any()) {
                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine("ARGUMENTS", ConsoleColor.Cyan, 1);
                
                foreach (var arg in visibleArguments) {
                    Console.WriteOnNewLine(arg.Name.Replace("[", "").Replace("]", "").PadRight(firstColumnWidth + 2), padding: 3);
                    Console.Write(arg.Description, padding: firstColumnWidth + 5);
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about options
        /// </summary>
        protected virtual void GenerateOptions(IReadOnlyList<CommandOption> visibleOptions, int optionShortNameColumnWidth, int optionLongNameColumnWidth) {
            if (visibleOptions.Any()) {
                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine("OPTIONS", ConsoleColor.Cyan, 1);
                
                foreach (var opt in visibleOptions) {
                    if (!string.IsNullOrEmpty(opt.ShortName) && optionShortNameColumnWidth > 2) {
                        Console.WriteOnNewLine($"-{opt.ShortName}, ".PadRight(optionShortNameColumnWidth), padding: 3);
                        Console.Write($"--{opt.LongName}".PadRight(optionLongNameColumnWidth + 2), padding: optionShortNameColumnWidth + 3);
                    } else {
                        Console.WriteOnNewLine($"--{opt.LongName}".PadRight(optionLongNameColumnWidth + 2), padding: optionShortNameColumnWidth + 3);
                    }
                    Console.Write(opt.Description, padding: optionShortNameColumnWidth + optionLongNameColumnWidth + 5);
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about subcommands
        /// </summary>
        protected virtual void GenerateCommands(CommandLineApplication application, string thisCommandLine, IReadOnlyList<CommandLineApplication> visibleCommands, int firstColumnWidth) {
            if (visibleCommands.Any()) {
                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine("COMMANDS", ConsoleColor.Cyan, 1);

                foreach (var cmd in visibleCommands.OrderBy(c => c.Name)) {
                    Console.WriteOnNewLine(cmd.Name.PadRight(firstColumnWidth + 2), padding: 3);
                    Console.Write(cmd.Description, padding: firstColumnWidth + 5);
                }

                Console.WriteOnNewLine(null);
                Console.WriteOnNewLine($"Run '{thisCommandLine} [command] --{application.OptionHelp.LongName}' for more information about a command.", padding: 3);
            }
        }

    }
}