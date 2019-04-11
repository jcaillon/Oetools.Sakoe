#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConsoleLogger.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.ConLog {

    public class ConsoleLogger : ConsoleOutput, ILogger, ITraceLogger {

        private static ConsoleLogger _instance;

        /// <summary>
        /// A singleton instance of <see cref="ConsoleLogger" />.
        /// </summary>
        public static ConsoleLogger Singleton => _instance ?? (_instance = new ConsoleLogger(ConsoleImplementation.Singleton));

        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleLogger"/>.
        /// </summary>
        protected ConsoleLogger(IConsoleImplementation console) : base(console) {
            _stopwatch = Stopwatch.StartNew();
            _console = console;
        }

        private readonly IConsoleImplementation _console;
        protected Stopwatch _stopwatch;
        private ConsoleProgressBar _progressBar;

        /// <summary>
        /// Sets and gets the current threshold at which to start logging events.
        /// </summary>
        public ConsoleLogThreshold LogTheshold { get; set; } = ConsoleLogThreshold.Info;

        /// <inheritdoc />
        public ITraceLogger Trace => LogTheshold <= ConsoleLogThreshold.Debug ? this : null;

        /// <summary>
        /// Progress bar display mode.
        /// </summary>
        public ConsoleProgressBarDisplayMode ProgressBarDisplayMode { get; set; }

        /// <inheritdoc />
        public override void Dispose() {
            base.Dispose();
            _progressBar?.Dispose();
            _progressBar = null;
        }

        /// <inheritdoc />
        public void Fatal(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Fatal, message, e);
        }

        /// <inheritdoc />
        public void Error(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Error, message, e);
        }

        /// <inheritdoc />
        public void Warn(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Warn, message, e);
        }

        /// <inheritdoc />
        public void Info(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Info, message, e);
        }

        /// <inheritdoc />
        public void Done(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Done, message, e);
        }

        /// <inheritdoc />
        public void Debug(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Debug, message, e);
        }

        /// <summary>
        /// Writes in debug level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Write(string message, Exception e = null) {
            Log(ConsoleLogThreshold.Debug, message, e);
        }

        /// <inheritdoc />
        public ILogger If(bool condition) => condition ? this : null;

        /// <inheritdoc cref="ILogger.ReportProgress"/>
        public virtual void ReportProgress(int max, int current, string message) {
            if (ProgressBarDisplayMode == ConsoleProgressBarDisplayMode.Off) {
                return;
            }

            if (_console.IsOutputRedirected) {
                // cannot use the progress bar
                Debug($"[{$"{(int) Math.Round((decimal) current / max * 100, 2)}%".PadLeft(4)}] {message}");
                return;
            }


            try {
                if (_progressBar == null) {
                    _progressBar = new ConsoleProgressBar(_console, max, message) {
                        ClearProgressBarOnStop = ProgressBarDisplayMode == ConsoleProgressBarDisplayMode.On,
                        TextColor = ConsoleColor.Gray
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
                HasWroteToOuput = false;
            }
        }

        /// <inheritdoc />
        public void ReportGlobalProgress(int max, int current, string message) {
            Log(ConsoleLogThreshold.Info, $"{message} ({(int) Math.Round((decimal) current / max * 100, 2)}%)");
        }

        /// <inheritdoc />
        public override void WriteResult(string text, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResult(text, color);
        }

        /// <inheritdoc />
        public override void WriteResultOnNewLine(string text, ConsoleColor? color = null) {
            StopProgressBar();
            base.WriteResultOnNewLine(text, color);
        }

        /// <inheritdoc />
        public override void Write(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            StopProgressBar();
            base.Write(text, color, indentation, prefixForNewLines);
        }

        /// <inheritdoc />
        public override void WriteOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            StopProgressBar();
            base.WriteOnNewLine(text, color, indentation, prefixForNewLines);
        }

        /// <inheritdoc />
        public override void WriteError(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            StopProgressBar();
            base.WriteError(text, color, indentation, prefixForNewLines);
        }

        /// <inheritdoc />
        public override void WriteErrorOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            StopProgressBar();
            base.WriteErrorOnNewLine(text, color, indentation, prefixForNewLines);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual void Log(ConsoleLogThreshold level, string message, Exception e = null) {
            if (level < LogTheshold) {
                return;
            }
            StopProgressBar();

            var elapsed = _stopwatch.Elapsed;
            var logPrefix = $"{level.ToString().ToUpper().PadRight(5, ' ')} [{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}] ";

            ConsoleColor outputColor;
            switch (level) {
                case ConsoleLogThreshold.Debug:
                    outputColor = ConsoleColor.Gray;
                    break;
                case ConsoleLogThreshold.Info:
                    outputColor = ConsoleColor.Cyan;
                    break;
                case ConsoleLogThreshold.Done:
                    outputColor = ConsoleColor.Green;
                    break;
                case ConsoleLogThreshold.Warn:
                    outputColor = ConsoleColor.Yellow;
                    break;
                case ConsoleLogThreshold.Error:
                    outputColor = ConsoleColor.Red;
                    break;
                case ConsoleLogThreshold.Fatal:
                    outputColor = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            if (e != null && LogTheshold <= ConsoleLogThreshold.Debug) {
                base.WriteErrorOnNewLine(logPrefix, ConsoleColor.Gray);
                base.WriteError(e.ToString(), ConsoleColor.Gray, logPrefix.Length);
            }
            if (level >= ConsoleLogThreshold.Error) {
                base.WriteErrorOnNewLine(logPrefix, outputColor);
                base.WriteError(message, outputColor, logPrefix.Length);
            } else {
                base.WriteOnNewLine(logPrefix, outputColor);
                base.Write(message, outputColor, logPrefix.Length);
            }
        }
    }
}
