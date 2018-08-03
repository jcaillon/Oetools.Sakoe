using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public class XmlOeTaskVariable {
            
        [XmlAttribute("Name")]
        public string Name { get; set; }
            
        [XmlText]
        public string Value { get; set; }
    }
}