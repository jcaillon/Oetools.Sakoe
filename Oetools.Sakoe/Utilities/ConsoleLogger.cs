#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (Logger.cs) is part of Oetools.Sakoe.
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
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.ShellProgressBar;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleLogger : ILogger, ITraceLogger, IDisposable {

        private readonly LogLvl _logLevel;
        
        public ITraceLogger Trace { get; }

        private ProgressBarOptions _progressBarOptions;

        private ProgressBar _progressBar;

        private IConsole _console;

        public ConsoleLogger(IConsole console, LogLvl level = LogLvl.Info) {
            _console = console;
            _logLevel = level;
            if (_logLevel <= LogLvl.Debug) {
                Trace = this;
            }
            _progressBarOptions = new ProgressBarOptions {
                ForegroundColor = ConsoleColor.Cyan,
                ForegroundColorDone = ConsoleColor.DarkGray,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = true,
                EnableTaskBarProgress = true,
                CollapseWhenFinished = true,
                ProgressBarOnBottom = false,
                ProgressCharacter = '\u2593'
            };
        }

        public void Fatal(string message, Exception e = null) {
            Log(LogLvl.Fatal, message, e);
        }

        public void Error(string message, Exception e = null) {
            Log(LogLvl.Error, message, e);
        }

        public void Warn(string message, Exception e = null) {
            Log(LogLvl.Warn, message, e);
        }

        public void Info(string message, Exception e = null) {
            Log(LogLvl.Info, message, e);
        }

        public void Success(string message, Exception e = null) {
            Log(LogLvl.Done, message, e);
        }

        public void Debug(string message, Exception e = null) {
            Log(LogLvl.Debug, message, e);
        }

        /// <summary>
        /// Writes in debug level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Write(string message, Exception e = null) {
            Log(LogLvl.Debug, message, e);
        }

        public void ReportProgress(int max, int current, string message) {
            if (_progressBar == null) {
                _progressBar = new ProgressBar(max, message, _progressBarOptions);
            }
            if (max > 0 && max != _progressBar.MaxTicks) {
                _progressBar.MaxTicks = max;
            }
            _progressBar.Tick(current, message);
            if (max == current) {
                _progressBar.Dispose();
                _progressBar = null;
            }
        }

        public void ReportGlobalProgress(int max, int current, string message) {
            Log(LogLvl.Info, $"global progress : {message}");
        }

        private void Log(LogLvl level, string message, Exception e = null) {
            _progressBar?.Dispose();
            _progressBar = null;

            if (level < _logLevel) {
                return;
            }

            switch (level) {
                case LogLvl.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogLvl.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLvl.Done:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLvl.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLvl.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogLvl.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            _console.WriteLine($"{level.ToString().ToUpper().PadRight(5, ' ')} [{DateTime.Now:dd.MM.yy HH:mm:ss}] {message}");
            if (e != null) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(e.ToString());
            }
            //_console.ResetColor();
        }
        
        public enum LogLvl {
            Debug,
            Info,
            Done,
            Warn,
            Error,
            Fatal
        }

        public void Dispose() {
            _progressBar?.Dispose();
        }
    }
}