using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe;
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        FullName = "SAKOE - a Swiss Army Knife for OpenEdge",
        Description = "SAKOE is a collection of tools aimed to simplify your work in Openedge environments.",
        ExtendedHelpText = @"Website: 
  https://jcaillon.github.io/Oetools.Sakoe/

Get raw output:
  If you want a raw output for each commands (without display the log lines), you can set the verbosity to ""None"" and use the no progress bars option.
    sakoe [command] -vb None -nop

Exit code:
  The convention followed by this tool is the following.
    0 : used when a command completed successfully, without errors nor warnings.
    1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
    9 : used when a command does not complete and ends up in error.

Response file parsing:
  Instead of using a long command line, you can use a response file that contains each argument/option that should be used.
  Everything that is usually separated by a space in the command line should be separated by a new line in the file.
  You do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.
    sakoe @responsefile.txt
",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ResponseFileHandling = ResponseFileHandling.ParseArgsAsLineSeparated
    )]
    [HelpOption("-?|-h|" + HelpLongName, Description = "Show help information.", Inherited = true)]
#if DEBUG
    [Subcommand(typeof(SelfTestCommand))]
#endif
    [Subcommand(typeof(DatabaseCommand))]
    [Subcommand(typeof(LintCommand))]
    [Subcommand( typeof(ProjectCommand))]
    [Subcommand(typeof(BuildCommand))]
    [Subcommand(typeof(ManCommand))]
    [Subcommand(typeof(ShowVersionCommand))]
    [Subcommand(typeof(XcodeCommand))]
    [Subcommand(typeof(HashCommand))]
    [Subcommand(typeof(ProHelpCommand))]
    [Subcommand(typeof(UtilitiesCommand))]
    [Subcommand(typeof(ProlibCommand))]
    internal class MainCommand : BaseCommand {

        public const string HelpLongName = "--help";
        
        public static int ExecuteMainCommand(string[] args) {
            try {
                using (var app = new CommandLineApplicationCustomHint<MainCommand>(HelpGenerator.Singleton, PhysicalConsole.Singleton, Directory.GetCurrentDirectory(), true)) {
                    app.Conventions.UseDefaultConventions();
                    app.ParserSettings.MakeSuggestionsInErrorMessage = true;
                    return app.Execute(args);
                } 
            } catch (Exception ex) {
                using (var log = new ConsoleIo(PhysicalConsole.Singleton, ConsoleIo.LogLvl.Info, true)) {
                    log.Error(ex.Message, ex);

                    if (ex is CommandParsingException) {
                        //if (ex is UnrecognizedCommandParsingException unrecognizedCommandParsingException) {
                        //    log.Info($"Did you mean {unrecognizedCommandParsingException.NearestMatch}?");
                        //}
                        log.Info($"Specify {HelpLongName} for a list of available options and commands.");
                    }

                    log.Fatal($"Exit code {FatalExitCode}");
                    log.WriteOnNewLine(null);
                    return FatalExitCode;
                }
            }
        }
    }
}