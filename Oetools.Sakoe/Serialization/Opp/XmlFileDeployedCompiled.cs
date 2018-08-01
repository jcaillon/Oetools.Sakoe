using System;
using System.Collections.Generic;

namespace Oetools.Sakoe.Serialization.Opp {
    
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
}