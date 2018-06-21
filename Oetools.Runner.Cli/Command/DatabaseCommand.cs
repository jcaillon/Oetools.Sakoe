using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Runner.Cli.Command {
    
    
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
    internal class DatabaseCommand : BaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteWarning("You must provide a command");
            app.ShowHint();
            return 1;
        }

    }
    
    [Command(
        Description = "TODO : repair database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class RepairDatabaseCommand : BaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to repair (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbAdministrator = new DatabaseAdministrator(GetDlcPath());
                        
            dbAdministrator.ProstrctRepair(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : delete database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class DeleteDatabaseCommand : BaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to delete (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbAdministrator = new DatabaseAdministrator(GetDlcPath());
                        
            dbAdministrator.Delete(TargetDatabasePath);
           
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : stop database",
        ExtendedHelpText = "TODO : database",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class StopDatabaseCommand : BaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Target database path", Description = "Path to the database to stop (.db extension is optional)")]
        protected string TargetDatabasePath { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            // to absolute path
            TargetDatabasePath = !string.IsNullOrEmpty(TargetDatabasePath) ? TargetDatabasePath.MakePathAbsolute() : null;
            
            var dbAdministrator = new DatabaseAdministrator(GetDlcPath());
                        
            if (RemainingArgs != null) {
                WriteInformationHeader($"Extra parameters : {string.Join(" ", RemainingArgs)}");
            }
           
            dbAdministrator.Proshut(TargetDatabasePath, string.Join(" ", RemainingArgs));
            WriteInformationBody(dbAdministrator.LastOperationStandardOutput);
            
            return 0;
        }
    }
    
    [Command(
        Description = "Kill all the _mprosrv process"
    )]
    internal class KillAllDatabaseCommand : BaseCommand {
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
    internal class StartDatabaseCommand : BaseCommand {
        
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
            
            var dbAdministrator = new DatabaseAdministrator(GetDlcPath());
                       
            if (RemainingArgs != null) {
                WriteInformationHeader($"Extra parameters for proserve : {string.Join(" ", RemainingArgs)}");
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
            
            var options = dbAdministrator.ProServe(TargetDatabasePath, ServiceName, NbUsers.value > 0 ? (int?) NbUsers.value : null, string.Join(" ", RemainingArgs));
            WriteInformationHeader($"Proserve with options : {options.Quoter()}");
            
            WriteSuccess($"Database started successfully, connection string :");
            WriteSuccess($"{DatabaseAdministrator.GetConnectionString(TargetDatabasePath, ServiceName)}");
            
            return 0;
        }
    }
    
    [Command(
        Description = "TODO : database creation",
        ExtendedHelpText = "TODO : database creation",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class CreateDatabaseCommand : BaseCommand {
        
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
            
            var dbAdministrator = new DatabaseAdministrator(GetDlcPath());

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
            
            // generate a structure file from df?
            bool stAutoGenerated = false;
            if (!string.IsNullOrEmpty(SchemaDefinitionFilePath)) {
                if (string.IsNullOrEmpty(StuctureFilePath)) {
                    WriteInformationHeader("Generating structure file (.st) from schema definition file (.df)");
                    StuctureFilePath = dbAdministrator.GenerateStructureFileFromDf(TargetDatabasePath, SchemaDefinitionFilePath);
                    WriteInformationBody($"File generated : {StuctureFilePath}");
                    WriteInformationBody(dbAdministrator.LastOperationStandardOutput);
                    stAutoGenerated = true;
                }
            }
            
            // copy structure file to target
            if (!string.IsNullOrEmpty(StuctureFilePath)) {
                WriteInformationHeader("Copying source structure file (.st) to target database folder");
                dbAdministrator.CopyStructureFile(TargetDatabasePath, StuctureFilePath);
            }
            
            // prostct create
            if (!string.IsNullOrEmpty(StuctureFilePath)) {
                WriteInformationHeader($"Create database structure from structure file (.st) using {blockSize} blocksize");
                dbAdministrator.ProstrctCreate(TargetDatabasePath, StuctureFilePath, blockSize);
                WriteInformationBody(dbAdministrator.LastOperationStandardOutput);
            }
            
            // procopy empty
            WriteInformationHeader($"Procopy the empty database from the dlc folder corresponding to a {blockSize} blocksize");
            dbAdministrator.Procopy(TargetDatabasePath, blockSize, Codepage, NewInstance, RelativePath);
            WriteInformationBody(dbAdministrator.LastOperationStandardOutput);

            if (!dbAdministrator.DatabaseExists(TargetDatabasePath)) {
                throw new CommandException("The database does not exist for unknown reasons");
            }
            
            // Load .df
            if (!string.IsNullOrEmpty(SchemaDefinitionFilePath)) {
                WriteInformationHeader($"Loading the schema definition from {SchemaDefinitionFilePath}");
                dbAdministrator.LoadDf(TargetDatabasePath, SchemaDefinitionFilePath);
                WriteInformationBody(dbAdministrator.LastOperationStandardOutput);
            }

            if (stAutoGenerated) {
                WriteWarning("The database physical structure (described in .st) has been generated automatically, this database should not be used in production");
            }

            WriteSuccess($"Database created successfully : {TargetDatabasePath.Quoter()}");
            return 0;
        }
    }
}