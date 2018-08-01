#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (OeProject.cs) is part of Oetools.Sakoe.
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
using System.IO;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization {
    
    [Serializable]
    [XmlRoot("Project")]
    public class XmlOeProject {
        
        #region static

        internal static XmlOeProject Load(string path) {
            XmlOeProject interfaceXml;
            var serializer = new XmlSerializer(typeof(XmlOeProject));
            using (var reader = new StreamReader(path)) {
                interfaceXml = (XmlOeProject) serializer.Deserialize(reader);
            }

            return interfaceXml;
        }

        internal static void Save(XmlOeProject xml, string path) {
            var serializer = new XmlSerializer(typeof(XmlOeProject));
            using (TextWriter writer = new StreamWriter(path, false)) {
                serializer.Serialize(writer, xml);
            }
        }

        #endregion
        
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation = "https://raw.githubusercontent.com/jcaillon/Oetools.Sakoe/master/docs/XmlOeProject.xsd";

        [XmlElement("Properties")]
        public XmlOeProjectConfiguration Properties { get; set; }
        
        
        [XmlElement("WebclientProperties")]
        public XmlOeProjectWebclientConfiguration WebclientProperties { get; set; }

        
        [XmlArray("BuildConfigurations")]
        [XmlArrayItem("Build", typeof(XmlOeBuildConfiguration))]
        public List<XmlOeBuildConfiguration> BuildConfigurations { get; set; }
        
        [Serializable]
        public class XmlOeProjectConfiguration {
            
            [XmlArray("DatabaseDataDefinitionFiles")]
            [XmlArrayItem("DfPath", typeof(string))]
            public List<string> DatabaseDataDefinitionFiles { get; set; }
            
            [XmlElement(ElementName = "DatabaseConnectionExtraParameters")]
            public string DatabaseConnectionExtraParameters { get; set; }

            [XmlArray("DatabaseAliases")]
            [XmlArrayItem("Alias", typeof(XmlOeDatabaseAlias))]
            public List<XmlOeDatabaseAlias> DatabaseAliases { get; set; }

            public class XmlOeDatabaseAlias {
                
                [XmlAttribute(AttributeName = "ProgresCommandLineExtraParameters")]
                public string AliasLogicalName { get; set; }

                [XmlAttribute(AttributeName = "PreProgresExecutionProgramPath")]
                public string DatabaseLogicalName { get; set; }
            }
            
            
            [XmlElement(ElementName = "IniFilePath")]
            public string IniFilePath { get; set; }

            [XmlElement(ElementName = "AddAllSourceDirectoriesToPropath")]
            public bool AddAllSourceDirectoriesToPropath { get; set; }

            [XmlArrayItem("Path", typeof(string))]
            [XmlArray("PropathEntries")]
            public List<string> PropathEntries { get; set; }
            
            [XmlArray("PropathFilter")]
            [XmlArrayItem("Regex", typeof(string))]
            public List<string> PropathFilter { get; set; }


            [XmlElement(ElementName = "DlcDirectoryPath")]
            public string DlcDirectoryPath { get; set; }

            [XmlElement(ElementName = "UseCharacterModeExecutable")]
            public bool UseCharacterModeExecutable { get; set; }

            [XmlElement(ElementName = "NeverUseBatchModeOption")]
            public bool NeverUseBatchModeOption { get; set; }

            [XmlElement(ElementName = "ProgresCommandLineExtraParameters")]
            public string ProgresCommandLineExtraParameters { get; set; }

            [XmlElement(ElementName = "ProcedurePathToExecuteBeforeAnyProgressExecution")]
            public string ProcedurePathToExecuteBeforeAnyProgressExecution { get; set; }

            [XmlElement(ElementName = "ProcedurePathToExecuteAfterAnyProgressExecution")]
            public string ProcedurePathToExecuteAfterAnyProgressExecution { get; set; }

            [XmlElement(ElementName = "TemporaryDirectory")]
            public string TemporaryDirectory { get; set; }
        }

        [Serializable]
        public class XmlOeProjectWebclientConfiguration {
            
            [XmlElement(ElementName = "VendorName")]
            public string VendorName { get; set; }

            [XmlElement(ElementName = "ApplicationName")]
            public string ApplicationName { get; set; }

            [XmlElement(ElementName = "StartupParameters")]
            public string StartupParameters { get; set; }

            /// <summary>
            /// Will be used for both Prowcapp and Codebase by default, provide a custom prowcapp template to change this behavior
            /// </summary>
            
            [XmlElement(ElementName = "LocatorUrl")]
            public string LocatorUrl { get; set; }

            /// <summary>
            /// Valid oe version for this application
            /// </summary>

            [XmlElement(ElementName = "WebClientVersion")]
            public string WebClientVersion { get; set; } = "11.7";

            /// <summary>
            /// The Directory path from which to create the webclient files (can be relative to the build output directory, the default value is ".")
            /// </summary>

            [XmlElement(ElementName = "WebclientRootDirectoryPath")]
            public string WebclientRootDirectoryPath { get; set; } = ".";

            /// <summary>
            /// The output directory in which the .prowcapp and .cab + diffs/.cab files will be created (can be relative to the build output directory)
            /// </summary>

            [XmlElement(ElementName = "WebclientOutputDirectory")]
            public string WebclientOutputDirectory { get; set; } = "webclient";

            /// <summary>
            /// Path to the model of the .prowcapp to use (can be left empty and the internal model will be used)
            /// </summary>
            
            [XmlElement(ElementName = "ProwcappTemplateFilePath")]
            public string ProwcappTemplateFilePath { get; set; }
            
        }

        [Serializable]
        public class XmlOeBuildConfiguration {
            
            [XmlAttribute("Name")]
            public string Name { get; set; }
            
            [XmlElement(ElementName = "OutputDirectory")]
            public string OutputDirectory { get; set; }

            
            [XmlArray("TaskVariables")]
            [XmlArrayItem("Variable", typeof(XmlOeTaskVariables))]
            public List<XmlOeTaskVariables> TaskVariables { get; set; }

            
            [XmlArray("SourceCompilationTasks")]
            [XmlArrayItem("Compile", typeof(XmlOeTaskCompile))]
            [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
            [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
            [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
            [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
            [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
            public List<XmlOeTask> SourceCompilationTasks { get; set; }
            
            
            [XmlArray("SourceFilesTasks")]
            [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
            [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
            [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
            [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
            [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
            [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
            public List<XmlOeTask> SourceFilesTasks { get; set; }
            
            
            [XmlArray("OutputDeploymentTasks")]
            [XmlArrayItem("Step", typeof(XmlOeDeploymentStep))]
            public List<XmlOeDeploymentStep> OutputDeploymentTasks { get; set; }
            
            
            [XmlElement(ElementName = "BuildTasksFilePath")]
            public string BuildTasksFilePath { get; set; }
            
            
            [XmlElement("CompilationOptions")]
            public XmlOeCompilation CompilationOptions { get; set; }

            
            [XmlElement("XmlOeBuildOptions")]
            public XmlOeBuildOption XmlOeBuildOptions { get; set; }
            
            [Serializable]
            public class XmlOeCompilation {

                [XmlElement(ElementName = "CompileWithDebugList")]
                public bool CompileWithDebugList { get; set; }

                [XmlElement(ElementName = "CompileWithXref")]
                public bool CompileWithXref { get; set; }

                [XmlElement(ElementName = "CompileWithListing")]
                public bool CompileWithListing { get; set; }

                [XmlElement(ElementName = "CompileUseXmlXref")]
                public bool CompileUseXmlXref { get; set; }

                /// <summary>
                /// Force the usage of a temporary Directory to compile the .r code files
                /// </summary>
                [XmlElement(ElementName = "CompileForceUsageOfTemporaryDirectory")]
                public bool CompileForceUsageOfTemporaryDirectory { get; set; }

                [XmlElement(ElementName = "CompilableFilePattern")]
                public string CompilableFilePattern { get; set; }
                
                [XmlElement(ElementName = "CompileForceSingleProcess")]
                public bool CompileForceSingleProcess { get; set; }

                [XmlElement(ElementName = "CompileNumberProcessPerCore")]
                public byte CompileNumberProcessPerCore { get; set; }
            }
            
            [Serializable]
            public class XmlOeBuildOption {
                
                [XmlElement(ElementName = "ArchivesCompressionLevel")]
                public XmlOeCompressionLevel ArchivesCompressionLevel { get; set; }
                
                [XmlElement(ElementName = "BuildHookFilePath")]
                public string BuildHookFilePath { get; set; }
                
                /// <summary>
                /// True if the tool should use a MD5 sum for each file to figure out if it has changed
                /// </summary>
                [XmlElement(ElementName = "StoreSourceMd5")]
                public bool StoreSourceMd5 { get; set; }
                
                /// <summary>
                /// Create the package in the temp directory then copy it to the remote location (target dir) at the end
                /// </summary>
                [XmlElement(ElementName = "CreatePackageInTempDir")]
                public bool OutputBuildInTempDirectoryThenCopyAtTheEnd { get; set; }

            }    

            [Serializable]
            public enum XmlOeCompressionLevel {
                [XmlEnum("None")] None,
                [XmlEnum("Max")] Max
            }

            [Serializable]
            public class XmlOeDeploymentStep {
                
                [XmlAttribute("Label")]
                public string Label { get; set; }
                
                [XmlArray("OutputDeploymentTasks")]
                [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
                [XmlArrayItem("Move", typeof(XmlOeTaskMove))]
                [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
                [XmlArrayItem("RemoveDir", typeof(XmlOeTaskRemoveDir))]
                [XmlArrayItem("Delete", typeof(XmlOeTaskDelete))]
                [XmlArrayItem("DeleteInProlib", typeof(XmlOeTaskDeleteInProlib))]
                [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
                [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
                [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
                [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
                public List<XmlOeTask> OutputDeploymentTasks { get; set; }
            }
        }
        
        [Serializable]
        public class XmlOeTaskVariables {
            
            [XmlAttribute("Name")]
            public string Name { get; set; }
            
            [XmlText]
            public string Value { get; set; }
        }
        
        public abstract class XmlOeTask {
        }
        
        public abstract class XmlOeTaskIncludeExclude : XmlOeTask {
            
            [XmlAttribute("Include")]
            public string Include { get; set; }
            
            [XmlAttribute("Exclude")]
            public string Exclude { get; set; }
        }
        
        public abstract class XmlOeTaskIncludeExcludeTarget : XmlOeTaskIncludeExclude {
            
            [XmlAttribute("Target")]
            public string Target { get; set; }
        }

        public class XmlOeTaskCompile : XmlOeTaskIncludeExcludeTarget {
        }

        public class XmlOeTaskCopy : XmlOeTaskIncludeExcludeTarget {
        }

        public class XmlOeTaskMove : XmlOeTaskIncludeExcludeTarget {
        }
        
        public class XmlOeTaskExec : XmlOeTask {
            
            [XmlAttribute("ExecuablePath")]
            public string ExecuablePath { get; set; }
            
            /// <summary>
            /// (you can use task variables in this string)
            /// </summary>
            [XmlAttribute("Parameters")]
            public string Parameters { get; set; }
            
            [XmlAttribute("HiddenExecution")]
            public bool HiddenExecution { get; set; }
            
            /// <summary>
            /// With this option, the task will not fail if the exit code is different of 0
            /// </summary>
            [XmlAttribute("IgnoreExitCode")]
            public bool IgnoreExitCode { get; set; }
            
            /// <summary>
            /// (default to output directory)
            /// </summary>
            [XmlAttribute("WorkingDirectory")]
            public string WorkingDirectory { get; set; }
        }
                
        public class XmlOeTaskDelete : XmlOeTaskIncludeExclude {
        }
        
        public class XmlOeTaskRemoveDir : XmlOeTaskIncludeExclude {
        }

        public class XmlOeTaskDeleteInProlib : XmlOeTaskIncludeExclude {
            
            /// <summary>
            /// The relative file path pattern to delete inside the matched prolib file
            /// </summary>
            [XmlAttribute("RelativeFilePatternToDelete")]
            public string RelativeFilePatternToDelete { get; set; }
        }
        
        public class XmlOeTaskProlib : XmlOeTaskIncludeExclude {
            
            [XmlAttribute("ProlibTarget")]
            public string ProlibTarget { get; set; }
        }
        
        public class XmlOeTaskZip : XmlOeTaskIncludeExclude {
            
            [XmlAttribute("ZipTarget")]
            public string ZipTarget { get; set; }
        }
        
        public class XmlOeTaskCab : XmlOeTaskIncludeExclude {
            
            [XmlAttribute("ZipTarget")]
            public string CabTarget { get; set; }
        }
        
        public class XmlOeTaskFtp : XmlOeTaskIncludeExclude {
            
            [XmlAttribute("ZipTarget")]
            public string FtpTarget { get; set; }
        }
        
    }

}