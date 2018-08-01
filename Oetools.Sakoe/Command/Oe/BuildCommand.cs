using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Lib;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class BuildCommand : OeBaseCommand {
        
        [Required]
        [LegalFilePath]
        [Argument(0, Name = "Project file path", Description = "Path to the project file (" + OeConstants.OeProjectExtension + " extension is optional)")]
        protected string ProjectFilePath { get; set; }

        [Required]
        [Option("-o|--output-directory", "", CommandOptionType.SingleValue)]
        protected string OutputDirectory { get; set; }

        [Option("-c|--config-name", "The name of the configuration to use for the build, found in " + OeConstants.OeProjectExtension + " file", CommandOptionType.SingleValue)]
        protected string ConfigurationName { get; set; }

        [Option("-r|--report-path", "", CommandOptionType.SingleValue)]
        protected string ReportPath { get; set; }
        
        [Option("-tf|--target-folder", "Override the target folder specified in the project file", CommandOptionType.SingleValue)]
        protected string TargetFolder { get; set; }
        
        [Option("-ho|--history-output", "", CommandOptionType.SingleValue)]
        protected string DeploymentHistoryOutputPath { get; set; }
        
        [Option("-hi|--history-input", "", CommandOptionType.SingleValue)]
        protected string DeploymentHistoryInputPath { get; set; }        
        
        [Option("-tm|--test-mode", "", CommandOptionType.NoValue)]
        protected bool TestMode { get; set; }
        
        [Option("-fr|--force-rebuild", "", CommandOptionType.NoValue)]
        protected bool ForceFullRebuild { get; set; }
                
        /// <summary>
        /// It defaults to the output directory
        /// </summary>
        [Option("-rd|--reference-directory", "", CommandOptionType.SingleValue)]
        protected string ReferenceDirectory { get; set; }
       
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteWarn("Build");
            app.ShowHint();
            return 1;
        }

    }
    
    
}