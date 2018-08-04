using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    [Serializable]
    public class XmlOeFileBuiltCompiled : XmlOeFileBuilt {
            
        /// <summary>
        /// Represents the source file (i.e. includes) used to generate a given .r code file
        /// </summary>
        [XmlArray("RequiredFiles")]
        [XmlArrayItem("RequiredFile", typeof(XmlOeFile))]
        public List<XmlOeFile> RequiredFiles { get; set; }

        /// <summary>
        ///     represent the tables that were referenced in a given .r code file
        /// </summary>
        [XmlArray("RequiredTables")]
        [XmlArrayItem("RequiredTable", typeof(XmlTableCrc))]
        public List<XmlTableCrc> RequiredTables { get; set; }
    }
}