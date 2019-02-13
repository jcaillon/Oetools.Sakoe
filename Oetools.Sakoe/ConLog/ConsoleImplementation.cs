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

namespace Oetools.Sakoe.ConLog {

    /// <summary>
    /// A wrapper around <see cref="Console"/> implementing <see cref="IConsoleImplementation"/>.
    /// </summary>
    public class ConsoleImplementation : IConsoleImplementation {

        private static ConsoleImplementation _instance;

        /// <summary>
        /// A singleton instance of <see cref="ConsoleImplementation" />.
        /// </summary>
        public static ConsoleImplementation Singleton => _instance ?? (_instance = new ConsoleImplementation());

        private bool? _isOutputRedirect;
        private bool _hasWindowWidth = true;

        protected ConsoleImplementation() {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }

        /// <inheritdoc />
        public event ConsoleCancelEventHandler CancelKeyPress {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        /// <inheritdoc />
        public TextWriter Error => Console.Error;

        /// <inheritdoc />
        public TextReader In => Console.In;

        /// <inheritdoc />
        public TextWriter Out => Console.Out;

        /// <inheritdoc />
        public bool IsInputRedirected => Console.IsInputRedirected;

        /// <inheritdoc />
        public bool IsOutputRedirected => _isOutputRedirect ?? (_isOutputRedirect = Console.IsOutputRedirected).Value;

        /// <inheritdoc />
        public bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <inheritdoc />
        public ConsoleColor ForegroundColor {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc />
        public ConsoleColor BackgroundColor {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc />
        public void Write(string text = null) {
            Out.Write(text);
        }

        /// <inheritdoc />
        public void WriteLine(string text = null) {
            Out.WriteLine(text);
        }

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey(bool intercept = true) => Console.ReadKey(intercept);

        /// <inheritdoc />
        public bool KeyAvailable => Console.KeyAvailable;

        /// <inheritdoc />
        public void ResetColor() {
            Console.ResetColor();
        }

        /// <inheritdoc />
        public int CursorTop {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool CursorVisible {
            get {
                try {
                    return Console.CursorVisible;
                } catch (Exception) {
                    return false;
                }
            }
            set {
                try {
                    Console.CursorVisible = value;
                } catch (Exception) {
                    // ignored.
                }
            }
        }

        /// <inheritdoc />
        public void SetCursorPosition(int left, int top) {
            Console.SetCursorPosition(left, top);
        }

    }
}
