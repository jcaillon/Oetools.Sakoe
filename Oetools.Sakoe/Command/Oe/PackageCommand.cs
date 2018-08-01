using System;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class PackageCommand : BuildCommand {
        
        [Option("-ru|--referenceurl", "", CommandOptionType.SingleValue)]
        protected string PreviousPackageUrl { get; set; }
        
        /// <summary>
        /// The version name to use for this generated webclient package
        /// </summary>
        [Option("-wcav|--webclient-app-version", "", CommandOptionType.SingleValue)]
        protected string WebclientApplicationVersion { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteWarn("Package");
            app.ShowHint();
            return 1;
        }

    }
    
    
}