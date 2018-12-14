#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConsoleImplementation.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.Utilities {
    public class ConsoleImplementation : IConsoleImplementation {

        #region singleton
        
        private static ConsoleImplementation _instance;
        private bool? _isConsoleFullFeatured;
        private bool? _isOutputRedirect;
        private bool _hasWindowWidth = true;

        /// <summary>
        /// A singleton instance of <see cref="ConsoleImplementation" />.
        /// </summary>
        public static ConsoleImplementation Singleton => _instance ?? (_instance = new ConsoleImplementation());

        private ConsoleImplementation() {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }

        #endregion

        /// <summary>
        /// <see cref="Console.CancelKeyPress"/>.
        /// </summary>
        public event ConsoleCancelEventHandler CancelKeyPress {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        /// <summary>
        /// <see cref="Console.Error"/>.
        /// </summary>
        public TextWriter Error => Console.Error;

        /// <summary>
        /// <see cref="Console.In"/>.
        /// </summary>
        public TextReader In => Console.In;

        /// <summary>
        /// <see cref="Console.Out"/>.
        /// </summary>
        public TextWriter Out => Console.Out;

        /// <summary>
        /// <see cref="Console.IsInputRedirected"/>.
        /// </summary>
        public bool IsInputRedirected => Console.IsInputRedirected;

        /// <summary>
        /// <see cref="Console.IsOutputRedirected"/>.
        /// </summary>
        public bool IsOutputRedirected => _isOutputRedirect ?? (_isOutputRedirect = Console.IsOutputRedirected).Value;

        /// <summary>
        /// <see cref="Console.IsErrorRedirected"/>.
        /// </summary>
        public bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <summary>
        /// <see cref="Console.ForegroundColor"/>.
        /// </summary>
        public ConsoleColor ForegroundColor {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <summary>
        /// <see cref="Console.BackgroundColor"/>.
        /// </summary>
        public ConsoleColor BackgroundColor {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public void Write(string text = null) {
            Out.Write(text);
        }

        public void WriteLine(string text = null) {
            Out.WriteLine(text);
        }

        /// <summary>
        /// <see cref="Console.ResetColor"/>.
        /// </summary>
        public void ResetColor() {
            Console.ResetColor();
        }

        /// <summary>
        /// <see cref="Console.CursorTop"/>.
        /// </summary>
        public int CursorTop {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }
        
        /// <summary>
        /// <see cref="Console.WindowWidth"/>.
        /// </summary>
        public int WindowWidth {
            get {
                try {
                    return _hasWindowWidth ? Console.WindowWidth : 0;
                } catch (IOException) {
                    _hasWindowWidth = false;
                    return 0;
                }
            }
            set => Console.WindowWidth = value;
        }
        
        /// <summary>
        /// <see cref="Console.CursorVisible"/>.
        /// </summary>
        public bool CursorVisible {
            get => Console.CursorVisible;
            set {
                try {
                    Console.CursorVisible = value;
                } catch (IOException) {
                    _isConsoleFullFeatured = false;
                }
            }
        }

        /// <summary>
        /// <see cref="Console.SetCursorPosition"/>.
        /// </summary>
        public void SetCursorPosition(int left, int top) {
            Console.SetCursorPosition(left, top);
        }

        public bool IsConsoleFullFeatured {
            get {
                if (!_isConsoleFullFeatured.HasValue) {
                    try {
                        Console.CursorVisible = Console.CursorVisible;
                        _isConsoleFullFeatured = true;
                    } catch (IOException) {
                        _isConsoleFullFeatured = false;
                    }
                }
                return _isConsoleFullFeatured.Value;
            }
        }
        
    }
}