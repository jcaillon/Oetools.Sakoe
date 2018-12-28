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
using Oetools.Sakoe.ConLog;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;

#if !WINDOWSONLYBUILD
using Oetools.Sakoe.Command.Oe;
#endif

namespace Oetools.Sakoe.Utilities {
    
    public class HelpGenerator : IHelpTextGenerator, IHelpFormatter {
        
        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static HelpGenerator Singleton => _instance ?? (_instance = new HelpGenerator(ConsoleLogger2.Singleton));

        /// <summary>
        /// Initializes a new instance of <see cref="HelpGenerator"/>.
        /// </summary>
        protected HelpGenerator(IConsoleOutput console) {
            _console = console;
        }
        
        private static HelpGenerator _instance;

        private IConsoleOutput _console;

        public static string GetHelpProvideCommand(CommandLineApplication application) {
            return $"You must provide a command: {$"{application.GetFullCommandLine()} [command]".PrettyQuote()}.";
        }

        /// <inheritdoc />
        public virtual void Generate(CommandLineApplication application, TextWriter output) {
            try {
                _console.OutputTextWriter = output;
                GenerateCommandHelp(application);
            } finally {
                _console.OutputTextWriter = null;
            }
        }

        protected virtual void GenerateCommandHelp(CommandLineApplication application) {
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

            var firstColumnWidth = Math.Max(optionShortNameColumnWidth + optionLongNameColumnWidth, commands.Count > 0 ? commands.Max(c => c.Name?.Length ?? 0) : 0);
            firstColumnWidth = Math.Max(firstColumnWidth, arguments.Count > 0 ? arguments.Max(a => a.Name.IndexOf('[') < 0 ? a.Name.Length : a.Name.Length - 2) : 0);
            firstColumnWidth = Math.Max(firstColumnWidth, 20);
            firstColumnWidth = Math.Min(firstColumnWidth, 35);
            
            if (firstColumnWidth != optionShortNameColumnWidth + optionLongNameColumnWidth) {
                optionLongNameColumnWidth = firstColumnWidth - optionShortNameColumnWidth;
            }

            var fullCommandLine = application.GetFullCommandLine();

            if (!string.IsNullOrEmpty(application.Description)) {
                WriteOnNewLine(null);
                WriteSectionTitle("SYNOPSIS");
                WriteOnNewLine(application.Description);
            }
            
            var commandType = application.GetTypeFromCommandLine();
            
            GenerateUsage(fullCommandLine, arguments, options, commands, commandType.GetProperty("RemainingArgs"));
            GenerateArguments(arguments, firstColumnWidth);
            GenerateOptions(options, optionShortNameColumnWidth, optionLongNameColumnWidth);
            GenerateCommands(application, fullCommandLine, commands, firstColumnWidth);

            var additionalHelpTextAttribute = (CommandAdditionalHelpTextAttribute) Attribute.GetCustomAttribute(commandType, typeof(CommandAdditionalHelpTextAttribute), true);
            if (additionalHelpTextAttribute != null) {
            var methodInfo = commandType.GetMethod(additionalHelpTextAttribute.MethodName, BindingFlags.Public | BindingFlags.Static);
                if (methodInfo != null) {
                    methodInfo.Invoke(null, new object[]{ this, application, firstColumnWidth });
                }
            }
            
            if (!string.IsNullOrEmpty(application.ExtendedHelpText)) {
                WriteOnNewLine(null);
                WriteSectionTitle("DESCRIPTION");
                WriteOnNewLine(application.ExtendedHelpText);
            }
            
            WriteOnNewLine(null);
        }

        /// <summary>
        /// Generate the line that shows usage
        /// </summary>
        protected virtual void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            WriteOnNewLine(null);
            WriteSectionTitle("USAGE");
            WriteOnNewLine(thisCommandLine);
            
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
            
            #if !WINDOWSONLYBUILD
            if (!File.Exists(CreateStarterCommand.StartScriptFilePath)) {
                WriteOnNewLine(null);
                WriteTip($"Tip: use the command '{typeof(CreateStarterCommand).GetFullCommandLine()}' to simplify your invocation of sakoe.");
            }
            #endif
        }

        /// <summary>
        /// Generate the lines that show information about arguments
        /// </summary>
        protected virtual void GenerateArguments(IReadOnlyList<CommandArgument> visibleArguments, int firstColumnWidth) {
            if (visibleArguments.Any()) {
                WriteOnNewLine(null);
                WriteSectionTitle("ARGUMENTS");
                
                foreach (var arg in visibleArguments) {
                    var name = arg.Name.Replace("[", "").Replace("]", "");
                    WriteOnNewLine(name.PadRight(firstColumnWidth + 2));
                    if (name.Length > firstColumnWidth) {
                        WriteOnNewLine(arg.Description, padding: firstColumnWidth + 2);
                    } else {
                        Write(arg.Description, padding: firstColumnWidth + 2);
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
                WriteOnNewLine(null);
                WriteSectionTitle("OPTIONS");
                
                foreach (var opt in visibleOptions) {
                    var shortName = string.IsNullOrEmpty(opt.SymbolName) ? string.IsNullOrEmpty(opt.ShortName) ? "" : $"-{opt.ShortName}, " : $"-{opt.SymbolName}, ";
                    var longName = string.IsNullOrEmpty(opt.LongName) ? "" : $"--{opt.LongName}";
                    string valueName = "";
                    if (!string.IsNullOrEmpty(opt.ValueName)) {
                        valueName = opt.OptionType == CommandOptionType.SingleOrNoValue ? $"[:{opt.ValueName.Replace("_", " ")}]" : $" <{opt.ValueName.Replace("_", " ")}>";
                    }
                    var firstColumn = $"{shortName.PadRight(optionShortNameColumnWidth)}{$"{longName}{valueName}".PadRight(optionLongNameColumnWidth)}";
                    WriteOnNewLine(firstColumn.PadRight(firstColumnWidth + 2));
                    var text = opt.Description;
                    if (opt.OptionType == CommandOptionType.MultipleValue) {
                        text = $"(Can be used multiple times) {text}";
                    }
                    if (firstColumn.Length > firstColumnWidth) {
                        WriteOnNewLine(text, padding: firstColumnWidth + 2);
                    } else {
                        Write(text, padding: firstColumnWidth + 2);
                    }
                }
            }
        }

        /// <summary>
        /// Generate the lines that show information about subcommands
        /// </summary>
        protected virtual void GenerateCommands(CommandLineApplication application, string thisCommandLine, IReadOnlyList<CommandLineApplication> visibleCommands, int firstColumnWidth) {
            if (visibleCommands.Any()) {
                WriteOnNewLine(null);
                WriteSectionTitle("COMMANDS");

                foreach (var cmd in visibleCommands.OrderBy(c => c.Name)) {
                    WriteOnNewLine(cmd.Name.PadRight(firstColumnWidth + 2));
                    if (cmd.Name.Length > firstColumnWidth) {
                        WriteOnNewLine(cmd.Description, padding: firstColumnWidth + 2);
                    } else {
                        Write(cmd.Description, padding: firstColumnWidth + 2);
                    }
                }

                WriteOnNewLine(null);
                WriteTip($"Tip: run '{thisCommandLine} [command] --{application.OptionHelp.LongName}' for more information about a command.");
            }
        }

        protected const int DefaultPadding = 1;
        protected const int SectionPadding = 2;

        private int _currentPadding;

        /// <inheritdoc cref="IConsoleOutput.WriteOnNewLine"/>
        public virtual void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0, string prefixForNewLines = null) {
            _console.WriteOnNewLine(result, color, padding + DefaultPadding + _currentPadding, DefaultPadding + _currentPadding == 0 || string.IsNullOrEmpty(prefixForNewLines) ? prefixForNewLines : prefixForNewLines.PadLeft(prefixForNewLines.Length + DefaultPadding + _currentPadding, ' '));
        }
        
        /// <inheritdoc cref="IConsoleOutput.Write"/>
        public virtual void Write(string result, ConsoleColor? color = null, int padding = 0, string prefixForNewLines = null) {
            _console.Write(result, color, padding + DefaultPadding + _currentPadding, DefaultPadding + _currentPadding == 0 || string.IsNullOrEmpty(prefixForNewLines) ? prefixForNewLines : prefixForNewLines.PadLeft(prefixForNewLines.Length + DefaultPadding + _currentPadding, ' '));
        }
        
        public virtual void WriteSectionTitle(string result, int padding = 0) {
            _currentPadding = 0;
            _console.WriteOnNewLine(result, ConsoleColor.Cyan, padding + DefaultPadding + _currentPadding);
            _currentPadding = SectionPadding;
        }
        
        /// <inheritdoc cref="IConsoleOutput.Write"/>
        /// <summary>Write a tip.</summary>
        public virtual void WriteTip(string result, int padding = 0, string prefixForNewLines = null) {
            _console.WriteOnNewLine(result, ConsoleColor.Gray, padding + DefaultPadding + _currentPadding, DefaultPadding + _currentPadding == 0 || string.IsNullOrEmpty(prefixForNewLines) ? prefixForNewLines : prefixForNewLines.PadLeft(prefixForNewLines.Length + DefaultPadding + _currentPadding, ' '));
        }

    }
}