#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ArchiverBaseCommand.cs) is part of Oetools.Sakoe.
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.History;
using Oetools.Builder.Project;
using Oetools.Builder.Project.Task;
using Oetools.Builder.Utilities.Attributes;
using Oetools.Utilities.Archive;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    internal abstract class ArchiverBaseCommand : BaseCommand {
        
        public abstract IArchiver GetArchiver();
        
    }
    
    internal abstract class ArchiverBaseSubCommand : ProcessFileListBaseCommand {
                
        private ArchiverBaseCommand Parent { get; }

    }
    
    internal abstract class ArchiverBaseAddSubCommand : ArchiverBaseSubCommand {
        
        [LegalFilePath]
        [Argument(0, "<archive>", "The archive to process.")]
        public string ArchivePath { get; }
        
        [LegalFilePath]
        [Argument(1, "<file or directory>", "The file to add or a directory with files to add. Defaults to the current directory.")]
        public override string FileOrDirectory { get; }
        
        private ArchiverBaseCommand Parent { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var archivePath = ArchivePath.MakePathAbsolute();

            Log.Info("Listing files to process...");

            var filesList = GetFilesList(app).ToList();
            filesList.RemoveAll(f => f.PathEquals(archivePath));
            
            Log.Info($"Starting the process on {filesList.Count} files.");

            var archiver = Parent.GetArchiver();
            
            /*
            archiver.PackFileSet(filesList.Select(f => new FileInArchive {
                SourcePath = f
            }))
            
            var i = 0;
            var outputList = new List<string>();
            
            foreach (var file in filesList) {
                CancelToken?.ThrowIfCancellationRequested();
                var isEncrypted = xcode.IsFileEncrypted(file);
                if (isEncrypted && !ListDecrypted || !isEncrypted && ListDecrypted) {
                    outputList.Add(file);
                }
                i++;
                Log.ReportProgress(filesList.Count, i, $"Analyzing {file}.");
            }

            foreach (var file in outputList) {
                Out.WriteResultOnNewLine(file);
            }

            Log.Info($"A total of {outputList.Count} files are {(ListDecrypted ? "decrypted" : "encrypted")}.");
            */
            
            return 0;
        }
    }
    
    public class TaskArchive : OeTaskFileTargetArchive {
        
        [XmlAttribute("TargetCabFilePath")]
        [ReplaceVariables(LeaveUnknownUntouched = true)]
        public string TargetCabFilePath { get; set; }
        
        [XmlAttribute(AttributeName = "ArchivesCompressionLevel")]
        public string ArchivesCompressionLevel { get; set; }

        public override OeCompressionLevel GetArchivesCompressionLevel() {
            if (Enum.TryParse(ArchivesCompressionLevel, true, out OeCompressionLevel level)) {
                return level;
            }
            return OeCompressionLevel.None;
        }

        protected override IArchiver GetArchiver() {
            throw new NotImplementedException();
        }

        protected override string GetTargetArchive() => TargetCabFilePath;
        
        protected override string GetTargetArchivePropertyName() => nameof(TargetCabFilePath);

        protected override OeTargetArchive GetNewTargetArchive() => new OeTargetArchiveCab();

        public override void ExecuteForFilesTargetArchives(IEnumerable<IOeFileToBuildTargetArchive> files) => throw new NotImplementedException();
    }
    
    internal class FileInArchive : IFileInArchiveToExtract, IFileInArchiveToDelete, IFileToArchive {
        public string ArchivePath { get; set; }
        public string RelativePathInArchive { get; set; }
        public string ExtractionPath { get; set; }
        public string SourcePath { get; set; }
    }
}