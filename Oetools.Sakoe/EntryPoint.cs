#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (EntryPoint.cs) is part of Oetools.Sakoe.
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

using CommandLineUtilsPlus;
using Oetools.Sakoe.Command;
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe {

    /// <summary>
    /// The underlying entry point of this program.
    /// </summary>
    /// <remarks>We can reference external libraries in this class because the assemblies can be resolved at this point.</remarks>
    public static class EntryPoint {

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Execute(string[] args) {
            return CommandLineApplicationPlus<MainCommand>.ExecuteCommand(args, ci => new GlobalLogger(ci));
        }
    }
}
