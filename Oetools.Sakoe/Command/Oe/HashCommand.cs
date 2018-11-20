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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "hash", "ha",
        Description = "Compute hash values of files or strings using the Openedge ENCODE function.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(HashStringCommand))]
    [Subcommand(typeof(HashFilesCommand))]
    internal class HashCommand : BaseCommand {
    }
    
    [Command(
        "string", "encode", "st",
        Description = "Returns a 16 byte hash value from a string.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class HashStringCommand : BaseCommand {
        
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
    
    [Command(
        "files", "fi",
        Description = "Returns 16 byte hash values computed from the content of input files.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class HashFilesCommand : ProcessFileListBaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Log.Info("Listing files...");

            var filesList = GetFilesList(app).ToList();
            
            Log.Info($"Processing {filesList.Count} files.");
            
            var i = 0;
            var outputList = new List<string>();
            foreach (var file in filesList) {
                outputList.Add($"{file} >> {UoeHash.Hash(File.ReadAllBytes(file))}");
                i++;
                Log.ReportProgress(filesList.Count, i, $"Computing hash for : {file}.");
            }

            foreach (var file in outputList) {
                Out.WriteResultOnNewLine(file);
            }
            
            return 0;
        }
    }
}