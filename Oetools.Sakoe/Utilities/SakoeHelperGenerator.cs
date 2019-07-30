#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (SakoeHelperGenerator.cs) is part of Oetools.Sakoe.
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
using System.Reflection;
using CommandLineUtilsPlus;
using CommandLineUtilsPlus.Console;
using McMaster.Extensions.CommandLineUtils;
#if !WINDOWSONLYBUILD && !SELFCONTAINEDWITHEXE
using System.IO;
using CommandLineUtilsPlus.Extension;
using Oetools.Sakoe.Command;
using Oetools.Sakoe.Command.Oe.Starter;
using Oetools.Utilities.Lib.Extension;
#endif

namespace Oetools.Sakoe.Utilities {

    public class SakoeHelperGenerator : CommandLineHelpGenerator {

        public SakoeHelperGenerator(IConsoleWriter console) : base(console) { }

        protected override void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            base.GenerateUsage(thisCommandLine, visibleArguments, visibleOptions, visibleCommands, remainingArgs);

#if !WINDOWSONLYBUILD && !SELFCONTAINEDWITHEXE
            if (!File.Exists(CreateStarterCommand.StartScriptFilePath)) {
                WriteOnNewLine(null);
                WriteTip($"Tip: use the command {typeof(CreateStarterCommand).GetFullCommandLine<MainCommand>().PrettyQuote()} to simplify your invocation of sakoe.");
            }
#endif
        }
    }
}
