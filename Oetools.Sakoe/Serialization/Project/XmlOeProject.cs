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

namespace Oetools.Sakoe.Serialization.Project {
    
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
        public const string SchemaLocation = "https://raw.githubusercontent.com/jcaillon/Oetools.Sakoe/master/docs/Project.xsd";

        [XmlElement("Properties")]
        public XmlOeProjectProperties Properties { get; set; }
        
        [XmlArray("BuildConfigurations")]
        [XmlArrayItem("Build", typeof(XmlOeBuildConfiguration))]
        public List<XmlOeBuildConfiguration> BuildConfigurations { get; set; }
                
        [XmlElement("AutomatedTasks")]
        public XmlOeAutomatedTasks AutomatedTasks { get; set; }
    }
}