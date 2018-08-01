using System;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.ShellProgressBar;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        Description = "TODO : deploy help text",
        ExtendedHelpText = "TODO : help",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class DeployCommand : OeBaseCommand {
        
        [Argument(0)]
        [LegalFilePath]
        public string[] Files { get; set; }
        
        [Option("--git-dir")]
        [DirectoryExists]
        public string GitDir { get; set; }

        // You can use this pattern when the parent command may have options or methods you want to
        // use from sub-commands.
        // This will automatically be set before OnExecute is invoked
        private MainCommand Parent { get; set; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            CreateHashFiles();
            //return base.OnExecute(app, console);
            return 1;
        }
    
        private static void CreateHashFiles() {
            const int totalTicks = 10;
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false,
                EnableTaskBarProgress = true
            };
            var childOptions = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Magenta,
                BackgroundColor = ConsoleColor.DarkMagenta,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false,
                CollapseWhenFinished = false,
                ForegroundColorDone = ConsoleColor.DarkGreen,
            };
            using (var pbar = new ProgressBar(totalTicks, "main progressbar", options))
            {
                TickToCompletion(pbar, totalTicks, sleep: 10, childAction: () =>
                {
                    using (var child = pbar.Spawn(totalTicks, "child actions", childOptions))
                    {
                        TickToCompletion(child, totalTicks, sleep: 100);
                    }
                });
            }
        }
        
        private static void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action childAction = null)
        {
            var initialMessage = pbar.Message;
            for (var i = 0; i < ticks; i++) {
                pbar.Message = $"Start {i + 1} of {ticks}: {initialMessage}";
                childAction?.Invoke();
                Thread.Sleep(sleep);
                pbar.Tick($"End {i + 1} of {ticks}: {initialMessage}");
            }
        }

    }
}