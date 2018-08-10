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
    [Subcommand("create", typeof(CreateDatabaseCommand))]
    [Subcommand("start", typeof(StartDatabaseCommand))]
    [Subcommand("stop", typeof(StopDatabaseCommand))]
    [Subcommand("kill", typeof(KillAllDatabaseCommand))]
    [Subcommand("delete", typeof(DeleteDatabaseCommand))]
    [Subcommand("repair", typeof(RepairDatabaseCommand))]
    internal class DatabaseCommand : OeBaseCommand {
    }
    
    [Command(
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class RepairDatabaseCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to repair (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new DatabaseOperator(GetDlcPath());
                        
            dbOperator.ProstrctRepair(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : delete database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class DeleteDatabaseCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to delete (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new DatabaseOperator(GetDlcPath());
                        
            dbOperator.Delete(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : stop database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class StopDatabaseCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to stop (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new DatabaseOperator(GetDlcPath());
                        
            if (RemainingArgs != null) {
                WriteInfo($"Extra parameters : {string.Join(" ", RemainingArgs)}");
            }
           
            dbOperator.Proshut(TargetDatabasePath, string.Join(" ", RemainingArgs));
            WriteDebug(dbOperator.LastOperationOutput);
            
            return 0;
        }
    }
    
    [Command(
        Description = "Kill all the _mprosrv process"
    )]
    internal class KillAllDatabaseCommand : OeBaseCommand {
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            DatabaseOperator.KillAllMproSrv();
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : database proserve",
        ExtendedHelpText = "TODO : database proserve",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class StartDatabaseCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to start (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }
        
        [Option("-np|--nextport", "Port number, the next available port after this number will be used to start the database", CommandOptionType.SingleValue)]
        protected (bool HasValue, int value) NextPortAvailable { get; set; }
        
        [Option("-p|--port", "Port number that will be used by this database", CommandOptionType.SingleValue)]
        protected (bool HasValue, int value) Port { get; set; }
        
        [Option("-n|--nbusers", "Number of users that should be able to connect to this database simultaneously", CommandOptionType.SingleValue)]
        protected (bool HasValue, int value) NbUsers { get; set; }
        
        [Option("-s|--service", "Service name for the database, an alternative to the port number", CommandOptionType.SingleValue)]
        protected string ServiceName { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbOperator = new DatabaseOperator(GetDlcPath());
                       
            if (RemainingArgs != null) {
                WriteInfo($"Extra parameters for proserve : {string.Join(" ", RemainingArgs)}");
            }
           
            // service or port
            if (!string.IsNullOrEmpty(ServiceName) && Port.HasValue) {
                throw new CommandException($"Both {nameof(ServiceName)} and {nameof(Port)} are defined but they are mutually exclusive options");
            }

            // port or next port
            if (Port.HasValue && NextPortAvailable.HasValue) {
                throw new CommandException($"Both {nameof(NextPortAvailable)} and {nameof(Port)} are defind but they are mutually exclusive options");
            }

            if (NbUsers.HasValue && NbUsers.value <= 0) {
                throw new CommandException($"{nameof(NbUsers)} can only be > 0");
            }
            
            ServiceName = !string.IsNullOrEmpty(ServiceName) ? ServiceName : null;
            ServiceName = ServiceName ?? (Port.HasValue && Port.value > 0 ? Port.value : (NextPortAvailable.HasValue ? DatabaseOperator.GetNextAvailablePort(NextPortAvailable.value) : DatabaseOperator.GetNextAvailablePort())).ToString();
            
            var options = dbOperator.ProServe(TargetDatabasePath, ServiceName, NbUsers.value > 0 ? (int?) NbUsers.value : null, string.Join(" ", RemainingArgs));
            WriteInfo($"Proserve with options : {options.PrettyQuote()}");
            
            WriteOk($"Database started successfully, connection string :");
            WriteOk($"{DatabaseAdministrator.GetConnectionString(TargetDatabasePath, ServiceName)}");
            
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : database creation",
        ExtendedHelpText = "TODO : database creation",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class CreateDatabaseCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to create (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }
        
        [Option("-df|--df", "Path to the .df file containing the database schema definition", CommandOptionType.SingleValue)]
        [FileExists]
        protected string SchemaDefinitionFilePath { get; set; }
        
        [Option("-st|--st", "Path to the .st file containing the database physical structure", CommandOptionType.SingleValue)]
        [FileExists]
        protected string StuctureFilePath { get; set; }
        
        [Option("-bs|--blocksize", "The blocksize to use when creating the database", CommandOptionType.SingleValue)]
        protected (bool HasValue, string value) BlockSize { get; }

        [Option("-cp|--codepage", "Existing codepage in the openedge installation $DLC/prolang/(codepage)", CommandOptionType.SingleValue)]
        protected string Codepage { get; } = null;

        [Option("-ni|--newinstance", "Use -newinstance in the procopy command", CommandOptionType.NoValue)]
        protected bool NewInstance { get; } = false;

        [Option("-rp|--relativepath", "Use -relativepath in the procopy command", CommandOptionType.NoValue)]
        protected bool RelativePath { get; } = false;
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            using (var dbAdministrator = new DatabaseAdministrator(GetDlcPath())) {

                // get blocksize
                var blockSize = DatabaseBlockSize.S4096;
                if (BlockSize.HasValue) {
                    if (!TryGetEnumValue<DatabaseBlockSize>($"{DatabaseBlockSize.S4096.ToString().Substring(0, 1)}{BlockSize.value}", out long blockSizeValue, out List<string> list)) {
                        throw new CommandException($"Invalid value for {nameof(BlockSize)}, valid values are : {string.Join(", ", list.Select(s => s.Substring(1)))}");
                    }

                    blockSize = (DatabaseBlockSize) blockSizeValue;
                } else if (!Utils.IsRuntimeWindowsPlatform) {
                    // default value for linux
                    blockSize = DatabaseBlockSize.S8192;
                }

                // to absolute path
                StuctureFilePath = !string.IsNullOrEmpty(StuctureFilePath) ? StuctureFilePath.MakePathAbsolute() : null;
                SchemaDefinitionFilePath = !string.IsNullOrEmpty(SchemaDefinitionFilePath) ? SchemaDefinitionFilePath.MakePathAbsolute() : null;
                TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;

                // exists?
                if (dbAdministrator.DatabaseExists(TargetDatabasePath)) {
                    throw new CommandException("The target database already exists, choose a new name or delete the existing database");
                }

                bool stAutoGenerated = false;

                // copy structure file to target
                if (!string.IsNullOrEmpty(StuctureFilePath)) {
                    WriteInfo("Copying source structure file (.st) to target database folder");
                    dbAdministrator.CopyStructureFile(TargetDatabasePath, StuctureFilePath);
                } else if (!string.IsNullOrEmpty(SchemaDefinitionFilePath)) {
                    
                    // generate a structure file from df?
                    WriteInfo("Generating structure file (.st) from schema definition file (.df)");
                    StuctureFilePath = dbAdministrator.GenerateStructureFileFromDf(TargetDatabasePath, SchemaDefinitionFilePath);
                    WriteDebug($"File generated : {StuctureFilePath}");
                    WriteDebug(dbAdministrator.LastOperationOutput);
                    stAutoGenerated = true;
                }

                // prostct create
                if (!string.IsNullOrEmpty(StuctureFilePath)) {
                    WriteInfo($"Create database structure from structure file (.st) using {blockSize} blocksize");
                    dbAdministrator.ProstrctCreate(TargetDatabasePath, StuctureFilePath, blockSize);
                    WriteDebug(dbAdministrator.LastOperationOutput);
                }

                // procopy empty
                WriteInfo($"Procopy the empty database from the dlc folder corresponding to a {blockSize} blocksize");
                dbAdministrator.Procopy(TargetDatabasePath, blockSize, Codepage, NewInstance, RelativePath);
                WriteDebug(dbAdministrator.LastOperationOutput);

                if (!dbAdministrator.DatabaseExists(TargetDatabasePath)) {
                    throw new CommandException("The database does not exist for unknown reasons");
                }

                // Load .df
                if (!string.IsNullOrEmpty(SchemaDefinitionFilePath)) {
                    WriteInfo($"Loading the schema definition from {SchemaDefinitionFilePath}");
                    dbAdministrator.LoadDf(TargetDatabasePath, SchemaDefinitionFilePath);
                    WriteDebug(dbAdministrator.LastOperationOutput);
                }

                if (stAutoGenerated) {
                    WriteWarn("The database physical structure (described in .st) has been generated automatically, this database should not be used in production");
                }

                WriteOk($"Database created successfully : {TargetDatabasePath.PrettyQuote()}");

            }

            return 0;
        }
    }
}