using System;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        Description = "============\nTODO : short description of this CLI",
        ExtendedHelpText = "\nTODO : extended help for this CLI\n============",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false,
        ResponseFileHandling = ResponseFileHandling.ParseArgsAsSpaceSeparated
    )]
    [VersionOptionFromMember("-version|--version", MemberName = nameof(GetVersion), Description = "Show version information")]
    [HelpOption("-?|-h|--help", Description = "Show help information", Inherited = true)]
    [Subcommand("deploy", typeof(DeployCommand))]
    [Subcommand("db", typeof(DatabaseCommand))]
    [Subcommand("lint", typeof(LintCommand))]
    [Subcommand("project", typeof(ProjectCommand))]
    [Subcommand("build", typeof(BuildCommand))]
    internal class MainCommand : BaseCommand {
        
        private static string GetVersion() => typeof(MainCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public static int ExecuteMainCommand(string[] args) => CommandLineApplication.Execute<MainCommand>(args);
    }
}