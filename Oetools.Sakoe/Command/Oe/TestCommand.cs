using System;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge.Database;
using ShellProgressBar;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "test", "te",
        Description = "A command to test the behaviour of this tool",
        ExtendedHelpText = "TODO : help",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class TestCommand : OeBaseCommand {
        
        [Argument(0)]
        [LegalFilePath]
        public string[] Files { get; set; }
        
        [Option("--git-dir")]
        [DirectoryExists]
        public string GitDir { get; set; }
        
        [Option("-b|--block")]
        public (bool HasValue, DatabaseBlockSize value) Block { get; set; }

        // You can use this pattern when the parent command may have options or methods you want to
        // use from sub-commands.
        // This will automatically be set before OnExecute is invoked
        private MainCommand Parent { get; set; }


        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            Log.Info(Block.HasValue.ToString());
            Log.Info(Block.value.ToString());
            
            
            Log.Info("info");
            Log.Info($"info : {console.IsOutputRedirected}");
            Log.Debug("this is a debug");
            Log.Warn("this is a war 2");
            Log.Error("this is a err 2");
            Log.Fatal("this is a fat 2");
            Log.Success("this is a ok 2");
            
            Log.Trace?.Write("test write trace");
            
            for (var i = 0; i <= 99; i++) {
                Log.ReportProgress(100, i, $"Executing task {i}");
                Thread.Sleep(10);
            }
            
            Log.Success("cool end");
            
            for (var i = 0; i <= 100; i++) {
                Log.ReportProgress(100, i, $"Executing task {i}");
                Thread.Sleep(10);
            }
            
            Log.Success("end 2");
            
            Log.ReportProgress(50, 1, "Init...");

            Log.Info("info");
            
            Log.ReportProgress(50, 25, "half");
            
            Log.Info("info");

            for (int j = 0; j < 15; j++) {
                for (var i = 0; i <= 100; i++) {
                    _cancelSource.Token.ThrowIfCancellationRequested();
                    Log.ReportProgress(100, i, $"Executing {j} task {i}");
                    Thread.Sleep(10);
                }
                Log.Success($"end of {j}");
            }
            
            //CreateHashFiles();
            //return base.OnExecute(app, console);
            return 1;
        }
    
        private void CreateHashFiles() {
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
        
        private void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action childAction = null)
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