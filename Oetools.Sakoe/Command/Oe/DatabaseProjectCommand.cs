using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "project", "pr",
        Description = "TODO : db",
        ExtendedHelpText = "TODO : db"
    )]
    [Subcommand(typeof(ConnectDatabaseProjectCommand))]
    internal class DatabaseProjectCommand : ABaseCommand {
    }
    
    [Command(
        "create", "cr",
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database"
    )]
    internal class ConnectDatabaseProjectCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "<target database path>", "Path to the database to repair (.db extension is optional)")]
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