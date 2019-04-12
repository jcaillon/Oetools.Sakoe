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
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "log", "lg",
        Description = "Operate on database log files (" + UoeDatabaseLocation.LogFileExtention+ ")."
    )]
    [Subcommand(typeof(DatabaseLogTruncateCommand))]
    internal class DatabaseLogCommand : AExpectSubCommand {
    }

    [Command(
        "truncate", "tr",
        Description = "Truncates the log file. If the database is started, it will re-log the database startup parameters to the log file."
    )]
    internal class DatabaseLogTruncateCommand : ADatabaseSingleLocationCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().TruncateLog(GetSingleDatabaseLocation());
            return 0;
        }
    }
}
