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

namespace Oetools.Sakoe.Config.v2 {
    
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

        [XmlElement("Properties")]
        public XmlOeProjectConfiguration Properties { get; set; }

        [XmlElement("WebclientProperties")]
        public XmlOeProjectWebclientConfiguration WebclientProperties { get; set; }

        [XmlArrayItem("BuildConfiguration", typeof(XmlOeBuildConfiguration))]
        [XmlArray("BuildConfigurations")]
        public List<XmlOeBuildConfiguration> BuildConfigurations { get; set; }
        
        [Serializable]
        public class XmlOeProjectConfiguration {
            
            [XmlElement(ElementName = "RequiredDatabaseDfPathList")]
            public string RequiredDatabaseDfPathList { get; set; }

            [XmlArrayItem("Alias", typeof(string))]
            [XmlArray("RequiredDatabaseAliasesList")]
            public List<string> RequiredDatabaseAliasesList { get; set; }

            [XmlElement(ElementName = "RequiredIniPath")]
            public string RequiredIniPath { get; set; }

            [XmlElement(ElementName = "UseSourceDirectoriesAsPropath")]
            public bool UseSourceDirectoriesAsPropath { get; set; }

            [XmlArrayItem("RegexFilter", typeof(string))]
            [XmlArray("AutoPropathDirectoryFilter")]
            public List<string> AutoPropathDirectoryFilter { get; set; }

            [XmlArrayItem("PropathEntry", typeof(string))]
            [XmlArray("ExtraPropathEntries")]
            public List<string> ExtraPropathEntries { get; set; }

            [XmlElement(ElementName = "DlcPath")]
            public string DlcPath { get; set; }

            [XmlElement(ElementName = "UseCharacterModeOfProgress")]
            public bool UseCharacterModeOfProgress { get; set; }

            [XmlElement(ElementName = "NeverUseProgresBatchMode")]
            public bool NeverUseProgresBatchMode { get; set; }

            [XmlElement(ElementName = "ProgresCommandLineExtraParameters")]
            public string ProgresCommandLineExtraParameters { get; set; }

            [XmlElement(ElementName = "PreProgresExecutionProgramPath")]
            public string PreProgresExecutionProgramPath { get; set; }

            [XmlElement(ElementName = "PostProgresExecutionProgramPath")]
            public string PostProgresExecutionProgramPath { get; set; }

            [XmlElement(ElementName = "TemporaryDirectory")]
            public string TemporaryDirectory { get; set; }
        }

        [Serializable]
        public class XmlOeProjectWebclientConfiguration {
            
            [XmlElement(ElementName = "WebclientApplicationName")]
            public string WebclientApplicationName { get; set; }

            [XmlElement(ElementName = "WebclientVendorName")]
            public string WebclientVendorName { get; set; }

            [XmlElement(ElementName = "WebclientStartupParam")]
            public string WebclientStartupParam { get; set; }

            [XmlElement(ElementName = "WebclientLocatorUrl")]
            public string WebclientLocatorUrl { get; set; }

            [XmlElement(ElementName = "WebclientVersion")]
            public string WebclientVersion { get; set; }

            /// <summary>
            /// The folder name of the networking client directory
            /// </summary>
            [XmlElement(ElementName = "NetworkingDirectoryNameInPackage")]
            public string NetworkingDirectoryNameInPackage { get; set; }

            /// <summary>
            /// The folder name of the webclient directory (if left empty, the tool will not generate the webclient dir!)
            /// </summary>
            [XmlElement(ElementName = "WebclientDirectoryNameToCreate")]
            public string WebclientDirectoryNameToCreate { get; set; }

            /// <summary>
            /// Path to the model of the .prowcapp to use (can be left empty and the internal model will be used)
            /// </summary>
            [XmlElement(ElementName = "CustomProwcappModelPath")]
            public string CustomProwcappModelPath { get; set; }
        }


        [Serializable]
        [XmlType(TypeName = "Build")]
        [XmlInclude(typeof(XmlOeDifferentialBuildConfiguration))]
        [XmlInclude(typeof(XmlOePackageBuildConfiguration))]
        public class XmlOeBuildConfiguration {
            
            [XmlAttribute("Name")]
            public string Name { get; set; }

            [XmlElement("Compilation")]
            public XmlOeCompilation Compilation { get; set; }

            [XmlElement("ParallelCompilation")]
            public XmlOeParallelCompilation ParallelCompilation { get; set; }
            
            [Serializable]
            public class XmlOeCompilation {
                
                [XmlElement(ElementName = "SourceDirectory")]
                public string SourceDirectory { get; set; }

                [XmlElement(ElementName = "TargetDirectory")]
                public string TargetDirectory { get; set; }

                [XmlElement(ElementName = "DeploymentRulesFilePath")]
                public string DeploymentRulesFilePath { get; set; }

                [XmlElement(ElementName = "CompileNextToSource")]
                public bool CompileNextToSource { get; set; }

                [XmlElement(ElementName = "CompileWithDebugList")]
                public bool CompileWithDebugList { get; set; }

                [XmlElement(ElementName = "CompileWithXref")]
                public bool CompileWithXref { get; set; }

                [XmlElement(ElementName = "CompileWithListing")]
                public bool CompileWithListing { get; set; }

                [XmlElement(ElementName = "CompileUseXmlXref")]
                public bool CompileUseXmlXref { get; set; }

                [XmlElement(ElementName = "CompileUnmatchedProgressFilesToTarget")]
                public bool CompileUnmatchedProgressFilesToTarget { get; set; }

                /// <summary>
                /// Force the usage of a temporary folder to compile the .r code files
                /// </summary>
                [XmlElement(ElementName = "CompileForceUsageOfTemporaryDirectory")]
                public bool CompileForceUsageOfTemporaryDirectory { get; set; }
                
                [XmlElement(ElementName = "ArchivesCompressionLevel")]
                public XmlOeCompressionLevel ArchivesCompressionLevel { get; set; }

            }

            [Serializable]
            public enum XmlOeCompressionLevel {
                [XmlEnum("None")] None,
                [XmlEnum("Max")] Max
            }

            [Serializable]
            public class XmlOeParallelCompilation {
                
                [XmlElement(ElementName = "CompileForceSingleProcess")]
                public bool CompileForceSingleProcess { get; set; }

                [XmlElement(ElementName = "CompileNumberProcessPerCore")]
                public int CompileNumberProcessPerCore { get; set; }
            }

        }

        [Serializable]
        [XmlType(TypeName = "DifferentialBuild")]
        [XmlInclude(typeof(XmlOePackageBuildConfiguration))]
        public class XmlOeDifferentialBuildConfiguration : XmlOeBuildConfiguration {
            
            [XmlElement("Deployment")]
            public XmlOeDeployment Deployment { get; set; }
            
            [Serializable]
            public class XmlOeDeployment {
                
                /// <summary>
                /// True if the tool should use a MD5 sum for each file to figure out if it has changed
                /// </summary>
                [XmlElement(ElementName = "StoreSourceMd5")]
                public bool StoreSourceMd5 { get; set; }

                [XmlElement(ElementName = "CompilableFilePattern")]
                public string CompilableFilePattern { get; set; }
                
                [XmlElement(ElementName = "BuildHookFilePath")]
                public string BuildHookFilePath { get; set; }

                [XmlElement(ElementName = "ExploreRecursively")]
                public bool ExploreRecursively { get; set; }

            }
        }

        [Serializable]
        [XmlType(TypeName = "PackageBuild")]
        public class XmlOePackageBuildConfiguration : XmlOeBuildConfiguration {

            [XmlElement("Packaging")]
            public XmlOePackaging Packaging { get; set; }
            
            [Serializable]
            public class XmlOePackaging {
                /// <summary>
                /// The initial deployment directory passed to this program
                /// </summary>
                [XmlElement(ElementName = "PackageTargetDirectory")]
                public string PackageTargetDirectory { get; set; }

                /// <summary>
                /// The reference directory that will be copied into the TargetDirectory before a packaging
                /// </summary>
                [XmlElement(ElementName = "ReferenceDirectory")]
                public string ReferenceDirectory { get; set; }

                /// <summary>
                /// Create the package in the temp directory then copy it to the remote location (target dir) at the end
                /// </summary>
                [XmlElement(ElementName = "CreatePackageInTempDir")]
                public bool CreatePackageInTempDir { get; set; }
            }
        }

    }
}