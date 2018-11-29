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
using System.Linq;
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
        
        [Option("-po|--progress-off", "Never show progress bars.", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        public bool IsProgressBarOff { get; }
        
        [Option("-wl|--with-logo", "Always show the logo on start.", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        public bool IsLogoOn { get; }
        
        protected ILogger Log { get; private set; }
        
        protected IConsoleOutput Out { get; private set; }
        
        private IConsole Console { get; set; }

        protected CancellationToken? CancelToken => _cancelSource?.Token;
        
        protected virtual void ExecutePreCommand(CommandLineApplication app, IConsole console) { }

        protected virtual void ExecutePostCommand(CommandLineApplication app, IConsole console) { }

        private int _numberOfCancelKeyPress;
        
        protected object _lock = new object();
        
        protected CancellationTokenSource _cancelSource;
        
        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app, IConsole console) {
            Console = console;
            
            using (var consoleIo = new ConsoleIo(console, Verbosity, IsProgressBarOff)) {
                Log = consoleIo;
                Out = consoleIo;
                HelpGenerator.Singleton.Console = consoleIo;
                
                _cancelSource = new CancellationTokenSource();
                console.CancelKeyPress += ConsoleOnCancelKeyPress;
                
                if (IsLogoOn) {
                    Out.DrawLogo();
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
                        Out.WriteOnNewLine(null);
                    }
                    return exitCode;
                    
                } catch (Exception e) {
                    Log.Error(e.Message, e);
                    if (Verbosity > ConsoleIo.LogLvl.Debug) {
                        Log.Info(UseVerboseMessage);
                    }
                    if (e is CommandException ce) {
                        exitCode = ce.ExitCode;
                    }
                }

                Log.Fatal($"Exit code {exitCode}");
                if (Verbosity < ConsoleIo.LogLvl.None) {
                    Out.WriteOnNewLine(null);
                }
                return exitCode;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public int OnValidationError(ValidationResult r) {
            using (var log = new ConsoleIo(PhysicalConsole.Singleton, ConsoleIo.LogLvl.Info, true)) {
                var faultyMembers = string.Join(", ", r.MemberNames);
                log.Error($"{(faultyMembers.Length > 0 ? $"{faultyMembers} : ": "")}{r.ErrorMessage}");
                log.Info($"Specify {MainCommand.HelpLongName} for a list of available options and commands.");
                log.Fatal($"Exit code {FatalExitCode}");
                log.WriteOnNewLine(null);
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
                Out.DrawLogo();
            }
            app.ShowHelp();
            Log.Warn("You must provide a command.");
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
        
        /// <summary>
        /// On CTRL+C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            if (Monitor.TryEnter(_lock, 500)) {
                try {
                    _numberOfCancelKeyPress++;
                    Log.Warn($"CTRL+C pressed (press {4 - _numberOfCancelKeyPress} times more for emergency exit)");
                    Log.Warn("Cancelling execution, please be patient...");
                    Console.ResetColor();
                    _cancelSource.Cancel();
                    if (_numberOfCancelKeyPress < 4) {
                        e.Cancel = true;
                    }
                } finally {
                    Monitor.Exit(_lock);
                }
            }
        }
    }
    
}