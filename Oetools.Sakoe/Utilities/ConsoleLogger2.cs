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
using System.IO;
using System.Text;
using Oetools.Sakoe.ConLog;
using Oetools.Utilities.Lib.Extension;
using ILogger = Oetools.Builder.Utilities.ILogger;
using ITraceLogger = Oetools.Builder.Utilities.ITraceLogger;
using Utils = Oetools.Utilities.Lib.Utils;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleLogger2 : ConsoleLogger, ILogger, ITraceLogger {

        private static ConsoleLogger2 _instance;

        /// <summary>
        /// A singleton instance of <see cref="ConsoleLogger2" />.
        /// </summary>
        public new static ConsoleLogger2 Singleton => _instance ?? (_instance = new ConsoleLogger2(ConsoleImplementation.Singleton));

        /// <summary>
        /// Initializes a new instance of <see cref="ConsoleLogger2"/>.
        /// </summary>
        private ConsoleLogger2(IConsoleImplementation console) : base(console) { }
        
        private StringBuilder _logContent;
        private string _logOutputFilePath;

        /// <inheritdoc />
        public new ITraceLogger Trace => LogTheshold <= ConsoleLogThreshold.Debug ? this : null;
        
        /// <inheritdoc />
        public new ILogger If(bool condition) => condition ? this : null;

        /// <inheritdoc />
        public override void Dispose() {
            base.Dispose();
            FlushLogToFile();
        }

        /// <inheritdoc cref="ILogger.ReportProgress"/>
        public override void ReportProgress(int max, int current, string message) {
            LogToFile(ConsoleLogThreshold.Debug, $"[{$"{(int) Math.Round((decimal) current / max * 100, 2)}%".PadLeft(4)}] {message}", null);
            base.ReportProgress(max, current, message);
        }

        /// <inheritdoc />
        protected override void Log(ConsoleLogThreshold level, string message, Exception e = null) {
            LogToFile(level, message, e);
            base.Log(level, message, e);
        }

        /// <summary>
        /// Sets or gets the path to a log file where every event will be written.
        /// </summary>
        /// <exception cref="Exception"></exception>
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
                _logContent.AppendLine("============================================");
                _logContent.AppendLine($"========= NEW LOG SESSION {DateTime.Now:yy-MM-dd} =========");
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