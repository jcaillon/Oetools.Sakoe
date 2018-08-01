using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Lib;
using Oetools.Sakoe.Serialization;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    [Subcommand("new", typeof(NewProjectCommand))]
    internal class ProjectCommand : OeBaseCommand {
   }
    
    [Command(
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class NewProjectCommand : OeBaseCommand {
        
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