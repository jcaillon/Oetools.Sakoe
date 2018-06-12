using System.IO;
using System.Text;
using Oetools.Packager.Core;

namespace Oetools.HtmlExport.Html {
    
    internal static class HtmlFileToDeploy {

        public static string Description(FileToDeploy toDeploy, string sourceDir = null) {
            var sb = new StringBuilder();
            sb.Append("<div style='padding-left: 10px'>");
            if (toDeploy.IsOk) {
                sb.Append("<img height='15px' src='" + DeployImage(toDeploy) + "'>");
            } else {
                sb.Append("<img height='15px' src='Error_25x25'>Transfert échoué pour ");
            }
            sb.Append("<span style='padding-right: 8px;'>(" + toDeploy.DeployType);
            if (toDeploy.RuleReference != null)
                sb.Append(" " + HtmlDeployRule.Description(toDeploy.RuleReference));
            sb.Append(")</span>");
            sb.Append(DeployText(toDeploy, sourceDir));
            if (!toDeploy.IsOk) {
                sb.Append("<br>");
                sb.Append(toDeploy.DeployError);
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public static string DeployText(FileToDeploy toDeploy, string sourceDir = null) {
            var sb = new StringBuilder();

            if (toDeploy is FileToDeployDelete) {
                return Path.GetDirectoryName(toDeploy.To).ToHtmlLink(toDeploy.To, true);
            }
            if (toDeploy is FileToDeployDeleteFolder) {
                return toDeploy.To.ToHtmlLink(null, true);
            }
            if (toDeploy is FileToDeployDeleteInProlib) {
                sb.Append(toDeploy.To.ToHtmlLink(((FileToDeployInPack)toDeploy).RelativePathInPack));
                sb.Append("<span style='padding-left: 8px; padding-right: 8px;'>dans</span>");
                sb.Append(((FileToDeployInPack)toDeploy).PackPath.ToHtmlLink(null, true));
                return sb.ToString();
            }
            if (toDeploy is FileToDeployCopyFolder) {
                sb.Append("<span style='padding-right: 8px;'>from</span>");
                sb.Append(toDeploy.Origin.ToHtmlLink(null, true));
                return sb.ToString();
            }

            sb.Append(toDeploy.To.ToHtmlLink(toDeploy.To.Replace(toDeploy.GroupKey, "").TrimStart('\\')));
            sb.Append("<span style='padding-left: 8px; padding-right: 8px;'>depuis</span>");
            sb.Append(toDeploy.Origin.ToHtmlLink(!string.IsNullOrEmpty(sourceDir) ? toDeploy.Origin.Replace(sourceDir, "").TrimStart('\\') : Path.GetFileName(toDeploy.Origin), true));
            return sb.ToString();
        }

        /// <summary>
        /// The representation of the group for this item
        /// </summary>
        /// <param name="toDeploy"></param>
        /// <returns></returns>
        public static string GroupHeader(FileToDeploy toDeploy) {
            if (toDeploy is FileToDeployDelete) {
                return "<div style='padding-bottom: 5px;'><img src='Delete_15px' height='15px'><b>Fichiers et dossiers supprimés</b></div>";
            }
            if (toDeploy is FileToDeployDeleteFolder) {
                return "<div style='padding-bottom: 5px;'><img src='Delete_15px' height='15px'><b>Fichiers et dossiers supprimés</b></div>";
            }
            if (toDeploy is FileToDeployDeleteInProlib) {
                return "<div style='padding-bottom: 5px;'><img src='Delete_15px' height='15px'><b>Fichiers supprimés dans les .pl</b></div>";
            }
            if (toDeploy is FileToDeployFtp) {
                return "<div style='padding-bottom: 5px;'><img src='" + Helper.GetExtensionImage("Ftp", true) + "' height='15px'><b>" + toDeploy.GroupKey.ToHtmlLink(null, true) + "</b></div>";
            }
            if (toDeploy is FileToDeployInPack) {
                return "<div style='padding-bottom: 5px;'><img src='" + Helper.GetExtensionImage(((FileToDeployInPack)toDeploy).PackExt.Replace(".", "")) + "' height='15px'><b>" + toDeploy.GroupKey.ToHtmlLink(null, true) + "</b></div>";
            }

            return "<div style='padding-bottom: 5px;'><img src='" + Helper.GetExtensionImage("Folder", true) + "' height='15px'><b>" + toDeploy.GroupKey.ToHtmlLink(null, true) + "</b></div>";
        }

        /// <summary>
        /// The image that should be used for this deployment representation
        /// </summary>
        public static string DeployImage(FileToDeploy toDeploy) {
            if (toDeploy is FileToDeployDelete) {
                return "Delete_15px";
            }
            if (toDeploy is FileToDeployDeleteFolder) {
                return Helper.GetExtensionImage("Folder", true);
            }
            if (toDeploy is FileToDeployDeleteInProlib) {
                return Helper.GetExtensionImage("Pl", true);
            }
            if (toDeploy is FileToDeployFtp) {
                return Helper.GetExtensionImage("Ftp", true);
            }
            if (toDeploy is FileToDeployInPack) {
                return Helper.GetExtensionImage(((FileToDeployInPack) toDeploy).PackExt.Replace(".", ""));
            }
            if (toDeploy is FileToDeployCopyFolder) {
                return Helper.GetExtensionImage("Folder", true);
            }
            return Helper.GetExtensionImage((Path.GetExtension(toDeploy.To) ?? "").Replace(".", ""));
        }

    }
}
