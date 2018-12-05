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
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "utilities", "ut",
        Description = "Miscellaneous utility commands.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(GetConnectionStringUtilitiesCommand))]
    [Subcommand(typeof(GetProExecPathUtilitiesCommand))]
    [Subcommand(typeof(GetPropathFromIniUtilitiesCommand))]
    [Subcommand(typeof(GetProVersionUtilitiesCommand))]
    internal class ProUtilitiesCommand : AExpectSubCommand {
    }
    
    [Command(
        "connectstr", "co",
        Description = "Returns a single line connection string from a .pf file.", 
        ExtendedHelpText = @"This command will skip unnecessary whitespaces and new lines.
It will also ignore comment lines starting with #.", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class GetConnectionStringUtilitiesCommand : ABaseCommand {
        
        [Required]
        [FileExists]
        [Argument(0, "<.pf path>", "The file path to the parameter file (.pf) to use.")]
        public string File { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(UoeUtilities.GetConnectionStringFromPfFile(File));
            
            return 0;
        }
    }
    
    [Command(
        "execpath", "ex",
        Description = "Returns the pro executable full path.", 
        ExtendedHelpText = @"", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class GetProExecPathUtilitiesCommand : AOeDlcCommand {
        
        [Option("-c|--char-mode", "Specify to return the path of the character mode executable.", CommandOptionType.NoValue)]
        public bool CharMode { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(UoeUtilities.GetProExecutableFromDlc(GetDlcPath(), CharMode));
            
            return 0;
        }
    }
    
    [Command(
        "propathfromini", "pr",
        Description = "Returns PROPATH value found in a .ini file.", 
        ExtendedHelpText = @"This command returns only absolute path.
Relative path are converted to absolute using the command folder option.
It returns only existing directories or .pl files.
It also expands environment variables like %TEMP% or $DLC.", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class GetPropathFromIniUtilitiesCommand : AOeDlcCommand {
        
        [Required]
        [FileExists]
        [Argument(0, "<.ini path>", "The file path to the .ini file to read.")]
        public string File { get; }
        
        [Option("-rd|--base-directory", "The base directory to use to convert to absolute path. Default to current directory.", CommandOptionType.SingleValue)]
        public string BaseDirectory { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (string.IsNullOrEmpty(BaseDirectory)) {
                BaseDirectory = Directory.GetCurrentDirectory();
                Log.Info($"Using current directory as base directory : {BaseDirectory.PrettyQuote()}.");
            }
            
            foreach (var path in UoeUtilities.GetProPathFromIniFile(File, BaseDirectory)) {
                Out.WriteResultOnNewLine(path);
            }
            
            return 0;
        }
    }
    
    [Command(
        "version", "ve",
        Description = "Returns the version found for the Openedge installation.", 
        ExtendedHelpText = @"", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class GetProVersionUtilitiesCommand : AOeDlcCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(UoeUtilities.GetProVersionFromDlc(GetDlcPath()).ToString());
            
            return 0;
        }
    }
}