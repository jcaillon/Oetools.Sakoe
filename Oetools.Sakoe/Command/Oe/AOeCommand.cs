#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (AOeCommand.cs) is part of Oetools.Sakoe.
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
using System.IO;
using System.Linq;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
    public abstract class AOeCommand : ABaseCommand {

        /// <summary>
        /// Returns the path of the unique project file in the current directory (if any)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CommandException"></exception>
        protected string GetCurrentProjectFilePath() {
            var list = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).ToList();
            if (list.Count == 0) {
                var oeDir = OeBuilderConstants.GetProjectDirectory(Directory.GetCurrentDirectory());
                if (Directory.Exists(oeDir)) {
                    list = Directory.EnumerateFiles(oeDir, $"*{OeBuilderConstants.OeProjectExtension}", SearchOption.TopDirectoryOnly).ToList();
                }
                if (list.Count == 0) {
                    throw new CommandException($"No project file {OeBuilderConstants.OeProjectExtension.PrettyQuote()} found in the current folder {Directory.GetCurrentDirectory().PrettyQuote()} nor the {OeBuilderConstants.OeProjectDirectory} directory. Initialize a new project file using the command: {typeof(ProjectInitCommand).GetFullCommandLine().PrettyQuote()}.");
                }

                if (list.Count > 1) {
                    throw new CommandException($"Ambiguous project, found {list.Count} project files in {OeBuilderConstants.OeProjectDirectory.PrettyQuote()}, specify the project file to use in the command line.");
                }
            }

            if (list.Count > 1) {
                throw new CommandException($"Ambiguous project, found {list.Count} project files in the current folder, specify the project file to use in the command line.");
            }
            return list.First();
        }

        /// <summary>
        /// Returns the path of the given project file in the current directory or .oe directory.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CommandException"></exception>
        protected string GetProjectFilePath(string projectFileName) {
            if (Utils.IsPathRooted(projectFileName) && File.Exists(projectFileName)) {
                return projectFileName;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), projectFileName);
            if (File.Exists(path)) {
                return path;
            }
            path = Path.Combine(Directory.GetCurrentDirectory(), $"{projectFileName}{OeBuilderConstants.OeProjectExtension}");
            if (File.Exists(path)) {
                return path;
            }
            path = Path.Combine(Directory.GetCurrentDirectory(), OeBuilderConstants.OeProjectDirectory, projectFileName);
            if (File.Exists(path)) {
                return path;
            }
            path = Path.Combine(Directory.GetCurrentDirectory(), OeBuilderConstants.OeProjectDirectory, $"{projectFileName}{OeBuilderConstants.OeProjectExtension}");
            if (File.Exists(path)) {
                return path;
            }
            throw new CommandException($"No project file ({OeBuilderConstants.OeProjectExtension}) named {projectFileName} found in the current folder {Directory.GetCurrentDirectory().PrettyQuote()} nor the {OeBuilderConstants.OeProjectDirectory} directory.");
        }

    }
}
