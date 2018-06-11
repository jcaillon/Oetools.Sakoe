using Oetools.HtmlExport.Lib;
using Oetools.Packager.Core;

namespace Oetools.HtmlExport.Html {

    internal static class HtmlDeployRule {

        public static string Description(string source, int line) {
            return (source + "|" + line).ToHtmlLink("Règle ligne " + line);
        }

        public static string Description(DeployRule rule) {
            return Description(rule.Source, rule.Line);
        }
    }
}
