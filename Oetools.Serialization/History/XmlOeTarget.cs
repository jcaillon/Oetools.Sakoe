using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    public abstract class XmlOeTarget {
        /// <summary>
        /// Relative target path (relative to the target directory)
        /// </summary>
        [XmlAttribute(AttributeName = "TargetPath")]
        public string TargetPath { get; set; }
    }
}