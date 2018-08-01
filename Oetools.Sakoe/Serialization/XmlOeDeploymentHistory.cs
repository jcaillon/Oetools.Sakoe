#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlOeHistory.cs) is part of Oetools.Sakoe.
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
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization {
    
    [Serializable]
    [XmlRoot("DeploymentHistory")]
    public class XmlOeDeploymentHistory {     
        
        #region static

        internal static XmlOeDeploymentHistory Load(string path) {
            XmlOeDeploymentHistory xml;
            var serializer = new XmlSerializer(typeof(XmlOeDeploymentHistory));
            using (var reader = new StreamReader(path)) {
                xml = (XmlOeDeploymentHistory) serializer.Deserialize(reader);
            }
            return xml;
        }

        internal static void Save(XmlOeDeploymentHistory xml, string path) {
            var serializer = new XmlSerializer(typeof(XmlOeDeploymentHistory));
            using (TextWriter writer = new StreamWriter(path, false)) {
                serializer.Serialize(writer, xml);
            }
        }

        #endregion

        [XmlElement(ElementName = "PackageInfo")]
        public XmlOePackage PackageInfo { get; set; }

        /// <summary>
        /// List of all the files deployed from the source directory
        /// </summary>
        [XmlArray("BuiltFiles")]
        [XmlArrayItem("BuiltFile", typeof(XmlOeBuiltFile))]
        [XmlArrayItem("BuiltFileCompiled", typeof(XmlOeBuiltFileCompiled))]
        public List<XmlOeBuiltFile> BuiltFiles { get; set; }
        
        [XmlArray("CompilationProblems")]
        [XmlArrayItem("Error", typeof(XmlOeCompilationError))]
        [XmlArrayItem("Warning", typeof(XmlCompilationWarning))]
        public List<XmlCompilationProblem> CompilationProblems { get; set; }
        

        [Serializable]
        public class XmlOePackage {

            /// <summary>
            /// Prowcapp version, automatically computed by this tool
            /// </summary>
            [XmlElement(ElementName = "WebclientProwcappVersion")]
            public int WebclientProwcappVersion { get; set; }
        }
        
        [Serializable]
        public class XmlOeFileInfo {
            /// <summary>
            /// The relative path of the source file
            /// </summary>
            [XmlAttribute(AttributeName = "SourcePath")]
            public string SourcePath { get; set; }

            [XmlAttribute(AttributeName = "LastWriteTime")]
            public DateTime LastWriteTime { get; set; }

            [XmlAttribute(AttributeName = "Size")]
            public long Size { get; set; }

            /// <summary>
            ///     MD5
            /// </summary>
            [XmlAttribute(AttributeName = "Md5")]
            public string Md5 { get; set; }
        }
        
        [Serializable]
        public class XmlOeBuiltFile : XmlOeFileInfo {
            /// <summary>
            /// Represents the state of the file for this build compare to the previous one
            /// </summary>
            [XmlElement(ElementName = "State")]
            public XmlOeFileState State { get; set; }

            /// <summary>
            /// A list of the targets for this file
            /// </summary>
            [XmlArray("Targets")]
            [XmlArrayItem("Compiled", typeof(XmlOeTargetCompile))]
            [XmlArrayItem("Copied", typeof(XmlOeTargetCopy))]
            [XmlArrayItem("Prolibed", typeof(XmlOeTargetProlib))]
            [XmlArrayItem("Zipped", typeof(XmlOeTargetZip))]
            [XmlArrayItem("Cabbed", typeof(XmlOeTargetCab))]
            public List<XmlOeTarget> Targets { get; set; }
        }
        
        [Serializable]
        public class XmlOeBuiltFileCompiled : XmlOeBuiltFile {
            
            /// <summary>
            /// Represents the source file (i.e. includes) used to generate a given .r code file
            /// </summary>
            [XmlArray("RequiredFiles")]
            [XmlArrayItem("RequiredFile", typeof(XmlOeFileInfo))]
            public List<XmlOeFileInfo> RequiredFiles { get; set; }

            /// <summary>
            ///     represent the tables that were referenced in a given .r code file
            /// </summary>
            [XmlArray("RequiredTables")]
            [XmlArrayItem("RequiredTable", typeof(XmlTableCrc))]
            public List<XmlTableCrc> RequiredTables { get; set; }
        }
        
        [Serializable]
        public enum XmlOeFileState {
            [XmlEnum("Added")]
            Added,
            [XmlEnum("Replaced")]
            Replaced,
            [XmlEnum("Deleted")]
            Deleted,
            [XmlEnum("Existing")]
            Existing
        }
        
        public abstract class XmlOeTarget {
            /// <summary>
            /// Relative target path (relative to the target directory)
            /// </summary>
            [XmlAttribute(AttributeName = "TargetPath")]
            public string TargetPath { get; set; }
        }

        [Serializable]
        public class XmlOeTargetCompile : XmlOeTarget {
        }

        [Serializable]
        public class XmlOeTargetCopy : XmlOeTarget {
        }
        
        [Serializable]
        public class XmlOeTargetProlib : XmlOeTarget {
            /// <summary>
            /// Relative path of the pack in which this file is deployed (if any)
            /// </summary>
            [XmlAttribute(AttributeName = "TargetProlibPath")]
            public string TargetProlibPath { get; set; }
        }

        [Serializable]
        public class XmlOeTargetZip : XmlOeTarget {
            /// <summary>
            /// Relative path of the pack in which this file is deployed (if any)
            /// </summary>
            [XmlAttribute(AttributeName = "TargetZipPath")]
            public string TargetZipPath { get; set; }
        }

        [Serializable]
        public class XmlOeTargetCab : XmlOeTarget {
            /// <summary>
            /// Relative path of the pack in which this file is deployed (if any)
            /// </summary>
            [XmlAttribute(AttributeName = "TargetCabPath")]
            public string TargetCabPath { get; set; }
        }


        /// <summary>
        ///     This class represent the tables that were referenced in a given .r code file
        /// </summary>
        [Serializable]
        public class XmlTableCrc {
            [XmlAttribute(AttributeName = "QualifiedTableName")]
            public string QualifiedTableName { get; set; }
            
            [XmlAttribute(AttributeName = "Crc")]
            public string Crc { get; set; }
        }
        
        /// <summary>
        ///     Types of deploy, used during rules sorting
        /// </summary>
        [Serializable]
        public enum XmlDeployType : byte {
            [XmlEnum("None")]
            None = 0,
            [XmlEnum("Delete")]
            Delete = 1,
            [XmlEnum("DeleteFolder")]
            DeleteFolder = 2,
            [XmlEnum("DeleteInProlib")]
            DeleteInProlib = 10,
            [XmlEnum("Prolib")]
            Prolib = 11,
            [XmlEnum("Zip")]
            Zip = 12,
            [XmlEnum("Cab")]
            Cab = 13,
            [XmlEnum("Ftp")]
            Ftp = 14,
            [XmlEnum("CopyFolder")]
            CopyFolder = 21,
            [XmlEnum("Copy")]
            Copy = 30,
            [XmlEnum("Move")]
            Move = 31
        }

        public abstract class XmlCompilationProblem {
            
            /// <summary>
            /// Path of the file in which we found the error
            /// </summary>
            [XmlAttribute(AttributeName ="SourcePath")]
            public string SourcePath { get; set; }
            
            /// <summary>
            /// The path to the file that was compiled to generate this error (you can compile a .p and have the error on a .i)
            /// </summary>
            [XmlAttribute(AttributeName = "CompiledFilePath")]
            public string CompiledFilePath { get; set; }
            
            [XmlAttribute(AttributeName ="Line")]
            public int Line { get; set; }
            
            [XmlAttribute(AttributeName ="Column")]
            public int Column { get; set; }
                        
            /// <summary>
            /// indicates if the error appears several times
            /// </summary>
            [XmlAttribute(AttributeName ="Times")]
            public int Times { get; set; }
            
            [XmlAttribute(AttributeName ="ErrorNumber")]
            public int ErrorNumber { get; set; }
            
            [XmlAttribute(AttributeName ="Message")]
            public string Message { get; set; }
        }

        [Serializable]
        public class XmlOeCompilationError : XmlCompilationProblem {
        }

        [Serializable]
        public class XmlCompilationWarning : XmlCompilationProblem {
        }

    }
}