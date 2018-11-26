#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (BaseCommand.cs) is part of Oetools.Sakoe.
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// Defines options that will be inherited by all the commands
    /// </summary>
    public abstract class BaseCommand {

        protected const int FatalExitCode = 9;
        
        private const string VerbosityTemplate = "-vb|--verbosity";
        
        private string UseVerboseMessage => $"Get more details on this error by activating the debug verbosity : {VerbosityTemplate} debug";

        [Option(VerbosityTemplate + " <level>", "Sets the verbosity of this command line tool.", CommandOptionType.SingleValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        public ConsoleIo.LogLvl Verbosity { get; } = ConsoleIo.LogLvl.Info;
        
        [Option("-nop|--no-progress", "Never show progress bars.", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        public bool IsProgressBarOff { get; }
        
        [Option("-logo|--with-logo", "Always show the logo on start.", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        public bool IsLogoOn { get; }
        
        protected ILogger Log { get; private set; }
        
        protected IResultWriter Out { get; private set; }
        
        private IConsole Console { get; set; }

        protected CancellationToken? CancelToken => _cancelSource?.Token;
        
        protected virtual void ExecutePreCommand(CommandLineApplication app, IConsole console) { }

        protected virtual void ExecutePostCommand(CommandLineApplication app, IConsole console) { }

        private int _numberOfCancelKeyPress;
        
        protected CancellationTokenSource _cancelSource;
        
        public static readonly HelpGenerator HelpGenerator = HelpGenerator.Singleton;
        
        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app, IConsole console) {
            Console = console;
            
            using (var logger = new ConsoleIo(console, Verbosity, IsProgressBarOff)) {
                Log = logger;
                Out = logger;
                
                _cancelSource = new CancellationTokenSource();
                console.CancelKeyPress += ConsoleOnCancelKeyPress;
                
                if (IsLogoOn) {
                    HelpGenerator.DrawLogo(console.Out);
                }
                
                int exitCode = FatalExitCode;
                
                try {
                    Log.Debug($"Starting execution: {DateTime.Now:yy-MM-dd} @ {DateTime.Now:HH:mm:ss}.");
                    ExecutePreCommand(app, console);
                    exitCode = ExecuteCommand(app, console);
                    ExecutePostCommand(app, console);
                    if (exitCode.Equals(0)) {
                        Log.Done("Exit code 0");
                    } else {
                        Log.Warn($"Exit code {exitCode}");
                    }
                    if (Verbosity < ConsoleIo.LogLvl.None) {
                        Out.WriteNewLine();
                    }

                    Console.ResetColor();
                    return exitCode;
                    
                } catch (Exception e) {
                    Log.Error($"{e.Message}", e);
                    if (Verbosity > ConsoleIo.LogLvl.Debug) {
                        Log.Info(UseVerboseMessage);
                    }
                    if (e is CommandException ce) {
                        exitCode = ce.ExitCode;
                    }
                }

                Log.Fatal($"Exit code {exitCode}");
                if (Verbosity < ConsoleIo.LogLvl.None) {
                    Out.WriteNewLine();
                }
                
                Console.ResetColor();
                return exitCode;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public int OnValidationError(ValidationResult r) {
            using (var log = new ConsoleIo(PhysicalConsole.Singleton, ConsoleIo.LogLvl.Info, true)) {
                var faultyMembers = string.Join(", ", r.MemberNames);
                log.Error($"{(faultyMembers.Length > 0 ? $"{faultyMembers} : " : "")}{r.ErrorMessage}");
                log.Info($"Specify {MainCommand.HelpLongName} for a list of available options and commands.");
                log.Fatal($"Exit code {FatalExitCode} - Error");
                
                PhysicalConsole.Singleton.ResetColor();
                return FatalExitCode;
            }
        }

        /// <summary>
        /// The method to override for each command
        /// </summary>
        /// <param name="app"></param>
        /// <param name="console"></param>
        /// <returns></returns>
        protected virtual int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (!IsLogoOn && app.Parent == null) {
                HelpGenerator.DrawLogo(console.Out);
            }
            app.ShowHelp();
            Log.Warn("You must provide a command");
            return 1;
        }

        /// <summary>
        /// Return the command option of this type from the property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected OptionAttribute GetCommandOptionFromPropertyName(string propertyName) {
            var propertyInfo = GetType().GetProperty(propertyName);
            if (propertyInfo != null && Attribute.GetCustomAttribute(propertyInfo, typeof(OptionAttribute), true) is OptionAttribute option) {
                return option;
            }
            return null;
        }
        
        protected static Type GetTypeFromCommandLine(CommandLineApplication app) {
            var stack = new Stack<CommandLineApplication>();
            stack.Push(app);
            var rootApp = app;
            while (rootApp.Parent != null) {
                rootApp = rootApp.Parent;
                stack.Push(rootApp);
            }

            Type currentType = typeof(MainCommand);
            while (stack.Count > 0) {
                var subCommands = Attribute.GetCustomAttributes(currentType, typeof(SubcommandAttribute), true).OfType<SubcommandAttribute>().ToList();
                var commandName = stack.Pop().Name;
                foreach (var subCommand in subCommands) {
                    foreach (var subCommandType in subCommand.Types) {
                        var commandAttr = Attribute.GetCustomAttribute(subCommandType, typeof(CommandAttribute), true) as CommandAttribute;
                        if (commandAttr != null && commandAttr.Name.Equals(commandName)) {
                            currentType = subCommandType;
                        }
                    }
                }
            }
            return currentType;
        }

        /// <summary>
        /// Get the command line that calls the given type.
        /// For instance, it will return sakoe project init of the type given is the command for init.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static string GetCommandLineFromType(CommandLineApplication app, Type type) {
            var rootApp = app;
            while (rootApp.Parent != null) {
                rootApp = rootApp.Parent;
            }
            var sb = new StringBuilder(rootApp.Name);
            foreach (var command in GetCommandStackFromType(type)) {
                sb.Append(' ');
                sb.Append(command.Name);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Returns a "stack" of <see cref="CommandAttribute"/> to reach the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static List<CommandAttribute> GetCommandStackFromType(Type type) {
            var subCommands = Attribute.GetCustomAttributes(typeof(MainCommand), typeof(SubcommandAttribute), true).OfType<SubcommandAttribute>().ToList();
            var stack = new Stack<Tuple<List<CommandAttribute>, List<SubcommandAttribute>>>();
            stack.Push(new Tuple<List<CommandAttribute>, List<SubcommandAttribute>>(new List<CommandAttribute>(), subCommands));
            while (stack.Count > 0) {
                var tuple = stack.Pop();
                subCommands = tuple.Item2;
                foreach (var subCommand in subCommands) {
                    if (subCommand.Types == null) {
                        continue;
                    }
                    foreach (var subCommandType in subCommand.Types) {
                        var commandStack = tuple.Item1.ToList();
                        commandStack.Add((CommandAttribute) Attribute.GetCustomAttribute(subCommandType, typeof(CommandAttribute), true));
                        if (subCommandType == type) {
                            return commandStack;
                        }
                        var subCommandList = Attribute.GetCustomAttributes(subCommandType, true).OfType<SubcommandAttribute>().ToList();
                        stack.Push(new Tuple<List<CommandAttribute>, List<SubcommandAttribute>>(commandStack, subCommandList));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Resolve a string into an enumeration value, returning the possible values for said string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="enumValue"></param>
        /// <param name="validValuesList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool TryGetEnumValue<T>(string stringValue, out long enumValue, out List<string> validValuesList) {
            bool found = false;
            var outValidValuesList = new List<string>();
            long outEnumValue = 0;
            typeof(T).ForEach<T>((name, val) => {
                if (name.Equals(stringValue)) {
                    outEnumValue = val;
                    found = true;
                }
                outValidValuesList.Add(name);
            });
            enumValue = outEnumValue;
            validValuesList = outValidValuesList;
            return found;
        }
        
        /// <summary>
        /// On CTRL+C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            _numberOfCancelKeyPress++;
            Log.Warn($"CTRL+C pressed (press {4 - _numberOfCancelKeyPress} times more for emergency exit)");
            Log.Warn("Cancelling execution, please be patient...");
            Console.ResetColor();
            _cancelSource.Cancel();

            if (_numberOfCancelKeyPress < 4) {
                e.Cancel = true;
            }
        }
    }
    
}