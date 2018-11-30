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
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleOutput : IConsoleOutput, IDisposable {

        private ConsoleColor _originalForegroundColor;

        protected readonly TextWriterOutputWordWrap WordWrapWriter;
        
        protected IConsole _console;

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
        public virtual void WriteResult(string result, ConsoleColor? color = null) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            (OutputTextWriter ?? _console.Out).Write(result);
            WordWrapWriter.HasWroteToOuput = true;
        }

        /// <inheritdoc />
        public virtual void WriteResultOnNewLine(string result, ConsoleColor? color = null) {
            if (WordWrapWriter.HasWroteToOuput) {
                WordWrapWriter.WriteLine(OutputTextWriter ?? _console.Out);
            }
            WriteResult(result, color);
        }
        
        /// <inheritdoc />
        public virtual void Write(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(result, false, padding, OutputTextWriter ?? _console.Out);
        }

        /// <inheritdoc />
        public virtual void WriteOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(result, true, padding, OutputTextWriter ?? _console.Out);
        }

        /// <inheritdoc />
        public virtual void WriteError(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(result, false, padding, OutputTextWriter ?? _console.Error);
        }

        /// <inheritdoc />
        public virtual void WriteErrorOnNewLine(string result, ConsoleColor? color = null, int padding = 0) {
            _console.ForegroundColor = color ?? _originalForegroundColor;
            WordWrapWriter.Write(result, true, padding, OutputTextWriter ?? _console.Error);
        }

        /// <summary>
        /// Draw the logo of this tool.
        /// </summary>
        public void DrawLogo() {
            WriteOnNewLine(null);
            WriteOnNewLine(@"                '`.        ", ConsoleColor.DarkGray);
            Write(@"== SAKOE ==");
            WriteOnNewLine(@" '`.    ", ConsoleColor.DarkGray);
            Write(@".^", ConsoleColor.Gray);
            Write(@"      \  \       ", ConsoleColor.DarkGray);
            Write(@"A ");
            Write(@"S", ConsoleColor.Cyan);
            Write(@"wiss ");
            Write(@"A", ConsoleColor.Cyan);
            Write(@"rmy ");
            Write(@"K", ConsoleColor.Cyan);
            Write(@"nife for ");
            Write(@"O", ConsoleColor.Cyan);
            Write(@"pen");
            Write(@"E", ConsoleColor.Cyan);
            Write(@"dge.");
            WriteOnNewLine(@"  \ \", ConsoleColor.DarkGray);
            Write(@"  /;/", ConsoleColor.Gray);
            Write(@"       \ \\      ", ConsoleColor.DarkGray);
            Write($"Version {typeof(HelpGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}.", ConsoleColor.Gray);
            WriteOnNewLine(@"   \ \", ConsoleColor.DarkGray);
            Write(@"/", ConsoleColor.Gray);
            Write(@"_", ConsoleColor.Red);
            Write(@"/", ConsoleColor.Gray);
            Write(@"_________", ConsoleColor.Red);
            Write(@"\", ConsoleColor.DarkGray);
            Write(@"__", ConsoleColor.Red);
            Write(@"\     ", ConsoleColor.DarkGray);
            Write($"Running with {(Utils.IsNetFrameworkBuild ? ".netframework" : $".netcore-{(Utils.IsRuntimeWindowsPlatform ? "win" : "unix")}")}.", ConsoleColor.Gray);
            WriteOnNewLine(@"    `", ConsoleColor.DarkGray);
            Write(@"/ ", ConsoleColor.Red);
            Write(@".           _", ConsoleColor.White);
            Write(@"  \    ", ConsoleColor.Red);
            Write($"Session started on {DateTime.Now:yy-MM-dd} at {DateTime.Now:HH:mm:ss}.", ConsoleColor.Gray);
            WriteOnNewLine(@"     \________________/    ", ConsoleColor.Red);
            Write(@"https://github.com/jcaillon", ConsoleColor.Gray);
            WriteOnNewLine(null);
        }

    }
}