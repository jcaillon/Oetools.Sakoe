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
        /// Allows to exclude path from being treated by <see cref="BuildSourceTasks"/>
        /// </summary>
        [XmlArray("SourcePathFilters")]
        [XmlArrayItem("Filter", typeof(XmlOeSourceFilter))]
        public List<XmlOeSourceFilter> SourcePathFilters { get; set; }
            
        [XmlArray("TaskVariables")]
        [XmlArrayItem("Variable", typeof(XmlOeTaskVariable))]
        public List<XmlOeTaskVariable> TaskVariables { get; set; }
        
        /// <summary>
        /// This list of tasks can include any file
        /// </summary>
        [XmlArray("PreBuildTasks")]
        [XmlArrayItem("Step", typeof(XmlOeBuildStep))]
        public List<XmlOeBuildStep> PreBuildTasks { get; set; }

        /// <summary>
        /// This list of tasks can only include files located in the source directory
        /// </summary>
        [XmlArray("BuildSourceTasks")]
        [XmlArrayItem("Step", typeof(XmlOeBuildCompileStep))]
        public List<XmlOeBuildCompileStep> BuildSourceTasks { get; set; }
            
        /// <summary>
        /// This list of tasks can only include files located in the output directory
        /// </summary>
        [XmlArray("BuildOutputTasks")]
        [XmlArrayItem("Step", typeof(XmlOeBuildStep))]
        public List<XmlOeBuildStep> BuildOutputTasks { get; set; }
        
        /// <summary>
        /// This list of tasks can include any file
        /// </summary>
        [XmlArray("PostBuildTasks")]
        [XmlArrayItem("Step", typeof(XmlOeBuildStep))]
        public List<XmlOeBuildStep> PostBuildTasks { get; set; }
            
        [Serializable]
        public class XmlOeCompilationOptions {

            [XmlElement(ElementName = "CompileWithDebugList")]
            public bool CompileWithDebugList { get; set; }

            [XmlElement(ElementName = "CompileWithXref")]
            public bool CompileWithXref { get; set; }

            [XmlElement(ElementName = "CompileWithListing")]
            public bool CompileWithListing { get; set; }

            [XmlElement(ElementName = "CompileWithPreprocess")]
            public bool CompileWithPreprocess { get; set; }

            [XmlElement(ElementName = "UseCompilerMultiCompile")]
            public bool UseCompilerMultiCompile { get; set; }

            /// <summary>
            /// only since 11.7 : require-full-names, require-field-qualifiers, require-full-keywords
            /// </summary>
            [XmlElement(ElementName = "CompileOptions")]
            public bool CompileOptions { get; set; }

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
            /// If false, there will be no analyze of compiled files (ref tables/files), no storage
            /// of the build history after the build, no computation of MD5 nor comparison of date/size of files
            /// </summary>
            [XmlElement(ElementName = "EnableDifferentialBuild")]
            public bool EnableDifferentialBuild { get; set; }
                
            /// <summary>
            /// True if the tool should use a MD5 sum for each file to figure out if it has changed
            /// </summary>
            [XmlElement(ElementName = "StoreSourceMd5")]
            public bool StoreSourceMd5 { get; set; }
            
            /// <summary>
            /// If a source file has been deleted since the last build, should we try to delete it in the output directory
            /// if it still exists?
            /// </summary>
            [XmlElement(ElementName = "MirrorDeletedSourceFileToOutput")]
            public bool MirrorDeletedSourceFileToOutput { get; set; }
        }

        [Serializable]
        public class XmlOeBuildCompileStep {
                
            [XmlAttribute("Label")]
            public string Label { get; set; }
                
            [XmlArray("Tasks")]
            [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
            [XmlArrayItem("Compile", typeof(XmlOeTaskCompile))]
            [XmlArrayItem("CompileInProlib", typeof(XmlOeTaskCompileProlib))]
            [XmlArrayItem("CompileInZip", typeof(XmlOeTaskCompileZip))]
            [XmlArrayItem("CompileInCab", typeof(XmlOeTaskCompileCab))]
            [XmlArrayItem("CompileUploadFtp", typeof(XmlOeTaskCompileUploadFtp))]
            [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
            [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
            [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
            [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
            [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
            public List<XmlOeTask> Tasks { get; set; }
        }

        [Serializable]
        public class XmlOeBuildStep {
                
            [XmlAttribute("Label")]
            public string Label { get; set; }
                
            [XmlArray("Tasks")]
            [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
            [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
            [XmlArrayItem("Move", typeof(XmlOeTaskMove))]
            [XmlArrayItem("RemoveDir", typeof(XmlOeTaskRemoveDir))]
            [XmlArrayItem("Delete", typeof(XmlOeTaskDelete))]
            [XmlArrayItem("DeleteInProlib", typeof(XmlOeTaskDeleteInProlib))]
            [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
            [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
            [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
            [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
            [XmlArrayItem("Webclient", typeof(XmlOeTaskWebclient))]
            public List<XmlOeTask> Tasks { get; set; }
        }
            
    }
}