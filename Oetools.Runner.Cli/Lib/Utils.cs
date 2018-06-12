#region header
// ========================================================================
// Copyright (c) 2017 - Julien Caillon (julien.caillon@gmail.com)
// This file (Utils.cs) is part of csdeployer.
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

using System.Collections.Generic;
using System.IO;

namespace Oetools.Runner.Cli.Lib {
    /// <summary>
    /// Class that exposes utility methods
    /// </summary>
    internal static class Utils {
        
        #region File manipulation wrappers

        /// <summary>
        /// Same as Directory.EnumerateDirectories but doesn't list hidden folders
        /// </summary>
        public static IEnumerable<string> EnumerateFolders(string folderPath, string pattern, SearchOption options) {
            var hiddenDirList = new List<string>();
            foreach (var dir in Directory.EnumerateDirectories(folderPath, pattern, options)) {
                if (new DirectoryInfo(dir).Attributes.HasFlag(FileAttributes.Hidden)) {
                    hiddenDirList.Add(dir);
                }
                bool hidden = false;
                foreach (var hiddenDir in hiddenDirList) {
                    if (dir.StartsWith(hiddenDir)) {
                        hidden = true;
                    }
                }
                if (!hidden)
                    yield return dir;
            }
        }

        /// <summary>
        /// Same as Directory.EnumerateFiles but doesn't list files in hidden folders
        /// </summary>
        public static IEnumerable<string> EnumerateFiles(string folderPath, string pattern, SearchOption options) {
            foreach (var file in Directory.EnumerateFiles(folderPath, pattern, SearchOption.TopDirectoryOnly)) {
                yield return file;
            }
            if (options == SearchOption.AllDirectories) {
                foreach (var folder in EnumerateFolders(folderPath, "*", SearchOption.AllDirectories)) {
                    foreach (var file in Directory.EnumerateFiles(folder, pattern, SearchOption.TopDirectoryOnly)) {
                        yield return file;
                    }
                }
            }
        }

        #endregion


    }
}