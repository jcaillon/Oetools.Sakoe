using System;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command.Oe;
using Oetools.Sakoe.ConLog;
using Oetools.Sakoe.Utilities;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Command {
    
    /// <summary>
    /// The main command of the application, called when the user passes no arguments/commands
    /// </summary>
    [Command(
        Name = "sakoe",
        FullName = "SAKOE - a Swiss Army Knife for OpenEdge",
        Description = "SAKOE is a collection of tools aimed to simplify your work in Openedge environments."
    )]
    [HelpOption("-?|-h|" + HelpLongName, Description = "Show this help text.", Inherited = true)]
#if DEBUG
    [Subcommand(typeof(SelfTestCommand))]
#endif
    [Subcommand(typeof(ManCommand))]
    [Subcommand(typeof(UpdateCommand))]
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
    [CommandAdditionalHelpTextAttribute(nameof(GetAdditionalHelpText))]
    internal class MainCommand : AExpectSubCommand {

        public const string HelpLongName = "--help";
        
        public static void GetAdditionalHelpText(IHelpFormatter formatter, CommandLineApplication application, int firstColumnWidth) {
            formatter.WriteOnNewLine(null);
            formatter.WriteSectionTitle("HOW TO");
            formatter.WriteOnNewLine($"Start by reading the manual for this tool: {typeof(ManCommand).GetFullCommandLine()}.");
            formatter.WriteOnNewLine($"Get a full list of commands available: {typeof(ListAllCommandsManCommand).GetFullCommandLine()}.");

        }
        
        public static int ExecuteMainCommand(string[] args) {
            // TODO: global configuration in an .xml next to sakoe.exe that store default verbosity, log path, http proxy and so on...
            var console = ConsoleImplementation2.Singleton;
            try {
                console.CursorVisible = false;
                using (var app = new CommandLineApplicationCustomHint<MainCommand>(HelpGenerator.Singleton, console, Directory.GetCurrentDirectory(), true)) {
                    app.Conventions.UseDefaultConventions();
                    app.Conventions.AddConvention(new CommandCommonOptionsConvention());
                    return app.Execute(args);
                }
            } catch (Exception ex) {
                var log = ConsoleLogger2.Singleton;
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
                ConsoleLogger2.Singleton.Dispose();
                console.CursorVisible = true;
            }
        }
        
        /// <summary>
        /// Draw the logo of this tool.
        /// </summary>
        public static void DrawLogo(IConsoleOutput console) {
            console.WriteOnNewLine(null);
            console.WriteOnNewLine(@"                '`.        ", ConsoleColor.DarkGray);
            console.Write(@"============ ", ConsoleColor.DarkGray);
            console.Write(@"SAKOE", ConsoleColor.Yellow);
            console.Write(@" ============", ConsoleColor.DarkGray);
            console.WriteOnNewLine(@" '`.    ", ConsoleColor.DarkGray);
            console.Write(@".^", ConsoleColor.Gray);
            console.Write(@"      \  \       ", ConsoleColor.DarkGray);
            console.Write(@"A ");
            console.Write(@"S", ConsoleColor.Yellow);
            console.Write(@"wiss ");
            console.Write(@"A", ConsoleColor.Yellow);
            console.Write(@"rmy ");
            console.Write(@"K", ConsoleColor.Yellow);
            console.Write(@"nife for ");
            console.Write(@"O", ConsoleColor.Yellow);
            console.Write(@"pen");
            console.Write(@"E", ConsoleColor.Yellow);
            console.Write(@"dge.");
            console.WriteOnNewLine(@"  \ \", ConsoleColor.DarkGray);
            console.Write(@"  /;/", ConsoleColor.Gray);
            console.Write(@"       \ \\      ", ConsoleColor.DarkGray);
            console.Write("Version ", ConsoleColor.DarkGray);
            console.Write(typeof(HelpGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion, ConsoleColor.Gray);
            console.Write(".", ConsoleColor.DarkGray);
            console.WriteOnNewLine(@"   \ \", ConsoleColor.DarkGray);
            console.Write(@"/", ConsoleColor.Gray);
            console.Write(@"_", ConsoleColor.Red);
            console.Write(@"/", ConsoleColor.Gray);
            console.Write(@"_________", ConsoleColor.Red);
            console.Write(@"\", ConsoleColor.DarkGray);
            console.Write(@"__", ConsoleColor.Red);
            console.Write(@"\     ", ConsoleColor.DarkGray);
            console.Write($"Running with {(Utils.IsNetFrameworkBuild ? ".netframework" : $".netcore-{(Utils.IsRuntimeWindowsPlatform ? "win" : "unix")}")}.", ConsoleColor.DarkGray);
            console.WriteOnNewLine(@"    `", ConsoleColor.DarkGray);
            console.Write(@"/ ", ConsoleColor.Red);
            console.Write(@".           _", ConsoleColor.White);
            console.Write(@"  \    ", ConsoleColor.Red);
            console.Write("Session started on ", ConsoleColor.DarkGray);
            console.Write($"{DateTime.Now:yy-MM-dd} at {DateTime.Now:HH:mm:ss}", ConsoleColor.Gray);
            console.Write(".", ConsoleColor.DarkGray);
            console.WriteOnNewLine(@"     \________________/    ", ConsoleColor.Red);
            console.Write(@"Source code on ", ConsoleColor.DarkGray);
            console.Write(@"github.com/jcaillon", ConsoleColor.Gray);
            console.Write(@".", ConsoleColor.DarkGray);
            console.WriteOnNewLine(null);
        }
    }
}