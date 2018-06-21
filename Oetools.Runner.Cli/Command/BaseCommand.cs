#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (BaseCommand.cs) is part of Oetools.Runner.Cli.
// 
// Oetools.Runner.Cli is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Runner.Cli is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Runner.Cli. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;

namespace Oetools.Runner.Cli.Command {
    
    /// <summary>
    /// Defines options that will be inherited by all the commands
    /// </summary>
    public abstract class BaseCommand {
        
        private const string VerboseTemplate = "-vb|--verbose";
        
        [Option(VerboseTemplate, "Execute the command using a verbose log/error output", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool IsVerbose { get; }

        protected IConsole Console { get; private set; }
        
        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app, IConsole console) {
            Console = console;
            try {
                var returnCode = ExecuteCommand(app, console);
                if (returnCode.Equals(0)) {
                    WriteSuccess("OK");
                }
                return returnCode;
            } catch (Exception e) {
                console.ForegroundColor = ConsoleColor.Red;
                console.Error.WriteLine("----------------------------");
                console.Error.WriteLine($"**{(IsVerbose ? e.ToString() : e.Message)}");
                console.Error.WriteLine("----------------------------");
                if (!IsVerbose) {
                    console.Error.WriteLine($"Get more details on this error by adding the verbose option : {VerboseTemplate}");
                }
                console.Error.WriteLine("**ERROR");
                console.ResetColor();
            }
            return 1;
        }

        protected virtual int ExecuteCommand(CommandLineApplication app, IConsole console) {
            console.ForegroundColor = ConsoleColor.Red;
            console.Error.WriteLine("**Command not implemented!");
            console.ResetColor();
            app.ShowHint();
            return 1;
        }

        protected void WriteInformationHeader(string message) {
            if (!IsVerbose) {
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        protected void WriteInformationBody(string message) {
            if (!IsVerbose) {
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        protected void WriteWarning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        protected void WriteError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"**{message}");
            Console.ResetColor();
        }

        protected void WriteSuccess(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        protected string GetDlcPath() {
            var dlcPath = ProUtilities.GetDlcPath();
            if (string.IsNullOrEmpty(dlcPath) || !Directory.Exists(dlcPath)) {
                throw new Exception("DLC folder not found, you must set the environment variable DLC to locate your openedge installation folder");
            }
            return dlcPath;
        }

        protected bool TryGetEnumValue<T>(string stringValue, out long enumValue, out List<string> validValuesList) {
            bool found = false;
            var outValidValuesList = new List<string>();
            long outEnumvalue = 0;
            typeof(T).ForEach<T>((name, val) => {
                if (name.Equals(stringValue)) {
                    outEnumvalue = val;
                    found = true;
                }
                outValidValuesList.Add(name);
            });
            enumValue = outEnumvalue;
            validValuesList = outValidValuesList;
            return found;
        }
        
    }
}