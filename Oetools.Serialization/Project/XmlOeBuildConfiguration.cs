using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    
    [Serializable]
    [XmlRoot("BuildConfiguration")]
    public class XmlOeBuildConfiguration {
            
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public const string SchemaLocation = "https://raw.githubusercontent.com/jcaillon/Oetools.Sakoe/master/Oetools.Serialization/Resources/Xsd/BuildConfiguration.xsd";
            
        [XmlAttribute("Name")]
        public string ConfigurationName { get; set; }
            
        [XmlElement(ElementName = "OutputDirectory")]
        public string OutputDirectory { get; set; } = Path.Combine("<SOURCE_DIRECTORY>", "bin");

        [XmlElement(ElementName = "ReportFilePath")]
        public string ReportFilePath { get; set; } = Path.Combine("<SOURCE_DIRECTORY>", ".oe", "build", "latest.html");
            
        [XmlElement(ElementName = "BuildHistoryOutputFilePath")]
        public string BuildHistoryOutputFilePath { get; set; } = Path.Combine("<SOURCE_DIRECTORY>", ".oe", "build", "latest.xml");
            
        [XmlElement(ElementName = "BuildHistoryInputFilePath")]
        public string BuildHistoryInputFilePath { get; set; } = Path.Combine("<SOURCE_DIRECTORY>", ".oe", "build", "latest.xml");
                                    
        [XmlElement("CompilationOptions")]
        public XmlOeCompilationOptions CompilationOptions { get; set; }
            
        [XmlElement("BuildOptions")]
        public XmlOeBuildOptions BuildOptions { get; set; }
            
        /// <summary>
        /// Allows to exclude path from being treated by either <see cref="SourceCompilationTasks"/> or <see cref="SourceFilesTasks"/>
        /// </summary>
        [XmlArray("SourcePathFilters")]
        [XmlArrayItem("Filter", typeof(XmlOeSourceFilter))]
        public List<XmlOeSourceFilter> SourcePathFilters { get; set; }
            
        [XmlArray("TaskVariables")]
        [XmlArrayItem("Variable", typeof(XmlOeTaskVariable))]
        public List<XmlOeTaskVariable> TaskVariables { get; set; }
            
        [XmlArray("SourceCompilationTasks")]
        [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
        [XmlArrayItem("Compile", typeof(XmlOeTaskCopy))]
        [XmlArrayItem("CompileInProlib", typeof(XmlOeTaskProlib))]
        [XmlArrayItem("CompileInZip", typeof(XmlOeTaskZip))]
        [XmlArrayItem("CompileInCab", typeof(XmlOeTaskCab))]
        [XmlArrayItem("CompileUploadFtp", typeof(XmlOeTaskFtp))]
        public List<XmlOeTask> SourceCompilationTasks { get; set; }
            
        [XmlArray("SourceFilesTasks")]
        [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
        [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
        [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
        [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
        [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
        [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
        public List<XmlOeTask> SourceFilesTasks { get; set; }
            
        [XmlArray("OutputDeploymentTasks")]
        [XmlArrayItem("Step", typeof(XmlOeDeploymentStep))]
        public List<XmlOeDeploymentStep> OutputDeploymentTasks { get; set; }
            
        [Serializable]
        public class XmlOeCompilationOptions {

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
        public class XmlOeBuildOptions {
                
            [XmlElement(ElementName = "ArchivesCompressionLevel")]
            public XmlOeCompressionLevel ArchivesCompressionLevel { get; set; }
                
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
            [XmlArrayItem("Webclient", typeof(XmlOeTaskWebclient))]
            public List<XmlOeTask> OutputDeploymentTasks { get; set; }
        }
            
    }
}