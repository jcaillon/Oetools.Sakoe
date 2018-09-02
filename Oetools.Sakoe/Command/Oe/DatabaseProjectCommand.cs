using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Description = "TODO : db",
        ExtendedHelpText = "TODO : db",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    [Subcommand("connect", typeof(CreateDatabaseCommand))]
    [Subcommand("create", typeof(CreateDatabaseCommand))]
    [Subcommand("create", typeof(CreateDatabaseCommand))]
    internal class DatabaseProjectCommand : OeBaseCommand {
    }
    
    [Command(
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class ConnectDatabaseProjectCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to repair (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new UoeDatabaseOperator(GetDlcPath());
                        
            dbOperator.ProstrctRepair(TargetDatabasePath);
           
            return 0;
        }
    }
    
    
}