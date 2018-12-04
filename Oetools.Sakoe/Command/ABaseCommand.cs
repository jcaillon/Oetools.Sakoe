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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// Defines options that will be inherited by all the commands
    /// </summary>
    public abstract class ABaseCommand {

        public const int FatalExitCode = 9;
        
        private const string VerbosityShortName = "-vb";
        
        [Option(VerbosityShortName + "|--verbosity <level>", "Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'.", CommandOptionType.SingleOrNoValue)]
        public (bool HasValue, ConsoleLogThreshold? Value) VerbosityThreshold { get; set; }
        
        [Option("-pm|--progress-mode <mode>", "Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done.", CommandOptionType.SingleValue)]
        public ConsoleProgressBarDisplayMode? ProgressBarDisplayMode { get; set; }
        
        [Option("-do|--debug-output <file>", "Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'.", CommandOptionType.SingleOrNoValue)]
        public (bool HasValue, string Value) LogOutputFilePath { get; set; }
        
        [Option("-wl|--with-logo", "Always show the logo on start.", CommandOptionType.NoValue)]
        public bool IsLogoOn { get; set; }

        protected ConsoleLogThreshold Verbosity => VerbosityThreshold.HasValue ? VerbosityThreshold.Value ?? ConsoleLogThreshold.Debug : ConsoleLogThreshold.Info;
        
        protected ILogger Log { get; private set; }
        
        protected IConsoleOutput Out { get; private set; }

        protected CancellationToken? CancelToken => _cancelSource?.Token;
        
        protected virtual void ExecutePreCommand(CommandLineApplication app, IConsole console) { }

        protected virtual void ExecutePostCommand(CommandLineApplication app, IConsole console) { }

        private int _numberOfCancelKeyPress;
        
        protected object _lock = new object();
        
        protected CancellationTokenSource _cancelSource;
        
        /// <summary>
        /// Called when the command is executed.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="console"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app, IConsole console) {
            Log = ConsoleIo.Singleton;
            Out = ConsoleIo.Singleton;
            ConsoleIo.Singleton.LogTheshold = Verbosity;
            ConsoleIo.Singleton.ProgressBarDisplayMode = ProgressBarDisplayMode ?? ConsoleProgressBarDisplayMode.On;
            if (LogOutputFilePath.HasValue) {
                var logFilePath = LogOutputFilePath.Value;
                if (string.IsNullOrEmpty(logFilePath)) {
                    if (Directory.Exists(OeBuilderConstants.GetProjectDirectory(Directory.GetCurrentDirectory()))) {
                        logFilePath = Path.Combine(OeBuilderConstants.GetProjectDirectoryLocal(Directory.GetCurrentDirectory()), "logs", "sakoe.log");
                    } else {
                        logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "sakoe.log");
                    }
                }
                ConsoleIo.Singleton.LogOutputFilePath = logFilePath;
            }
            
            
            _cancelSource = new CancellationTokenSource();
            console.CancelKeyPress += ConsoleOnCancelKeyPress;
            
            if (IsLogoOn) {
                Out.DrawLogo();
            }
            
            int exitCode = FatalExitCode;
            
            try {
                Log.Debug($"Starting execution: {DateTime.Now:yyyy MMM dd} @ {DateTime.Now:HH:mm:ss}.");
                ExecutePreCommand(app, console);
                exitCode = ExecuteCommand(app, console);
                ExecutePostCommand(app, console);
                if (exitCode.Equals(0)) {
                    Log.Done("Exit code 0");
                } else {
                    Log.Warn($"Exit code {exitCode}");
                }
                if (Verbosity < ConsoleLogThreshold.None) {
                    Out.WriteOnNewLine(null);    
                }
                return exitCode;
                
            } catch (Exception e) {
                Log.Error(e.Message, e);
                if (Verbosity > ConsoleLogThreshold.Debug) {
                    Log.Info($"Get more details on this error by switching to debug verbosity: {VerbosityShortName}.");
                }
                if (e is CommandException ce) {
                    exitCode = ce.ExitCode;
                }
            }
            Log.Fatal($"Exit code {exitCode}");
            if (Verbosity < ConsoleLogThreshold.None) {
                Out.WriteOnNewLine(null);
            }
            
            return exitCode;
        }
        
        /// <summary>
        /// Called when the options of the command line are not validated correctly.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public int OnValidationError(ValidationResult r) {
            var log = ConsoleIo.Singleton;
            var faultyMembers = string.Join(", ", r.MemberNames);
            log.Error($"{(faultyMembers.Length > 0 ? $"{faultyMembers} : ": "")}{r.ErrorMessage}");
            log.Info($"Specify {MainCommand.HelpLongName} for a list of available options and commands.");
            log.Fatal($"Exit code {FatalExitCode}");
            log.WriteOnNewLine(null);
            return FatalExitCode;
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
            Log.Warn(HelpGenerator.GetHelpProvideCommand(app));
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