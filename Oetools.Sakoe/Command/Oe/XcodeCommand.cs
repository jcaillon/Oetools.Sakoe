using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "xcode", "xc",
        Description = "Encrypt and decrypt files using the algorithm of the XCODE utility",
        ExtendedHelpText = "The original idea of the XCODE utility is to obfuscate your openedge code before making it available.\nThis is an encryption feature which uses a ASCII key/password of a maximum of 8 characters.\nThe original XCODE utility uses the default key \"Progress\" if no custom key is supplied (so does this command).\nThe encryption process does not use a standard cryptography method, it uses a 16-bits CRC inside a custom algo.",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
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
    internal class EncryptXcodeCommand : OeBaseCommand {
        
        [Option("-f|--filter", "", CommandOptionType.SingleValue)]
        protected string IncludeFilter { get; set; }
        
        [Option("-k|--key", "", CommandOptionType.SingleValue)]
        protected string EncryptionKey { get; set; }
        
        [Option("-pre|--prefix", "", CommandOptionType.SingleValue)]
        protected string OutputFilePrefix { get; set; }
        
        [DirectoryExists]
        [Option("-od|--output-directory", "", CommandOptionType.SingleValue)]
        protected string OutputDirectory { get; set; }
        
        public string[] RemainingArgs { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            foreach (var arg in RemainingArgs) {
                Log.Info(arg);
            }
            
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
        protected string EncryptionKey { get; set; }

        [Option("-pre|--prefix", "", CommandOptionType.SingleValue)]
        protected string OutputFilePrefix { get; set; }

        [DirectoryExists]
        [Option("-d|--output-directory", "", CommandOptionType.SingleValue)]
        protected string OutputDirectory { get; set; }

        public string[] RemainingArgs { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            return 0;
        }
    }
}