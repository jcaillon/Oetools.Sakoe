using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Project {
    
    [Serializable]
    public class XmlOeProjectProperties {
            
        [XmlArray("ProjectDatabases")]
        [XmlArrayItem("ProjectDatabase", typeof(XmlOeProjectDatabase))]
        public List<XmlOeProjectDatabase> ProjectDatabases { get; set; }
            
        [XmlElement(ElementName = "DatabaseConnectionExtraParameters")]
        public string DatabaseConnectionExtraParameters { get; set; }

        [XmlArray("DatabaseAliases")]
        [XmlArrayItem("Alias", typeof(XmlOeDatabaseAlias))]
        public List<XmlOeDatabaseAlias> DatabaseAliases { get; set; }

        public class XmlOeDatabaseAlias {
                
            [XmlAttribute(AttributeName = "ProgresCommandLineExtraParameters")]
            public string AliasLogicalName { get; set; }

            [XmlAttribute(AttributeName = "PreProgresExecutionProgramPath")]
            public string DatabaseLogicalName { get; set; }
        }
            
            
        [XmlElement(ElementName = "IniFilePath")]
        public string IniFilePath { get; set; }

        [XmlElement(ElementName = "AddAllSourceDirectoriesToPropath")]
        public bool AddAllSourceDirectoriesToPropath { get; set; }
        
        /// <summary>
        /// Adds the gui or tty (depending on <see cref="UseCharacterModeExecutable"/>) folder as well as the contained .pl to the propath
        /// Also adds dlc and dlc/bin
        /// </summary>
        [XmlElement(ElementName = "AddDefaultOpenedgePropath")]
        public bool AddDefaultOpenedgePropath { get; set; }

        [XmlArrayItem("Path", typeof(string))]
        [XmlArray("PropathEntries")]
        public List<string> PropathEntries { get; set; }
            
        [XmlArray("PropathFilter")]
        [XmlArrayItem("Regex", typeof(string))]
        public List<string> PropathFilter { get; set; }

        [XmlElement(ElementName = "DlcDirectoryPath")]
        public string DlcDirectoryPath { get; set; }

        [XmlElement(ElementName = "UseCharacterModeExecutable")]
        public bool UseCharacterModeExecutable { get; set; }

        [XmlElement(ElementName = "NeverUseBatchModeOption")]
        public bool NeverUseBatchModeOption { get; set; }

        [XmlElement(ElementName = "ProgresCommandLineExtraParameters")]
        public string ProgresCommandLineExtraParameters { get; set; }

        [XmlElement(ElementName = "ProcedurePathToExecuteBeforeAnyProgressExecution")]
        public string ProcedurePathToExecuteBeforeAnyProgressExecution { get; set; }

        [XmlElement(ElementName = "ProcedurePathToExecuteAfterAnyProgressExecution")]
        public string ProcedurePathToExecuteAfterAnyProgressExecution { get; set; }

        [XmlElement(ElementName = "TemporaryDirectory")]
        public string TemporaryDirectory { get; set; }
    }
    
    [Serializable]
    public class XmlOeProjectDatabase {
      
        [XmlAttribute(AttributeName = "LogicalName")]
        public bool LogicalName { get; set; }
            
        [XmlAttribute(AttributeName = "DataDefinitionFilePath")]
        public string DataDefinitionFilePath { get; set; }
        
    }
}