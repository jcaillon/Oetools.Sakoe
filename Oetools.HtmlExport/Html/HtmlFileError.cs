using System.IO;
using System.Text;
using Oetools.HtmlExport.Lib;
using Oetools.Builder.Core;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.HtmlExport.Html {

    internal static class HtmlFileError {

        public static string Description(CompilationError error) {
            var sb = new StringBuilder();
            sb.Append("<div>");
            sb.Append("<img height='15px' src='");
            sb.Append(error.Level == CompilationErrorLevel.Warning ? "Warning_25x25" : "Error_25x25");
            sb.Append("'>");
            if (!error.CompiledFilePath.Equals(error.SourcePath)) {
                sb.Append("in ");
                sb.Append(Extensions.ToHtmlLink(error.SourcePath, Path.GetFileName(error.SourcePath)));
                sb.Append(", ");
            }
            sb.Append(Extensions.ToHtmlLink((error.SourcePath + "|" + error.Line), "Ligne " + (error.Line + 1)));
            sb.Append(" (erreur n°" + error.ErrorNumber + ")");
            if (error.Times > 0) {
                sb.Append(" (x" + error.Times + ")");
            }
            sb.Append(" " + System.Security.SecurityElement.Escape(error.Message));
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
