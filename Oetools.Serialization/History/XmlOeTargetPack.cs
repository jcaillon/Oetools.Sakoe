using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    public abstract class XmlOeTargetPack : XmlOeTarget {
        /// <summary>
        /// Relative path of the pack in which this file is deployed (if any)
        /// </summary>
        [XmlAttribute(AttributeName = "TargetPack")]
        public string TargetPack { get; set; }
    }
}