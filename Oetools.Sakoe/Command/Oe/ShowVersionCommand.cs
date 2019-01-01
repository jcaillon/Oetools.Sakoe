#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ShowVersionCommand.cs) is part of Oetools.Sakoe.
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
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "version", "ve",
        Description = "Show the version information of this tool.",
        ExtendedHelpText = ""
    )]
    internal class ShowVersionCommand : ABaseCommand {

        [Option("-b|--bare-version", "Output the raw assembly version of the tool, without logo and pre-release tag.", CommandOptionType.NoValue)]
        public bool BareVersion { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (BareVersion) {
                Out.WriteResultOnNewLine(RunningAssembly.Info.AssemblyVersion.ToString());
            } else {
                MainCommand.DrawLogo(Out);
            }
            return 0;
        }
    }

}
