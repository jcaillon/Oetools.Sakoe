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
        ThrowOnUnexpectedArgument = false,
        ResponseFileHandling = ResponseFileHandling.ParseArgsAsSpaceSeparated
    )]
    [VersionOptionFromMember("-version|--version", MemberName = nameof(GetVersion), Description = "Show version information")]
    [HelpOption("-?|-h|--help", Description = "Show help information", Inherited = true)]
    [Subcommand("deploy", typeof(DeployCommand))]
    [Subcommand("db", typeof(DatabaseCommand))]
    [Subcommand("prolint", typeof(ProlintCommand))]
    [Subcommand("project", typeof(ProjectCommand))]
    [Subcommand("build", typeof(BuildCommand))]
    [Subcommand("package", typeof(PackageCommand))]
    internal class MainCommand : BaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion() => typeof(MainCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}