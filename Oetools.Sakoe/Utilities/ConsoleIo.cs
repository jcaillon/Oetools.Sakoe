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
using System.IO;
using System.Text;
using Oetools.Builder.Utilities;
using Oetools.Utilities.Lib.Extension;
using Utils = Oetools.Utilities.Lib.Utils;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleIo : ConsoleOutput, ILogger, ITraceLogger {

        #region singleton

        private static ConsoleIo _instance;

        /// <summary>
        /// A singleton instance of <see cref="HelpGenerator" />.
        /// </summary>
        public static ConsoleIo Singleton => _instance ?? (_instance = new ConsoleIo(ConsoleImplementation.Singleton));

        #endregion

        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleIo"/>.
        /// </summary>
        private ConsoleIo(IConsoleImplementation console) : base(console) {
            _stopwatch = Stopwatch.StartNew();
            _console = console;
        }
        
        private readonly IConsoleImplementation _console;
        
        private Stopwatch _stopwatch;

        private ConsoleProgressBar _progressBar;
        
        private ConsoleLogThreshold _logThreshold = ConsoleLogThreshold.Info;

        private StringBuilder _logContent;
        private string _logOutputFilePath;

        public ConsoleLogThreshold LogTheshold {
            get => _logThreshold;
            set {
                _logThreshold = value;
                if (LogTheshold <= ConsoleLogThreshold.Debug) {
                    Trace = this;
                } else {
                    Trace = null;
                }
            }
        }

        public string LogOutputFilePath {
            get => _logOutputFilePath;
            set {
                _logOutputFilePath = value;
                if (!Utils.IsPathRooted(_logOutputFilePath)) {
                    _logOutputFilePath = Path.Combine(Directory.GetCurrentDirectory(), _logOutputFilePath);
                }
                const int maxSizeInMo = 100;
                if (File.Exists(_logOutputFilePath)) {
                    if (new FileInfo(_logOutputFilePath).Length > maxSizeInMo * 1024 * 1024) {
                        Warn($"The log file has a size superior to {maxSizeInMo}MB, please consider clearing it: {_logOutputFilePath.PrettyQuote()}.");
                    }
                } else {
                    try {
                        var dirName = Path.GetDirectoryName(_logOutputFilePath);
                        if (!Directory.Exists(dirName)) {
                            Directory.CreateDirectory(dirName);
                        }
                        File.WriteAllText(_logOutputFilePath, "");
                    } catch (Exception e) {
                        throw new Exception($"Could not create the log file: {_logOutputFilePath.PrettyQuote()}. {e.Message}", e);
                    }
                }
                Info($"Logging to file: {_logOutputFilePath.PrettyQuote()}.");
                _logContent = new StringBuilder();
                _logContent.AppendLine("===================================");
                _logContent.AppendLine("========= NEW LOG SESSION =========");
            }
        }

        /// <inheritdoc />
        public ITraceLogger Trace { get; private set; }

        /// <summary>
        /// Progress bar display mode.
        /// </summary>
        public ConsoleProgressBarDisplayMode ProgressBarDisplayMode { get; set; }

        public override void Dispose() {
            base.Dispose();
            _progressBar?.Dispose();
            _progressBar = null;
            FlushLogToFile();
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
        public ILogger If(bool condition) {
            return condition ? this : null;
        }

        /// <inheritdoc cref="ILogger.ReportProgress"/>
        public void ReportProgress(int max, int current, string message) {
            LogToFile(ConsoleLogThreshold.Debug, $"[{$"{(int) Math.Round((decimal) current / max * 100, 2)}%".PadLeft(4)}] {message}", null);
            
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


        private void Log(ConsoleLogThreshold level, string message, Exception e = null) {
            StopProgressBar();           
            
            LogToFile(level, message, e);

            if (level < LogTheshold) {
                return;
            }
            var elapsed = _stopwatch.Elapsed;
            var logPrefix = $"{level.ToString().ToUpper().PadRight(5, ' ')} [{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}] ";
            
            ConsoleColor outputColor;
            switch (level) {
                case ConsoleLogThreshold.Debug:
                    outputColor = ConsoleColor.DarkGray;
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

            if (level >= ConsoleLogThreshold.Error) {
                base.WriteErrorOnNewLine(logPrefix, outputColor);
                base.WriteError(message, outputColor, logPrefix.Length);
            } else {
                base.WriteOnNewLine(logPrefix, outputColor);
                base.Write(message, outputColor, logPrefix.Length);
            }
            if (e != null && LogTheshold <= ConsoleLogThreshold.Debug) {
                base.WriteErrorOnNewLine(e.ToString(), ConsoleColor.DarkGray, logPrefix.Length);
            }
        }

        private void FlushLogToFile() {
            if (string.IsNullOrEmpty(LogOutputFilePath) || _logContent == null) {
                return;
            }
            File.AppendAllText(LogOutputFilePath, _logContent.ToString());
            _logContent.Clear();
        }

        private void LogToFile(ConsoleLogThreshold level, string message, Exception e) {
            if (_logContent != null) {
                var elapsed = _stopwatch.Elapsed;
                _logContent.AppendLine($"{level.ToString().ToUpper().PadRight(5, ' ')} [{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}] {message}");
                if (e != null) {
                    _logContent.AppendLine(e.ToString());
                }
                if (_logContent.Length > 100000) {
                    FlushLogToFile();
                }
            }
        }
        
    }
}