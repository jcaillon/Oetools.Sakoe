#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (BaseCommand.cs) is part of Oetools.Sakoe.
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
using System.Reflection;
using System.Runtime.CompilerServices;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// Defines options that will be inherited by all the commands
    /// </summary>
    public abstract class BaseCommand {
        
        private const string VerboseTemplate = "-vb|--verbose";

        protected virtual string UseVerboseMessage => $"Get more details on this error by adding the verbose option : {VerboseTemplate}";
        
        [Option(VerboseTemplate, "Execute the command using a verbose log/error output", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool IsVerbose { get; }
        
        [Option("-nopb|--no-progressbar", "Never display progress bars", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool IsProgressBarOff { get; }
        
        [Option("-logo|--with-logo", "Always display the logo on start", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool IsLogoOn { get; }

        private IConsole Console { get; set; }
        
        protected ILogger Log { get; private set; }
        
        protected virtual void ExecutePreCommand(CommandLineApplication app, IConsole console) { }

        protected virtual void ExecutePostCommand(CommandLineApplication app, IConsole console) { }
        
        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app, IConsole console) {
            using (var logger = new ConsoleLogger(console, IsVerbose ? ConsoleLogger.LogLvl.Debug : ConsoleLogger.LogLvl.Info, IsProgressBarOff)) {
                Console = console;
                Log = logger;
                try {
                    if (IsLogoOn) {
                        DrawLogo(console);
                    }
                    ExecutePreCommand(app, console);
                    var returnCode = ExecuteCommand(app, console);
                    ExecutePostCommand(app, console);
                    if (returnCode.Equals(0)) {
                        Log.Success("OK");
                    } else {
                        Log.Warn($"EXIT CODE {returnCode}");
                    }

                    return returnCode;
                } catch (Exception e) {
                    Log.Error($"**{e.Message}", e);
                    if (!IsVerbose) {
                        Log.Info(UseVerboseMessage);
                    }
                }

                Log.Fatal("ERROR");
                return 9;
            }
        }

        /// <summary>
        /// The method to override for each command
        /// </summary>
        /// <param name="app"></param>
        /// <param name="console"></param>
        /// <returns></returns>
        protected virtual int ExecuteCommand(CommandLineApplication app, IConsole console) {
            app.ShowHelp();
            Log.Warn("You must provide a command");
            return 1;
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

        private void DrawLogo(IConsole console) {
            console.WriteLine($"SAKOE v{typeof(BaseCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion} )xxxxx[;;;;;;;;;>");
        }
        
    }
}