using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "project", "pr",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(NewProjectCommand))]
    internal class ProjectCommand : OeBaseCommand {
   }
    
    [Command(
        "new", "ne",
        Description = "TODO",
        ExtendedHelpText = "TODO",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class NewProjectCommand : OeBaseCommand {
        
        [LegalFilePath]
        [Argument(0, Name = "Project file path", Description = "The path to the project file to create (without extension)")]
        protected string ProjectFilePath { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            if (ProjectFilePath?.EndsWith(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) ?? false) {
                if (ProjectFilePath.Equals(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase)) {
                    ProjectFilePath = null;
                } else {
                    ProjectFilePath = ProjectFilePath.Substring(0, ProjectFilePath.IndexOf(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) - 1);
                }
            }
            
            if (string.IsNullOrEmpty(ProjectFilePath)) {
                ProjectFilePath = Path.GetFileName(Directory.GetCurrentDirectory());
            }
            
            // to absolute path
            var projectPah = $"{ProjectFilePath.MakePathAbsolute()}{OeBuilderConstants.OeProjectExtension}";
            
            //XmlOeProject.Save(new XmlOeProject { Properties = new XmlOeProjectProperties() }, projectPah);
            
            // .gitignore
            //"############\n#  Sakoe   #\n############\n# never push the local directory\n.oe/local/\n# do not push the bin directory\nbin/"
            
            return 0;
        }
    }
    
}