using System;

namespace Oetools.Runner.Cli.Config.v1 {
    
    /// <summary>
    ///     This class represent the tables that were referenced in a given .r code file
    /// </summary>
    [Serializable]
    public class XmlTableCrc {
        public string QualifiedTableName { get; set; }
        public string Crc { get; set; }
    }
}