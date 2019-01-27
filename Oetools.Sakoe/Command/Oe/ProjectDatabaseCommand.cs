#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (DatabaseProjectCommand.cs) is part of Oetools.Sakoe.
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

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "project", "pr",
        Description = "Manipulate a database belonging to a project."
    )]
    [Subcommand(typeof(ConnectDatabaseProjectCommand))]
    internal class ProjectDatabaseCommand : ABaseCommand {
    }

    [Command(
        "create", "cr",
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database"
    )]
    internal class ConnectDatabaseProjectCommand : AOeDlcCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {


            return 0;
        }
    }


}
