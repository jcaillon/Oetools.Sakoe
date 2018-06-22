using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Runner.Cli.Config.v2;
using Oetools.Runner.Cli.Lib;
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
    [Subcommand("new", typeof(NewProjectCommand))]
    internal class ProjectCommand : BaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            WriteWarning("You must provide a command");
            app.ShowHint();
            return 1;
        }

    }
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class NewProjectCommand : BaseCommand {
        
        [LegalFilePath]
        [Argument(0, Name = "Project file path", Description = "The path to the project file to create (without extension)")]
        protected string ProjectFilePath { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            if (ProjectFilePath?.EndsWith(OeConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) ?? false) {
                if (ProjectFilePath.Equals(OeConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase)) {
                    ProjectFilePath = null;
                } else {
                    ProjectFilePath = ProjectFilePath.Substring(0, ProjectFilePath.IndexOf(OeConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) - 1);
                }
            }
            
            if (string.IsNullOrEmpty(ProjectFilePath)) {
                ProjectFilePath = Path.GetFileName(Directory.GetCurrentDirectory());
            }
            
            // to absolute path
            var projectPah = $"{ProjectFilePath.MakePathAbsolute()}{OeConstants.OeProjectExtension}";
            
            XmlOeProject.Save(new XmlOeProject { Properties = new XmlOeProject.XmlOeProjectConfiguration(), WebclientProperties = new XmlOeProject.XmlOeProjectWebclientConfiguration() }, projectPah);
            
            return 0;
        }
    }
    
}