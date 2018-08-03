using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    [Serializable]
    public class XmlOePackage {

        /// <summary>
        /// Prowcapp version, automatically computed by this tool
        /// </summary>
        [XmlElement(ElementName = "WebclientProwcappVersion")]
        public int WebclientProwcappVersion { get; set; }
    }
}