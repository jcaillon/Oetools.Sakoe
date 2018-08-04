using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    [XmlRoot("AutomatedTasks")]
    public class XmlOeAutomatedTasks {
        
        [XmlAttribute("Label")]
        public string Label { get; set; }
        
        [XmlElement(ElementName = "ArchivesCompressionLevel")]
        public XmlOeCompressionLevel ArchivesCompressionLevel { get; set; }
        
        [XmlArray("Tasks")]
        [XmlArrayItem("Copy", typeof(XmlOeTaskCopy))]
        [XmlArrayItem("Move", typeof(XmlOeTaskMove))]
        [XmlArrayItem("Execute", typeof(XmlOeTaskExec))]
        [XmlArrayItem("RemoveDir", typeof(XmlOeTaskRemoveDir))]
        [XmlArrayItem("Delete", typeof(XmlOeTaskDelete))]
        [XmlArrayItem("DeleteInProlib", typeof(XmlOeTaskDeleteInProlib))]
        [XmlArrayItem("Prolib", typeof(XmlOeTaskProlib))]
        [XmlArrayItem("Zip", typeof(XmlOeTaskZip))]
        [XmlArrayItem("Cab", typeof(XmlOeTaskCab))]
        [XmlArrayItem("UploadFtp", typeof(XmlOeTaskFtp))]
        [XmlArrayItem("Webclient", typeof(XmlOeTaskWebclient))]
        public List<XmlOeTask> Tasks { get; set; }
    }
}