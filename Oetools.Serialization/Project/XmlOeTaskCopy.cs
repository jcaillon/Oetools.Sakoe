using System;

namespace Oetools.Sakoe.Serialization.Project {
    [Serializable]
    public class XmlOeTaskCopy : XmlOeTaskOnFileWithTarget {
    }
    
    [Serializable]
    public class XmlOeTaskCompile : XmlOeTaskCopy {
    }
    
    [Serializable]
    public class XmlOeTaskCompileProlib : XmlOeTaskProlib {
    }
    
    [Serializable]
    public class XmlOeTaskCompileZip : XmlOeTaskZip {
    }
    
    [Serializable]
    public class XmlOeTaskCompileCab : XmlOeTaskCab {
    }
    
    [Serializable]
    public class XmlOeTaskCompileUploadFtp : XmlOeTaskFtp {
    }
}