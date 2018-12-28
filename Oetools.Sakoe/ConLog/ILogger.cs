#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ITrace.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.ConLog {
    
    /// <summary>
    /// Log stuff happening in the program.
    /// </summary>
    public interface ILogger {
        
        /// <summary>
        /// Log a fatal error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Fatal(string message, Exception e = null);
        
        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Error(string message, Exception e = null);
        
        /// <summary>
        /// Log a warning.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Warn(string message, Exception e = null);
        
        /// <summary>
        /// Log an information.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Info(string message, Exception e = null);
        
        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Debug(string message, Exception e = null);
        
        /// <summary>
        /// Log a thing done.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Done(string message, Exception e = null);
        
        /// <summary>
        /// Returns a trace logger that allows to write debug traces. Only if debug mode is active.
        /// </summary>
        ITraceLogger Trace { get; }
        
        /// <summary>
        /// Returns this <see cref="ILogger"/> if the <paramref name="condition"/> is true, null otherwise.
        /// Allows to do stuff like:
        /// <code>Log?.If(true)?.Info("");</code>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        ILogger If(bool condition);
        
        /// <summary>
        /// Reports a "local" progress of the program.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="current"></param>
        /// <param name="message"></param>
        void ReportProgress(int max, int current, string message);
        
        /// <summary>
        /// Reports the global progress of the the program.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="current"></param>
        /// <param name="message"></param>
        void ReportGlobalProgress(int max, int current, string message);
    }
}