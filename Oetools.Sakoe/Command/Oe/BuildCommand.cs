using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder;
using Oetools.Builder.Project;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Name, "bu",
        Description = "Build automation for Openedge projects. This command is the bread and butter of this tool.",
        ExtendedHelpText = "Disclaimer: this command will not make your coffee... Yet!",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class BuildCommand : AOeCommand {
        public const string Name = "build";
        private const string PropertyHelpLongName = "--property-help";
        private const string ConfigurationNameLongName = "--config-name";
        
        [LegalFilePath]
        [Argument(0, "[<project file>]", "Path or name of the project file. The " + OeBuilderConstants.OeProjectExtension + " extension is optional. Defaults to the " + OeBuilderConstants.OeProjectExtension + " file found. The search is done in the current directory and in the " + OeBuilderConstants.OeProjectDirectory + " directory when it exists.")]
        public string ProjectFile { get; set; }

        [Option("-c|" + ConfigurationNameLongName + " <config_name>", "The name of the build configuration to use for the build. This name is found in the " + OeBuilderConstants.OeProjectExtension + " file.\nDefaults to the first build configuration found in the project file.", CommandOptionType.SingleValue)]
        public string ConfigurationName { get; set; }
        
        [Option(CommandOptionType.MultipleValue, ShortName = "e", LongName = "extra-config", ValueName = "project file=config name", Description = "In addition to the base build configuration specified by <project file> and " + ConfigurationNameLongName + ", you can dynamically add a child configuration to the base configuration with this option. This option can be used multiple times, each new configuration will be added as a child of the previously defined configuration.\nThis option allows you to share, with your colleagues, a common project file that holds the property of your application and have an extra configuration in local (just for you) which you can use to build the project in a specific local directory.\nFor each extra configuration, specify the path or the name of the project file and the configuration name to use. If the project file name if empty, the main <project file> is used.")]
        public string[] ExtraConfigurations { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "p", LongName = "property", ValueName = "key=value", Description = "A pair of key/value to dynamically set a property for this build.\nEach pair should specify the name of the property to set and the value that should be used.\nUse the option " + PropertyHelpLongName + " to see the full list of properties available as well as their documentation.")]
        public string[] BuildProperties { get; set; }
        
        [Option("-ph|" + PropertyHelpLongName, "Shows the list of each build property usable with its full documentation.", CommandOptionType.NoValue)]
        public bool ShowBuildPropertyHelp { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (ShowBuildPropertyHelp) {
                Out.WriteOnNewLine(null);
                Out.WriteOnNewLine("BUILD PROPERTIES", ConsoleColor.Cyan, 1);
            
                foreach (var property in GetAvailableBuildProperties().OrderBy(p => p)) {
                    Out.WriteOnNewLine(property, padding: 3);
                    Out.WriteOnNewLine(BuilderHelp.GetPropertyDocumentation(property), padding: 20);
                    Out.WriteOnNewLine(null);
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
                    if (!availableBuildProperties.Contains(split[0])) {
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
            
            var config = OeProject.GetBuildConfigurationCopy(configQueue);
            using (var builder = new BuilderAuto(config)) {
                builder.BuildConfiguration.Properties.SetPropertiesFromKeyValuePairs(keyValueProperties);
                builder.CancelToken = CancelToken;
                builder.Log = Log;
                builder.Build();
                //var task = Task.Factory.StartNew(builder.Build);
                //task.Wait();
            }
            return 0;
        }

        public static void GetAdditionalHelpText(IConsoleOutput console, CommandLineApplication application, int firstColumnWidth) {
            console.WriteOnNewLine(null);
            console.WriteOnNewLine("BUILD PROPERTIES", ConsoleColor.Cyan, 1);
            
            foreach (var property in GetAvailableBuildProperties().OrderBy(p => p)) {
                console.WriteOnNewLine(property, padding: 3);
            }
            console.WriteOnNewLine(null);
            console.WriteOnNewLine($"Display the full documentation of each build property by specifying the option {PropertyHelpLongName}.", padding: 3);
        }

        private static HashSet<string> GetAvailableBuildProperties() {
            var properties = new HashSet<string>();
            var nameSpace = $"{nameof(Oetools)}.{nameof(Builder)}.{nameof(Builder.Project)}.{nameof(Builder.Project.Properties)}";
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(nameof(Oetools)))
                .SelectMany(t => t.GetTypes())
                .Where(t => t.Namespace?.StartsWith(nameSpace, StringComparison.OrdinalIgnoreCase) ?? false)) {
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
                        if (!properties.Contains(name)) {
                            properties.Add(name);
                        }
                    }
                }
            }
            return properties;
        }

    }
    
    
}