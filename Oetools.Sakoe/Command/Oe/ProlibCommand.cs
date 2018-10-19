#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (HashCommand.cs) is part of Oetools.Sakoe.
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

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Archive;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "prolib", "pl",
        Description = "CRUD operations for pro-libraries (.pl files).", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(ListProlibCommand))]
    internal class ProlibCommand : ArchiverBaseCommand {

        public override IArchiver GetArchiver() => Archiver.New(ArchiverType.Prolib);
        
    }
    
    [Command(
        "list", "ls", "li",
        Description = "List the content of a prolib file.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ListProlibCommand : BaseCommand {
        
        [Required]
        [Argument(0, "<string>", "The string to hash.")]
        public string Value { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (!string.IsNullOrEmpty(Value)) {
                Out.WriteResultOnNewLine(UoeHash.Hash(Encoding.Default.GetBytes(Value)));
            }
            
            return 0;
        }
    }
    
}