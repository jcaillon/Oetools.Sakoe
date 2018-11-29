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
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleIo : ConsoleOutput, ILogger, ITraceLogger, IDisposable {
        
        private readonly LogLvl _logLevel;

        public ITraceLogger Trace { get; }

        private bool _isProgressBarOff;

        private Stopwatch _stopwatch;

        private ConsoleProgressBar _progressBar;

        public ConsoleIo(IConsole console, LogLvl level, bool isProgressBarOff) : base(console) {
            _stopwatch = Stopwatch.StartNew();
            _console = console;
            _logLevel = level;
            _isProgressBarOff = isProgressBarOff;
            if (_logLevel <= LogLvl.Debug) {
                Trace = this;
            }
        }

        public void Dispose() {
            _progressBar?.Dispose();
            _console.ResetColor();
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
                    _progressBar = new ConsoleProgressBar(max, message) {
                        ClearProgressBarOnStop = true,
                        TextColor = ConsoleColor.DarkGray
                    };
                }
                if (!_progressBar.IsRunning) {
                    WriteNewLine();
                }
                if (max > 0 && max != _progressBar.MaxTicks) {
                    _progressBar.MaxTicks = max;
                }
                _progressBar.Tick(current, message);
                if (max == current) {
                    StopProgressBar();
                }
            } catch (Exception) {
                // ignored
            }
        }

        private void StopProgressBar() {
            if (_progressBar?.Stop() ?? false) {
                _hasWroteToOuput = false;
            }
        }

        public void ReportGlobalProgress(int max, int current, string message) {
            Log(LogLvl.Info, $"global progress : {message}");
        }

        public override void WriteResult(string result, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResult(result, color);
        }

        public override void WriteResultOnNewLine(string result, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResultOnNewLine(result, color);
        }
        
        public override void Write(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.Write(result, color, padding);
        }

        public override void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.WriteOnNewLine(result, color, padding);
        }

        protected override void WriteNewLine() {
            StopProgressBar();
            base.WriteNewLine();
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

        private void Log(LogLvl level, string message, Exception e = null) {
            StopProgressBar();

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

            var elapsed = _stopwatch.Elapsed;
            var outputMessage = $"{level.ToString().ToUpper().PadRight(5, ' ')} [{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}] {message}";
            if (level >= LogLvl.Error) {
                WriteToConsoleWithWordWrap(_console.Error, outputMessage, true, 0);
            } else {
                WriteToConsoleWithWordWrap(_console.Out, outputMessage, true, 0);
            }

            if (e != null) {
                _console.ForegroundColor = ConsoleColor.DarkGray;
                WriteToConsoleWithWordWrap(_console.Error, e.ToString(), true, 0);
            }
        }
    }
}