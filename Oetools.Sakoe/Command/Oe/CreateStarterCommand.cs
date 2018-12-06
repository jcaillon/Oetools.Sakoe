using System;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command.Oe {
#if !WINDOWSONLYBUILD
    [Command(
        "starter", "cs",
        Description = "Create a platform specific starter script for sakoe to allow a more natural way of calling this tool: 'sakoe [command]'.",
        ExtendedHelpText = ""
    )]
    internal class CreateStarterCommand : ABaseCommand {

        public static string StartScriptFilePath {
            get {
                string starterFilePath = null;
                string executableDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!string.IsNullOrEmpty(executableDir)) {
                    starterFilePath = Path.Combine(executableDir, Utils.IsRuntimeWindowsPlatform ? "sakoe.cmd" : "sakoe.sh");
                }
                return starterFilePath;
            }
        }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            string executableDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(executableDir)) {
                throw new CommandException($"Could not find the directory of the executing assembly: {Assembly.GetExecutingAssembly().Location}.");
            }
            
            var starterFilePath = StartScriptFilePath;
            Log.Debug($"Creating starter script: {starterFilePath.PrettyQuote()}.");
            
            if (Utils.IsRuntimeWindowsPlatform) {
                File.WriteAllText(starterFilePath, $"@echo off\r\ndotnet exec \"%~dp0{Path.GetFileName(Assembly.GetExecutingAssembly().Location)}\" %*");
            } else {
                File.WriteAllText(starterFilePath, @"#!/bin/bash
SOURCE=""${BASH_SOURCE[0]}""
while [ -h ""$SOURCE"" ]; do
    DIR=""$( cd -P ""$( dirname ""$SOURCE"" )"" && pwd )""
    SOURCE=""$(readlink ""$SOURCE"")""
    [[ $SOURCE != /* ]] && SOURCE=""$DIR/$SOURCE""
done
DIR=""$( cd -P ""$( dirname ""$SOURCE"" )"" && pwd )""
dotnet exec ""$DIR/" + Path.GetFileName(Assembly.GetExecutingAssembly().Location) + @""" ""$@""".Replace("\r", ""));
            }
            
            Log.Info($"Starter script created: {starterFilePath.PrettyQuote()}.");

            HelpFormatter.WriteOnNewLine(null);
            HelpFormatter.WriteSectionTitle("IMPORTANT README:");
            HelpFormatter.WriteOnNewLine(@"
A starter script has been created in the same directory as this executable: " + starterFilePath.PrettyQuote() + @".

It allows you to call this tool in a more natural way: 'sakoe [command]'. This strips the need to run the .dll with dotnet (the script does that for you).

The directory containing the starter script created should be added to your system PATH in order to be able to call 'sakoe [command]' from anywhere on your system.

The command to add this directory to your path is:");
            HelpFormatter.WriteOnNewLine(null);
            
            if (Utils.IsRuntimeWindowsPlatform) {
                Out.WriteResultOnNewLine("for /f \"usebackq tokens=2,*\" %A in (`reg query HKCU\\Environment /v PATH`) do set my_user_path=%B && SetX Path \"%my_user_path%;" + Path.GetDirectoryName(starterFilePath) + "\"");
            } else {
                Out.WriteResultOnNewLine("echo $\"export PATH=\\$PATH:" + Path.GetDirectoryName(starterFilePath) + "\" >> ~/.bashrc && source ~/.bashrc && chmod +x \"" + starterFilePath + "\"");
            }
            
            HelpFormatter.WriteOnNewLine(null);
            
            return 0;
        }
    }
#endif
}