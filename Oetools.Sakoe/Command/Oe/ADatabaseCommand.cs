using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    internal abstract class ADatabaseCommand : AOeDlcCommand {

        protected const string DbPathArgumentName = "<db path>";

        [LegalFilePath]
        [Argument(0, "[" + DbPathArgumentName + "]", "Path to the database (" + UoeConstants.DatabaseFileExtension + " file). The " + UoeConstants.DatabaseFileExtension + " extension is optional and the path can be relative. Defaults to the path of the first " + UoeConstants.DatabaseFileExtension + " file found in the current directory.")]
        public string TargetDatabasePath { get; set; }

        protected string GetTargetDatabasePath() {
            var databasePath = TargetDatabasePath?.MakePathAbsolute();
            if (string.IsNullOrEmpty(databasePath)) {
                databasePath = GetFirstDatabaseInCurrentDirectory();
                Log?.Debug($"Using the first {UoeConstants.DatabaseFileExtension} file found in the current directory: {databasePath.PrettyQuote()}.");
            }
            return databasePath;
        }

        public static string GetFirstDatabaseInCurrentDirectory(bool throwWhenNotFound = true) {
            var list = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{UoeConstants.DatabaseFileExtension}", SearchOption.TopDirectoryOnly).ToList();
            if (list.Count == 0) {
                if (!throwWhenNotFound) {
                    return null;
                }
                throw new CommandException($"No database file {UoeConstants.DatabaseFileExtension.PrettyQuote()} found in the current folder: {Directory.GetCurrentDirectory().PrettyQuote()}. Initialize a new database using the command: {typeof(CreateDatabaseCommand).GetFullCommandLine().PrettyQuote()}.");
            }
            if (list.Count > 1) {
                if (!throwWhenNotFound) {
                    return null;
                }
                throw new CommandException($"Ambiguous database, found {list.Count} databases in the current folder, specify the database to use in the command line with the argument {DbPathArgumentName.PrettyQuote()}.");
            }
            return list.First();
        }
    }

    internal abstract class ADatabaseWithProwinCommand : ADatabaseCommand {

        [Description("[-- <connection string>...]")]
        public string[] RemainingArgs { get; set; }

        protected string GetConnectionString(string dlcPath, CommandLineApplication app, bool throwWhenNotFound = true) {
            var prowinParameters = RemainingArgs != null ? string.Join(" ", RemainingArgs) : null;
            if (!string.IsNullOrEmpty(prowinParameters)) {
                Log.Info($"Extra parameters used: {prowinParameters.PrettyQuote()}.");
            }

            var databasePath = TargetDatabasePath;

            if (string.IsNullOrEmpty(databasePath)) {
                databasePath = GetFirstDatabaseInCurrentDirectory(false);
                if (!string.IsNullOrEmpty(databasePath)) {
                    Log?.Debug($"Using the first {UoeConstants.DatabaseFileExtension} file found in the current directory: {databasePath.PrettyQuote()}.");
                } else {
                    var idx = prowinParameters?.IndexOf("-db", StringComparison.OrdinalIgnoreCase) ?? -1;
                    if (idx >= 0) {
                        Log?.Debug("Found a database connection in the extra parameters.");
                    } else if (throwWhenNotFound) {
                        throw new CommandException($"This command expects at least 1 database to be connected, use the argument {DbPathArgumentName.PrettyQuote()} or specify a database connection string using the following syntax: {$"{app.GetFullCommandLine()} -- -db database.db".PrettyQuote()}");
                    }
                }
            }

            if (!string.IsNullOrEmpty(databasePath)) {
                var connectString = new UoeDatabaseOperator(dlcPath) {
                    Log = Log
                }.GetConnectionString(databasePath);
                prowinParameters = $" {connectString} {prowinParameters}";
            }

            return prowinParameters;
        }
    }
}
