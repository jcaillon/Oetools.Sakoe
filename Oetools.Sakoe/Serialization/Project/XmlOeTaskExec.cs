using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public class XmlOeTaskExec : XmlOeTask {
            
        [XmlAttribute("ExecuablePath")]
        public string ExecuablePath { get; set; }
            
        /// <summary>
        /// (you can use task variables in this string)
        /// </summary>
        [XmlAttribute("Parameters")]
        public string Parameters { get; set; }
            
        [XmlAttribute("HiddenExecution")]
        public bool HiddenExecution { get; set; }
            
        /// <summary>
        /// With this option, the task will not fail if the exit code is different of 0
        /// </summary>
        [XmlAttribute("IgnoreExitCode")]
        public bool IgnoreExitCode { get; set; }
            
        /// <summary>
        /// (default to output directory)
        /// </summary>
        [XmlAttribute("WorkingDirectory")]
        public string WorkingDirectory { get; set; }
    }
}