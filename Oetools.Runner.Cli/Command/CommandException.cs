#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (CommandException.cs) is part of Oetools.Runner.Cli.
// 
// Oetools.Runner.Cli is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Runner.Cli is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Runner.Cli. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;

namespace Oetools.Runner.Cli.Command {
    public class CommandException : Exception {
        public CommandException() { }
        public CommandException(string message) : base(message) { }
        public CommandException(string message, Exception innerException) : base(message, innerException) { }
    }
}