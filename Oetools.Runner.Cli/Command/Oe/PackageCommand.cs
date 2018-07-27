using System;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Runner.Cli.Command.Oe {
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class PackageCommand : BuildCommand {
        

        [Option("-rf|--referencefolder", "", CommandOptionType.SingleValue)]
        protected string ReferenceFolder { get; set; }
        
        [Option("-ru|--referenceurl", "", CommandOptionType.SingleValue)]
        protected string PreviousPackageUrl { get; set; }
        
        [Option("-pn|--packagename", "", CommandOptionType.SingleValue)]
        protected string PackageName { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteWarn("Package");
            app.ShowHint();
            return 1;
        }

    }
    
    
}