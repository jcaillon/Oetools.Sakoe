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
using System.Runtime.InteropServices;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "prohelp", "ph",
        Description = "Access the Openedge help.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(ProMsgCommand))]
#if WINDOWSONLYBUILD
    [Subcommand(typeof(ListChmProHelpCommand))]
    [Subcommand(typeof(ChmProHelpCommand))]
    [Subcommand(typeof(KeywordProHelpCommand))]
#endif
    internal class ProHelpCommand : BaseCommand {
    }
    
    [Command(
        "promsg", "pm",
        Description = "Displays the extended error message using an error number.", 
        ExtendedHelpText = "This command uses the content of files located in $DLC/prohelp/msgdata to display information.", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ProMsgCommand : AOeDlcCommand {
        
        [Required]
        [Argument(0, "<message number>", "The number of the error message to show.")]
        public int MessageNumber { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var msg = UoeUtilities.GetOpenedgeProMessage(GetDlcPath(), MessageNumber);
            
            Out.WriteResultOnNewLine($"{(string.IsNullOrEmpty(msg.Category) ? "" : $"({msg.Category}) ")}{msg.Text}");
            Out.WriteResultOnNewLine(null);
            
            if (!string.IsNullOrEmpty(msg.Description)) {
                Out.WriteResultOnNewLine(msg.Description);
            }
            if (msg.KnowledgeBase.Length > 2) {
                Out.WriteResultOnNewLine(msg.KnowledgeBase.StripQuotes());
            }

            return 0;
        }
    }
    
#if WINDOWSONLYBUILD
    
    internal class ChmDisplayProHelpCommand : AOeDlcCommand {     

        protected void OpenChmAndWait(string toOpen, string topic) {
            var helpWhdl = HtmlHelpInterop.DisplayIndex(toOpen, topic ?? "");

            Out.WriteResultOnNewLine("Press CTRL+C to close the help window.");

            while (IsWindow(helpWhdl) && !_cancelSource.IsCancellationRequested) {
                Thread.Sleep(500);
            }
        }
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool IsWindow(IntPtr hWnd);
    }
    
    [Command(
        "chm", "ch",
        Description = "Opens a .chm file (windows help file) present in $DLC/prohelp.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ChmProHelpCommand : ChmDisplayProHelpCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "<chm file name>", "The file name of the .chm file to display.")]
        public string ChmFileName { get; }
        
        [Argument(1, "<topic>", "Open the .chm on the given topic.")]
        public string Topic { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var toOpen = Path.Combine(GetDlcPath(), "prohelp", ChmFileName);

            if (!File.Exists(toOpen)) {
                throw new CommandValidationException($"The name of the chm file is invalid, the file does not exist : {toOpen.PrettyQuote()}.");
            }
            
            OpenChmAndWait(toOpen, Topic);

            return 0;
        }
    }
    
    [Command(
        "keyword", "ke",
        Description = "Look for help on the given Openedge keyword in the language windows help file.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class KeywordProHelpCommand : ChmDisplayProHelpCommand {
        
        [Argument(0, "<keyword>", "The keyword you would like to find in the help.")]
        public string Keyword { get; }
        
        private const string DefaultChmFileName = "lgrfeng.chm";
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
           
            OpenChmAndWait(Path.Combine(GetDlcPath(), "prohelp", DefaultChmFileName), Keyword);
            
            return 0;
        }
    }
    
    
    [Command(
        "listchm", "li",
        Description = "List all the .chm files (windows help files) available in $DLC/prohelp.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ListChmProHelpCommand : AOeDlcCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            foreach (var chmPath in Directory.EnumerateFiles(Path.Combine(GetDlcPath(), "prohelp"), "*.chm", SearchOption.TopDirectoryOnly)) {
                Out.WriteResultOnNewLine(Path.GetFileName(chmPath));
            }
            
            return 0;
        }
    }
    
#endif
}