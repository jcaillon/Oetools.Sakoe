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

using System.Collections.Generic;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project.Task;
using Oetools.Builder.Utilities;

namespace Oetools.Sakoe.Command.Oe {
    
    public abstract class FileListBaseCommand : BaseCommand {
        
        [FileExists]
        [Option("-f|--file <path>", "File that should be added to the listing. Can be used multiple times.", CommandOptionType.MultipleValue)]
        public string[] Files { get; protected set; }
        
        [DirectoryExists]
        [Option("-d|--directory <path>", "Directory containing files that should be added to the listing. Can be used multiple times.", CommandOptionType.MultipleValue)]
        public string[] Directories { get; protected set; }
        
        [Option("-r|--recursive", "Recursive listing in the directories.", CommandOptionType.NoValue)]
        public bool RecursiveListing { get; }
        
        [Option("-i|--include <filter>", "Include filter for directory listing. Can use wildcards (**,*,?). See " + ManCommand.Name + " " + FiltersHelpCommand.Name + ".", CommandOptionType.SingleValue)]
        public string IncludeFilter { get; }
        
        [Option("-e|--exclude <filter>", "Exclude filter for directory listing. Can use wildcards (**,*,?). See " + ManCommand.Name + " " + FiltersHelpCommand.Name + ".", CommandOptionType.SingleValue)]
        public string ExcludeFilter { get; }
        
        [Option("-ir|--include-regex <filter>", "Regular expression include filter for directory listing. See " + ManCommand.Name + " " + FiltersHelpCommand.Name + ".", CommandOptionType.SingleValue)]
        public string IncludeRegexFilter { get; }
        
        [Option("-er|--exclude-regex <filter>", "Regular expression include filter for directory listing. See " + ManCommand.Name + " " + FiltersHelpCommand.Name + ".", CommandOptionType.SingleValue)]
        public string ExcludeRegexFilter { get; }

        public virtual IEnumerable<string> GetFilesList(CommandLineApplication app) {
            if (Files == null && Directories == null) {
                Log.Debug($"No files or directories in options, adding the current directory : {Directory.GetCurrentDirectory()}");
                Directories = new[] {
                    Directory.GetCurrentDirectory()
                };
            }

            var filter = new OeTaskFilter {
                Include = IncludeFilter,
                Exclude = ExcludeFilter,
                IncludeRegex = IncludeRegexFilter,
                ExcludeRegex = ExcludeRegexFilter
            };
            filter.Validate();
            
            if (Files != null) {
                foreach (var file in Files) {
                    yield return file;
                }
            }
            if (Directories != null) {
                foreach (var directory in Directories) {
                    var lister = new PathLister(directory, CancelToken) {
                        PathFilter = filter, 
                        Log = Log,
                        RecursiveListing = RecursiveListing
                    };
                    foreach (var file in lister.GetFileList()) {
                        yield return file.Path;
                    }
                }
            }
        }
        
    }
}