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
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    
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
        [XmlArrayItem("BuiltFile", typeof(XmlOeFileBuilt))]
        [XmlArrayItem("BuiltFileCompiled", typeof(XmlOeFileBuiltCompiled))]
        public List<XmlOeFileBuilt> BuiltFiles { get; set; }
        
        [XmlArray("CompilationProblems")]
        [XmlArrayItem("Error", typeof(XmlOeOeCompilationError))]
        [XmlArrayItem("Warning", typeof(XmlOeCompilationWarning))]
        public List<XmlOeCompilationProblem> CompilationProblems { get; set; }
    }
}