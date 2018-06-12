using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Runner.Cli.Config.v1 {
    
    [Serializable]
    [XmlInclude(typeof(XmlFileDeployedCompiled))]
    public class XmlFileDeployed : XmlFileSourceInfo {
        /// <summary>
        ///     a list of the targets for this deployment
        /// </summary>
        public List<XmlDeploymentTarget> Targets { get; set; }

        /// <summary>
        ///     The action done for this file
        /// </summary>
        public XmlDeploymentAction Action { get; set; }
    }
}