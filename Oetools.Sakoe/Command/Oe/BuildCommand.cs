using System;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Oe.Abstract;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "build", "bu",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class BuildCommand : AOeBaseCommand {
        
        [LegalFilePath]
        [Argument(0, "<project file path>", "Path to the project file (" + OeBuilderConstants.OeProjectExtension + " extension is optional), default to any " + OeBuilderConstants.OeProjectExtension + " file in the .oe folder if it exists")]
        protected string ProjectFilePath { get; set; }

        [Option("-c|--config-name", "The name of the configuration to use for the build, found in " + OeBuilderConstants.OeProjectExtension + " file", CommandOptionType.SingleValue)]
        protected string ConfigurationName { get; set; }
        
        [LegalFilePath]
        [FileExists]
        [Option("-cf|--extra-config-file", "", CommandOptionType.SingleValue)]
        protected string ExtraConfigFilePath { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            // TODO : list leafs of xml in the help. Use the xsd to get the help text of each

            var project = OeProject.Load(ProjectFilePath ?? GetCurrentProjectFilePath());
            using (var builder = new BuilderAuto(project, ConfigurationName)) {
                builder.CancelToken = CancelToken;
                builder.Log = Log;
                builder.Build();}
            return 0;
        }

    }
    
    
}