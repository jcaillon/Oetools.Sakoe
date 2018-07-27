using System;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Config.v1 {
    
    /// <summary>
    ///     Types of deploy, used during rules sorting
    /// </summary>
    [Serializable]
    public enum XmlDeployType : byte {
        [XmlEnum("None")]
        None = 0,
        [XmlEnum("Delete")]
        Delete = 1,
        [XmlEnum("DeleteFolder")]
        DeleteFolder = 2,
        [XmlEnum("DeleteInProlib")]
        DeleteInProlib = 10,
        [XmlEnum("Prolib")]
        Prolib = 11,
        [XmlEnum("Zip")]
        Zip = 12,
        [XmlEnum("Cab")]
        Cab = 13,
        [XmlEnum("Ftp")]
        Ftp = 14,
        [XmlEnum("CopyFolder")]
        CopyFolder = 21,
        [XmlEnum("Copy")]
        Copy = 30,
        [XmlEnum("Move")]
        Move = 31
    }
}