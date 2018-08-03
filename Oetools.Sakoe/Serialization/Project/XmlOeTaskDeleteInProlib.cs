using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public class XmlOeTaskDeleteInProlib : XmlOeTaskOnFile {
            
        /// <summary>
        /// The relative file path pattern to delete inside the matched prolib file
        /// </summary>
        [XmlAttribute("RelativeFilePatternToDelete")]
        public string RelativeFilePatternToDelete { get; set; }
    }
}