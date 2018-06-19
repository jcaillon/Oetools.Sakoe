using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Oetools.HtmlExport.Config;
using Oetools.HtmlExport.Config.Interface;
using Oetools.HtmlExport.Lib;
using Oetools.HtmlExport.Resources.Css;
using Oetools.HtmlExport.Resources.Images;
using Oetools.Packager.Core;
using Oetools.Packager.Core.Config;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.HtmlExport.Html {

    internal class HtmlReport {

        public static void ExportReport(string exportPath, ConfigDeployment config, ResultDeployment result, IRunConfiguration runConfig) {

            Exception lastCatchedExceptions = null;
            
            var html = new StringBuilder();
            html.AppendLine("<html class='NormalBackColor'>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='UTF-8'>");
            html.AppendLine("<style>");
            html.AppendLine(CssResources.GetCssAsStringFromResources("StyleSheet.css"));
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine(@"
                <table class='ToolTipName' style='margin-bottom: 0px; width: 100%'>
                    <tr>
                        <td rowspan='2' style='width: 95px; padding-left: 10px'><img src='Report_64x64' width='64' height='64' /></td>
                        <td class='Title'>Rapport de déploiement</td>
                    </tr>
                    <tr>");
            switch (result.ReturnCode) {
                case ReturnCode.CompilationError:
                    html.AppendLine(@"<td class='SubTitle'><img style='padding-right: 2px;' src='Error_25x25' height='25px'>Une erreur de compilation est survenue</td>");
                    break;
                case ReturnCode.DeploymentError:
                    html.AppendLine(@"<td class='SubTitle'><img style='padding-right: 2px;' src='Error_25x25' height='25px'>Une erreur de déploiement est survenue</td>");
                    break;
                case ReturnCode.Ok:
                    if (config.IsTestMode)
                        html.AppendLine(@"<td class='SubTitle'><img style='padding-right: 2px;' src='Test_25x25' height='25px'>Test réalisé avec succès</td>");
                    else
                        html.AppendLine(@"<td class='SubTitle'><img style='padding-right: 2px;' src='Ok_25x25' height='25px'>Le déploiement s'est déroulé correctement</td>");
                    break;
                case ReturnCode.Canceled:
                    html.AppendLine(@"<td class='SubTitle'><img style='padding-right: 2px;' src='Warning_25x25' height='25px'>Déploiement annulé par l'utilisateur</td>");
                    break;
            }
            html.AppendLine(@"
                    </tr>
                </table>");

            try {
                html.AppendLine(FormatDeploymentParameters(config, result));
            } catch (Exception e) {
                lastCatchedExceptions = new Exception("Erreur lors de la génération des paramètres de déploiement", e);
            }

            if (!string.IsNullOrEmpty(runConfig.RaisedException)) {
                html.AppendLine(@"<h2>Problèmes rencontrés pendant le déploiement :</h2>");
                html.AppendLine(@"<div class='IndentDiv errors'>");
                html.AppendLine(runConfig.RaisedException);
                html.AppendLine(@"</div>");
                html.AppendLine(@"<div class='IndentDiv'>");
                html.AppendLine(@"<b>Lien vers le fichier .log pour plus de détails :</b><br>");
                html.AppendLine(runConfig.ErrorLogFilePath.ToHtmlLink());
                html.AppendLine(@"</div>");
            }

            if (result.RulesErrors != null && result.RulesErrors.Count > 0) {
                html.AppendLine(@"<h2>Erreurs de règles :</h2>");
                html.AppendLine(@"<div class='IndentDiv errors'>");
                foreach (var error in result.RulesErrors) {
                    html.AppendLine(HtmlDeployRule.Description(error.RuleFilePath, error.ErrorLine) + " : " + error.ErrorDescription);
                }
                html.AppendLine(@"</div>");
            }

            try {
                html.AppendLine(FormatDeploymentResults(config, result));
            } catch (Exception e) {
                lastCatchedExceptions = new Exception("Erreur lors de la génération des résultats de déploiement", e);
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            var outputHtml = html.ToString();

            outputHtml = new Regex("src=[\"'](.*?)[\"']", RegexOptions.Compiled)
                .Replace(outputHtml, match => $"data:image/png;base64,{ImagesResource.GetImageBase64Encoded(match.Groups[1].Value)}");

            outputHtml = new Regex("<a href=\"(.*?)[|\"]", RegexOptions.Compiled)
                .Replace(outputHtml, "<a href=\"file:///$1\"");
            
            File.WriteAllText(exportPath, outputHtml, Encoding.UTF8);

            if (lastCatchedExceptions != null) {
                throw lastCatchedExceptions;
            }
        }

        private static string FormatDeploymentParameters(ConfigDeployment config, ResultDeployment result) {
            var sb = new StringBuilder(@"             
                <h2>Paramètres du déploiement :</h2>
                <div class='IndentDiv'>");
            if (result.DeploymentStartTime != null) {
                sb.Append($"<div>Date de début de déploiement : <b>{result.DeploymentStartTime}</b></div>");
            }
            if (result.CompilationStartTime != null) {
                sb.Append($"<div>Date de début de compilation : <b>{result.CompilationStartTime}</b></div>");
            }
            if (result is ResultDeploymentPackaging resultPackaging && resultPackaging.PackagingStartTime != null) {
                sb.Append($"<div>Date de début de packaging : <b>{resultPackaging.PackagingStartTime}</b></div>");
            }
            sb.Append(@"
                    <div>Nombre de processeurs sur cet ordinateur : <b>" + Environment.ProcessorCount + @"</b></div>
                    <div>Nombre de process progress utilisés pour la compilation : <b>" + result.TotalNumberOfProcesses + @"</b></div>
                    <div>Compilation forcée en mono-process? : <b>" + config.ForceSingleProcess + (config.IsDatabaseSingleUser ? " (connecté à une base de données en mono-utilisateur!)" : "") + @"</b></div>
                    <div>Répertoire des sources : " + config.Env.SourceDirectory.ToHtmlLink() + @"</div>
                    <div>Répertoire cible pour le déploiement : " + config.Env.TargetDirectory.ToHtmlLink() + @"</div>       
                </div>");
            if (config is ConfigDeploymentDifferential configDifferential) {
                sb.Append(@"
                <div class='IndentDiv'>
                    <div>Déploiement FULL : <b>" + configDifferential.ForceFullDeploy + @"</b></div>
                    <div>Calcul du MD5 des fichiers : <b>" + configDifferential.ComputeMd5 + @"</b></div> 
                </div>");
            }

            if (config is ConfigDeploymentPackaging configPackaging) {
                sb.Append(@"
                <div class='IndentDiv'>
                    <div>Répertoire de référence : " + configPackaging.ReferenceDirectory.ToHtmlLink() + @"</div>
                </div>");
            }
            return sb.ToString();
        }

        private static string FormatDeploymentResults(ConfigDeployment config, ResultDeployment result) {
            StringBuilder currentReport = new StringBuilder();

            if (result is ResultDeploymentPackaging resultPackaging && config is ConfigDeploymentPackaging configPackaging) {
                if (resultPackaging.WebClientCreated) {
                    currentReport.AppendLine(@"             
                <h2>Fichiers webclient créés :</h2>
                <div class='IndentDiv'>");
                    //sb.AppendLine(@"<h3>Fichier " + _proEnv.WcApplicationName + @".cab webclient complet créé</h3>");
                    currentReport.AppendLine(@"<div><img height='15px' src='" + Helper.GetExtensionImage("cab", true) + "'>" + Path.Combine(config.Env.TargetDirectory, configPackaging.ClientWcpDirectoryName, configPackaging.WcApplicationName + ".prowcapp").ToHtmlLink(null, true) + @"</div>");
                    currentReport.AppendLine(@"<div><img height='15px' src='" + Helper.GetExtensionImage("cab", true) + "'>" + Path.Combine(config.Env.TargetDirectory, configPackaging.ClientWcpDirectoryName, configPackaging.WcApplicationName + ".cab").ToHtmlLink(null, true) + @"</div>");
                    if (resultPackaging.DifferentialCabinets.Count > 0) {
                        //sb.AppendLine(@"<h3>Fichier(s) .cab diffs webclient créé(s)</h3>");
                        foreach (var cab in resultPackaging.DifferentialCabinets) {
                            currentReport.AppendLine(@"<div><img height='15px' src='" + Helper.GetExtensionImage("pl", true) + "'>" + cab.CabPath.ToHtmlLink(null, true) + @"</div>");
                        }
                    }
                    currentReport.AppendLine(@"</div>");
                } else if (resultPackaging.HasWebClient) {
                    var prevWcpDir = Path.Combine(configPackaging.ReferenceDirectory, configPackaging.ClientWcpDirectoryName);
                    if (Directory.Exists(prevWcpDir)) {
                        currentReport.AppendLine(@"             
                        <h2>Fichiers webclient copiés :</h2>
                        <div class='IndentDiv'>");
                        currentReport.AppendLine(@"<div>Pas de différences au niveau client sur ce paquet, copie du dossier webclient du dernier paquet</div>");
                        currentReport.AppendLine(@"<div>Dossier du dernier paquet : " + prevWcpDir.ToHtmlLink() + "</div>");
                        currentReport.AppendLine(@"<div>Dossier de ce paquet : " + Path.Combine(configPackaging.PackageTargetDirectory, configPackaging.ClientWcpDirectoryName).ToHtmlLink() + "</div>");
                        currentReport.AppendLine(@"</div>");
                    }
                }
            }

            currentReport.Append(@"<h2>Détails sur le déploiement :</h2>");
            currentReport.Append(@"<div class='IndentDiv'>");

            switch (result.ReturnCode) {
                case ReturnCode.NoSet:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>Unexpected error!</div>");
                    break;
                case ReturnCode.DeploymentError:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>The deployment has failed</div>");
                    break;
                case ReturnCode.CompilationError:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>A _progres/prowin process has ended with errors</div>");
                    break;
                case ReturnCode.CompilationErrorOnMaxUser:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>One or more processes started for this compilation tried to connect to the database and failed because the maximum number of connection has been reached (error 748).</div>");
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Help_25x25' height='15px'>To correct this problem, you can either :<br><li>reduce the number of processes to use for each core of your computer</li><li>or increase the maximum of connections for your database (-n parameter in the PROSERVE command)</li></div>");
                    break;
                case ReturnCode.Ok:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Ok_25x25' height='15px'>Deployment ended correctly</div>");
                    break;
                case ReturnCode.Canceled:
                    currentReport.Append(@"<div><img style='padding-right: 20px;' src='Warning_25x25' height='15px'>Deployment cancelled by the user</div>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var listLinesCompilation = new List<Tuple<int, string>>();
            StringBuilder line = new StringBuilder();

            var totalDeployedFiles = 0;
            var nbDeploymentError = 0;
            var nbCompilationError = 0;
            var nbCompilationWarning = 0;

            // compilation errors
            foreach (var fileInError in result.ListFilesToCompile.Where(file => file.Errors != null)) {
                bool hasError = fileInError.Errors.Exists(error => error.Level >= ErrorLevel.Error);
                bool hasWarning = fileInError.Errors.Exists(error => error.Level < ErrorLevel.Error);

                if (hasError || hasWarning) {
                    // only add compilation errors
                    line.Clear();
                    line.Append("<div %ALTERNATE%style=\"background-repeat: no-repeat; background-image: url('" + (hasError ? "Error_25x25" : "Warning_25x25") + "'); padding-left: 35px; padding-top: 6px; padding-bottom: 6px;\">");
                    line.Append(FormatCompilationResultForSingleFile(fileInError.SourcePath, fileInError, null));
                    line.Append("</div>");
                    listLinesCompilation.Add(new Tuple<int, string>(hasError ? 3 : 2, line.ToString()));
                }

                if (hasError) {
                    nbCompilationError++;
                } else if (hasWarning)
                    nbCompilationWarning++;
            }

            // for each deploy step
            var listLinesByStep = new Dictionary<int, List<Tuple<int, string>>> {
                {0, new List<Tuple<int, string>>()}
            };
            foreach (var kpv in result.FilesToDeployPerStep) {
                // group either by directory name or by pack name
                var groupDirectory = kpv.Value.GroupBy(deploy => deploy.GroupKey).Select(deploys => deploys.ToList()).ToList();

                foreach (var group in groupDirectory.OrderByDescending(list => list.First().DeployType).ThenBy(list => list.First().GroupKey)) {
                    var deployFailed = group.Exists(deploy => !deploy.IsOk);
                    var first = group.First();

                    line.Clear();
                    line.Append("<div %ALTERNATE%style=\"background-repeat: no-repeat; background-image: url('" + (deployFailed ? "Error_25x25" : "Ok_25x25") + "'); padding-left: 35px; padding-top: 6px; padding-bottom: 6px;\">");
                    line.Append(HtmlFileToDeploy.GroupHeader(first));
                    foreach (var fileToDeploy in group.OrderBy(deploy => deploy.To)) {
                        line.Append(HtmlFileToDeploy.Description(fileToDeploy, kpv.Key <= 1 ? config.Env.SourceDirectory : config.Env.TargetDirectory));
                    }
                    line.Append("</div>");

                    if (!listLinesByStep.ContainsKey(kpv.Key))
                        listLinesByStep.Add(kpv.Key, new List<Tuple<int, string>>());

                    listLinesByStep[kpv.Key].Add(new Tuple<int, string>(deployFailed ? 3 : 1, line.ToString()));

                    if (deployFailed)
                        nbDeploymentError += group.Count(deploy => !deploy.IsOk);
                    else
                        totalDeployedFiles += group.Count;
                }
            }

            // compilation
            currentReport.Append(@"<div style='padding-top: 7px; padding-bottom: 7px;'>Nombre de fichiers à compiler : <b>" + result.TotalNumberOfFilesToCompile+ "</b>, répartition : " + GetNbFilesPerType(result.ListFilesToCompile.Select(compile => compile.SourcePath).ToList()).Aggregate("", (current, kpv) => current + (@"<img style='padding-right: 5px;' src='" + Helper.GetExtensionImage(kpv.Key.ToString(), true) + "' height='15px'><span style='padding-right: 12px;'>x" + kpv.Value + "</span>")) + "</div>");

            // compilation time
            currentReport.Append(@"<div><img style='padding-right: 20px;' src='Clock_15px' height='15px'>Temps de compilation total : <b>" + result.TotalCompilationTime.ConvertToHumanTime() + @"</b></div>");

            if (nbCompilationError > 0)
                currentReport.Append("<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>Nombre de fichiers avec erreur(s) de compilation : " + nbCompilationError + "</div>");
            if (nbCompilationWarning > 0)
                currentReport.Append("<div><img style='padding-right: 20px;' src='Warning_25x25' height='15px'>Nombre de fichiers avec avertissement(s) de compilation : " + nbCompilationWarning + "</div>");
            if (result.ListFilesToCompile.Count - nbCompilationError - nbCompilationWarning > 0)
                currentReport.Append("<div><img style='padding-right: 20px;' src='Ok_25x25' height='15px'>Nombre de fichiers compilés correctement : " + (result.ListFilesToCompile.Count - nbCompilationError - nbCompilationWarning) + "</div>");

            // deploy
            currentReport.Append(@"<div style='padding-top: 7px; padding-bottom: 7px;'>Nombre de fichiers déployés : <b>" + totalDeployedFiles + "</b>, répartition : " + GetNbFilesPerType(result.FilesToDeployPerStep.SelectMany(pair => pair.Value).Select(deploy => deploy.To).ToList()).Aggregate("", (current, kpv) => current + (@"<img style='padding-right: 5px;' src='" + Helper.GetExtensionImage(kpv.Key.ToString(), true) + "' height='15px'><span style='padding-right: 12px;'>x" + kpv.Value + "</span>")) + "</div>");

            // deployment time
            currentReport.Append(@"<div><img style='padding-right: 20px;' src='Clock_15px' height='15px'>Temps de déploiement total : <b>" + result.TotalDeploymentTime.ConvertToHumanTime() + @"</b></div>");

            if (nbDeploymentError > 0)
                currentReport.Append("<div><img style='padding-right: 20px;' src='Error_25x25' height='15px'>Nombre de fichiers avec erreur(s) de déploiement : " + nbDeploymentError + "</div>");
            if (totalDeployedFiles - nbDeploymentError > 0)
                currentReport.Append("<div><img style='padding-right: 20px;' src='Ok_25x25' height='15px'>Nombre de fichiers déployés correctement : " + (totalDeployedFiles - nbDeploymentError) + "</div>");

            // compilation
            if (listLinesCompilation.Count > 0) {
                currentReport.Append("<h3>Détails des erreurs/avertissements de compilation :</h3>");
                var boolAlternate = false;
                foreach (var listLine in listLinesCompilation.OrderByDescending(tuple => tuple.Item1)) {
                    currentReport.Append(listLine.Item2.Replace("%ALTERNATE%", boolAlternate ? "class='AlternatBackColor' " : "class='NormalBackColor' "));
                    boolAlternate = !boolAlternate;
                }
            }

            // deployment steps
            foreach (var listLinesKpv in listLinesByStep.Where(pair => pair.Value != null && pair.Value.Count > 0)) {
                currentReport.Append("<h3>Détails sur l'étape " + listLinesKpv.Key + " du déploiement :</h3>");
                var boolAlternate2 = false;
                foreach (var listLine in listLinesKpv.Value.OrderByDescending(tuple => tuple.Item1)) {
                    currentReport.Append(listLine.Item2.Replace("%ALTERNATE%", boolAlternate2 ? "class='AlternatBackColor' " : "class='NormalBackColor' "));
                    boolAlternate2 = !boolAlternate2;
                }
            }

            currentReport.Append(@"</div>");

            if (result is ResultDeploymentDifferential resultDifferential) {
                currentReport.AppendLine(@"             
                <h2>Informations complémentaires sur le listing :</h2>
                <div class='IndentDiv'>");
                var deployedFiles = result.DeployedFiles.Select(deployed => deployed.SourcePath).ToList();
                deployedFiles.AddRange(result.DeployedFiles.Where(deployed => deployed is FileDeployedCompiled).Cast<FileDeployedCompiled>().SelectMany(deployed => deployed.RequiredFiles).Select(info => info.SourcePath));
                var sourceFilesNotDeployed = resultDifferential.SourceFiles.Select(pair => pair.Key.Replace(config.Env.SourceDirectory.CorrectDirPath(), "")).Where(sourceFilePath => !deployedFiles.Exists(path => path.Equals(sourceFilePath))).ToList();
                currentReport.AppendLine(@"             
                    <div>Nombre de nouveaux fichiers à (potentiellement) déployer : <b>" + resultDifferential.SourceFiles.Count(f => f.Value.FileState == SourceFileState.New) + @"</b></div>
                    <div>Nombre de fichiers identiques au dernier déploiement : <b>" + resultDifferential.SourceFiles.Count(f => f.Value.FileState == SourceFileState.UpToDate) + @"</b></div> 
                    <div>Nombre de fichiers manquants par rapport au dernier déploiement : <b>" + resultDifferential.SourceFiles.Count(f => f.Value.FileState == SourceFileState.Missing) + @"</b></div>
                    <div>Nombre de fichiers source non utilisés pour ce déploiement : <b>" + sourceFilesNotDeployed.Count + @"</b></div>
            ");
                if (sourceFilesNotDeployed.Count > 0) {
                    currentReport.AppendLine(@"<h3>Liste des fichiers non utilisés pour ce déploiement</h3>");
                    currentReport.AppendLine(@"<div class='IndentDiv'>");
                    foreach (var undeployedFile in sourceFilesNotDeployed) {
                        currentReport.AppendLine(@"<div><img height='15px' src='" + Helper.GetExtensionImage((Path.GetExtension(undeployedFile) ?? "").Replace(".", "")) + "'>" + Path.Combine(config.Env.SourceDirectory, undeployedFile).ToHtmlLink(undeployedFile, true) + @"</div>");
                    }
                    currentReport.AppendLine(@"</div>");
                }
                currentReport.AppendLine(@"</div>");
            }

            return currentReport.ToString();
        }

        /// <summary>
        /// Allows to format a small text to explain the errors found in a file and the generated files...
        /// </summary>
        private static string FormatCompilationResultForSingleFile(string sourceFilePath, FileToCompile fileToCompile, List<FileToDeploy> listDeployedFiles) {
            var line = new StringBuilder();

            line.Append("<div style='padding-bottom: 5px;'>");
            line.Append("<img height='15px' src='" + Helper.GetExtensionImage((Path.GetExtension(sourceFilePath) ?? "").Replace(".", "")) + "'>");
            line.Append("<b>" + sourceFilePath.ToHtmlLink(Path.GetFileName(sourceFilePath), true) + "</b> in " + Path.GetDirectoryName(sourceFilePath).ToHtmlLink());
            line.Append("</div>");

            if (fileToCompile != null && fileToCompile.Errors != null) {
                line.Append("<div style='padding-left: 10px; padding-bottom: 5px;'>");
                foreach (var error in fileToCompile.Errors) {
                    line.Append(HtmlFileError.Description(error));
                }
                line.Append("</div>");
            }

            if (listDeployedFiles != null) {
                line.Append("<div>");
                // group either by directory name or by pack name
                var groupDirectory = listDeployedFiles.GroupBy(deploy => deploy.GroupKey).Select(deploys => deploys.ToList()).ToList();
                foreach (var group in groupDirectory.OrderByDescending(list => list.First().DeployType).ThenBy(list => list.First().GroupKey)) {
                    line.Append(HtmlFileToDeploy.GroupHeader(group.First()));
                    foreach (var fileToDeploy in group.OrderBy(deploy => deploy.To)) {
                        line.Append(HtmlFileToDeploy.Description(fileToDeploy));
                    }
                }
                line.Append("</div>");
            }

            return line.ToString();
        }
        
        /// <summary>
        /// Allows to know how many files of each file type there is
        /// </summary>
        private static Dictionary<FileExt, int> GetNbFilesPerType(List<string> files) {
            Dictionary<FileExt, int> output = new Dictionary<FileExt, int>();

            foreach (var file in files) {
                FileExt fileExt;
                if (!Enum.TryParse((Path.GetExtension(file) ?? "").Replace(".", ""), true, out fileExt))
                    fileExt = FileExt.Unknow;
                if (output.ContainsKey(fileExt))
                    output[fileExt]++;
                else
                    output.Add(fileExt, 1);
            }

            return output;
        }
    }
    
}
