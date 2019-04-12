#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (DataDiggerCommand.cs) is part of Oetools.Sakoe.
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

using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "admin", "ad",
        Description = "The openedge database administration tool."
    )]
    [Subcommand(typeof(DatabaseAdminRunCommand))]
    internal class DatabaseAdminCommand : AExpectSubCommand {
    }

    [Command(
        "run", "ru",
        Description = "Run an instance of the database administration tool."
    )]
    internal class DatabaseAdminRunCommand : ADatabaseToolCommand {

        protected override ProcessArgs ToolArguments() =>  new ProcessArgs().Append("-p", "_admin.p");
    }
}
