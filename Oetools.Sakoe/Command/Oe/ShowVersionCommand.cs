using System;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Utilities;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command.Oe {

    [Command(
        "showversion", "version", "ve",
        Description = "Show the version information of this tool.",
        ExtendedHelpText = "",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ShowVersionCommand : OeBaseCommand {
        
        [Option("-b|--bare", "Only output the version, no logo.", CommandOptionType.NoValue)]
        public bool BareVersion { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            if (BareVersion) {
                WriteLineOutput($"v{typeof(HelpTextGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
            } else {
                HelpTextGenerator.DrawLogo(console.Out);
            }
            return 0;
        }
    }
    
}