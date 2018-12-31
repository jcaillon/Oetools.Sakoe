#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConsoleOutput.cs) is part of Oetools.Sakoe.
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
    
    public class ConsoleOutput : IConsoleOutput, IDisposable {
        
        // TODO: add interface to draw a tree

        private ConsoleColor _originalForegroundColor;
        private readonly TextWriterOutputWordWrap _wordWrapWriter;
        private readonly IConsoleImplementation _console;

        private int _treeLevel;
        private string _treeNewLinePrefix;
        private bool _pushNewNode;

        /// <inheritdoc />
        public TextWriter OutputTextWriter {
            get => _wordWrapWriter.UnderLyingWriter;
            set => _wordWrapWriter.UnderLyingWriter = value ?? _console.Out;
        }

        /// <inheritdoc cref="TextWriterOutputWordWrap.HasWroteToOuput"/>
        protected bool HasWroteToOuput {
            get => _wordWrapWriter.HasWroteToOuput;
            set => _wordWrapWriter.HasWroteToOuput = value;
        }

        protected ConsoleOutput(IConsoleImplementation console) {
            _console = console;
            _console.ResetColor();
            _originalForegroundColor = _console.ForegroundColor;
            _wordWrapWriter = new TextWriterOutputWordWrap(_console.Out);
        }

        /// <summary>
        /// Disposable implementation.
        /// </summary>
        public virtual void Dispose() {
            _console.ResetColor();
        }
        
        /// <inheritdoc />
        public virtual void WriteResult(string text, ConsoleColor? color = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(text, 0, false);
        }

        /// <inheritdoc />
        public virtual void WriteResultOnNewLine(string text, ConsoleColor? color = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(text, 0, true);
        }
        
        /// <inheritdoc />
        public virtual void Write(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(text, _console.IsOutputRedirected ? 0 : _console.WindowWidth, false, indentation, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(GetText(text), _console.IsOutputRedirected ? 0 : _console.WindowWidth, true, indentation, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteError(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(text, _console.IsOutputRedirected ? 0 : _console.WindowWidth, false, indentation, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteErrorOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            _wordWrapWriter.Write(GetText(text), _console.IsOutputRedirected ? 0 : _console.WindowWidth, true, indentation, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        private string GetText(string text) {
            if (_treeLevel > 0 || _pushNewNode) {
                string prefix = null;
                for (int j = 0; j < _treeLevel - 1; j++) {
                    prefix = $"│  {prefix}";
                }   
                if (_pushNewNode) {
                    _pushNewNode = false;
                    _treeLevel++;
                    if (_treeLevel > 1) {
                        text = $"{prefix}├─ {text}";
                        _treeNewLinePrefix = $"{prefix}│  │  ";
                    } else {
                        _treeNewLinePrefix = $"{prefix}│  ";
                    }
                } else {
                    text = $"{prefix}│  {text}";
                }
            }
            return text;
        }

        /// <inheritdoc />
        public virtual IConsoleOutput PushNode(bool isLastChild = false) {
            _pushNewNode = true;
            return this;
        }

        /// <inheritdoc />
        public virtual IConsoleOutput PopNode() {
            if (_treeLevel > 0) {
                _treeLevel--;
                _treeNewLinePrefix = _treeNewLinePrefix.Length <= 3 ? null : _treeNewLinePrefix.Substring(0, _treeNewLinePrefix.Length - 3);
            }
            return this;
        }

    }
}