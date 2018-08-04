using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public enum XmlOeCompressionLevel {
        [XmlEnum("None")] None,
        [XmlEnum("Max")] Max
    }
}