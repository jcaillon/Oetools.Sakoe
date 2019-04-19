#region header
// ========================================================================
// Copyright (c) 2019 - Julien Caillon (julien.caillon@gmail.com)
// This file (HelpGenerator2.cs) is part of Oetools.Sakoe.
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
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.ConLog;

#if !WINDOWSONLYBUILD
using Oetools.Sakoe.Command.Oe;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;
using System.IO;
#endif

namespace Oetools.Sakoe.Utilities {

    public class HelpGenerator2 : HelpGenerator {
        protected HelpGenerator2(IConsoleOutput console) : base(console) { }

        protected override void GenerateUsage(string thisCommandLine, IReadOnlyList<CommandArgument> visibleArguments, IReadOnlyList<CommandOption> visibleOptions, IReadOnlyList<CommandLineApplication> visibleCommands, PropertyInfo remainingArgs) {
            base.GenerateUsage(thisCommandLine, visibleArguments, visibleOptions, visibleCommands, remainingArgs);

#if !WINDOWSONLYBUILD && !SELFCONTAINEDWITHEXE
            if (!File.Exists(CreateStarterCommand.StartScriptFilePath)) {
                WriteOnNewLine(null);
                WriteTip($"Tip: use the command {typeof(CreateStarterCommand).GetFullCommandLine().PrettyQuote()} to simplify your invocation of sakoe.");
            }
#endif
        }
    }
}
