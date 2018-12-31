#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConsoleLogThreshold.cs) is part of Oetools.Sakoe.
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
namespace Oetools.Sakoe.ConLog {
    
    /// <summary>
    /// The threshold above which to log events.
    /// </summary>
    public enum ConsoleLogThreshold {
        
        /// <summary>
        /// Min threshold, log everything.
        /// </summary>
        Debug,
        
        /// <summary>
        /// Log info.
        /// </summary>
        Info,
        
        /// <summary>
        /// Log done.
        /// </summary>
        Done,
        
        /// <summary>
        /// Log warn.
        /// </summary>
        Warn,
        
        /// <summary>
        /// Log errors.
        /// </summary>
        Error,
        
        /// <summary>
        /// Log fatal errors.
        /// </summary>
        Fatal,
        
        /// <summary>
        /// Max threshold level, logs nothing.
        /// </summary>
        None
    }
}