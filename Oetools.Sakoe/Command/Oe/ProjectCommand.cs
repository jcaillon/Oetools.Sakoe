using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "project", "pr",
        Description = "Commands related to an Openedge project (.oe directory).",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(NewProjectCommand))]
    internal class ProjectCommand : OeBaseCommand {
   }
    
    [Command(
        "initialize", "init", "in",
        Description = "Initialize a new Openedge project.",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class NewProjectCommand : OeBaseCommand {
        
        [DirectoryExists]
        [Argument(0, Name = "<directory>", Description = "The directory in which to initialize the project. Default to current directory.")]
        public string SourceDirectory { get; set; }
        
        [LegalFilePath]
        [Option("-p|--project-name <name>", "The name of the project to create. Default to the current directory name.", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (string.IsNullOrEmpty(SourceDirectory)) {
                SourceDirectory = Directory.GetCurrentDirectory();
                Log.Trace?.Write($"Using current directory to initialize the project : {SourceDirectory.PrettyQuote()}.");
            }

            SourceDirectory = SourceDirectory.MakePathAbsolute().ToCleanPath();

            if (string.IsNullOrEmpty(ProjectName)) {
                ProjectName = Path.GetFileName(SourceDirectory);
                Log.Trace?.Write($"Using directory name for the project name : {ProjectName.PrettyQuote()}.");
            }

            var projectDirectory = Path.Combine(SourceDirectory, OeBuilderConstants.OeProjectDirectory);
            if (Utils.CreateDirectoryIfNeeded(projectDirectory, FileAttributes.Hidden)) {
                Log.Info($"Created project directory : {projectDirectory.PrettyQuote()}.");
            }

            Log.Trace?.Write("Generating a default project.");
            var project = OeProject.GetStandardProject();
            
            var projectFilePath = Path.Combine(projectDirectory, $"{ProjectName}{OeBuilderConstants.OeProjectExtension}");
            if (File.Exists(projectFilePath)) {
                throw new CommandValidationException($"The project file already exists, delete it first : {projectFilePath.PrettyQuote()}.");
            }
            Log.Info($"Creating openedge project file : {projectFilePath.PrettyQuote()}.");
            project.Save(projectFilePath);
            
            
            
            
            //if (ProjectFilePath?.EndsWith(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) ?? false) {
            //    if (ProjectFilePath.Equals(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase)) {
            //        ProjectFilePath = null;
            //    } else {
            //        ProjectFilePath = ProjectFilePath.Substring(0, ProjectFilePath.IndexOf(OeBuilderConstants.OeProjectExtension, StringComparison.CurrentCultureIgnoreCase) - 1);
            //    }
            //}
            //
            //if (string.IsNullOrEmpty(ProjectFilePath)) {
            //    ProjectFilePath = Path.GetFileName(Directory.GetCurrentDirectory());
            //}
            //
            //// to absolute path
            //var projectPah = $"{ProjectFilePath.MakePathAbsolute()}{OeBuilderConstants.OeProjectExtension}";
            //
            ////XmlOeProject.Save(new XmlOeProject { Properties = new XmlOeProjectProperties() }, projectPah);
            //
            //// .gitignore
            ////"############\n#  Sakoe   #\n############\n# never push the local directory\n.oe/local/\n# do not push the bin directory\nbin/"
            
            return 0;
        }
    }
    
}