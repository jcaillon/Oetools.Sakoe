#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlConfiguration.cs) is part of Oetools.Builder.
// 
// Oetools.Builder is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Builder is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Builder. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Oetools.Builder.Opp {
    
    [Serializable]
    [XmlRoot("Config")]
    public class XmlConfiguration {

        #region static

        internal static XmlConfiguration Load(string path) {
            XmlConfiguration interfaceXml;
            var serializer = new XmlSerializer(typeof(XmlConfiguration));
            using (var reader = new StreamReader(path)) {
                interfaceXml = (XmlConfiguration) serializer.Deserialize(reader);
            }
            return interfaceXml;
        }

        internal static void Save(XmlConfiguration interfaceXml, string path) {
            var serializer = new XmlSerializer(typeof(XmlConfiguration));
            using (TextWriter writer = new StreamWriter(path, false)) {
                serializer.Serialize(writer, interfaceXml);
            }
        }

        #endregion
        
        /// <summary>
        /// List all the files that were deployed from the source directory
        /// </summary>
        public List<XmlFileDeployed> DeployedFiles { get; set; }
    }
}