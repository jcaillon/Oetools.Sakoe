#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConsoleOutputWrapText.cs) is part of Oetools.Sakoe.
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
    
    /// <summary>
    /// This class provides methods to output text to a <see cref="TextWriter"/> with word wrap.
    /// </summary>
    public class TextWriterOutputWordWrap {
        
        public TextWriter UnderLyingWriter { get; set; }

        public TextWriterOutputWordWrap(TextWriter writer) {
            if (writer == null) {
                throw new NullReferenceException($"Can't be null {nameof(writer)}.");
            }
            UnderLyingWriter = writer;
        }
        
        /// <summary>
        /// Has this writer alright wrote thing to the output?
        /// Allows to know when to write a new line.
        /// </summary>
        public bool HasWroteToOuput { get; set; }

        /// <summary>
        /// The length of what is already written in the current line.
        /// </summary>
        private int _currentLineSpaceTaken;

        /// <summary>
        /// Write a new line char into the text writer
        /// </summary>
        /// <param name="underLyingWriter"></param>
        /// <param name="newLine"></param>
        public void WriteLine(TextWriter underLyingWriter = null, string newLine = null) {
            (underLyingWriter ?? UnderLyingWriter).Write(newLine ?? UnderLyingWriter.NewLine);
            _currentLineSpaceTaken = 0;
            HasWroteToOuput = true;
        }

        /// <summary>
        /// Writes a text to the console with word wrap.
        /// </summary>
        /// <param name="message">the message to write</param>
        /// <param name="maxLineWidth">the maximum length of a line before line break.</param>
        /// <param name="writeToNewLine">write the message to a separated line (otherwise continue to output on the same line)</param>
        /// <param name="indentation">Apply indentation when writing on a new line.</param>
        /// <param name="underLyingWriter"></param>
        /// <param name="prefixForNewLines">The text to put at the beginning of each new line that need to be created because of word wrap.</param>
        public void Write(string message, int maxLineWidth, bool writeToNewLine, int indentation = 0, TextWriter underLyingWriter = null, string prefixForNewLines = null) {
            
            // handle null message
            if (message == null) {
                // write a new line
                if (writeToNewLine && HasWroteToOuput) {
                    WriteLine(underLyingWriter);
                }
                HasWroteToOuput = true;
                return;
            }
            
            // check indentation
            if (maxLineWidth > 0 && indentation >= maxLineWidth) {
                indentation = maxLineWidth - 1;
            }
            indentation = Math.Max(indentation, 0);

            // write without word wrap
            if (maxLineWidth <= 0) {
                // write a new line
                if (writeToNewLine && HasWroteToOuput) {
                    WriteLine(underLyingWriter);
                    _currentLineSpaceTaken = 0;
                }
                message = _currentLineSpaceTaken == 0 && indentation > 0 ? message.PadLeft(message.Length + indentation) : message;
                (underLyingWriter ?? UnderLyingWriter).Write(message);
                _currentLineSpaceTaken += message.Length;
                HasWroteToOuput = true;
                return;
            }
            
            // for each line of text
            int lineStartPos, nextLineStartPos;
            for (lineStartPos = 0; lineStartPos < message.Length; lineStartPos = nextLineStartPos) {
                
                int eolPosition = message.IndexOf('\n', lineStartPos);
                if (eolPosition == -1) {
                    nextLineStartPos = eolPosition = message.Length;
                } else {
                    nextLineStartPos = eolPosition + 1;
                    if (eolPosition == lineStartPos) {
                        // found a new line
                        WriteLine(underLyingWriter);
                        continue;
                    }
                }

                // an input line can have indentation; if we split this input line into several lines (because it is too long),
                // we need to keep the same indentation for each new line
                bool newInputLineStarting = true;
                int paragraphIndent = 0;
                
                if (eolPosition > lineStartPos) {
                    do {
                        // write a new line
                        if (writeToNewLine && HasWroteToOuput || _currentLineSpaceTaken >= maxLineWidth) {
                            if (newInputLineStarting || string.IsNullOrEmpty(prefixForNewLines)) {
                                WriteLine(underLyingWriter);
                            } else {
                                var newLinePrefix = prefixForNewLines.PadRight(Math.Max(0, indentation + paragraphIndent));
                                WriteLine(underLyingWriter, $"{UnderLyingWriter.NewLine}{newLinePrefix}");
                                _currentLineSpaceTaken += newLinePrefix.Length;
                            }
                        }
                        
                        int lineLength = eolPosition - lineStartPos;
                        int totalIndent = _currentLineSpaceTaken > 0 ? 0 : indentation + paragraphIndent;
                        int currentConsoleLineSpaceLeft = maxLineWidth - _currentLineSpaceTaken - totalIndent;
                        
                        if (lineLength > currentConsoleLineSpaceLeft) {
                            lineLength = GetLineLengthToKeep(message, lineStartPos, currentConsoleLineSpaceLeft, !writeToNewLine && message.Length <= maxLineWidth - totalIndent);
                        }

                        if (lineLength > 0) {
                            var line = message.Substring(lineStartPos, lineLength);
                            line = _currentLineSpaceTaken > 0 ? line : line.PadLeft(lineLength + indentation + paragraphIndent, ' ');
                            (underLyingWriter ?? UnderLyingWriter).Write(line);
                            _currentLineSpaceTaken += line.Length;
                            HasWroteToOuput = true;
                        }
                        
                        if (newInputLineStarting && writeToNewLine) {
                            while (lineStartPos + paragraphIndent < message.Length && char.IsWhiteSpace(message[lineStartPos + paragraphIndent])) {
                                paragraphIndent++;
                            }
                            if (paragraphIndent >= maxLineWidth) {
                                paragraphIndent = maxLineWidth - 1;
                            }
                        }
                        newInputLineStarting = false;
                        writeToNewLine = true;

                        // trim the whitespaces following a word break
                        lineStartPos += lineLength;
                        while (lineStartPos < eolPosition && message[lineStartPos] != '\n' && char.IsWhiteSpace(message[lineStartPos])) {
                            lineStartPos++;
                        }
                    } while (eolPosition > lineStartPos);
                }
            }
        }
        
        /// <summary>
        /// Returns the length to keep for this line in order to wrap words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length allowed</param>
        /// <param name="allowedToReturnZero">Can this method return 0 if there are no spaces between <paramref name="pos"/> and <paramref name="pos"/> + <paramref name="max"/></param>
        /// <returns>The length to keep for this line</returns>
        private static int GetLineLengthToKeep(string text, int pos, int max, bool allowedToReturnZero) {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !char.IsWhiteSpace(text[pos + i])) {
                i--;
            }

            // If no whitespace found, break at maximum length
            // or allow to return nothing for this line and put it on the next line instead
            if (i < 0) {
                return allowedToReturnZero ? 0 : max;
            }

            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i])) {
                i--;
            }

            if (i == -1) {
                return max;
            }

            // Return length of text before whitespace
            return i + 1;
        }
    }
}