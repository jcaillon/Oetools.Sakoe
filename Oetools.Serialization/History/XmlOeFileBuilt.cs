using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.History {
    [Serializable]
    public class XmlOeFileBuilt : XmlOeFile {
        /// <summary>
        /// Represents the state of the file for this build compare to the previous one
        /// </summary>
        [XmlElement(ElementName = "State")]
        public XmlOeFileState State { get; set; }

        /// <summary>
        /// A list of the targets for this file
        /// </summary>
        [XmlArray("Targets")]
        [XmlArrayItem("Compiled", typeof(XmlOeTargetCompile))]
        [XmlArrayItem("Copied", typeof(XmlOeTargetCopy))]
        [XmlArrayItem("Prolibed", typeof(XmlOeTargetProlib))]
        [XmlArrayItem("Zipped", typeof(XmlOeTargetZip))]
        [XmlArrayItem("Cabbed", typeof(XmlOeTargetCab))]
        public List<XmlOeTarget> Targets { get; set; }
    }
}