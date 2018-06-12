#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (Helper.cs) is part of Oetools.HtmlExport.
// 
// Oetools.HtmlExport is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.HtmlExport is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.HtmlExport. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.IO;
using System.Text;
using csdeployer.Lib;
using Oetools.HtmlExport.Lib;
using Oetools.Utilities.Lib;

namespace Oetools.HtmlExport.Html {
    public static class Helper {
        
        /// <summary>
        /// Returns the html link representation from a url
        /// </summary>
        /// <returns></returns>
        public static string ToHtmlLink(this string url, string urlName = null, bool accentuate = false) {
            try {
                if (File.Exists(url) || Directory.Exists(url)) {
                    var splitName = (urlName ?? url).Split('\\');
                    if (urlName == null || splitName.Length > 0) {
                        var splitUrl = url.Split('\\');
                        var output = new StringBuilder();
                        var path = new StringBuilder();
                        var j = 0;
                        for (int i = 0; i < splitUrl.Length; i++) {
                            path.Append(splitUrl[i]);
                            if (splitUrl[i].EqualsCi(splitName[j])) {
                                output.Append(string.Format("<a {3}href='{0}'>{1}</a>{2}", path, splitUrl[i], i < splitUrl.Length - 1 ? "<span class='linkSeparator'>\\</span>" : "", i == splitUrl.Length - 1 && accentuate ? "class='SubTextColor' " : ""));
                                j++;
                            }
                            path.Append("\\");
                        }
                        for (int i = splitUrl.Length; i < splitName.Length; i++) {
                            output.Append("<span class='linkSeparator'>\\</span>");
                            output.Append(splitName[i]);
                        }
                        if (output.Length > 0)
                            return output.ToString();
                    }
                }
            } catch (Exception) {
                // ignored invalid char path
            }
            return string.Format("<a {2}href='{0}'>{1}</a>", url, urlName ?? url, accentuate ? "class='SubTextColor' " : "");
        }
        
                
        /// <summary>
        /// Returns the name of the image to use for a particular extension
        /// </summary>
        public static string GetExtensionImage(string ext, bool exist = false) {
            if (exist)
                return "Ext" + ext;
            FileExt fileExt;
            if (!Enum.TryParse(ext, true, out fileExt))
                fileExt = FileExt.Unknow;
            return "Ext" + fileExt;
        }
    }
}