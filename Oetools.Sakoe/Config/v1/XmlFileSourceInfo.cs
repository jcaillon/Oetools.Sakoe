using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Config.v1 {
    
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
}