using System;

namespace Oetools.Sakoe.Serialization.Opp {
    
    [Serializable]
    public class XmlDeploymentTarget {
        /// <summary>
        ///     Relative target path (relative to the target directory)
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        ///     The type of deployment done for this target
        /// </summary>
        public XmlDeployType DeployType { get; set; }

        /// <summary>
        ///     Relative path of the pack in which this file is deployed (if any)
        /// </summary>
        public string TargetPackPath { get; set; }

        /// <summary>
        ///     Relative path within the pack (if any)
        /// </summary>
        public string TargetPathInPack { get; set; }
    }
}