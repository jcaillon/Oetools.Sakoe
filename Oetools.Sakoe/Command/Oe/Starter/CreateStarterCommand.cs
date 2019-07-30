#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (CreateStarterCommand.cs) is part of Oetools.Sakoe.
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

using CommandLineUtilsPlus.Command;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
#if !WINDOWSONLYBUILD && !SELFCONTAINEDWITHEXE
using System.IO;

namespace Oetools.Sakoe.Command.Oe.Starter {

    [Command(
        "starter", "st",
        Description = "Create a platform specific starter script for sakoe.",
        ExtendedHelpText = "Allow a more natural way of calling this tool: `sakoe [command]`."
    )]
    internal class CreateStarterCommand : ABaseExecutionCommand {

        public static string StartScriptFilePath {
            get {
                string starterFilePath = null;
                string executableDir = Path.GetDirectoryName(RunningAssembly.Info.Location);
                if (!string.IsNullOrEmpty(executableDir)) {
                    starterFilePath = Path.Combine(executableDir, Utils.IsRuntimeWindowsPlatform ? "sakoe.cmd" : "sakoe");
                }
                return starterFilePath;
            }
        }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            string executableDir = Path.GetDirectoryName(RunningAssembly.Info.Location);
            if (string.IsNullOrEmpty(executableDir)) {
                throw new CommandException($"Could not find the directory of the executing assembly: {RunningAssembly.Info.Location}.");
            }

            var starterFilePath = StartScriptFilePath;
            Log.Debug($"Creating starter script: {starterFilePath.PrettyQuote()}.");

            if (Utils.IsRuntimeWindowsPlatform) {
                File.WriteAllText(starterFilePath, $"@echo off\r\ndotnet exec \"%~dp0{Path.GetFileName(RunningAssembly.Info.Location)}\" %*");
            } else {
                File.WriteAllText(starterFilePath, (@"#!/bin/bash
SOURCE=""${BASH_SOURCE[0]}""
while [ -h ""$SOURCE"" ]; do
    DIR=""$( cd -P ""$( dirname ""$SOURCE"" )"" && pwd )""
    SOURCE=""$(readlink ""$SOURCE"")""
    [[ $SOURCE != /* ]] && SOURCE=""$DIR/$SOURCE""
done
DIR=""$( cd -P ""$( dirname ""$SOURCE"" )"" && pwd )""
dotnet exec ""$DIR/" + Path.GetFileName(RunningAssembly.Info.Location) + @""" ""$@""").Replace("\r", ""));
            }

            Log.Info($"Starter script created: {starterFilePath.PrettyQuote()}.");

            HelpWriter.WriteOnNewLine(null);
            HelpWriter.WriteSectionTitle("IMPORTANT README:");
            HelpWriter.WriteOnNewLine(@"
A starter script has been created in the same directory as this executable: " + starterFilePath.PrettyQuote() + @".

It allows you to call this tool in a more natural way: `sakoe [command]`. This strips the need to run the .dll with dotnet (the script does that for you).

The directory containing the starter script created should be added to your system PATH in order to be able to call `sakoe [command]` from anywhere on your system.

The command to add this directory to your path is:");
            HelpWriter.WriteOnNewLine(null);

            if (Utils.IsRuntimeWindowsPlatform) {
                Out.WriteResultOnNewLine("for /f \"usebackq tokens=2,*\" %A in (`reg query HKCU\\Environment /v PATH`) do set my_user_path=%B && SetX Path \"%my_user_path%;" + Path.GetDirectoryName(starterFilePath) + "\"");
            } else {
                Out.WriteResultOnNewLine("echo $\"export PATH=\\$PATH:" + Path.GetDirectoryName(starterFilePath) + "\" >> ~/.bashrc && source ~/.bashrc && chmod +x \"" + starterFilePath + "\"");
            }

            HelpWriter.WriteOnNewLine(null);

            return 0;
        }
    }
}

#endif
