using System;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe;
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        FullName = "SAKOE - a Swiss Army Knife for OpenEdge",
        Description = "TODO : short description of this CLI",
        ExtendedHelpText = "TODO : extended help for this CLI",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ResponseFileHandling = ResponseFileHandling.ParseArgsAsSpaceSeparated
    )]
    [VersionOptionFromMember("-version|--version", MemberName = nameof(GetVersion), Description = "Show version information")]
    [HelpOption("-?|-h|--help", Description = "Show help information", Inherited = true)]
    [Subcommand(typeof(SelfTestCommand))]
    [Subcommand(typeof(DatabaseCommand))]
    [Subcommand(typeof(LintCommand))]
    [Subcommand( typeof(ProjectCommand))]
    [Subcommand(typeof(BuildCommand))]
    [Subcommand(typeof(HelpCommand))]
    [Subcommand(typeof(XcodeCommand))]
    internal class MainCommand : BaseCommand {
        
        private static string GetVersion() => $"v{typeof(MainCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";

        public static int ExecuteMainCommand(string[] args) {
            var console = PhysicalConsole.Singleton;
            try {
                using (var app = new CommandLineApplicationCustomHint<MainCommand>(HelpTextGenerator, console, Directory.GetCurrentDirectory(), true)) {
                    app.Conventions.UseDefaultConventions();
                    app.ParserSettings.MakeSuggestionsInErrorMessage = true;
                    return app.Execute(args);
                } 
            } catch (CommandParsingException ex) {
                using (var log = new ConsoleLogger(console, ConsoleLogger.LogLvl.Info, true)) {
                    log.Error(ex.Message);
                    log.Fatal($"Exit code {FatalExitCode} - Error");
                    return FatalExitCode;
                }
            }
        }
    }
}