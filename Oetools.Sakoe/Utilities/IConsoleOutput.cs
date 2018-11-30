#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (IResultWriter.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.Utilities {
    public interface IConsoleOutput {
        
        /// <summary>
        /// Writes a result (no word wrap), appending to the existing line.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        void WriteResult(string result, ConsoleColor? color = null);
        
        /// <summary>
        /// Writes a result (no word wrap) on a new line.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        void WriteResultOnNewLine(string result, ConsoleColor? color = null);
        
        /// <summary>
        /// Writes text, appending to the current line. Has word wrap.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        /// <param name="padding"></param>
        void Write(string result, ConsoleColor? color = null, int padding = 0);
        
        /// <summary>
        /// Writes text on a new line. Has word wrap.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        /// <param name="padding"></param>
        void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0);
        
        /// <summary>
        /// Writes a new error, appending to the current line. Has word wrap.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        /// <param name="padding"></param>
        void WriteError(string result, ConsoleColor? color = null, int padding = 0);
        
        /// <summary>
        /// Writes an error on a new line. Has word wrap.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="color"></param>
        /// <param name="padding"></param>
        void WriteErrorOnNewLine(string result, ConsoleColor? color = null, int padding = 0);
        
        /// <summary>
        /// Draw the logo.
        /// </summary>
        void DrawLogo();
        
        TextWriter OutputTextWriter { get; set; }
    }
}