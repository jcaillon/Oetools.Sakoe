#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlFileError.cs) is part of Oetools.Sakoe.
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

namespace Oetools.Sakoe.Config.v1 {

    /// <summary>
    ///     Errors found for this file, either from compilation or from prolint
    /// </summary>
    [Serializable]
    public class XmlFileError {

        /// <summary>
        ///     The path to the file that was compiled to generate this error (you can compile a .p and have the error on a .i)
        /// </summary>
        public string CompiledFilePath { get; set; }

        /// <summary>
        ///     Path of the file in which we found the error
        /// </summary>
        public string SourcePath { get; set; }
        
        public XmlErrorLevel Level { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int ErrorNumber { get; set; }
        public string Message { get; set; }
        public string Help { get; set; }
        public bool FromProlint { get; set; }

        /// <summary>
        ///     indicates if the error appears several times
        /// </summary>
        public int Times { get; set; }
    }
}