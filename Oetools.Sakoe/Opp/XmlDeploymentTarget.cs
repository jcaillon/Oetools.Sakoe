#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlDeploymentTarget.cs) is part of Oetools.Builder.
// 
// Oetools.Builder is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Builder is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Builder. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion
using System;

namespace Oetools.Builder.Opp {
    
    [Serializable]
    public class XmlDeploymentTarget {
        /// <summary>
        ///     Relative target path (relative to the target directory)
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        ///     The type of deployment done for this target
        /// </summary>
        public XmlDeployType DeployType { get; set; }

        /// <summary>
        ///     Relative path of the pack in which this file is deployed (if any)
        /// </summary>
        public string TargetPackPath { get; set; }

        /// <summary>
        ///     Relative path within the pack (if any)
        /// </summary>
        public string TargetPathInPack { get; set; }
    }
}