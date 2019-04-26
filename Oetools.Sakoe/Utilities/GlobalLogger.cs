#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (GlobalLogger.cs) is part of Oetools.Sakoe.
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
using CommandLineUtilsPlus.Console;
using Oetools.Builder.Utilities;

namespace Oetools.Sakoe.Utilities {

    /// <summary>
    /// Gloabl logger.
    /// </summary>
    public class GlobalLogger : CommandLineConsoleLogger, ILogger, ITraceLogger {

        /// <inheritdoc />
        public GlobalLogger(IConsoleInterface console) : base(console) { }

        /// <inheritdoc />
        public new ITraceLogger Trace => LogTheshold <= ConsoleLogThreshold.Debug ? this : null;

        /// <inheritdoc />
        public new ILogger If(bool condition) => condition ? this : null;
    }

}