#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (IRunConfiguration.cs) is part of Oetools.HtmlExport.
// 
// Oetools.HtmlExport is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.HtmlExport is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.HtmlExport. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

namespace Oetools.HtmlExport.Config.Interface {
    
    public interface IRunConfiguration {
       
        /// <summary>
        ///     Path to the report directory, in which the html will be exported
        /// </summary>
        string OutPathReportDir { get; set; }

        /// <summary>
        ///     Path to the output xml result that can later be used in PreviousDeploymentFiles
        /// </summary>
        string OutPathDeploymentResults { get; set; }

        /// <summary>
        ///     Path to the error log file to use
        /// </summary>
        string ErrorLogFilePath { get; set; }
        
        string RaisedException { get; set; }
        
    }
}