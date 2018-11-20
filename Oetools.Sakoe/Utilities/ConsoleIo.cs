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
    
    public class ConsoleIo : IResultWriter, ILogger, ITraceLogger, IDisposable {

        private readonly LogLvl _logLevel;
        
        public ITraceLogger Trace { get; }

        private ProgressBarOptions _progressBarOptions;

        private ProgressBar _progressBar;

        private IConsole _console;

        private bool _isProgressBarOff;

        private bool _hasWroteToOuput;

        public ConsoleIo(IConsole console, LogLvl level, bool isProgressBarOff) {
            _console = console;
            _logLevel = level;
            _isProgressBarOff = isProgressBarOff;
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

        public void Done(string message, Exception e = null) {
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
            if (_isProgressBarOff) {
                return;
            }
            if (_console.IsOutputRedirected) {
                // cannot use the progress bar
                Log(LogLvl.Debug, $"{Math.Round((decimal) current / max * 100, 2)}% : {message}");
                return;
            }

            try {
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

                _hasWroteToOuput = false;
            } catch (Exception) {
                // ignored
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
                    _console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogLvl.Info:
                    _console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLvl.Done:
                    _console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLvl.Warn:
                    _console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLvl.Error:
                    _console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLvl.Fatal:
                    _console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            var outputMessage = $"{level.ToString().ToUpper().PadRight(5, ' ')} [{DateTime.Now:yy-MM-dd HH:mm:ss}] {message}";
            if (level >= LogLvl.Error) {
                WriteConsole(outputMessage, true, true);
            } else {
                WriteConsole(outputMessage, true);
            }

            if (e != null) {
                _console.ForegroundColor = ConsoleColor.DarkGray;
                WriteConsole(e.ToString(), true, true);
            }
            //_console.ResetColor();
        }

        public void WriteResult(string result) {
            _console.ForegroundColor = ConsoleColor.White;
            WriteConsole(result);
        }

        public void WriteResultOnNewLine(string result) {
            _console.ForegroundColor = ConsoleColor.White;
            WriteConsole(result, true);
        }

        private void WriteConsole(string message, bool newLine = false, bool writeToErrorStream = false) {
            if (newLine && _hasWroteToOuput) {
                (writeToErrorStream ? _console.Error : _console.Out).Write(_console.Out.NewLine);
            }
            if (!string.IsNullOrEmpty(message)) {
                (writeToErrorStream ? _console.Error : _console.Out).Write(message);
                _hasWroteToOuput = true;
            }
        }
        
        public enum LogLvl {
            Debug,
            Info,
            Done,
            Warn,
            Error,
            Fatal,
            None
        }

        public void Dispose() {
            _progressBar?.Dispose();
        }
    }
}