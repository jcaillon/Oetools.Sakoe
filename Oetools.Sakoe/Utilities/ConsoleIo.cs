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
using System.Globalization;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleIo : ConsoleOutput, ILogger, ITraceLogger {
        
        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static ConsoleIo Singleton => _instance ?? (_instance = new ConsoleIo(PhysicalConsole.Singleton));

        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleIo"/>.
        /// </summary>
        private ConsoleIo(IConsole console) : base(console) {
            _stopwatch = Stopwatch.StartNew();
            _console = console;
        }

        private static ConsoleIo _instance;
        
        private Stopwatch _stopwatch;

        private ConsoleProgressBar _progressBar;
        
        private LogLvl _logLevel = LogLvl.Info;     
        
        public LogLvl LogLevel {
            get => _logLevel;
            set {
                _logLevel = value;
                if (LogLevel <= LogLvl.Debug) {
                    Trace = this;
                } else {
                    Trace = null;
                }
            }
        }
        
        /// <inheritdoc />
        public ITraceLogger Trace { get; private set; }

        public bool IsProgressBarOff { get; set; }

        public override void Dispose() {
            _progressBar?.Dispose();
            _progressBar = null;
            base.Dispose();
        }
        
        /// <inheritdoc />
        public void Fatal(string message, Exception e = null) {
            Log(LogLvl.Fatal, message, e);
        }

        /// <inheritdoc />
        public void Error(string message, Exception e = null) {
            Log(LogLvl.Error, message, e);
        }

        /// <inheritdoc />
        public void Warn(string message, Exception e = null) {
            Log(LogLvl.Warn, message, e);
        }
        
        /// <inheritdoc />
        public void Info(string message, Exception e = null) {
            Log(LogLvl.Info, message, e);
        }

        /// <inheritdoc />
        public void Done(string message, Exception e = null) {
            Log(LogLvl.Done, message, e);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public ILogger If(bool condition) {
            return condition ? this : null;
        }

        /// <inheritdoc cref="ILogger.ReportProgress"/>
        public void ReportProgress(int max, int current, string message) {
            if (IsProgressBarOff) {
                return;
            }

            if (_console.IsOutputRedirected) {
                // cannot use the progress bar
                Log(LogLvl.Debug, $"{$"{Math.Round((decimal) current / max * 100, 2)}%".PadLeft(4)} {message}");
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
                    base.WriteResultOnNewLine(null);
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
                WordWrapWriter.HasWroteToOuput = false;
            }
        }

        /// <inheritdoc />
        public void ReportGlobalProgress(int max, int current, string message) {
            Log(LogLvl.Info, $"@ {$"{Math.Round((decimal) current / max * 100, 2)}%".PadRight(4)} - {message}");
        }

        /// <inheritdoc />
        public override void WriteResult(string result, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResult(result, color);
        }

        /// <inheritdoc />
        public override void WriteResultOnNewLine(string result, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResultOnNewLine(result, color);
        }
        
        /// <inheritdoc />
        public override void Write(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.Write(result, color, padding);
        }

        /// <inheritdoc />
        public override void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.WriteOnNewLine(result, color, padding);
        }
        
        /// <inheritdoc />
        public override void WriteError(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.WriteError(result, color, padding);
        }

        /// <inheritdoc />
        public override void WriteErrorOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            StopProgressBar();
            base.WriteErrorOnNewLine(result, color, padding);
        }

        /// <inheritdoc />
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

            if (level < LogLevel) {
                return;
            }

            ConsoleColor outputColor;
            switch (level) {
                case LogLvl.Debug:
                    outputColor = ConsoleColor.DarkGray;
                    break;
                case LogLvl.Info:
                    outputColor = ConsoleColor.Cyan;
                    break;
                case LogLvl.Done:
                    outputColor = ConsoleColor.Green;
                    break;
                case LogLvl.Warn:
                    outputColor = ConsoleColor.Yellow;
                    break;
                case LogLvl.Error:
                    outputColor = ConsoleColor.Red;
                    break;
                case LogLvl.Fatal:
                    outputColor = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            var elapsed = _stopwatch.Elapsed;
            var outputMessage = $"{level.ToString().ToUpper().PadRight(5, ' ')} [{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}] {message}";
            if (level >= LogLvl.Error) {
                base.WriteErrorOnNewLine(outputMessage, outputColor);
            } else {
                base.WriteOnNewLine(outputMessage, outputColor);
            }

            if (e != null && LogLevel <= LogLvl.Debug) {
                base.WriteErrorOnNewLine(e.ToString(), ConsoleColor.DarkGray);
            }
        }
    }
}