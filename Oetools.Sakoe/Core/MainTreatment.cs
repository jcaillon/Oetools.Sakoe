#region header
// ========================================================================
// Copyright (c) 2017 - Julien Caillon (julien.caillon@gmail.com)
// This file (MainTreatment.cs) is part of csdeployer.
// 
// csdeployer is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// csdeployer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with csdeployer. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

namespace Oetools.Sakoe.Core {
    /*
    internal class MainTreatment : ProgressTreatment {

        #region Override

        /// <summary>
        /// Should return the progression of the treatment
        /// </summary>
        /// <returns></returns>
        protected override ProgressionEventArgs GetProgress() {
            if (_deployment == null)
                return new ProgressionEventArgs {
                    GlobalProgression = 0,
                    CurrentStepProgression = 0,
                    CurrentStepName = "Initialisation du déploiement",
                    ElpasedTime = ElapsedTime
                };
            string currentOperationName;
            switch (_deployment.CurrentOperationName) {
                case DeploymentStep.CopyingReference:
                    currentOperationName = "Copie du paquet de référence";
                    break;
                case DeploymentStep.Listing:
                    currentOperationName = "Listing des fichiers sources";
                    break;
                case DeploymentStep.Compilation:
                    currentOperationName = "Compilation";
                    break;
                case DeploymentStep.DeployRCode:
                    currentOperationName = "Déploiement des rcodes";
                    break;
                case DeploymentStep.DeployFile:
                    currentOperationName = "Déploiement des fichiers";
                    break;
                case DeploymentStep.CopyingFinalPackageToDistant:
                    currentOperationName = "Copie du paquet sur répertoire final";
                    break;
                case DeploymentStep.BuildingWebclientDiffs:
                    currentOperationName = "Création des cab webclient différentiels";
                    break;
                case DeploymentStep.BuildingWebclientCompleteCab:
                    currentOperationName = "Création du cab webclient complet";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new ProgressionEventArgs {
                GlobalProgression = _deployment.OverallProgressionPercentage / _deployment.TotalNumberOfOperations,
                ElpasedTime = ElapsedTime,
                CurrentStepName = currentOperationName,
                CurrentStepProgression = _deployment.CurrentOperationPercentage
            };
        }

        /// <summary>
        /// Called when the treatment starts
        /// </summary>
        protected override void StartJob() {

            // load the input .xml
            try {
                _inputXml = CsDeployerInterfaceXml.Load(csdeployer.Start.XmlConfigPath);
                _inputXml.ReturnCode = ReturnCode.NoSet;
                _inputXml.DeploymentDateTime = DateTime.Now;
            } catch (Exception e) {
                MessageBox.Show(@"Impossible de lire le fichier xml d'entrée : " + csdeployer.Start.XmlConfigPath.Quoter() + Environment.NewLine + Environment.NewLine + e, @"Une erreur est survenue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Stop();
                return;
            }

            // start deploying
            Task.Factory.StartNew(() => {
                try {
                    if (StartDeploying()) {
                        return;
                    }
                } catch (Exception e) {
                    ErrorHandler.LogErrors(e, "Erreur lors de l'initialisation du déploiement");
                }
                Stop();
            });
        }

        /// <summary>
        /// Called when the treatment stops
        /// </summary>
        protected override void StopJob() {
            // save the output xml
            try {
                _inputXml.CompilationErrors = _deployment.CompilationErrorsOutput;
                _inputXml.DeployedFiles = _deployment.DeployedFilesOutput;
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur lors du listing des erreurs de compilation et des fichiers déployés");
            }
            try {
                _inputXml.WcProwcappVersion = CsDeployerInterface.Instance.WcProwcappVersion;
                _inputXml.ReturnCode = CsDeployerInterface.Instance.ReturnCode;
                _inputXml.DeploymentDateTime = CsDeployerInterface.Instance.DeploymentDateTime;
                if (_inputXml.ReturnCode == ReturnCode.NoSet) {
                    _inputXml.ReturnCode = ReturnCode.CompilationError;
                }
                if (_inputXml.DeployedFiles == null || _inputXml.DeployedFiles.Count == 0) {
                    _inputXml.DeployedFiles = _inputXml._previousDeployedFiles;
                }
                CsDeployerInterfaceXml.Save(_inputXml, CsDeployerInterface.Instance.OutPathDeploymentResults);
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur lors de l'enregistrement du fichier de sortie");
            }

            // stop deploying
            try {
                StopDeploying();
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur survenue pendant la finalisation du déploiement");
            }

            // get rid of the temp folder
            Utils.DeleteDirectory(CsDeployerInterface.Instance.FolderTemp, true);
        }

        /// <summary>
        /// Method to call to cancel the treatment
        /// </summary>
        public override void Cancel() {
            try {
                if (_deployment != null) {
                    _deployment.Cancel();
                    return;
                }
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur pendant l'annulation du déploiement");
            }
            Stop();
        }

        #endregion

        #region Private fields

        private CsDeployerInterfaceXml _inputXml;

        private DeploymentHandlerDifferential _deployment;

        private DateTime _startingTime;

        #endregion

        #region Private methods

        /// <summary>
        /// Get the time elapsed since the beginning of the compilation in a human readable format
        /// </summary>
        private string ElapsedTime {
            get { return UtiXCopyls.ConvertToHumanTime(TimeSpan.FromMilliseconds(DateTime.Now.Subtract(_startingTime).TotalMilliseconds)); }
        }

        /// <summary>
        /// Start deploying
        /// </summary>
        private bool StartDeploying() {
            _startingTime = DateTime.Now;

            // read the previous source files
            var listPreviousDeployedFiles = new List<CsDeployerInterface>();
            try {
                var prevXmlList = CsDeployerInterface.Instance.PreviousDeployments;
                if (prevXmlList != null && prevXmlList.Count > 0) {
                    foreach (var xmlPath in prevXmlList) {
                        if (File.Exists(xmlPath)) {
                            var config = CsDeployerInterface.Load(xmlPath);
                            config.ExportXmlFile = xmlPath;
                            listPreviousDeployedFiles.Add(config);
                        }
                    }

                    // sort the deployment from oldest (0) to newer (n)
                    listPreviousDeployedFiles = listPreviousDeployedFiles.OrderBy(config => config.WcProwcappVersion).ThenBy(config => config.DeploymentDateTime).ToList();
                    var lastOrDefault = listPreviousDeployedFiles.LastOrDefault();
                    if (lastOrDefault != null) {
                        _previousDeployedFiles = lastOrDefault.;
                    }
                }
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur pendant le chargement des fichiers xml de déploiements précédents");
                return false;
            }

            switch (CsDeployerInterface.Instance.RunMode) {
                case RunMode.Deployment:
                    _deployment = new DeploymentHandlerDifferential(CsDeployerInterface.Instance, CsDeployerInterface.Instance.Env) {
                        IsTestMode = CsDeployerInterface.Instance.IsTestMode,
                        
                    };
                    break;
                case RunMode.Packaging:
                    _deployment = new DeploymentHandlerPackaging(CsDeployerInterface.Instance, CsDeployerInterface.Instance.Env);
                    ((DeploymentHandlerPackaging) _deployment).ListPreviousDeployements = listPreviousDeployedFiles.Cast<ConfigDeploymentPackaging>().ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _deployment.PreviousDeployedFiles = _previousDeployedFiles;
            _deployment.OnExecutionEnd = OnExecutionEnd;
            _deployment.OnExecutionOk = OnExecutionOk;
            _deployment.OnExecutionFailed = OnExecutionFailed;

            _deployment.Start();

            return true;
        }

        /// <summary>
        /// Stop deploying
        /// </summary>
        private void StopDeploying() {
            // we create the html report
            try {
                Utils.CreateDirectory(CsDeployerInterface.Instance.OutPathReportDir);
                HtmlReport.ExportReport(_deployment, Path.Combine(CsDeployerInterface.Instance.OutPathReportDir, "index.html"));
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur survenue pendant l'export du rapport html");
            }
        }

        /// <summary>
        /// On deployment end
        /// </summary>
        private void OnExecutionEnd(DeploymentHandler deploymentHandler) {
            // we save the data on the current source files
            try {
                if (CsDeployerInterface.Instance.ReturnCode == ReturnCode.Ok && !CsDeployerInterface.Instance.IsTestMode) {
                    CsDeployerInterface.Instance.DeployedFiles = _deployment.DeployedFilesOutput;
                    CsDeployerInterface.Instance.CompilationErrors = _deployment.CompilationErrorsOutput;
                }
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur survenue pendant la sauvegarde des fichiers déployés");
            }

            // we can now end the treatment
            Stop();
        }

        private void OnExecutionOk(DeploymentHandler deploymentHandler) {
            CsDeployerInterface.Instance.ReturnCode = ReturnCode.Ok;
        }

        private void OnExecutionFailed(DeploymentHandler deploymentHandler) {
            CsDeployerInterface.Instance.ReturnCode = deploymentHandler.HasBeenCancelled ? ReturnCode.Canceled : ReturnCode.CompilationError;
        }

        private static CsDeployerInterface GetConfigDeploymentPackaging(CsDeployerInterfaceXml interfaceXml) {
            var config = new CsDeployerInterface {
                
            };

            var listPreviousDeployedFiles = new List<CsDeployerInterface>();
            try {
                var prevXmlList = interfaceXml.PreviousDeploymentFiles;
                if (prevXmlList != null && prevXmlList.Count > 0) {
                    foreach (var xmlPath in prevXmlList) {
                        if (File.Exists(xmlPath)) {
                            var prevConfig = GetConfigDeploymentPackaging(CsDeployerInterfaceXml.Load(xmlPath));
                            prevConfig.ExportXmlFile = xmlPath;
                            listPreviousDeployedFiles.Add(prevConfig);
                        }
                    }

                    // sort the deployment from oldest (0) to newer (n)
                    listPreviousDeployedFiles = listPreviousDeployedFiles.OrderBy(conf => conf.WcProwcappVersion).ThenBy(conf => conf.DeploymentDateTime).ToList();
                    var lastOrDefault = listPreviousDeployedFiles.LastOrDefault();
                    if (lastOrDefault != null) {
                        _previousDeployedFiles = lastOrDefault.de;
                    }
                }
            } catch (Exception e) {
                ErrorHandler.LogErrors(e, "Erreur pendant le chargement des fichiers xml de déploiements précédents");
                return false;
            }

            return config;
        }

        #endregion
        
        private string CreateDirectory(string dir) {
            Utils.CreateDirectory(dir);
            return dir;
        }

        internal string FolderTemp {
            get { return CreateDirectory(Path.Combine(!String.IsNullOrEmpty(OverloadFolderTemp) ? OverloadFolderTemp : Path.Combine(Path.GetTempPath(), "AblDeployer"), Path.GetRandomFileName())); }
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
            get { return ConnectionString.RegexMatch(@"\s-1($|\s)", RegexOptions.Singleline); }
        }
    }
    */
}