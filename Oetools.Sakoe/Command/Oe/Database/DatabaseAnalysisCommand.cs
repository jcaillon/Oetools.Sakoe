#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (DataDiggerCommand.cs) is part of Oetools.Sakoe.
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Openedge.Execution;

namespace Oetools.Sakoe.Command.Oe.Database {

    [Command(
        "analysis", "an",
        Description = "Regroup commands related to database analysis."
    )]
    [Subcommand(typeof(DatabaseAnalysisAdviceCommand))]
    [Subcommand(typeof(DatabaseAnalysisReportCommand))]
    internal class DatabaseAnalysisCommand : AExpectSubCommand {
    }


    [Command(
        "report", "re",
        Description = @"Displays an analysis report. It is the combination of the output from proutil dbanalys, describe and iostats."
    )]
    internal class DatabaseAnalysisReportCommand : ADatabaseSingleLocationWithAccessArgsCommand {

        [Option("-o|--options <args>", "Extra options for the proutil dbanalys command", CommandOptionType.SingleValue)]
        public string DbAnalysOptions { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var report = GetOperator().GenerateAnalysisReport(GetSingleDatabaseLocation(), false, new UoeProcessArgs().AppendFromQuotedArgs(DatabaseAccessStartupParameters), new UoeProcessArgs().AppendFromQuotedArgs(DbAnalysOptions));

            Log.Info("Database analysis report:");
            Out.WriteOnNewLine(report);

            return 0;
        }
    }

    [Command(
        "advise", "ad",
        Description = "Generates an html report which provides pointers to common database configuration issues.",
        ExtendedHelpText = @"This command uses the Database Advisor which can be found here: http://practicaldba.com/dlmain.html.

The OpenEdge Database Advisor is intended to provide a quick checkup for common database configuration issues. Obviously proper tuning for an application requires much more than any tool can provide, but the advisor should highlight some of the most common low hanging fruit.

For best results you will need a recent database analysis file (proutil -C dbanalys) and you should run this against your production database. A large portion of the suggestions will be based on VST information that will differ greatly between your production and test environments."
    )]
    internal class DatabaseAnalysisAdviceCommand : ADatabaseSingleConnectionCommand {

        [Required]
        [LegalFilePath]
        [Argument(0, "<report-file>", "Path to the output html report file that will contain pointers to common database configuration issues. The extension is optional but it will be changed to .html if it is incorrectly provided.")]
        public string ReportFilePath { get; set; }

        [LegalFilePath]
        [Option("-a|--analysis-file <file>", "The file path to a database analysis output. If empty, the analysis will be carried on automatically if the database is local.", CommandOptionType.SingleValue)]
        public string AnalysisFilePath { get; set; }

        [Option("-u|--unattended", "Do not open the html report with the default browser after its creation.", CommandOptionType.NoValue)]
        public bool Unattended { get; set; } = false;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            using (var ope = GetAdministrator()) {
                ReportFilePath = ReportFilePath.ToAbsolutePath();
                ReportFilePath = !string.IsNullOrEmpty(ReportFilePath) ? Path.ChangeExtension(ReportFilePath, ".html") : null;
                ope.GenerateAdvisorReport(GetSingleDatabaseConnection(), ReportFilePath, AnalysisFilePath);
                if (!Unattended && !string.IsNullOrEmpty(ReportFilePath)) {
                    try {
                        Process.Start(ReportFilePath);
                    } catch (Exception) {
                        //ignored
                    }
                }
            }
            return 0;
        }
    }
}
