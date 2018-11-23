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
using Oetools.Sakoe.ShellProgressBar;

namespace Oetools.Sakoe.Utilities {
    public class ConsoleIo : ConsoleOutputWrapText, IResultWriter, ILogger, ITraceLogger, IDisposable {
        private readonly LogLvl _logLevel;

        public ITraceLogger Trace { get; }

        private ProgressBarOptions _progressBarOptions;

        private ProgressBar _progressBar;

        private IConsole _console;

        private bool _isProgressBarOff;

        private Stopwatch _stopwatch;

        public ConsoleIo(IConsole console, LogLvl level, bool isProgressBarOff) {
            _stopwatch = Stopwatch.StartNew();
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
            
            var textWidth = Console.WindowWidth - 1;

            try {
                if (_progressBar == null) {
                    _progressBar = new ProgressBar(max, message, _progressBarOptions);
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

        public void ReportGlobalProgress(int max, int current, string message) {
            Log(LogLvl.Info, $"global progress : {message}");
        }
        
        public void WriteResult(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? ConsoleColor.White;
            WriteToConsoleWithWordWrap(_console.Out, result, false, padding);
        }

        public void WriteResultOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? ConsoleColor.White;
            WriteToConsoleWithWordWrap(_console.Out, result, true, padding);
        }

        public void WriteNewLine() {
            WriteNewLine(_console.Out);
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

        private void StopProgressBar() {
            _progressBar?.Dispose();
            _progressBar = null;
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

            //_console.ResetColor();
        }

        public void Dispose() {
            _progressBar?.Dispose();
        }
    }
}