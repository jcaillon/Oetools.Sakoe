#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (OeBaseCommand.cs) is part of Oetools.Sakoe.
// 
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.IO;
using System.Linq;
using Oetools.Sakoe.Lib;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    public abstract class OeBaseCommand : BaseCommand {
        
        /// <summary>
        /// Returns the DLC PATH
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string GetDlcPath() {
            var dlcPath = ProUtilities.GetDlcPath();
            if (string.IsNullOrEmpty(dlcPath) || !Directory.Exists(dlcPath)) {
                throw new Exception("DLC folder not found, you must set the environment variable DLC to locate your openedge installation folder");
            }
            return dlcPath;
        }

        /// <summary>
        /// Returns the path of the unique project file in the current directory (if any)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CommandException"></exception>
        protected string GetCurrentProjectFilePath() {
            var list = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"{OeConstants.OeProjectExtension}*", SearchOption.TopDirectoryOnly).ToList();
            if (list.Count == 0) {
                throw new CommandException($"No project file ({OeConstants.OeProjectExtension}) found in the current folder {Directory.GetCurrentDirectory()}");
            }
            if (list.Count > 1) {
                throw new CommandException($"Ambigous project, found {list.Count} project files in the current folder, specify the project file to use in the command line");
            }
            return list.First();
        }
    }
}