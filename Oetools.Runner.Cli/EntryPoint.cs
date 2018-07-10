#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (Main.cs) is part of Oetools.Runner.Cli.
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
using Oetools.Runner.Cli.Command;
using Oetools.Runner.Cli.Lib;

namespace Oetools.Runner.Cli {
    
    /// <summary>
    /// Main entry point for this CLI program
    /// </summary>
    /// <remarks>
    /// we can't reference a lib in this class if we want the assembly loader to work correctly
    /// </remarks>
    public static class EntryPoint {
        public static int Main(string[] args) {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoader.AssemblyResolver;
            return MainCommand.ExecuteMainCommand(args);
        } 
    }
}