using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Runner.Cli.Config.v2;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Runner.Cli.Command {
    
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
            WriteWarning("Package");
            app.ShowHint();
            return 1;
        }

    }
    
    
}