using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project.Task;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "xcode", "xc",
        Description = "Encrypt and decrypt files using the algorithm of the XCODE utility",
        ExtendedHelpText = @"The original idea of the XCODE utility is to obfuscate your openedge code before making it available.
This is an encryption feature which uses a ASCII key/password of a maximum of 8 characters.
The original XCODE utility uses the default key ""Progress"" if no custom key is supplied (so does this command).
The encryption process does not use a standard cryptography method, it uses a 16-bits CRC inside a custom algo.",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(ListXcodeCommand))]
    [Subcommand(typeof(EncryptXcodeCommand))]
    [Subcommand(typeof(DecryptXcodeCommand))]
    internal class XcodeCommand : OeBaseCommand {
    }
    
    [Command(
        "encrypt", "en",
        Description = "Encrypt files using the openedge XCODE algorithm",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        AllowArgumentSeparator = true
    )]
    internal class EncryptXcodeCommand : OeFileListBaseCommand {
        
        [Option("-k|--key", "", CommandOptionType.SingleValue)]
        public string EncryptionKey { get; }
        
        [Option("-pre|--prefix", "", CommandOptionType.SingleValue)]
        public string OutputFilePrefix { get; }
        
        [LegalFilePath]
        [Option("-od|--output-directory", "", CommandOptionType.SingleValue)]
        public string OutputDirectory { get; }
        
        public string[] RemainingArgs { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            foreach (var arg in RemainingArgs) {
                Log.Info(arg);
            }

            var filter = new OeTaskFilter {
                
            };
            //var lister = new SourceFilesLister(sourceDirectory, _cancelSource) {
            //    SourcePathFilter = 
            //};
            //foreach (var directory in lister.GetDirectoryList()) {
            //    if (!output.Contains(directory)) {
            //        output.Add(directory);
            //    }
            //}
            
            
            
//            var lister = new SourceFilesLister(sourceDirectory, _cancelSource) {
//                SourcePathFilter = PropathSourceDirectoriesFilter
//            };
//            foreach (var directory in lister.GetDirectoryList()) {
//                if (!output.Contains(directory)) {
//                    output.Add(directory);
//                }
//            }
            
            return 0;
        }
    }

    [Command(
        "decrypt", "de",
        Description = "Decrypt files using the openedge XCODE algorithm", 
        ExtendedHelpText = "TODO", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase, 
        AllowArgumentSeparator = true
    )]
    internal class DecryptXcodeCommand : OeBaseCommand {

        [Option("-k|--key", "", CommandOptionType.SingleValue)]
        public string EncryptionKey { get; }

        [Option("-pre|--prefix", "", CommandOptionType.SingleValue)]
        public string OutputFilePrefix { get; }

        [DirectoryExists]
        [Option("-od|--output-directory", "", CommandOptionType.SingleValue)]
        public string OutputDirectory { get; }

        public string[] RemainingArgs { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            return 0;
        }
    }

    [Command(
        "list", "li",
        Description = "List encrypted (or decrypted) files", 
        ExtendedHelpText = "TODO", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase, 
        AllowArgumentSeparator = true
    )]
    internal class ListXcodeCommand : OeFileListBaseCommand {
        
        [DirectoryExists]
        [Argument(0, "directory", "The directory to list. Defaults to the current directory.")]
        public string Directory { get; }

        [Option("-de|--decrypted", "List all decrypted files (or default to listing encrypted files).", CommandOptionType.NoValue)]
        public bool ListDecrypted { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (!string.IsNullOrEmpty(Directory)) {
                Log.Debug($"Adding directory to list : {Directory}");
                var directories = Directories?.ToList() ?? new List<string>();
                directories.Add(Directory);
                Directories = directories.ToArray();
            }
            
            var xcode = new UoeEncryptor(null);
            
            foreach (var file in GetFilesList()) {
                var isEncrypted = xcode.IsFileEncrypted(file);
                if (isEncrypted && !ListDecrypted || !isEncrypted && ListDecrypted) {
                    WriteLineOutput(file);
                }
            }
            
            return 0;
        }
    }

}