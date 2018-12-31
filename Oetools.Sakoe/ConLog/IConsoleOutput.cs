#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (IConsoleOutput.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.ConLog {
    
    public interface IConsoleOutput {
        
        /// <summary>
        /// Writes a result (no word wrap), appending to the existing line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        void WriteResult(string text, ConsoleColor? color = null);
        
        /// <summary>
        /// Writes a result (no word wrap) on a new line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        void WriteResultOnNewLine(string text, ConsoleColor? color = null);

        /// <summary>
        /// Writes text, appending to the current line. Has word wrap.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="indentation">Apply indentation when writing on a new line.</param>
        /// <param name="prefixForNewLines">The text to put at the beginning of each new line that need to be created because of word wrap.</param>
        void Write(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null);
        
        /// <summary>
        /// Writes text on a new line. Has word wrap.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="indentation">Apply indentation when writing on a new line.</param>
        /// <param name="prefixForNewLines">The text to put at the beginning of each new line that need to be created because of word wrap.</param>
        void WriteOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null);
        
        /// <summary>
        /// Writes a new error, appending to the current line. Has word wrap.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="indentation">Apply indentation when writing on a new line.</param>
        /// <param name="prefixForNewLines">The text to put at the beginning of each new line that need to be created because of word wrap.</param>
        void WriteError(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null);
        
        /// <summary>
        /// Writes an error on a new line. Has word wrap.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="indentation">Apply indentation when writing on a new line.</param>
        /// <param name="prefixForNewLines">The text to put at the beginning of each new line that need to be created because of word wrap.</param>
        void WriteErrorOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null);

        /// <summary>
        /// Starts a new "tree" node. Write the next text as a node and subsequent texts as children of this node.
        /// </summary>
        /// <returns></returns>
        IConsoleOutput PushNode(bool isLastChild = false);
        
        /// <summary>
        /// Ends a "tree" node. The next text will wrote at the same level as the current node.
        /// </summary>
        /// <returns></returns>
        IConsoleOutput PopNode();
        
        /// <summary>
        /// Set or get the text writer to write to.
        /// </summary>
        TextWriter OutputTextWriter { get; set; }
    }
}