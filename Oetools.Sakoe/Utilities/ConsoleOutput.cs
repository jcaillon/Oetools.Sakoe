#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (BaseConsoleIo.cs) is part of Oetools.Sakoe.
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleOutput : IConsoleOutput, IDisposable {
        
        // TODO: add interface to draw a tree

        private ConsoleColor _originalForegroundColor;

        protected readonly TextWriterOutputWordWrap WordWrapWriter;
        
        private readonly IConsole _console;

        private int _treeLevel;
        private string _treeNewLinePrefix;
        private bool _pushNewNode;

        public TextWriter OutputTextWriter { get; set; }

        public ConsoleOutput(IConsole console) {
            _console = console;
            _console.ResetColor();
            _originalForegroundColor = _console.ForegroundColor;
            WordWrapWriter = new TextWriterOutputWordWrap(_console.Out);
        }

        public virtual void Dispose() {
            _console.ResetColor();
        }
        
        /// <inheritdoc />
        public virtual void WriteResult(string text, ConsoleColor? color = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            (OutputTextWriter ?? _console.Out).Write(text);
            WordWrapWriter.HasWroteToOuput = true;
        }

        /// <inheritdoc />
        public virtual void WriteResultOnNewLine(string text, ConsoleColor? color = null) {
            if (WordWrapWriter.HasWroteToOuput) {
                WordWrapWriter.WriteLine(OutputTextWriter ?? _console.Out);
            }
            WriteResult(text, color);
        }
        
        /// <inheritdoc />
        public virtual void Write(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(text, false, indentation, OutputTextWriter ?? _console.Out, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(GetText(text), true, indentation, OutputTextWriter ?? _console.Out, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteError(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(text, false, indentation, OutputTextWriter ?? _console.Error, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
        }

        /// <inheritdoc />
        public virtual void WriteErrorOnNewLine(string text, ConsoleColor? color = null, int indentation = 0, string prefixForNewLines = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(GetText(text), true, indentation, OutputTextWriter ?? _console.Error, string.IsNullOrEmpty(prefixForNewLines) ? _treeNewLinePrefix : $"{prefixForNewLines}{_treeNewLinePrefix}");
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

        /// <summary>
        /// Draw the logo of this tool.
        /// </summary>
        public void DrawLogo() {
            WriteOnNewLine(null);
            WriteOnNewLine(@"                '`.        ", ConsoleColor.DarkGray);
            Write(@"============ ", ConsoleColor.DarkGray);
            Write(@"SAKOE", ConsoleColor.Yellow);
            Write(@" ============", ConsoleColor.DarkGray);
            WriteOnNewLine(@" '`.    ", ConsoleColor.DarkGray);
            Write(@".^", ConsoleColor.Gray);
            Write(@"      \  \       ", ConsoleColor.DarkGray);
            Write(@"A ");
            Write(@"S", ConsoleColor.Yellow);
            Write(@"wiss ");
            Write(@"A", ConsoleColor.Yellow);
            Write(@"rmy ");
            Write(@"K", ConsoleColor.Yellow);
            Write(@"nife for ");
            Write(@"O", ConsoleColor.Yellow);
            Write(@"pen");
            Write(@"E", ConsoleColor.Yellow);
            Write(@"dge.");
            WriteOnNewLine(@"  \ \", ConsoleColor.DarkGray);
            Write(@"  /;/", ConsoleColor.Gray);
            Write(@"       \ \\      ", ConsoleColor.DarkGray);
            Write("Version ", ConsoleColor.DarkGray);
            Write(typeof(HelpGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion, ConsoleColor.Gray);
            Write(".", ConsoleColor.DarkGray);
            WriteOnNewLine(@"   \ \", ConsoleColor.DarkGray);
            Write(@"/", ConsoleColor.Gray);
            Write(@"_", ConsoleColor.Red);
            Write(@"/", ConsoleColor.Gray);
            Write(@"_________", ConsoleColor.Red);
            Write(@"\", ConsoleColor.DarkGray);
            Write(@"__", ConsoleColor.Red);
            Write(@"\     ", ConsoleColor.DarkGray);
            Write($"Running with {(Utils.IsNetFrameworkBuild ? ".netframework" : $".netcore-{(Utils.IsRuntimeWindowsPlatform ? "win" : "unix")}")}.", ConsoleColor.DarkGray);
            WriteOnNewLine(@"    `", ConsoleColor.DarkGray);
            Write(@"/ ", ConsoleColor.Red);
            Write(@".           _", ConsoleColor.White);
            Write(@"  \    ", ConsoleColor.Red);
            Write("Session started on .", ConsoleColor.DarkGray);
            Write($"{DateTime.Now:yy-MM-dd} at {DateTime.Now:HH:mm:ss}", ConsoleColor.Gray);
            Write(".", ConsoleColor.DarkGray);
            WriteOnNewLine(@"     \________________/    ", ConsoleColor.Red);
            Write(@"Source code on ", ConsoleColor.DarkGray);
            Write(@"github.com/jcaillon", ConsoleColor.Gray);
            Write(@".", ConsoleColor.DarkGray);
            WriteOnNewLine(null);
        }
    }
}