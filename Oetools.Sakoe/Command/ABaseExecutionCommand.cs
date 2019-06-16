#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ABaseCommand.cs) is part of Oetools.Sakoe.
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
using System.IO;
using System.Threading;
using CommandLineUtilsPlus.Command;
using CommandLineUtilsPlus.Console;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib;
using ILogger = Oetools.Builder.Utilities.ILogger;

namespace Oetools.Sakoe.Command {


    public abstract class ABaseExecutionCommand : AExecutionCommand {

        protected ILogger GetLogger() => Log as ILogger;

        protected override void DrawLogo(CommandLineApplication application, IConsoleWriter console) {
            MainCommand.DrawLogo(console);
        }

        protected override string GetLogFilePathDefaultValue() {
            if (Directory.Exists(OeBuilderConstants.GetProjectDirectory(Directory.GetCurrentDirectory()))) {
                return Path.Combine(OeBuilderConstants.GetProjectDirectoryLocal(Directory.GetCurrentDirectory()), "logs", "sakoe.log");
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "sakoe.log");
        }
    }

}
