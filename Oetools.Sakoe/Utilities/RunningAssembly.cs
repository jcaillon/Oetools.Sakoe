#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (RunningAssembly.cs) is part of Oetools.Sakoe.
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

using DotUtilities;

namespace Oetools.Sakoe.Utilities {

    /// <summary>
    /// The running assembly (Oetools.Sakoe).
    /// </summary>
    internal static class RunningAssembly {

        private static AssemblyInfo _assemblyInfo;

        /// <summary>
        /// Get information on the assembly.
        /// </summary>
        public static AssemblyInfo Info => _assemblyInfo ?? (_assemblyInfo = new AssemblyInfo(typeof(RunningAssembly).Assembly));

    }
}
