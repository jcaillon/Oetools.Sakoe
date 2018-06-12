#region header
// ========================================================================
// Copyright (c) 2017 - Julien Caillon (julien.caillon@gmail.com)
// This file (ErrorHandler.cs) is part of csdeployer.
// 
// csdeployer is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// csdeployer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with csdeployer. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oetools.Runner.Cli.Core {
    
    internal static class ErrorHandler {
        
        /// <summary>
        /// Log a piece of information
        /// returns false if the error already occurred during the session, true otherwise
        /// </summary>
        public static void LogErrors(Exception e, string message = null) {
            if (e == null)
                return;

            var toAppend = new StringBuilder();
            try {
                var info = GetExceptionInfo(e);

                if (message != null)
                    info.Message = message + " : " + info.Message;

                CsDeployerInterface.Instance.RaisedException = (string.IsNullOrEmpty(CsDeployerInterface.Instance.RaisedException) ? "" : CsDeployerInterface.Instance.RaisedException + "<br>") + info.Message;

                // write in the log
                toAppend.AppendLine("============================================================");
                toAppend.AppendLine("WHAT : " + info.Message);
                toAppend.AppendLine("WHEN : " + DateTime.Now.ToString(CultureInfo.CurrentCulture));
                toAppend.AppendLine("WHERE : " + info.OriginMethod + ", line " + info.OriginLine);
                toAppend.AppendLine("DETAILS : ");
                foreach (var line in info.FullException.Split('\n')) {
                    toAppend.AppendLine("    " + line.Trim());
                }
                toAppend.AppendLine("");
                toAppend.AppendLine("");
                File.AppendAllText(CsDeployerInterface.Instance.ErrorLogFilePath, toAppend.ToString(), Encoding.Default);
            } catch (Exception) {
                MessageBox.Show("Impossible d'écrire dans le fichier log :\r\n" + (CsDeployerInterface.Instance.ErrorLogFilePath ?? "null") + "\r\n\r\nL'erreur était :\r\n" + toAppend, "Une erreur est survenue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Returns info on an exception 
        /// </summary>
        private static ExceptionInfo GetExceptionInfo(Exception e) {
            ExceptionInfo output = null;
            var frame = new StackTrace(e, true).GetFrame(0);
            if (frame != null) {
                var method = frame.GetMethod();
                output = new ExceptionInfo {
                    OriginMethod = (method != null ? (method.DeclaringType != null ? method.DeclaringType.ToString() : "?") + "." + method.Name : "?") + "()",
                    OriginLine = frame.GetFileLineNumber(),
                    OriginVersion = AssemblyInfo.Version,
                    Message = e.Message,
                    FullException = e.ToString()
                };
            }
            if (output == null)
                output = new ExceptionInfo {
                    OriginMethod = Utils.CalculateMd5Hash(e.Message),
                    OriginVersion = AssemblyInfo.Version,
                    Message = e.Message,
                    FullException = e.ToString()
                };
            return output;
        }

        #region global error handler callbacks

        public static void UnhandledErrorHandler(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
                LogErrors(ex, "Erreur non gérée");
        }

        public static void ThreadErrorHandler(object sender, ThreadExceptionEventArgs e) {
            LogErrors(e.Exception, "Erreur dans un thread");
        }

        public static void UnobservedErrorHandler(object sender, UnobservedTaskExceptionEventArgs e) {
            LogErrors(e.Exception, "Erreur non observée");
        }

        #endregion
    }

    #region ExceptionInfo

    /// <summary>
    /// Represents an exception
    /// </summary>
    internal class ExceptionInfo {
        public string OriginVersion { get; set; }
        public string OriginMethod { get; set; }
        public int OriginLine { get; set; }
        public string ReceptionTime { get; set; }
        public string Message { get; set; }
        public string FullException { get; set; }
    }

    #endregion
}