using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public class XmlOeTaskWebclient : XmlOeTask {
            
        [XmlElement(ElementName = "VendorName")]
        public string VendorName { get; set; }

        [XmlElement(ElementName = "ApplicationName")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// defaults to ApplicationName + autoincremented webclient version
        /// </summary>
        [XmlElement(ElementName = "ApplicationVersion")]
        public string ApplicationVersion { get; set; }

        [XmlElement(ElementName = "StartupParameters")]
        public string StartupParameters { get; set; }

        /// <summary>
        /// Will be used for both Prowcapp and Codebase by default, provide a custom prowcapp template to change this behavior
        /// </summary>
        [XmlElement(ElementName = "LocatorUrl")]
        public string LocatorUrl { get; set; }

        /// <summary>
        /// Valid oe version for this application
        /// </summary>
        [XmlElement(ElementName = "WebClientVersion")]
        public string WebClientVersion { get; set; } = "11.7";

        /// <summary>
        /// The Directory path from which to create the webclient files (can be relative to the build output directory, the default value is ".")
        /// </summary>
        [XmlElement(ElementName = "WebclientRootDirectoryPath")]
        public string WebclientRootDirectoryPath { get; set; } = ".";

        /// <summary>
        /// The output directory in which the .prowcapp and .cab + diffs/.cab files will be created (can be relative to the build output directory)
        /// </summary>
        [XmlElement(ElementName = "WebclientOutputDirectory")]
        public string WebclientOutputDirectory { get; set; } = "webclient";

        /// <summary>
        /// Path to the model of the .prowcapp to use (can be left empty and the internal model will be used)
        /// </summary>
        [XmlElement(ElementName = "ProwcappTemplateFilePath")]
        public string ProwcappTemplateFilePath { get; set; }
            
        /// <summary>
        /// If null, all the files in the root path will be added to a default component named as <see cref="ApplicationVersion"/>
        /// </summary>
        [XmlArray("Components")]
        [XmlArrayItem("Component", typeof(string))]
        public List<XmlOeWebclientComponent> Components { get; set; }

        [Serializable]
        public class XmlOeWebclientComponent {
                
            [XmlAttribute(AttributeName = "DownloadMode")]
            public XmlOeWebclientComponentDownloadMode DownloadMode { get; set; }
                            
            [XmlArray("IncludedFiles")]
            [XmlArrayItem("IncludePathPattern", typeof(string))]
            public List<string> IncludedFiles { get; set; }
                
            [Serializable]
            public enum XmlOeWebclientComponentDownloadMode {
                [XmlEnum("Eager")] 
                Eager,
                [XmlEnum("Lazy")] 
                Lazy
            }
        }

    }
}