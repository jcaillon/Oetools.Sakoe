using System;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "build", "bu",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class BuildCommand : OeBaseCommand {
        
        [LegalFilePath]
        [Argument(0, Name = "Project file path", Description = "Path to the project file (" + OeBuilderConstants.OeProjectExtension + " extension is optional), default to any " + OeBuilderConstants.OeProjectExtension + " file in the .oe folder if it exists")]
        protected string ProjectFilePath { get; set; }

        [Option("-c|--config-name", "The name of the configuration to use for the build, found in " + OeBuilderConstants.OeProjectExtension + " file", CommandOptionType.SingleValue)]
        protected string ConfigurationName { get; set; }
        
        [Option("-tm|--test-mode", "", CommandOptionType.NoValue)]
        protected bool TestMode { get; set; }

        [Option("-fr|--force-rebuild", "", CommandOptionType.NoValue)]
        protected bool ForceFullRebuild { get; set; }    
        
        [LegalFilePath]
        [FileExists]
        [Option("-cf|--extra-config-file", "", CommandOptionType.SingleValue)]
        protected string ExtraConfigFilePath { get; set; }
        
        [LegalFilePath]
        [Option("-sd|--source-directory", "Specify the source directory for the build, default to the current directory", CommandOptionType.SingleValue)]
        protected string SourceDirectory { get; set; }
        
        // ----------- OVERRIDE ------------
        
        [LegalFilePath]
        [Option("-o|--output-directory", "Specify the output directory for the build (overrides value in " + OeBuilderConstants.OeProjectExtension + ")", CommandOptionType.SingleValue)]
        protected string OutputDirectory { get; set; }

        [LegalFilePath]
        [Option("-rp|--report-path", " (overrides value in " + OeBuilderConstants.OeProjectExtension + ")", CommandOptionType.SingleValue)]
        protected string ReportFilePath { get; set; }
        
        [LegalFilePath]
        [Option("-ho|--history-output", " (overrides value in " + OeBuilderConstants.OeProjectExtension + ")", CommandOptionType.SingleValue)]
        protected string BuildHistoryOutputFilePath { get; set; }
        
        [LegalFilePath]
        [FileExists]
        [Option("-hi|--history-input", " (overrides value in " + OeBuilderConstants.OeProjectExtension + ")", CommandOptionType.SingleValue)]
        protected string BuildHistoryInputFilePath { get; set; }    
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Log.Warn("Build");
            return 1;
        }

    }
    
    
}