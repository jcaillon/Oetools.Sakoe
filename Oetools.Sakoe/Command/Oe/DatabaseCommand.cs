using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {
    
    
    [Command(
        "database", "db", "da",
        Description = "TODO : db",
        ExtendedHelpText = "TODO : db"
    )]
    [Subcommand(typeof(DatabaseProjectCommand))]
    [Subcommand(typeof(CreateDatabaseCommand))]
    [Subcommand(typeof(StartDatabaseCommand))]
    [Subcommand(typeof(StopDatabaseCommand))]
    [Subcommand(typeof(KillAllDatabaseCommand))]
    [Subcommand(typeof(DeleteDatabaseCommand))]
    [Subcommand(typeof(RepairDatabaseCommand))]
    // TODO: generate a delta .df
    // TODO: get connection string
    internal class DatabaseCommand : AExpectSubCommand {
    }
    
    [Command(
        "repair", "re",
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database"
    )]
    internal class RepairDatabaseCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "[<db path>]", "Path to the database to repair (.db extension is optional)")]
        public string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) { Log = Log };
                        
            dbOperator.ProstrctRepair(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        "delete", "de",
        Description = "TODO : delete database",
        ExtendedHelpText = "TODO : database"
    )]
    internal class DeleteDatabaseCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "[<db path>]", "Path to the database to delete (.db extension is optional)")]
        public string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) { Log = Log };
                        
            dbOperator.Delete(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        "stop", "sto", "proshut",
        Description = "TODO : stop database",
        ExtendedHelpText = "TODO : database",
        AllowArgumentSeparator = true
    )]
    internal class StopDatabaseCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "[<db path>]", "Path to the database to stop (.db extension is optional)")]
        public string TargetDatabasePath { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) { Log = Log };
                        
            if (RemainingArgs != null) {
                Log.Info($"Extra parameters: {string.Join(" ", RemainingArgs)}.");
            }
           
            dbOperator.Proshut(TargetDatabasePath, RemainingArgs != null && RemainingArgs.Length > 0 ? string.Join(" ", RemainingArgs) : null);
            Log.Debug(dbOperator.LastOperationOutput);
            
            return 0;
        }
    }
    
    [Command(
        "kill", "ki",
        Description = "Kill all the _mprosrv process"
    )]
    internal class KillAllDatabaseCommand : ABaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            UoeDatabaseOperator.KillAllMproSrv();
            return 0;
        }
    }
    
    [Command(
        "start", "sta", "proserve",
        Description = "TODO : database proserve",
        ExtendedHelpText = "TODO : database proserve",
        AllowArgumentSeparator = true
    )]
    internal class StartDatabaseCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "[<db path>]", "Path to the database to start (.db extension is optional)")]
        public string TargetDatabasePath { get; set; }
        
        [Option("-np|--next-port", "Port number, the next available port after this number will be used to start the database", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) NextPortAvailable { get; set; }
        
        [Option("-p|--port", "Port number that will be used by this database", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) Port { get; set; }
        
        [Option("-n|--nbusers", "Number of users that should be able to connect to this database simultaneously", CommandOptionType.SingleValue)]
        public (bool HasValue, int value) NbUsers { get; set; }
        
        [Option("-s|--service", "Service name for the database, an alternative to the port number", CommandOptionType.SingleValue)]
        public string ServiceName { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new UoeDatabaseOperator(GetDlcPath()) { Log = Log };
                       
            if (RemainingArgs != null) {
                Log.Info($"Extra parameters for proserve : {string.Join(" ", RemainingArgs)}");
            }
           
            // service or port
            if (!string.IsNullOrEmpty(ServiceName) && Port.HasValue) {
                throw new CommandException($"Both {nameof(ServiceName)} and {nameof(Port)} are defined but they are mutually exclusive options.");
            }

            // port or next port
            if (Port.HasValue && NextPortAvailable.HasValue) {
                throw new CommandException($"Both {nameof(NextPortAvailable)} and {nameof(Port)} are defined but they are mutually exclusive options.");
            }

            if (NbUsers.HasValue && NbUsers.value <= 0) {
                throw new CommandException($"{nameof(NbUsers)} can only be > 0.");
            }
            
            ServiceName = !string.IsNullOrEmpty(ServiceName) ? ServiceName : null;
            ServiceName = ServiceName ?? (Port.HasValue && Port.value > 0 ? Port.value.ToString() : null);
            
            dbOperator.ProServe(TargetDatabasePath, ServiceName, NbUsers.value > 0 ? (int?) NbUsers.value : null, RemainingArgs == null ? null : string.Join(" ", RemainingArgs));
            
            Log.Info("Multi-user connection string:");
            Out.WriteResultOnNewLine($"{UoeDatabaseOperator.GetMultiUserConnectionString(TargetDatabasePath, ServiceName)}");
            Log.Done("Database started successfully.");
            
            return 0;
        }
    }
    
    [Command(
        "create", "cr",
        Description = "TODO : database creation",
        ExtendedHelpText = "TODO : extended database creation"
    )]
    internal class CreateDatabaseCommand : AOeDlcCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, "[<db path>]", "Path to the database to create (.db extension is optional)")]
        public string TargetDatabasePath { get; set; }
        
        [Option("-df|--df", "Path to the .df file containing the database schema definition", CommandOptionType.SingleValue)]
        [FileExists]
        public string SchemaDefinitionFilePath { get; set; }
        
        [Option("-st|--st", "Path to the .st file containing the database physical structure", CommandOptionType.SingleValue)]
        [FileExists]
        public string StuctureFilePath { get; set; }

        [Option("-bs|--blocksize", "The blocksize to use when creating the database", CommandOptionType.SingleValue)]
        public DatabaseBlockSize BlockSize { get; } = DatabaseBlockSize.DefaultForCurrentPlatform;

        [Option("-cp|--codepage", "Existing codepage in the openedge installation $DLC/prolang/(codepage)", CommandOptionType.SingleValue)]
        public string Codepage { get; } = null;

        [Option("-ni|--newinstance", "Use -newinstance in the procopy command", CommandOptionType.NoValue)]
        public bool NewInstance { get; } = false;

        [Option("-rp|--relativepath", "Use -relativepath in the procopy command", CommandOptionType.NoValue)]
        public bool RelativePath { get; } = false;
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            using (var dbAdministrator = new UoeDatabaseAdministrator(GetDlcPath())) {
                dbAdministrator.Log = Log;

                // to absolute path
                StuctureFilePath = !string.IsNullOrEmpty(StuctureFilePath) ? StuctureFilePath.MakePathAbsolute() : null;
                SchemaDefinitionFilePath = !string.IsNullOrEmpty(SchemaDefinitionFilePath) ? SchemaDefinitionFilePath.MakePathAbsolute() : null;
                TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;

                dbAdministrator.CreateDatabase(TargetDatabasePath, StuctureFilePath, BlockSize, Codepage, NewInstance, RelativePath, SchemaDefinitionFilePath);

                Log.Info("Single user connection string:");
                Out.WriteOnNewLine(UoeDatabaseOperator.GetSingleUserConnectionString(TargetDatabasePath));
                Log.Done($"Database created successfully: {TargetDatabasePath.PrettyQuote()}");
            }

            return 0;
        }
    }
}