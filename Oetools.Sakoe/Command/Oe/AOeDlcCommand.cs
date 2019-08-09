#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (AOeDlcCommand.cs) is part of Oetools.Sakoe.
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
using System.IO;
using DotUtilities.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {

    public abstract class AOeDlcCommand : AOeCommand {

        private string _dlcDirectoryPath;

        [DirectoryExists]
        [Option("-dl|--dlc <dir>", "The path to the directory containing the Openedge installation. Will default to the path found in the `" + UoeConstants.OeDlcAlternativeEnvVar + "` or `" + UoeConstants.OeDlcEnvVar + "` environment variable if it exists.", CommandOptionType.SingleValue, Inherited = true)]
        public string DlcDirectoryPath { get; }

        /// <summary>
        /// Returns the DLC PATH
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string GetDlcPath() {
            if (string.IsNullOrEmpty(_dlcDirectoryPath)) {
                if (!string.IsNullOrEmpty(DlcDirectoryPath)) {
                    _dlcDirectoryPath = DlcDirectoryPath;
                } else {
                    _dlcDirectoryPath = UoeUtilities.GetDlcPathFromEnv();
                    if (string.IsNullOrEmpty(_dlcDirectoryPath) || !Directory.Exists(_dlcDirectoryPath)) {
                        throw new Exception($"The path to the Openedge installation directory has not been found. Specify it with the --dlc option. Alternatively, set a {UoeConstants.OeDlcAlternativeEnvVar} (or {UoeConstants.OeDlcEnvVar}) environment variable containing the installation path.");
                    }
                    Log.Debug($"Using the DLC path found in the environment variable: {_dlcDirectoryPath.PrettyQuote()}.");
                }
            }
            return _dlcDirectoryPath;
        }
    }
}
