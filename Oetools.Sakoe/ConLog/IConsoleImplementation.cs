#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (IConsoleImplementation.cs) is part of Oetools.Sakoe.
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
    
    
    public interface IConsoleImplementation {
        
        /// <summary>
        /// <see cref="Console.CancelKeyPress"/>.
        /// </summary>
        event ConsoleCancelEventHandler CancelKeyPress;

        /// <summary>
        /// <see cref="Console.Out"/>.
        /// </summary>
        TextWriter Out { get; }

        /// <summary>
        /// <see cref="Console.Error"/>.
        /// </summary>
        TextWriter Error { get; }

        /// <summary>
        /// <see cref="Console.In"/>.
        /// </summary>
        TextReader In { get; }

        /// <summary>
        /// <see cref="Console.IsInputRedirected"/>.
        /// </summary>
        bool IsInputRedirected { get; }

        /// <summary>
        /// <see cref="Console.IsOutputRedirected"/>.
        /// </summary>
        bool IsOutputRedirected { get; }

        /// <summary>
        /// <see cref="Console.IsErrorRedirected"/>.
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// <see cref="Console.ForegroundColor"/>.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// <see cref="Console.BackgroundColor"/>.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Resets <see cref="ForegroundColor" /> and <see cref="BackgroundColor" />.
        /// </summary>
        void ResetColor();
        
        /// <summary>
        /// <see cref="Console.CursorTop"/>.
        /// </summary>
        int CursorTop { get; set; }
        
        /// <summary>
        /// <see cref="Console.WindowWidth"/>.
        /// </summary>
        int WindowWidth { get; set; }
        
        /// <summary>
        /// <see cref="Console.CursorVisible"/>.
        /// </summary>
        bool CursorVisible { get; set; }
        
        /// <summary>
        /// <see cref="Console.SetCursorPosition"/>.
        /// </summary>
        void SetCursorPosition(int left, int top);
        
        /// <summary>
        /// Write text.
        /// </summary>
        void Write(string text = null);
        
        /// <summary>
        /// Write a line.
        /// </summary>
        void WriteLine(string text = null);
        
    }
}