#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlErrorLevel.cs) is part of Oetools.Sakoe.
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
using System.Xml.Serialization;

namespace Oetools.Sakoe.Serialization.Opp {
    
    /// <summary>
    ///     Describes the error level, the num is also used for MARKERS in scintilla
    ///     and thus must start at 0
    /// </summary>
    [Serializable]
    public enum XmlErrorLevel {
        [XmlEnum("0")]
        NoErrors = 0,
        [XmlEnum("1")] 
        Information,
        [XmlEnum("2")]
        Warning,
        [XmlEnum("3")]
        StrongWarning,
        [XmlEnum("4")]
        Error,
        [XmlEnum("5")]
        Critical
    }
}