using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {

    internal abstract class ADatabaseCommand : AOeDlcCommand {

        protected const string OptionDatabaseFileShortName = "-f";
        protected const string OptionDatabaseConnectionShortName = "-c";

        [Option("-cp|--cp-params", "Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil).", CommandOptionType.SingleValue)]
        public string InternationalizationStartupParameters { get; set; }

        protected virtual IEnumerable<string> DatabasePaths { get; }

        protected virtual IEnumerable<string> DatabaseConnections { get; }

        protected virtual IEnumerable<UoeDatabaseLocation> GetDatabaseLocations(bool addDatabasesInCurrentDirectory, bool throwWhenNoDatabaseFound) {
            var nbYield = 0;
            foreach (var path in DatabasePaths) {
                nbYield++;
                yield return new UoeDatabaseLocation(path);
            }
            if (!addDatabasesInCurrentDirectory && throwWhenNoDatabaseFound) {
                throw new CommandException($"You must specify a database path by using the option {OptionDatabaseFileShortName.PrettyQuote()}.");
            }
            if (nbYield > 0 || !addDatabasesInCurrentDirectory) {
                yield break;
            }
            foreach (var databaseLocation in GetDatabaseLocationsFromCd(throwWhenNoDatabaseFound)) {
                yield return databaseLocation;
            }
        }

        protected virtual IEnumerable<UoeDatabaseLocation> GetDatabaseLocationsFromCd(bool throwWhenNoDatabaseFound) {
            var list = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{UoeDatabaseLocation.Extension}", SearchOption.TopDirectoryOnly).ToList();
            if (list.Count == 0) {
                if (!throwWhenNoDatabaseFound) {
                    yield break;
                }
                throw new CommandException($"No database file {UoeDatabaseLocation.Extension.PrettyQuote()} found in the current folder: {Directory.GetCurrentDirectory().PrettyQuote()}. Initialize a new database using the command: {typeof(CreateDatabaseCommand).GetFullCommandLine().PrettyQuote()}.");
            }
            foreach (var path in list) {
                var databaseLocation = new UoeDatabaseLocation(path);
                Log?.Debug($"Adding new database location from current directory: {databaseLocation.ToString().PrettyQuote()}.");
                yield return databaseLocation;
            }
        }

        protected virtual UoeDatabaseLocation GetSingleDatabaseLocation(bool addDatabasesInCurrentDirectory) {
            var list = GetDatabaseLocations(addDatabasesInCurrentDirectory, true).ToList();
            if (list.Count > 1) {
                throw new CommandException($"Ambiguous database, found {list.Count} databases in the current folder, specify the database to use using the option {OptionDatabaseFileShortName.PrettyQuote()}.");
            }
            Log?.Debug($"Using the first {UoeDatabaseLocation.Extension} file found in the current directory: {list.First().ToString().PrettyQuote()}.");
            return list.First();
        }

        protected virtual IEnumerable<UoeDatabaseConnection> GetDatabaseConnections(bool addDatabasesInCurrentDirectory, bool throwWhenNoDatabaseFound) {
            var nbYield = 0;
            foreach (var connection in DatabaseConnections) {
                foreach (var databaseConnection in UoeDatabaseConnection.GetConnectionStrings(connection)) {
                    nbYield++;
                    yield return databaseConnection;
                }
            }
            var ope = GetOperator();
            foreach (var path in DatabasePaths) {
                nbYield++;
                var loc = new UoeDatabaseLocation(path);
                loc.ThrowIfNotExist();
                yield return ope.GetDatabaseConnection(loc);
            }
            if (!addDatabasesInCurrentDirectory && throwWhenNoDatabaseFound) {
                throw new CommandException($"You must specify a database connection string using the option {OptionDatabaseConnectionShortName.PrettyQuote()} or a database path by using the option {OptionDatabaseFileShortName.PrettyQuote()}.");
            }
            if (nbYield > 0 || !addDatabasesInCurrentDirectory) {
                yield break;
            }
            foreach (var databaseLocation in GetDatabaseLocationsFromCd(throwWhenNoDatabaseFound)) {
                databaseLocation.ThrowIfNotExist();
                yield return ope.GetDatabaseConnection(databaseLocation);
            }
        }

        protected virtual UoeDatabaseConnection GetSingleDatabaseConnection(bool addDatabasesInCurrentDirectory) {
            var list = GetDatabaseConnections(false, false).ToList();
            if (list.Count > 1) {
                throw new CommandException($"Ambiguous database, found {list.Count} database connections, you must specify a single connection using either the option {OptionDatabaseConnectionShortName.PrettyQuote()} or the option {OptionDatabaseFileShortName.PrettyQuote()}.");
            }
            if (list.Count == 0) {
                list = GetDatabaseConnections(true, true).ToList();
                if (list.Count > 1) {
                    throw new CommandException($"Ambiguous database, found {list.Count} databases in the current folder, specify a database connection using the option {OptionDatabaseConnectionShortName.PrettyQuote()} or a database path by using the option {OptionDatabaseFileShortName.PrettyQuote()}.");
                }
                Log?.Debug($"Using the first {UoeDatabaseLocation.Extension} file found in the current directory: {list.First().ToString().PrettyQuote()}.");
            }
            return list.First();
        }


        protected Encoding GetOperatorEncoding() => null;

        protected UoeDatabaseOperator GetOperator() =>
            new UoeDatabaseOperator(GetDlcPath(), GetOperatorEncoding()) {
                Log = Log,
                CancelToken = CancelToken,
                InternationalizationStartupParameters = InternationalizationStartupParameters
            };

        protected UoeDatabaseAdministrator GetAdministrator() =>
            new UoeDatabaseAdministrator(GetDlcPath(), GetOperatorEncoding()) {
                Log = Log,
                CancelToken = CancelToken,
                InternationalizationStartupParameters = InternationalizationStartupParameters
            };
    }

    internal abstract class ADatabaseSingleLocationCommand : ADatabaseCommand {

        [LegalFilePath]
        [Option(OptionDatabaseFileShortName + "|--file", "Path to a database (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory. Defaults to the path of the unique " + UoeDatabaseLocation.Extension + " file found in the current directory.", CommandOptionType.SingleValue)]
        public string DatabasePath { get; set; }

        protected override IEnumerable<string> DatabasePaths => DatabasePath.Yield();

        protected UoeDatabaseLocation GetSingleDatabaseLocation() {
            return GetSingleDatabaseLocation(true);
        }
    }

    internal abstract class ADatabaseSingleLocationWithAccessArgsCommand : ADatabaseSingleLocationCommand {

        [Option("-a|--access", "Database access/encryption parameters: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`.", CommandOptionType.SingleValue)]
        public string DatabaseAccessStartupParameters { get; set; }
    }

    internal abstract class ADatabaseSingleConnectionCommand : ADatabaseCommand {

        [Option(OptionDatabaseConnectionShortName + "|--connection", "A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement.", CommandOptionType.SingleValue)]
        public string DatabaseConnection { get; set; }

        [LegalFilePath]
        [Option(OptionDatabaseFileShortName + "|--file", "Path to a database (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory. Defaults to the path of the unique " + UoeDatabaseLocation.Extension + " file found in the current directory.", CommandOptionType.SingleValue)]
        public string DatabasePath { get; set; }

        protected override IEnumerable<string> DatabasePaths => DatabasePath.Yield();

        protected override IEnumerable<string> DatabaseConnections => DatabaseConnection.Yield();

        protected UoeDatabaseConnection GetSingleDatabaseConnection() {
            return GetSingleDatabaseConnection(true);
        }
    }

    internal abstract class ADatabaseMultipleConnectionCommand : ADatabaseCommand {

        [Option(OptionDatabaseConnectionShortName + "|--connection", "A connection string that can be used to connect to one or more openedge database. The connection string will be used in a `CONNECT` statement.", CommandOptionType.MultipleValue)]
        public string[] MultipleDatabaseConnection { get; set; }

        [LegalFilePath]
        [Option(OptionDatabaseFileShortName + "|--file", "Path to a database (" + UoeDatabaseLocation.Extension + " file). The " + UoeDatabaseLocation.Extension + " extension is optional and the path can be relative to the current directory. Defaults to a list of path of all the " + UoeDatabaseLocation.Extension + " file found in the current directory.", CommandOptionType.MultipleValue)]
        public string[] MultipleDatabasePath { get; set; }

        protected override IEnumerable<string> DatabasePaths => MultipleDatabasePath;

        protected override IEnumerable<string> DatabaseConnections => MultipleDatabaseConnection;

        protected IEnumerable<UoeDatabaseConnection> GetDatabaseConnections() {
            return GetDatabaseConnections(true, true);
        }
    }

    internal abstract class ADatabaseToolCommand : ADatabaseMultipleConnectionCommand {

        [Description("[-- <extra pro args>...]")]
        public string[] RemainingArgs { get; set; }

        protected abstract string ToolArguments();

        protected virtual string ExecutionWorkingDirectory => null;

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            var dlcPath = GetDlcPath();

            var argsBuilder = new StringBuilder(ToolArguments());

            argsBuilder.Append(' ').Append(UoeDatabaseConnection.GetConnectionString(GetDatabaseConnections()));

            if (UoeUtilities.CanProVersionUseNoSplashParameter(UoeUtilities.GetProVersionFromDlc(dlcPath))) {
                argsBuilder.Append(" -nosplash");
            }

            argsBuilder.Append(' ').Append(GetRemainingArgsAsProArgs(RemainingArgs));

            var args = argsBuilder.Trim().ToString();

            var executable = UoeUtilities.GetProExecutableFromDlc(dlcPath);

            Log?.Debug($"Executing command:\n{executable.ToCliArg()} {args}");

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = executable,
                    Arguments = args,
                    WorkingDirectory = ExecutionWorkingDirectory ?? Directory.GetCurrentDirectory()
                }
            };

            process.Start();

            return 0;
        }
    }

}
