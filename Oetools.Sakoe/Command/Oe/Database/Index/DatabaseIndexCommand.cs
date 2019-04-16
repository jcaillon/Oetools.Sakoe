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
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "index", "id",
        Description = "Operate on database indexes."
    )]
    [Subcommand(typeof(DatabaseIndexRebuildCommand))]
    internal class DatabaseIndexCommand : AExpectSubCommand {
    }

    [Command(
        "rebuild", "rb",
        Description = @"Rebuild the indexes of a database. By default, rebuilds all the active indexes."
    )]
    internal class DatabaseIndexRebuildCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Option("-op|--options <args>", "Parameters for the proutil idxbuild command. Defaults to `activeindexes`.", CommandOptionType.SingleValue)]
        public string IdxBuildOptions { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().RebuildIndexes(GetSingleDatabaseLocation(), new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters), new UoeProcessArgs().AppendFromQuotedArgs(IdxBuildOptions));
            return 0;
        }
    }
}
