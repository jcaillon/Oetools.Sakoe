#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (OeProcessFileListBaseCommand.cs) is part of Oetools.Sakoe.
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe.Abstract {
    
    public abstract class ProcessFileListBaseCommand : FileListBaseCommand {
        
        [LegalFilePath]
        [Argument(0, "<file or directory>", "The file to process or a directory with files to process. Defaults to the current directory.")]
        public virtual string FileOrDirectory { get; }

        public override IEnumerable<string> GetFilesList(CommandLineApplication app) {
            if (!string.IsNullOrEmpty(FileOrDirectory)) {
                
                if (File.Exists(FileOrDirectory)) {
                    Log.Trace?.Write($"Adding file to the list : {FileOrDirectory}");
                    var files = Files?.ToList() ?? new List<string>();
                    files.Add(FileOrDirectory);
                    Files = files.ToArray();
                    
                } else if (Directory.Exists(FileOrDirectory)) {
                    Log.Trace?.Write($"Adding directory to the list : {FileOrDirectory}");
                    var directories = Directories?.ToList() ?? new List<string>();
                    directories.Add(FileOrDirectory);
                    Directories = directories.ToArray();
                    
                } else {
                    throw new CommandValidationException($"The argument <file or directory> is not a valid file nor a valid directory : {FileOrDirectory.PrettyQuote()}");
                }
                
            }
            return base.GetFilesList(app);
        }
    }
}