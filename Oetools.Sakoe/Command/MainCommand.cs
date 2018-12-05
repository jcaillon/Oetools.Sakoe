using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe;
using Oetools.Sakoe.Utilities;
using Oetools.Sakoe.Utilities.Extension;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        Name = "sakoe",
        FullName = "SAKOE - a Swiss Army Knife for OpenEdge",
        Description = "SAKOE is a collection of tools aimed to simplify your work in Openedge environments.",
        ExtendedHelpText = @"The 'sakoe " + ManCommand.Name + @"' command is the best place to learn about this tool.",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ResponseFileHandling = ResponseFileHandling.ParseArgsAsLineSeparated
    )]
    [HelpOption("-?|-h|" + HelpLongName, Description = "Show this help text.", Inherited = true)]
#if DEBUG
    [Subcommand(typeof(SelfTestCommand))]
#endif
    [Subcommand(typeof(ManCommand))]
    [Subcommand(typeof(DatabaseCommand))]
    [Subcommand(typeof(LintCommand))]
    [Subcommand(typeof(ProjectCommand))]
    [Subcommand(typeof(BuildCommand))]
    [Subcommand(typeof(ShowVersionCommand))]
    [Subcommand(typeof(XcodeCommand))]
    [Subcommand(typeof(HashCommand))]
    [Subcommand(typeof(ProHelpCommand))]
    [Subcommand(typeof(ProUtilitiesCommand))]
    [Subcommand(typeof(ProlibCommand))]
#if !WINDOWSONLYBUILD
    [Subcommand(typeof(CreateStarterCommand))]
#endif
    internal class MainCommand : AExpectSubCommand {

        public const string HelpLongName = "--help";
        
        public static int ExecuteMainCommand(string[] args) {
            // TODO: global configuration in an .xml next to sakoe.exe that store default verbosity, log path, http proxy and so on...
            var console = ConsoleImplementation.Singleton;
            try {
                console.CursorVisible = false;
                using (var app = new CommandLineApplicationCustomHint<MainCommand>(HelpGenerator.Singleton, console, Directory.GetCurrentDirectory(), true)) {
                    app.Conventions.UseDefaultConventions();
                    app.ParserSettings.MakeSuggestionsInErrorMessage = true;
                    app.OptionsComparison = StringComparison.CurrentCultureIgnoreCase;
                    return app.Execute(args);
                }
            } catch (Exception ex) {
                var log = ConsoleIo.Singleton;
                log.LogTheshold = ConsoleLogThreshold.Debug;
                
                if (ex is CommandParsingException) {
                    //if (ex is UnrecognizedCommandParsingException unrecognizedCommandParsingException) {
                    //    log.Info($"Did you mean {unrecognizedCommandParsingException.NearestMatch}?");
                    //}
                    var nearestMatch = ex.GetType().GetProperty("NearestMatch")?.GetValue(ex) as string;
                    log.Error(ex.Message);
                    log.If(!string.IsNullOrEmpty(nearestMatch))?.Info($"Did you mean {nearestMatch}?");
                    log.Info($"Specify {HelpLongName} for a list of available options and commands.");
                } else {
                    log.Error(ex.Message, ex);
                }

                log.Fatal($"Exit code {ABaseCommand.FatalExitCode}");
                log.WriteOnNewLine(null);
                return ABaseCommand.FatalExitCode;
            } finally {
                ConsoleIo.Singleton.Dispose();
                console.CursorVisible = true;
            }
        }
    }
}