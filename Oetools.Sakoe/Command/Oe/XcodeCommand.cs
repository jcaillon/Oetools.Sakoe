using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe.Abstract;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Exceptions;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "xcode", "xc",
        Description = "Encrypt and decrypt files using the XCODE utility algorithm.",
        ExtendedHelpText = @"About XCODE:
  The original idea of the XCODE utility is to obfuscate your Openedge code before making it available.
  This is an encryption feature which uses a ASCII key/password of a maximum of 8 characters.
  The original XCODE utility uses the default key ""Progress"" if no custom key is supplied (so does this command).
  The encryption process does not use a standard cryptography method, it uses a 16-bits CRC inside a custom algorithm."
    )]
    [Subcommand(typeof(ListXcodeCommand))]
    [Subcommand(typeof(EncryptXcodeCommand))]
    [Subcommand(typeof(DecryptXcodeCommand))]
    internal class XcodeCommand : AExpectSubCommand {
    }
    
    [Command(
        "encrypt", "en",
        Description = "Encrypt files using the XCODE algorithm. Output the list of processed files.",
        ExtendedHelpText = ""
    )]
    internal class EncryptXcodeCommand : ProcessFileListBaseCommand {
        
        [Option("-k|--key", @"The encryption key to use for the process. Defaults to ""Progress"".", CommandOptionType.SingleValue)]
        public string EncryptionKey { get; set; }
        
        [Option("-su|--suffix", "A suffix to append to each filename processed.", CommandOptionType.SingleValue)]
        public string OutputFileSuffix { get; }
        
        [LegalFilePath]
        [Option("-od|--output-directory", "Output all processed file in this common directory.", CommandOptionType.SingleValue)]
        public string OutputDirectory { get; }
        
        protected void ValidateOptions() {
            if (string.IsNullOrEmpty(EncryptionKey)) {
                EncryptionKey = "Progress";
                Log.Info($"Using default encryption key : {EncryptionKey.PrettyQuote()}.");
            }

            if (EncryptionKey.Length > 8) {
                Log.Warn("The max length for encryption key is 8 characters. Everything above is actually ignored.");
                Log.Warn($"The actual key used will be : {EncryptionKey.Substring(0, 8).PrettyQuote()}.");
            }
        }

        protected void ProcessFiles(CommandLineApplication app, bool encrypt) {
            ValidateOptions();

            if (!string.IsNullOrEmpty(OutputDirectory) && !Directory.Exists(OutputDirectory)) {
                Log.Info($"Creating directory : {OutputDirectory}.");
                Directory.CreateDirectory(OutputDirectory);
            }

            Log.Info("Listing files...");

            var filesList = GetFilesList(app).ToList();
            
            Log.Info($"Processing {filesList.Count} files.");
            
            var xcode = new UoeEncryptor(EncryptionKey);

            var i = 0;
            var outputList = new List<string>();
            foreach (var file in filesList) {
                CancelToken?.ThrowIfCancellationRequested();
                var outputFile = Path.Combine((string.IsNullOrEmpty(OutputDirectory) ? Path.GetDirectoryName(file) : OutputDirectory) ?? "", $"{Path.GetFileName(file)}{(string.IsNullOrEmpty(OutputFileSuffix) ? "" : OutputFileSuffix)}");
                try {
                    xcode.ConvertFile(file, encrypt, outputFile);
                    outputList.Add($"{file} >> {outputFile}");
                } catch (UoeAlreadyConvertedException) { }
                i++;
                Log.ReportProgress(filesList.Count, i, $"Converting file to : {outputFile}.");
            }

            foreach (var file in outputList) {
                Out.WriteResultOnNewLine(file);
            }

            Log.Info($"A total of {outputList.Count} files were converted.");
        }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            ProcessFiles(app, true);
            return 0;
        }
    }

    [Command(
        "decrypt", "de",
        Description = "Decrypt files using the XCODE algorithm. Output the list of processed files.", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class DecryptXcodeCommand : EncryptXcodeCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            ProcessFiles(app, false);
            return 0;
        }
    }

    [Command(
        "list", "li",
        Description = "List only encrypted (or decrypted) files.", 
        ExtendedHelpText = @"Examples:

  List only the encrypted files in a list of files in argument.
    sakoe xcode list -r -vb none -nop
  Get a raw list of all the encrypted files in the current directory (recursive).
    sakoe xcode list -r -vb none -nop", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ListXcodeCommand : ProcessFileListBaseCommand {
        
        [Option("-de|--decrypted", "List all decrypted files (or default to listing encrypted files).", CommandOptionType.NoValue)]
        public bool ListDecrypted { get; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Log.Info("Listing files...");

            var filesList = GetFilesList(app).ToList();
            
            Log.Info($"Starting the analyze on {filesList.Count} files.");
            
            var xcode = new UoeEncryptor(null);

            var i = 0;
            var outputList = new List<string>();
            foreach (var file in filesList) {
                CancelToken?.ThrowIfCancellationRequested();
                var isEncrypted = xcode.IsFileEncrypted(file);
                if (isEncrypted && !ListDecrypted || !isEncrypted && ListDecrypted) {
                    outputList.Add(file);
                }
                i++;
                Log.ReportProgress(filesList.Count, i, $"Analyzing {file}.");
            }

            foreach (var file in outputList) {
                Out.WriteResultOnNewLine(file);
            }

            Log.Info($"A total of {outputList.Count} files are {(ListDecrypted ? "decrypted" : "encrypted")}.");

            return 0;
        }
    }

}