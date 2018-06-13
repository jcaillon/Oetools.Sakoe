using System;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Runner.Cli.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        Description = "TODO : short description of this CLI",
        ExtendedHelpText = "TODO : extended help for this CLI",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    [VersionOptionFromMember("--version|-version", MemberName = nameof(GetVersion), Description = "Show version information")]
    [HelpOption("-?|-h|--help", Description = "Show help information", Inherited = true)]
    [Subcommand("add", typeof(DeployCommand))]
    [Subcommand("commit", typeof(CommitCommand))]
    internal class MainCommand : BaseCommand{
        
        private int OnExecute(CommandLineApplication app, IConsole console) {
            // this shows help even if the --help option isn't specified
            app.ShowRootCommandFullNameAndVersion();
            console.ForegroundColor = ConsoleColor.Red;
            console.Error.WriteLine("errr");
            console.ResetColor();
            app.ShowHint();
            return 1;
        }

        private static string GetVersion() => typeof(MainCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}