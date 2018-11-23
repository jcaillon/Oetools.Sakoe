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
    
    public class ConsoleOutputWrapText {
        
        private bool _hasWroteToOuput;

        private int _currentConsoleLineSpaceTaken;
        
        protected void WriteToConsoleWithWordWrap(TextWriter textWriter, string message, bool writeToNewLine, int padding) {
            if (message == null) {
                message = string.Empty;
            }

            var textWidth = Console.WindowWidth - 1;
            if (textWidth < 1) {
                textWidth = 1;
            }
            if (padding >= textWidth) {
                padding = textWidth - 1;
            }
            int lineStartPos, nextLineStartPos;
            
            // Parse each line of text
            for (lineStartPos = 0; lineStartPos < message.Length; lineStartPos = nextLineStartPos) {
                
                // Find end of line
                int eolPosition = message.IndexOf('\n', lineStartPos);
                if (eolPosition == -1) {
                    nextLineStartPos = eolPosition = message.Length;
                } else {
                    nextLineStartPos = eolPosition + 1;
                }

                bool newParagraph = true;
                int paragraphPadding = 0;
                
                // Write this line of message, breaking into smaller lines as needed
                if (eolPosition > lineStartPos) {
                    do {
                        if (writeToNewLine && _hasWroteToOuput || _currentConsoleLineSpaceTaken >= textWidth) {
                            WriteNewLine(textWriter);
                        }
                        
                        int lineLength = eolPosition - lineStartPos;
                        int totalPadding = (_currentConsoleLineSpaceTaken > 0 ? 0 : padding) + paragraphPadding;
                        int currentConsoleLineSpaceLeft = textWidth - _currentConsoleLineSpaceTaken - totalPadding;
                        
                        if (lineLength > currentConsoleLineSpaceLeft) {
                            lineLength = BreakLine(message, lineStartPos, currentConsoleLineSpaceLeft, !writeToNewLine && message.Length <= textWidth - totalPadding);
                        }

                        if (lineLength > 0) {
                            var line = message.Substring(lineStartPos, lineLength);
                            textWriter.Write(_currentConsoleLineSpaceTaken > 0 ? line : line.PadLeft(lineLength + padding + paragraphPadding, ' '));
                        }
                        
                        if (newParagraph && writeToNewLine) {
                            while (lineStartPos + paragraphPadding < message.Length && char.IsWhiteSpace(message[lineStartPos + paragraphPadding])) {
                                paragraphPadding++;
                            }
                            if (paragraphPadding >= textWidth) {
                                paragraphPadding = textWidth - 1;
                            }
                        }
                        newParagraph = false;
                        
                        _currentConsoleLineSpaceTaken += lineLength;
                        _hasWroteToOuput = true;
                        writeToNewLine = true;

                        // Trim whitespace following break
                        lineStartPos += lineLength;
                        while (lineStartPos < eolPosition && char.IsWhiteSpace(message[lineStartPos])) {
                            lineStartPos++;
                        }
                    } while (eolPosition > lineStartPos);
                }
            }
        }
        
        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <param name="allowedToReturnZero"></param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max, bool allowedToReturnZero) {
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
        
        protected void WriteNewLine(TextWriter textWriter) {
            textWriter.Write(Console.Out.NewLine);
            _currentConsoleLineSpaceTaken = 0;
        }
    }
}