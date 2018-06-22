#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlOeHistory.cs) is part of Oetools.Runner.Cli.
// 
// Oetools.Runner.Cli is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Runner.Cli is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Runner.Cli. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Oetools.Packager.Core;

namespace Oetools.Runner.Cli.Config.v2 {
    
    [Serializable]
    [XmlRoot("DeploymentHistory")]
    public class XmlOeDeploymentHistory {

        public List<FileDeployed> LastDeployedFiles { get; set; }

        [Serializable]
        public class XmlOePackage {

            public string WcPackageName { get; set; }

            /// <summary>
            ///     Prowcapp version, automatically computed by this tool
            /// </summary>
            public int WcProwcappVersion { get; set; }
        }
        
        [Serializable]
        [XmlInclude(typeof(XmlFileDeployed))]
        [XmlInclude(typeof(XmlFileDeployedCompiled))]
        public class XmlFileSourceInfo {
            /// <summary>
            ///     The relative path of the source file
            /// </summary>
            public string SourcePath { get; set; }

            public DateTime LastWriteTime { get; set; }

            public long Size { get; set; }

            /// <summary>
            ///     MD5
            /// </summary>
            public string Md5 { get; set; }
        }
        
        [Serializable]
        public class XmlFileDeployedCompiled : XmlFileDeployed {
            /// <summary>
            ///     represents the source file (i.e. includes) used to generate a given .r code file
            /// </summary>
            public List<XmlFileSourceInfo> RequiredFiles { get; set; }

            /// <summary>
            ///     represent the tables that were referenced in a given .r code file
            /// </summary>
            public List<XmlTableCrc> RequiredTables { get; set; }
        }
        
        [Serializable]
        [XmlInclude(typeof(XmlFileDeployedCompiled))]
        public class XmlFileDeployed : XmlFileSourceInfo {
            /// <summary>
            ///     a list of the targets for this deployment
            /// </summary>
            public List<XmlDeploymentTarget> Targets { get; set; }

            /// <summary>
            ///     The action done for this file
            /// </summary>
            public XmlDeploymentAction Action { get; set; }
        }
        
        [Serializable]
        public enum XmlDeploymentAction {
            [XmlEnum("Added")]
            Added,
            [XmlEnum("Replaced")]
            Replaced,
            [XmlEnum("Deleted")]
            Deleted,
            [XmlEnum("Existing")]
            Existing
        }
        
        [Serializable]
        public class XmlDeploymentTarget {
            /// <summary>
            ///     Relative target path (relative to the target directory)
            /// </summary>
            public string TargetPath { get; set; }

            /// <summary>
            ///     The type of deployment done for this target
            /// </summary>
            public XmlDeployType DeployType { get; set; }

            /// <summary>
            ///     Relative path of the pack in which this file is deployed (if any)
            /// </summary>
            public string TargetPackPath { get; set; }

            /// <summary>
            ///     Relative path within the pack (if any)
            /// </summary>
            public string TargetPathInPack { get; set; }
        }
        
        /// <summary>
        ///     This class represent the tables that were referenced in a given .r code file
        /// </summary>
        [Serializable]
        public class XmlTableCrc {
            public string QualifiedTableName { get; set; }
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
        
        /// <summary>
        ///     Errors found for this file, either from compilation or from prolint
        /// </summary>
        [Serializable]
        public class XmlFileError {

            /// <summary>
            ///     The path to the file that was compiled to generate this error (you can compile a .p and have the error on a .i)
            /// </summary>
            public string CompiledFilePath { get; set; }

            /// <summary>
            ///     Path of the file in which we found the error
            /// </summary>
            public string SourcePath { get; set; }
        
            public XmlErrorLevel Level { get; set; }
            public int Line { get; set; }
            public int Column { get; set; }
            public int ErrorNumber { get; set; }
            public string Message { get; set; }
            public string Help { get; set; }
            public bool FromProlint { get; set; }

            /// <summary>
            ///     indicates if the error appears several times
            /// </summary>
            public int Times { get; set; }
        }
        
        /// <summary>
        ///     Describes the error level, the num is also used for MARKERS in scintilla
        ///     and thus must start at 0
        /// </summary>
        [Serializable]
        public enum XmlErrorLevel {
            [XmlEnum("0")]
            NoErrors = 0,
            [XmlEnum("1")] 
            Information,
            [XmlEnum("2")]
            Warning,
            [XmlEnum("3")]
            StrongWarning,
            [XmlEnum("4")]
            Error,
            [XmlEnum("5")]
            Critical
        }

    }
}