#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (XmlDeployType.cs) is part of Oetools.Builder.
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
using System.Xml.Serialization;

namespace Oetools.Builder.Opp {
    
    /// <summary>
    ///     Types of deploy, used during rules sorting
    /// </summary>
    [Serializable]
    public enum XmlDeployType : byte {
        [XmlEnum("None")]
        None = 0,
        [XmlEnum("Delete")]
        Delete = 1,
        [XmlEnum("DeleteFolder")]
        DeleteFolder = 2,
        [XmlEnum("DeleteInProlib")]
        DeleteInProlib = 10,
        [XmlEnum("Prolib")]
        Prolib = 11,
        [XmlEnum("Zip")]
        Zip = 12,
        [XmlEnum("Cab")]
        Cab = 13,
        [XmlEnum("Ftp")]
        Ftp = 14,
        [XmlEnum("CopyFolder")]
        CopyFolder = 21,
        [XmlEnum("Copy")]
        Copy = 30,
        [XmlEnum("Move")]
        Move = 31
    }
}