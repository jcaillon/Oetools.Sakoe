#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ProUtilitiesCommand.cs) is part of Oetools.Sakoe.
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "progress", "pg",
        Description = "Progress utilities commands."
    )]
    [Subcommand(typeof(ProgressHelpCommand))]
    [Subcommand(typeof(ProgressReadPfCommand))]
    [Subcommand(typeof(ProgressExePathCommand))]
    [Subcommand(typeof(ProgressReadIniCommand))]
    [Subcommand(typeof(ProgressVersionCommand))]
    [Subcommand(typeof(ProgressWsdlDocCommand))]
    [Subcommand(typeof(ProgressGenerateDatasetCommand))]
    internal class ProgressCommand : AExpectSubCommand {
    }

    [Command(
        "read-pf", "rp",
        Description = "Get a single line argument string from a .pf file.",
        ExtendedHelpText = @"- This command will skip unnecessary whitespaces and new lines.
- This command will ignore comment lines starting with #.
- Resolves -pf parameters inside the .pf by reading the content of the files."
    )]
    internal class ProgressReadPfCommand : ABaseCommand {

        [Required]
        [FileExists]
        [Argument(0, "<pf path>", "The file path to the parameter file (.pf) to use.")]
        public string File { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(new UoeProcessArgs().AppendFromPfFilePath(File).ToQuotedArgs());

            return 0;
        }
    }

    [Command(
        "exe-path", "ep",
        Description = "Returns the full path of the progress executable."
    )]
    internal class ProgressExePathCommand : AOeDlcCommand {

        [Option("-c|--char-mode", "Always return the path of the character mode executable (otherwise prowin is returned by default on windows platform).", CommandOptionType.NoValue)]
        public bool CharMode { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(UoeUtilities.GetProExecutableFromDlc(GetDlcPath(), CharMode));

            return 0;
        }
    }

    [Command(
        "read-ini", "ri",
        Description = "Get the PROPATH value found in a .ini file.",
        ExtendedHelpText = @"- This command returns only absolute path.
- Relative path are converted to absolute using the command folder option.
- This command returns only existing directories or .pl files.
- This command expands environment variables like %TEMP% or $DLC."
    )]
    internal class ProgressReadIniCommand : AOeDlcCommand {

        [Required]
        [FileExists]
        [Argument(0, "<ini path>", "The file path to the .ini file to read.")]
        public string File { get; }

        [Option("-d|--directory", "The base directory to use to convert to absolute path. Default to the current directory.", CommandOptionType.SingleValue)]
        public string BaseDirectory { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            if (string.IsNullOrEmpty(BaseDirectory)) {
                BaseDirectory = Directory.GetCurrentDirectory();
                Log.Info($"Using current directory as base directory : {BaseDirectory.PrettyQuote()}.");
            }

            foreach (var path in UoeUtilities.GetProPathFromIniFile(File, BaseDirectory)) {
                Out.WriteResultOnNewLine(path);
            }

            return 0;
        }
    }

    [Command(
        "version", "ve",
        Description = "Get the version found for the Openedge installation."
    )]
    internal class ProgressVersionCommand : AOeDlcCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteResultOnNewLine(UoeUtilities.GetProVersionFromDlc(GetDlcPath()).ToString());

            return 0;
        }
    }

    [Command(
        "wsdl-doc", "wd",
        Description = "Generate an html documentation from a wsdl.",
        ExtendedHelpText = "Use this command to generate an html documentation that guides you on how to use the webservice using openedge ABL language."
    )]
    internal class ProgressWsdlDocCommand : AOeDlcCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<wsdl>", "Path to the wsdl file from which to generate the documentation.")]
        public string WsdlFilePath { get; set; }

        [Option("-o|--output <dir>", "The directory where to generate the documentation. Defaults to a sub folder named as the .wsdl in the current directory.", CommandOptionType.SingleValue)]
        public string OutputDirectory { get; set; }

        [Option("-u|--unattended", "Do not open the html documentation with the default browser after its creation.", CommandOptionType.NoValue)]
        public bool Unattended { get; set; } = false;

        [Option("-b|--bindings", "Force documentation of WSDL bindings.", CommandOptionType.NoValue)]
        public bool Bindings { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var outputDirectory = OutputDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(WsdlFilePath));

            Utils.CreateDirectoryIfNeeded(outputDirectory);

            var output = UoeUtilities.GenerateWsdlDoc(GetDlcPath(), WsdlFilePath.ToAbsolutePath(), outputDirectory, Bindings);
            Log.Debug(output);

            var reportFilePath = Path.Combine(outputDirectory, "index.html");

            if (!File.Exists(reportFilePath)) {
                throw new CommandException($"The documentation was not successfully generated, the following file has not been found: {reportFilePath.PrettyQuote()}.");
            }

            if (!Unattended && !string.IsNullOrEmpty(reportFilePath)) {
                try {
                    Process.Start(reportFilePath);
                } catch (Exception) {
                    //ignored
                }
            }

            return 0;
        }
    }

    [Command(
        "generate-ds", "gd",
        Description = "Generate a dataset definition from a xsd/xml file.",
        ExtendedHelpText = ""
    )]
    internal class ProgressGenerateDatasetCommand : AOeDlcCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<file>", "Path to an .xml or .xsd file from which to generate the dataset definition.")]
        public string InputFilePath { get; set; }

        [Option("-o|--output <file>", "The path to the output include file (.i) that will contain the dataset definition. Defaults to the input file name with the .i extension.", CommandOptionType.SingleValue)]
        public string OutputFile { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            var outputFile = OutputFile ?? Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(InputFilePath)}.i");

            Utils.CreateDirectoryForFileIfNeeded(outputFile);

            string output;
            if (InputFilePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) {
                var temporaryXsd = Path.Combine(Path.GetDirectoryName(outputFile.ToAbsolutePath()), Path.GetTempFileName());
                try {
                    using (XmlReader reader = XmlReader.Create(InputFilePath)) {
                        XmlSchemaInference schema = new XmlSchemaInference();
                        var schemaSet = schema.InferSchema(reader);
                        foreach (XmlSchema s in schemaSet.Schemas()) {
                            using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8)) {
                                using (var writer = XmlWriter.Create(stringWriter)) {
                                    s.Write(writer);
                                }

                                File.WriteAllText(temporaryXsd, stringWriter.ToString());
                            }
                        }
                    }

                    output = UoeUtilities.GenerateDatasetDefinition(GetDlcPath(), temporaryXsd, outputFile);
                } finally {
                    Utils.DeleteFileIfNeeded(temporaryXsd);
                }
            } else {
                output = UoeUtilities.GenerateDatasetDefinition(GetDlcPath(), InputFilePath.ToAbsolutePath(), outputFile);
            }

            Log.Debug(output);

            if (!File.Exists(outputFile)) {
                throw new CommandException($"The dataset definition file (.i) was not successfully generated, the following file has not been found: {outputFile.PrettyQuote()}.");
            }

            return 0;
        }
    }
}
