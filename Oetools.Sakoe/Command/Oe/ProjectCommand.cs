using System;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "project", "pr",
        Description = "Commands related to an Openedge project (.oe directory).",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(IniProjectCommand))]
    [Subcommand(typeof(InitGitignoreCommand))]
    internal class ProjectCommand : BaseCommand {
   }
    
    [Command(
        "init", "in",
        Description = "Initialize a new Openedge project.",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class IniProjectCommand : BaseCommand {
        
        [DirectoryExists]
        [Argument(0, Name = "<directory>", Description = "The directory in which to initialize the project. Default to current directory.")]
        public string SourceDirectory { get; set; }
        
        [LegalFilePath]
        [Option("-p|--project-name <name>", "The name of the project to create. Default to the current directory name.", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }
        
        
        [Option("-l|--local", "Indicates if the project should be created for a local use only.", CommandOptionType.NoValue)]
        public bool IsLocalProject { get; set; }
        
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

            var projectDirectory = IsLocalProject ? OeBuilderConstants.GetProjectDirectoryLocal(SourceDirectory) : OeBuilderConstants.GetProjectDirectory(SourceDirectory);
            if (Utils.CreateDirectoryIfNeeded(projectDirectory, FileAttributes.Hidden)) {
                Log.Info($"Created project directory : {projectDirectory.PrettyQuote()}.");
            }

            Log.Trace?.Write("Generating a default project.");
            var project = OeProject.GetStandardProject();
            
            var projectFilePath = Path.Combine(projectDirectory, $"{ProjectName}{OeBuilderConstants.OeProjectExtension}");
            if (File.Exists(projectFilePath)) {
                throw new CommandValidationException($"The project file already exists, delete it first : {projectFilePath.PrettyQuote()}.");
            }
            
            Log.Info($"Creating openedge project file: {projectFilePath.PrettyQuote()}.");
            project.Save(projectFilePath);
            
            Log.Info($"Project created: {projectFilePath.PrettyQuote()}.");
            
            Log.Warn(@"=============

IMPORTANT README:

The project file created (" + OeBuilderConstants.OeProjectDirectory + @") is defined in XML format and has a provided XML schema definition file (Project.xsd).

The project XML schema is fully documented and should be used to enable intellisense in your favorite editor.
Example of xml editors with out-of-the-box intellisense (autocomplete) features for xml:

- Progress Developer studio (eclipse)
- Visual studio
- Most jetbrain IDE

Drag and drop the created " + OeBuilderConstants.OeProjectExtension + @" file into the editor of your choice and start configuring your build.
You can use the command " + GetCommandLineFromType(app, typeof(InitGitignoreCommand)).PrettyQuote() + @" to setup your git repo.
The file " + Path.Combine(OeBuilderConstants.OeProjectDirectory, OeBuilderConstants.OeProjectExtension).PrettyQuote() + @" should be versioned in your source repository to allow anyone who clones it to build your application.
If you need to have a project file containing build configurations for your machine only, you can define a local project.
");
            
            return 0;
        }
        
        
    }

    [Command(
        "gitinit", "gi", 
        Description = "Initialize a .gitignore file adapted for sakoe projects (append if exist).", 
        ExtendedHelpText = "", 
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase)
    ]
    internal class InitGitignoreCommand : BaseCommand {

        [DirectoryExists]
        [Argument(0, Name = "<directory>", Description = "The project directory. Defaults to the current directory.")]
        public string SourceDirectory { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var gitIgnorePath = Path.Combine(SourceDirectory, ".gitignore");
            
            if (File.Exists(gitIgnorePath)) {
                var gitIgnoreContent = File.ReadAllText(gitIgnorePath);
                if (gitIgnoreContent.Contains(".oe/local/")) {
                    Log?.Info("The .gitignore already exists and seems to already contain the appropriate exclusions.");
                }
            }

            var ignoreContent = new StringBuilder(@"
############
#  Sakoe   #
############

# never push the local directory
.oe/local/

# do not push the bin directory
bin/

# file extensions that should not be versioned
");

            ignoreContent.Append("*").AppendLine(UoeConstants.ExtListing);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtPreprocessed);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtXref);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtXrefXml);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtDebugList);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtR);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtProlibFile);
            
            File.AppendAllText(gitIgnorePath, ignoreContent.ToString());
            
            Log?.Info($"File written: {gitIgnorePath}.");

            return 0;
        }
    }

}