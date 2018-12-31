#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (AExpectSubCommand.cs) is part of Oetools.Sakoe.
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
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.ConLog;
using Oetools.Sakoe.Utilities;
using ILogger = Oetools.Builder.Utilities.ILogger;

namespace Oetools.Sakoe.Command {
    public abstract class AExpectSubCommand {

        protected ILogger Log => ConsoleLogger2.Singleton;
        
        protected IConsoleOutput Out => ConsoleLogger2.Singleton;
        
        protected virtual int OnExecute(CommandLineApplication app, IConsole console) {
            if (app.Parent == null) {
                MainCommand.DrawLogo(Out);
            }
            app.ShowHelp();
            Log.Warn(HelpGenerator.GetHelpProvideCommand(app));
            Log.Warn($"Exit code {1}");
            Out.WriteOnNewLine(null);
            return 1;
        }
    }
}