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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using CommandLineUtilsPlus;
using CommandLineUtilsPlus.Extension;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder;
using Oetools.Builder.History;
using Oetools.Builder.Project;
using Oetools.Builder.Project.Properties;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Command.Oe.Database;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Attributes;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "project", "pj",
        Description = "Commands related to an Openedge project (.oe directory)."
    )]
    [Subcommand(typeof(ProjectBuildCommand))]
    [Subcommand(typeof(ProjectDatabaseCommand))]
    [Subcommand(typeof(ProjectInitCommand))]
    [Subcommand(typeof(ProjectGitignoreCommand))]
    [Subcommand(typeof(ProjectListCommand))]
    [Subcommand(typeof(ProjectUpdateCommand))]
    internal class ProjectCommand : ABaseParentCommand {
    }


    [Command(
        "init", "in",
        Description = "Initialize a new Openedge project file (" + OeBuilderConstants.OeProjectExtension + ").",
        ExtendedHelpText = ""
    )]
    internal class ProjectInitCommand : ABaseExecutionCommand {

        [LegalFilePath]
        [Option("-f|--file <name>", "The name of the project (project file) to create. Defaults to the current directory name.", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }

        [DirectoryExists]
        [Option("-d|--directory <dir>", "The directory in which to initialize the project. Defaults to the current directory.", CommandOptionType.SingleValue)]
        public string SourceDirectory { get; set; }

        [Option("-l|--local", "Create the new project file for local use only. A local project should contain build configurations specific to your machine and thus should not be shared or versioned in your source control system.", CommandOptionType.NoValue)]
        public bool IsLocalProject { get; set; }

        [Option("-o|--override", "Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists.", CommandOptionType.NoValue)]
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

            HelpWriter.WriteOnNewLine(null);
            HelpWriter.WriteSectionTitle("IMPORTANT README:");
            HelpWriter.WriteOnNewLine(@"
The project file created (" + OeBuilderConstants.OeProjectExtension + @") is defined in XML format and has a provided XML schema definition file (" + OeProject.XsdName + @").

The project XML schema is fully documented and should be used to enable intellisense in your favorite editor.
Example of xml editors with out-of-the-box intellisense (autocomplete) features for xml:

 - Progress Developer studio (eclipse)
 - Visual studio
 - Most jetbrain IDE

Drag and drop the created " + OeBuilderConstants.OeProjectExtension + @" file into the editor of your choice and start configuring your build.
The file " + Path.Combine(OeBuilderConstants.GetProjectDirectory(""), $"{ProjectName}{OeBuilderConstants.OeProjectExtension}").PrettyQuote() + @" should be versioned in your source repository to allow anyone who clones your application to build it.
The file " + OeProject.XsdName.PrettyQuote() + @", however, should not be versioned with your application because it depends on the version of this tool (sakoe). If this tool is updated, use the command " + typeof(ProjectUpdateCommand).GetFullCommandLine<MainCommand>().PrettyQuote() + @" to update the " + OeProject.XsdName.PrettyQuote() + @" to the latest version.

If you need to have a project file containing build configurations specific to your local machine, you can use the option " + (GetCommandOptionFromPropertyName(nameof(IsLocalProject))?.Template ?? "").PrettyQuote() + @". This will create the project file into the directory " + OeBuilderConstants.GetProjectDirectoryLocal("").PrettyQuote() + @" which should NOT be versioned.
For git repositories, use the command " + typeof(ProjectGitignoreCommand).GetFullCommandLine<MainCommand>().PrettyQuote() + @" to set up your .gitignore file for sakoe projects.");
            HelpWriter.WriteOnNewLine(null);
            return 0;
        }
    }

    [Command(
        "git-ignore", "gi",
        Description = "Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists).",
        ExtendedHelpText = ""
    )]
    internal class ProjectGitignoreCommand : ABaseExecutionCommand {

        [DirectoryExists]
        [Option("-d|--directory <dir>", "The repository base directory (source directory). Defaults to the current directory.", CommandOptionType.SingleValue)]
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
        "list", "ls",
        Description = "List all the project files or list the build configurations in a project file."
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
        [Option("-d|--directory <dir>", "The directory in which the project is located. Defaults to the current directory.", CommandOptionType.SingleValue)]
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


    [Command(
        Name, "bd",
        Description = "Build automation for Openedge projects."
    )]
    [CommandAdditionalHelpText(nameof(GetAdditionalHelpText))]
    internal class ProjectBuildCommand : AOeDlcCommand {

        public const string Name = "build";
        private const string PropertyHelpLongName = "--property-help";
        private const string ConfigurationNameLongName = "--config-name";
        private const string ExtraConfigOption = "extra-config";

        [LegalFilePath]
        [Option("-f|--file <file>", "Path or name of the project file. The " + OeBuilderConstants.OeProjectExtension + " extension is optional. Defaults to the first " + OeBuilderConstants.OeProjectExtension + " file found.\nThe search is done in the current directory and in the " + OeBuilderConstants.OeProjectDirectory + " directory when it exists.", CommandOptionType.SingleValue)]
        public string ProjectFile { get; set; }

        [Option("-c|" + ConfigurationNameLongName + " <config>", "The name of the build configuration to use for the build. This name is found in the " + OeBuilderConstants.OeProjectExtension + " file.\nDefaults to the first build configuration found in the project file.", CommandOptionType.SingleValue)]
        public string ConfigurationName { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "e", LongName = ExtraConfigOption, ValueName = "project=config", Description = @"In addition to the base build configuration specified by <project> and " + ConfigurationNameLongName + @", you can dynamically add a child configuration to the base configuration with this option. This option can be used multiple times, each new configuration will be added as a child of the previously defined configuration.
This option allows you to share, with your colleagues, a common project file that holds the property of your application and have an extra configuration in local (just for you) which you can use to build the project in a specific local directory.
For each extra configuration, specify the path or the name of the project file and the configuration name to use. If the project file name if empty, the main <project> is used.")]
        public string[] ExtraConfigurations { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "p", LongName = "property", ValueName = "key=value", Description = @"A pair of key/value to dynamically set a property for this build. The value set this way will prevail over the value defined in a project file.
Each pair should specify the name of the property to set and the value that should be used.
Use the option " + PropertyHelpLongName + " to see the full list of properties available as well as their documentation.")]
        public string[] BuildProperties { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "v", LongName = "variable", ValueName = "key=value", Description = @"A pair of key/value to dynamically set a variable for this build. A variable set this way will prevail over a variable with the same name defined in a project file.
Each pair should specify the name of the variable to set and the value that should be used.")]
        public string[] BuildVariables { get; set; }

        [Option("-ph|" + PropertyHelpLongName, "Shows the list of each build property usable with its full documentation.", CommandOptionType.NoValue)]
        public bool ShowBuildPropertyHelp { get; set; }

        /// <summary>
        /// Additional help
        /// </summary>
        public static void GetAdditionalHelpText(IHelpWriter formatter, CommandLineApplication application, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("BUILD PROPERTIES");

            foreach (var property in GetAvailableBuildProperties().OrderBy(p => p.Key)) {
                formatter.WriteOnNewLine($"* -p \"{property.Key}={property.Value ?? ""}\"");
            }
            formatter.WriteOnNewLine(null);
            formatter.WriteTip($"Display the full documentation of each build property by running {$"{application.GetFullCommandLine()} {PropertyHelpLongName}".PrettyQuote()}.");

            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("EXTRA CONFIG");
            formatter.WriteOnNewLine($"The example below illustrate the usage of the --{ExtraConfigOption} option:");
            formatter.WriteOnNewLine(@"We have 2 project files (p1 and p2) containing build configurations as described below:

p1" + OeBuilderConstants.OeProjectExtension + @"
├─ Configuration1
└─ Configuration2

p2" + OeBuilderConstants.OeProjectExtension + @"
└─ Configuration3
   ├─ Configuration4
   │  ├─ Configuration5
   │  └─ Configuration6
   └─ Configuration7

We use the following command line to start a build:");
            formatter.WriteOnNewLine(null);
            formatter.WriteOnNewLine($"{application.GetFullCommandLine()} p1 {ConfigurationNameLongName} Configuration1 --{ExtraConfigOption} p2=Configuration6 --{ExtraConfigOption} Configuration2".PrettyQuote(), padding: 2);
            formatter.WriteOnNewLine(null);
            formatter.WriteOnNewLine(@"Below is the equivalent of how build configurations are nested in this scenario:

Configuration1
└─ Configuration3
   └─ Configuration4
      └─ Configuration6
         └─ Configuration2

This allow a lot of flexibility for organizing and partitioning your build process.");

            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("NOTES");
            formatter.WriteOnNewLine($"Create a new project file using the command: {typeof(ProjectInitCommand).GetFullCommandLine<MainCommand>().PrettyQuote()}.");
            formatter.WriteOnNewLine($"Get a more in-depth help and learn about the concept of a build (in sakoe) using the command: {typeof(BuildManCommand).GetFullCommandLine<MainCommand>().PrettyQuote()}.");
        }

        /// <inheritdoc />
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            // TODO: show task error and compilation error

            // show help.
            if (ShowBuildPropertyHelp) {
                HelpWriter.WriteOnNewLine(null);
                HelpWriter.WriteSectionTitle("BUILD PROPERTIES");

                foreach (var property in GetAvailableBuildProperties().OrderBy(p => p.Key)) {
                    HelpWriter.WriteOnNewLine(property.Key);
                    HelpWriter.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(property.Key), padding: 20);
                    HelpWriter.WriteOnNewLine(null);
                }
                return 0;
            }

            if (string.IsNullOrEmpty(ProjectFile)) {
                ProjectFile = GetCurrentProjectFilePath();
            } else {
                ProjectFile = GetProjectFilePath(ProjectFile);
            }
            Log?.Debug($"Base project file: {ProjectFile.PrettyQuote()}.");

            // get extra config
            var configQueue = new Queue<Tuple<string, string>>();
            configQueue.Enqueue(new Tuple<string, string>(ProjectFile, ConfigurationName));

            if (ExtraConfigurations != null) {
                foreach (var extraConfig in ExtraConfigurations) {
                    if (string.IsNullOrEmpty(extraConfig)) {
                        throw new CommandValidationException("The extra configuration option can't be empty.");
                    }

                    string file;
                    string name;
                    var split = extraConfig.Split('=');
                    if (split.Length == 1) {
                        file = ProjectFile;
                        name = split[0];
                    } else if (split.Length == 2) {
                        file = split[0];
                        name = split[1];
                    } else {
                        throw new CommandValidationException("There should be only one character '=' in the extra configuration.");
                    }

                    configQueue.Enqueue(new Tuple<string, string>(file, name));
                    Log?.Debug($"Added an extra configuration: {name.PrettyQuote()} in {file.PrettyQuote()}.");
                }
            }

            // check properties
            var availableBuildProperties = GetAvailableBuildProperties();
            var keyValueProperties = new Dictionary<string, string>();
            if (BuildProperties != null) {
                foreach (var buildProperty in BuildProperties) {
                    var split = buildProperty.Split('=');
                    if (split.Length != 2) {
                        throw new CommandValidationException($"There should be exactly one character '=' in the property {buildProperty.PrettyQuote()}.");
                    }
                    if (!availableBuildProperties.ContainsKey(split[0])) {
                        throw new CommandValidationException($"The property {split[0].PrettyQuote()} does not exist, use the option {PropertyHelpLongName} to list the available properties.");
                    }
                    if (!keyValueProperties.ContainsKey(split[0])) {
                        keyValueProperties.Add(split[0], split[1]);
                    } else {
                        Log?.Debug($"The property {split[0]} has been defined several times, we keep only the latest.");
                        keyValueProperties[split[0]] = split[1];
                    }
                }
            }

            // check variables
            List<OeVariable> addedVariables = null;
            if (BuildVariables != null) {
                addedVariables = new List<OeVariable>();
                foreach (var kpv in BuildVariables) {
                    var split = kpv.Split('=');
                    if (split.Length != 2) {
                        throw new CommandValidationException($"There should be exactly one character '=' in the variable definition {kpv.PrettyQuote()}.");
                    }
                    addedVariables.Add(new OeVariable {
                        Name = split[0],
                        Value = split[1]
                    });
                }
            }

            var config = OeProject.GetConfiguration(configQueue);
            using (var builder = new BuilderAuto(config)) {
                builder.BuildConfiguration.Properties.SetPropertiesFromKeyValuePairs(keyValueProperties);
                if (addedVariables != null) {
                    builder.BuildConfiguration.Variables.AddRange(addedVariables);
                }
                builder.BuildConfiguration.Properties.DlcDirectoryPath = GetDlcPath();
                builder.CancelToken = CancelToken;
                builder.Log = GetLogger();
                builder.Build();


                // display compilation error.
                var compilationProblems = builder.CompilationProblems;
                if (compilationProblems.Count > 0) {
                    Out.WriteErrorOnNewLine(null);
                    Out.WriteErrorOnNewLine("COMPILATION ERRORS", ConsoleColor.Yellow, 1);
                    var groups = compilationProblems.GroupBy(cp => cp.FilePath).ToList();
                    var i = 0;
                    foreach (var filePathGrouped in groups) {
                        Out.WriteErrorOnNewLine($"{(groups.Count - 1 == i ? "└─ " : "├─ ")}In {filePathGrouped.Key}:", indentation: 1);
                        var j = 0;
                        foreach (var problem in filePathGrouped) {
                            Out.WriteErrorOnNewLine($"{(groups.Count - 1 == i ? "   " : "│  ")}{(filePathGrouped.Count() - 1 == j ? "└─ " : "├─ ")}", indentation: 1);
                            Out.WriteError($"Line {problem.Line}, Column {problem.Column}, error {problem.ErrorNumber} : {(string.IsNullOrEmpty(problem.FilePath) ? problem.Message : problem.Message.Replace(problem.FilePath, Path.GetFileName(problem.FilePath)))}", problem is OeCompilationWarning ? ConsoleColor.Yellow : ConsoleColor.Red, 1);
                            j++;
                        }
                        i++;
                    }
                }

                // display exceptions.
                var taskExceptions = builder.TaskExecutionExceptions;
                if (taskExceptions.Count > 0) {
                    Out.WriteErrorOnNewLine(null);
                    Out.WriteErrorOnNewLine("TASK EXCEPTIONS", ConsoleColor.Yellow, 1);
                    var i = 0;
                    foreach (var exception in taskExceptions) {
                        Out.WriteErrorOnNewLine(taskExceptions.Count - 1 == i ? "└─ " : "├─ ", indentation: 1);
                        Out.WriteError(exception.Message, exception.IsWarning ? ConsoleColor.Yellow : ConsoleColor.Red, 3);
                        i++;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Returns a dictionary of property NAME -> DEFAULT VALUE for all the available properties.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetAvailableBuildProperties() {
            var properties = new Dictionary<string, string>();
            foreach (var type in new List<Type> { typeof(OeProperties), typeof(OeSourceFilterOptions), typeof(OePropathFilterOptions), typeof(OeIncrementalBuildOptions), typeof(OeGitFilterOptions), typeof(OeCompilationOptions), typeof(OeBuildOptions) }) {
                if (!type.IsPublic || type.IsAbstract) {
                    continue;
                }
                foreach (var propertyInfo in type.GetProperties()) {
                    if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType.GenericTypeArguments.Length == 1 && propertyInfo.PropertyType.GenericTypeArguments[0].IsValueType) {
                        string name = null;
                        if (Attribute.GetCustomAttribute(propertyInfo, typeof(XmlElementAttribute), true) is XmlElementAttribute attr2) {
                            name = attr2.ElementName;
                        }
                        if (Attribute.GetCustomAttribute(propertyInfo, typeof(XmlAttributeAttribute), true) is XmlAttributeAttribute attr3) {
                            name = attr3.AttributeName;
                        }
                        if (string.IsNullOrEmpty(name) || name.Equals("Name")) {
                            continue;
                        }
                        if (!properties.ContainsKey(name)) {
                            string defaultValue = null;
                            if (Attribute.GetCustomAttribute(propertyInfo, typeof(DefaultValueMethodAttribute), true) is DefaultValueMethodAttribute attribute) {
                                if (!string.IsNullOrEmpty(attribute.MethodName)) {
                                    var methodInfo = type.GetMethod(attribute.MethodName, BindingFlags.Public | BindingFlags.Static| BindingFlags.FlattenHierarchy);
                                    if (methodInfo != null) {
                                        if (Attribute.GetCustomAttribute(methodInfo, typeof(DescriptionAttribute), true) is DescriptionAttribute description) {
                                            defaultValue = description.Description;
                                        } else if (!propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType == typeof(string)) {
                                            defaultValue = methodInfo.Invoke(null, null).ToString();
                                        }
                                    }
                                }
                            }
                            properties.Add(name, defaultValue);
                        }

                    }
                }
            }
            return properties;
        }

    }

}
