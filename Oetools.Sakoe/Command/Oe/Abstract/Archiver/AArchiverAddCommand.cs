#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (AArchiverBaseAddSubCommand.cs) is part of Oetools.Sakoe.
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

using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Project.Properties;
using Oetools.Builder.Project.Task;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe.Abstract.Archiver {
    internal abstract class AArchiverAddCommand : ABaseExecutionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<archive>", "The path to the targeted archive(s). Several archives can be targeted, separate them with a semi-colon (i.e. ;).")]
        public string TargetArchivePath { get; set; }

        [Option("-d|--directory <path>", "The relative target directory inside the archive. Defaults to blank which will put files in the archive root directory.", CommandOptionType.MultipleValue)]
        public string[] TargetDirectory { get; set; }

        [Option("-f|--file <path>", "The relative target file path inside the archive.", CommandOptionType.MultipleValue)]
        public string[] TargetFilePath { get; set; }

        [Required]
        [Argument(1, "<path>", @"A path pattern that indicates files that should be processed. Specify several paths by separating them with a semi-colon (i.e. ;).")]
        public string WildcardsPathToAdd { get; set; }

        [Option("-e|--exclude <filter>", "A path pattern that indicates paths that should be excluded from being processed. Can use wildcards (**,*,?).", CommandOptionType.MultipleValue)]
        public string[] ExcludeFilter { get; set; }

        [Option("-er|--exclude-regex <filter>", "A regular expression path pattern that indicates files that should be excluded from being processed.", CommandOptionType.MultipleValue)]
        public string[] ExcludeRegexFilter { get; set; }

        public abstract AOeTaskFileArchiverArchive GetArchiverTask();

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var archiver = GetArchiverTask();

            archiver.TargetArchivePath = TargetArchivePath == null ? null : string.Join(";", TargetArchivePath);
            archiver.Include = WildcardsPathToAdd;

            archiver.TargetDirectory = TargetDirectory == null ? null : string.Join(";", TargetDirectory);
            archiver.TargetFilePath = TargetFilePath == null ? null : string.Join(";", TargetFilePath);
            if (TargetFilePath == null && TargetDirectory == null) {
                archiver.TargetDirectory = "";
            }
            archiver.Exclude = ExcludeFilter == null ? null : string.Join(";", ExcludeFilter);
            archiver.ExcludeRegex = ExcludeRegexFilter == null ? null : string.Join(";", ExcludeRegexFilter);

            archiver.SetLog(GetLogger());
            archiver.SetCancelToken(CancelToken);
            archiver.SetTargetBaseDirectory(Directory.GetCurrentDirectory());

            archiver.Execute();

            Log.Info($"A total of {archiver.GetBuiltFiles().Count} files were added.");

            return 0;
        }
    }
}
