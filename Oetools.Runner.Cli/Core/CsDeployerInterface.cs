using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using csdeployer.Lib;
using Oetools.Packager.Core;
using Oetools.Packager.Core.Config;
using Oetools.Utilities.Lib;

namespace csdeployer.Core {

    internal class CsDeployerInterface : ConfigDeploymentPackaging{

        private static CsDeployerInterface _deployerInterface;

        /// <summary>
        ///     Return the current ProgressEnvironnement object (null if the list is empty!)
        /// </summary>
        internal static CsDeployerInterface Instance {
            get {
                if (_deployerInterface == null)
                    _deployerInterface = new CsDeployerInterface();
                return _deployerInterface;
            }
            set { _deployerInterface = value; }
        }

        /// <summary>
        ///     Indicates how the deployment went
        /// </summary>
        public ReturnCode ReturnCode { get; set; }

        public DateTime DeploymentDateTime { get; set; }

        /// <summary>
        ///     Either pack or deploy
        /// </summary>
        public RunMode RunMode { get; set; }

        /// <summary>
        ///     Path to the report directory, in which the html will be exporer
        /// </summary>
        public string OutPathReportDir { get; set; }

        /// <summary>
        ///     Path to the output xml result that can later be used in PreviousDeploymentFiles
        /// </summary>
        public string OutPathDeploymentResults { get; set; }
        /// <summary>
        ///     Path to the error log file to use
        /// </summary>
        public string ErrorLogFilePath { get; set; }

        /// <summary>
        ///     Returns the currently selected database's .pf for the current environment
        /// </summary>
        public string PfPath { get; set; }

        public string ExtraPf { get; set; }
        
        public string ExtraProPath { get; set; }
        
        public string OverloadFolderTemp { get; set; }
        public string OverloadFilesPatternCompilable { get; set; }

        /// <summary>
        ///     True if we only want to simulate a deployment w/o actually doing it
        /// </summary>
        public bool IsTestMode { get; set; }

        /// <summary>
        /// List of the compilation errors found
        /// </summary>
        public List<FileError> CompilationErrors { get; set; }


         /// <summary>
        ///     Returns the path to prolib.exe considering the path to prowin.exe
        /// </summary>
        internal string ProlibPath {
            get { return String.IsNullOrEmpty(ProwinPath) ? "" : Path.Combine(Path.GetDirectoryName(ProwinPath) ?? "", @"prolib.exe"); }
        }

        internal string FolderTemp {
            get { return CreateDirectory(Path.Combine(!String.IsNullOrEmpty(OverloadFolderTemp) ? OverloadFolderTemp : Path.Combine(Path.GetTempPath(), "AblDeployer"), Path.GetRandomFileName())); }
        }

        private string CreateDirectory(string dir) {
            Utils.CreateDirectory(dir);
            return dir;
        }

        internal string FilesPatternCompilable {
            get { return !String.IsNullOrEmpty(OverloadFilesPatternCompilable) ? OverloadFilesPatternCompilable : "*.p,*.w,*.t,*.cls"; }
        }

        
        /// <summary>
        ///     Returns the database connection string (complete with .pf + extra)
        /// </summary>
        internal string ConnectionString {
            get {
                var connectionString = new StringBuilder();
                if (File.Exists(PfPath)) {
                    Utils.ForEachLine(PfPath, new byte[0], (nb, line) => {
                        var commentPos = line.IndexOf("#", StringComparison.CurrentCultureIgnoreCase);
                        if (commentPos == 0)
                            return;
                        if (commentPos > 0)
                            line = line.Substring(0, commentPos);
                        line = line.Trim();
                        if (!String.IsNullOrEmpty(line)) {
                            connectionString.Append(" ");
                            connectionString.Append(line);
                        }
                    });
                    connectionString.Append(" ");
                }

                connectionString.Append(ExtraPf.Trim());
                return connectionString.ToString().Replace("\n", " ").Replace("\r", "").Trim();
            }
        }

        private List<string> _currentProPathDirList;

        /// <summary>
        ///     List the existing directories as they are listed in the .ini file + in the custom ProPath field,
        ///     this returns an exhaustive list of EXISTING folders and .pl files and ensure each item is present only once
        ///     It also take into account the relative path, using the BaseLocalPath (or currentFileFolder)
        /// </summary>
        internal List<string> GetProPathDirList {
            get {
                if (_currentProPathDirList == null) {
                    var ini = new IniReader(IniPath);
                    var completeProPath = ini.GetValue("PROPATH", "");
                    completeProPath = (completeProPath + "," + ExtraProPath).Trim(',');

                    var uniqueDirList = new HashSet<string>();
                    foreach (var path in completeProPath.Split(',', '\n', ';').Select(path => path.Trim()).Where(path => !String.IsNullOrEmpty(path))) {
                        var thisPath = path;
                        // need to take into account relative paths
                        if (!Path.IsPathRooted(thisPath))
                            try {
                                if (thisPath.Contains("%"))
                                    thisPath = Environment.ExpandEnvironmentVariables(thisPath);
                                thisPath = Path.GetFullPath(Path.Combine(SourceDirectory, thisPath));
                            } catch (Exception) {
                                //
                            }

                        if (Directory.Exists(thisPath) || File.Exists(thisPath))
                            if (!uniqueDirList.Contains(thisPath))
                                uniqueDirList.Add(thisPath);
                    }

                    // if the user didn't set a propath, add every folder of the source directory in the propath (don't add hidden folders though)
                    if (uniqueDirList.Count == 0)
                        try {
                            foreach (var folder in Utils.EnumerateFolders(SourceDirectory, "*", SearchOption.AllDirectories))
                                if (!uniqueDirList.Contains(folder))
                                    uniqueDirList.Add(folder);
                        } catch (Exception) {
                            //
                        }

                    // add the source directory
                    if (!uniqueDirList.Contains(SourceDirectory))
                        uniqueDirList.Add(SourceDirectory);

                    _currentProPathDirList = uniqueDirList.ToList();
                }

                return _currentProPathDirList;
            }
        }

        /// <summary>
        ///     Use this method to know if the CONNECT define for the current environment connects the database in
        ///     single user mode (returns false if not or if no database connection is set)
        /// </summary>
        /// <returns></returns>
        internal bool IsDatabaseSingleUser {
            get { return ConnectionString.RegexMatch(@"\s-1", RegexOptions.Singleline); }
        }
        
        
        /// <summary>
        /// We can use the nosplash parameter since progress 11.6
        /// </summary>
        internal bool CanProwinUseNoSplash {
            get { return (ProwinPath ?? "").Contains("116"); }
        }

                        
        [XmlIgnore]
        internal string ExportXmlFile { get; set; }

        [XmlIgnore]
        internal string RaisedException { get; set; }

        public EnvExecutionCompilation Env { get; set; }
    }
}
