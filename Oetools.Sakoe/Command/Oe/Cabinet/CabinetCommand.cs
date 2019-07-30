#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XcodeCommand.cs) is part of Oetools.Sakoe.
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project.Task;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Sakoe.Command.Oe.Abstract.Archiver;
using Oetools.Sakoe.Command.Oe.Database;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Exceptions;

namespace Oetools.Sakoe.Command.Oe.Cabinet {

    [Command(
        "cabinet", "ca", "cab",
        Description = "Cabinet files (.cab) creation and edition."
    )]
    [Subcommand(typeof(CabinetAddCommand))]
    [Subcommand(typeof(CabinetListCommand))]
    internal class CabinetCommand : ABaseParentCommand {
    }

    [Command(
        "add", "ad",
        Description = "Add."
    )]
    internal class CabinetAddCommand : AArchiverAddCommand {
        public override AOeTaskFileArchiverArchive GetArchiverTask() => new OeTaskFileArchiverArchiveCab {

        };
    }

    [Command(
        "list", "li",
        Description = "List only encrypted (or decrypted) files.",
        ExtendedHelpText = @"Examples:

  List only the encrypted files in a list of files in argument.
    sakoe xcode list -r -vb none -nop
  Get a raw list of all the encrypted files in the current directory (recursive).
    sakoe xcode list -r -vb none -nop"
    )]
    internal class CabinetListCommand : AOeCommand {

        [Option("-p|--path", "Path to list.", CommandOptionType.SingleValue)]
        public string Path { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Log.Info("Listing files...");

            var filesList = GetFilesFromWildcardsPath(Path).ToList();

            Log.Info($"Starting the analyze on {filesList.Count} files.");

            foreach (var file in filesList) {
                Out.WriteResultOnNewLine(file);
            }


            return 0;
        }
    }

}
