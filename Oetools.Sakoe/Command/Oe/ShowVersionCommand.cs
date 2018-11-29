using System;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Utilities;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "version", "ve",
        Description = "Show the version information of this tool.",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ShowVersionCommand : BaseCommand {
        
        [Option("-b|--bare", "Only output the version, no logo.", CommandOptionType.NoValue)]
        public bool BareVersion { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (BareVersion) {
                Out.WriteResultOnNewLine($"v{typeof(HelpGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
            } else {
                Out.DrawLogo();
            }
            return 0;
        }
    }
    
}