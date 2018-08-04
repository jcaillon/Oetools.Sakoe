using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    public abstract class XmlOeTaskOnFile : XmlOeTask {
            
        [XmlAttribute("Include")]
        public string Include { get; set; }
            
        [XmlAttribute("Exclude")]
        public string Exclude { get; set; }
    }
}