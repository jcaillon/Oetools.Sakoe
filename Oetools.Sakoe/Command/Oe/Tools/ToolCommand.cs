﻿#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (UpdateCommand.cs) is part of Oetools.Sakoe.
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

using System.IO;
using DotUtilities;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "tool", "tl",
        Description = "Manage external tools usable in sakoe."
    )]
    [Subcommand(typeof(ToolDataDiggerCommand))]
    internal class ToolCommand : ABaseParentCommand {

        /// <summary>
        /// Directory in which external tools are installed.
        /// </summary>
        public static string ExternalToolInstallationDirectory => Path.Combine(Utils.GetHomeDirectory(), ".sakoe");
    }

}
