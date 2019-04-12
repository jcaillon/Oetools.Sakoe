#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ProjectCommand.cs) is part of Oetools.Sakoe.
//
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "project", "pr",
        Description = "Commands related to an Openedge project (.oe directory).",
        ExtendedHelpText = ""
    )]
    [Subcommand(typeof(ProjectInitCommand))]
    [Subcommand(typeof(ProjectGitignoreCommand))]
    [Subcommand(typeof(ProjectListCommand))]
    [Subcommand(typeof(ProjectUpdateCommand))]
    internal class ProjectCommand : AExpectSubCommand {
    }


    [Command(
        "init", "in",
        Description = "Initialize a new Openedge project file (" + OeBuilderConstants.OeProjectExtension + ").",
        ExtendedHelpText = ""
    )]
    internal class ProjectInitCommand : ABaseCommand {

        [DirectoryExists]
        [Option("-d|--directory <path>", "The directory in which to initialize the project. Defaults to the current directory.", CommandOptionType.SingleValue)]
        public string SourceDirectory { get; set; }

        [LegalFilePath]
        [Option("-p|--project-name <name>", "The name of the project to create. Defaults to the current directory name.", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }

        [Option("-l|--local", "Create the new project file for local use only. A local project should contain build configurations specific to your machine and thus should not be shared or versioned in your source control system.", CommandOptionType.NoValue)]
        public bool IsLocalProject { get; set; }

        [Option("-f|--force", "Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists.", CommandOptionType.NoValue)]
        public bool Force { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (string.IsNullOrEmpty(SourceDirectory)) {
                SourceDirectory = Directory.GetCurrentDirectory();
                Log.Trace?.Write($"Using current directory to initialize the project: {SourceDirectory.PrettyQuote()}.");
            }

            SourceDirectory = SourceDirectory.ToAbsolutePath().ToCleanPath();

            if (string.IsNullOrEmpty(ProjectName)) {
                ProjectName = Path.GetFileName(SourceDirectory);
                Log.Info($"Using directory name for the project name: {ProjectName.PrettyQuote()}.");
            }

            var projectDirectory = IsLocalProject ? OeBuilderConstants.GetProjectDirectoryLocal(SourceDirectory) : OeBuilderConstants.GetProjectDirectory(SourceDirectory);
            if (Utils.CreateDirectoryIfNeeded(projectDirectory, FileAttributes.Hidden)) {
                Log.Info($"Created project directory: {projectDirectory.PrettyQuote()}.");
            }

            Log.Trace?.Write("Generating a default project.");
            var project = OeProject.GetStandardProject();

            var projectFilePath = Path.Combine(projectDirectory, $"{ProjectName}{OeBuilderConstants.OeProjectExtension}");
            if (File.Exists(projectFilePath)) {
                if (Force) {
                    File.Delete(projectFilePath);
                } else {
                    throw new CommandValidationException($"The project file already exists, delete it first: {projectFilePath.PrettyQuote()}.");
                }
            }

            Log.Info($"Creating Openedge project file: {projectFilePath.PrettyQuote()}.");
            project.Save(projectFilePath);

            Log.Info($"Project created: {projectFilePath.PrettyQuote()}.");

            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("IMPORTANT README:");
            HelpFormatter.WriteOnNewLine(@"
The project file created (" + OeBuilderConstants.OeProjectExtension + @") is defined in XML format and has a provided XML schema definition file (" + OeProject.XsdName + @").

The project XML schema is fully documented and should be used to enable intellisense in your favorite editor.
Example of xml editors with out-of-the-box intellisense (autocomplete) features for xml:

 - Progress Developer studio (eclipse)
 - Visual studio
 - Most jetbrain IDE

Drag and drop the created " + OeBuilderConstants.OeProjectExtension + @" file into the editor of your choice and start configuring your build.
The file " + Path.Combine(OeBuilderConstants.GetProjectDirectory(""), $"{ProjectName}{OeBuilderConstants.OeProjectExtension}").PrettyQuote() + @" should be versioned in your source repository to allow anyone who clones your application to build it.
The file " + OeProject.XsdName.PrettyQuote() + @", however, should not be versioned with your application because it depends on the version of this tool (sakoe). If this tool is updated, use the command " + typeof(ProjectUpdateCommand).GetFullCommandLine().PrettyQuote() + @" to update the " + OeProject.XsdName.PrettyQuote() + @" to the latest version.

If you need to have a project file containing build configurations specific to your local machine, you can use the option " + (GetCommandOptionFromPropertyName(nameof(IsLocalProject))?.Template ?? "").PrettyQuote() + @". This will create the project file into the directory " + OeBuilderConstants.GetProjectDirectoryLocal("").PrettyQuote() + @" which should NOT be versioned.
For git repositories, use the command " + typeof(ProjectGitignoreCommand).GetFullCommandLine().PrettyQuote() + @" to set up your .gitignore file for sakoe projects.");
            HelpFormatter.WriteOnNewLine(null);
            return 0;
        }
    }

    [Command(
        "gitignore", "gi",
        Description = "Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists).",
        ExtendedHelpText = ""
    )]
    internal class ProjectGitignoreCommand : ABaseCommand {

        [DirectoryExists]
        [Option("-d|--directory <path>", "The repository base directory (source directory). Defaults to the current directory.", CommandOptionType.SingleValue)]
        public string SourceDirectory { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var gitIgnorePath = Path.Combine(SourceDirectory, ".gitignore");

            bool hasCarriageReturn = false;

            if (File.Exists(gitIgnorePath)) {
                var gitIgnoreContent = File.ReadAllText(gitIgnorePath);
                if (gitIgnoreContent.Contains(".oe/local/")) {
                    Log?.Info("The .gitignore already exists and seems to already contain the appropriate exclusions.");
                    return 0;
                }

                hasCarriageReturn = gitIgnoreContent.IndexOf('\r') >= 0;
            }

            var ignoreContent = new StringBuilder(@"
############
#  Sakoe   #
############

# never push the local directory
.oe/local/

# do not push the bin directory
bin/

# do not version the project xsd
Project.xsd

# file extensions that should not be versioned
");

            ignoreContent.Append("*").AppendLine(UoeConstants.ExtListing);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtPreprocessed);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtXref);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtXrefXml);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtDebugList);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtR);
            ignoreContent.Append("*").AppendLine(UoeConstants.ExtProlibFile);

            ignoreContent.Replace("\r", "");

            if (hasCarriageReturn) {
                ignoreContent.Replace("\n", "\r\n");
            }

            File.AppendAllText(gitIgnorePath, ignoreContent.ToString());

            Log?.Info($"File written: {gitIgnorePath}.");

            return 0;
        }
    }

    [Command(
        "list", "li",
        Description = "List all the project files or list the build configurations in a project file.",
        ExtendedHelpText = ""
    )]
    internal class ProjectListCommand : AOeCommand {

        [LegalFilePath]
        [Option("-p|--path <path>", "The project file in which to list the build configurations or the project base directory (source directory) in which to list the project files. Defaults to the current directory.", CommandOptionType.SingleValue)]
        public string PathToList { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (string.IsNullOrEmpty(PathToList)) {
                PathToList = Directory.GetCurrentDirectory();
            }

            if (Directory.Exists(PathToList)) {
                var files = Directory.EnumerateFiles(PathToList, $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).ToList();
                files.AddRange(Directory.EnumerateFiles(Path.Combine(PathToList, OeBuilderConstants.OeProjectDirectory), $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).ToList());
                if (files.Count == 0) {
                    Log?.Warn($"No project file {OeBuilderConstants.OeProjectExtension} found.");
                } else {
                    foreach (var file in files) {
                        Out.WriteResultOnNewLine(file.ToRelativePath(PathToList));
                    }
                }
                return 0;
            }

            var projectFile = GetProjectFilePath(PathToList);

            var project = OeProject.Load(projectFile);

            foreach (var buildConfiguration in project.GetAllBuildConfigurations()) {
                Out.WriteResultOnNewLine((string.IsNullOrEmpty(buildConfiguration.Name) ? "Unnamed configuration" : "Named configuration").PadRight(30));
                Out.WriteResult(string.IsNullOrEmpty(buildConfiguration.Name) ? buildConfiguration.GetId().ToString() : buildConfiguration.Name);
            }

            return 0;
        }
    }

    [Command(
        "update", "up",
        Description = "Update the `" + OeProject.XsdName + "` file of the project with the latest version embedded in this tool.",
        ExtendedHelpText = ""
    )]
    internal class ProjectUpdateCommand : AOeCommand {

        [DirectoryExists]
        [Option("-d|--directory <path>", "The directory in which the project is located. Defaults to the current directory.", CommandOptionType.SingleValue)]
        public string ProjectDirectory { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var directory = ProjectDirectory ?? Directory.GetCurrentDirectory();

            if (Directory.EnumerateFiles(directory, $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).Any()) {
                OeProject.SaveXsd(directory);
            } else {
                directory = OeBuilderConstants.GetProjectDirectory(directory);
                if (Directory.EnumerateFiles(directory, $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).Any()) {
                    OeProject.SaveXsd(directory);
                } else {
                    directory = null;
                }
            }

            if (!string.IsNullOrEmpty(directory)) {
                Log.Info($"The file {Path.Combine(directory, OeProject.XsdName)} has been updated with the latest version.");
            }

            return 0;
        }
    }

}
