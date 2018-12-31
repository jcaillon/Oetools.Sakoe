#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ITraceLogger.cs) is part of Oetools.Sakoe.
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
    /// A debug/trace logger.
    /// </summary>
    public interface ITraceLogger {
        
        /// <summary>
        /// Write the message to the debug log.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void Write(string message, Exception e = null);
        
        /// <summary>
        /// Report progress.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="current"></param>
        /// <param name="message"></param>
        void ReportProgress(int max, int current, string message);
    }
}