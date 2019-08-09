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

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "before-image", "bi",
        Description = "Operate on database before-image file."
    )]
    [Subcommand(typeof(BiTruncateCommand))]
    internal class DatabaseBiCommand : ABaseParentCommand {
    }

    [Command(
        "truncate", "tr",
        Description = "Truncate the before-image of a database.",
        ExtendedHelpText = @"Performs the following three functions:
 - Uses the information in the before-image (BI) files to bring the database and after-image (AI) files up to date, waits to verify that the information has been successfully written to the disk, then truncates the before-image file to its original length.
 - Sets the BI cluster size using the Before-image Cluster Size (-bi) parameter.
 - Sets the BI block size using the Before-image Block Size (-biblocksize) parameter."
    )]
    internal class BiTruncateCommand : ADatabaseSingleLocationCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            GetOperator().TruncateBi(GetSingleDatabaseLocation());
            return 0;
        }
    }
}
