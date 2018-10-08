using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Utilities.Openedge.Database;
using ShellProgressBar;

namespace Oetools.Sakoe.Command.Oe {
    
    [Command(
        "selftest", "st",
        Description = "A command to test the behaviour of this tool",
        ExtendedHelpText = "sakoe selftest",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    [Subcommand(typeof(ProgressSelfTestCommand))]
    [Subcommand(typeof(LogSelfTestCommand))]
    [Subcommand(typeof(InputSelfTestCommand))]
    [Subcommand(typeof(PromptSelfTestCommand))]
    internal class SelfTestCommand : OeBaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            Log.Info("dummy command");
            
            return 0;
        }

    }
    
    [Command(
        "input", "in",
        Description = "Subcommand that shows the usage of options and arguments",
        ExtendedHelpText = @"sakoe st input boom.txt --attachment first --attachment ""second with spaces""
sakoe st input -b2 s1024",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        AllowArgumentSeparator = true
    )]
    internal class InputSelfTestCommand : OeBaseCommand {
        
        [Argument(0)]
        [LegalFilePath]
        public string File { get; }
        
        [Option("--git-dir")]
        [DirectoryExists]
        public string GitDir { get; }
        
        [Option("-X|--request", Description = "HTTP Method: GET or POST. Defaults to post.")]
        public HttpMethod RequestMethod { get; } = HttpMethod.Post;
        
        //[Required]
        [Option(Description = "Required. The message")]
        public string Message { get; }

        [EmailAddress]
        [Option("--to <EMAIL>", Description = "Required. The recipient.")]
        public string To { get; }
        
        //[FileExists]
        [Option("--attachment <FILE>")]
        public string[] Attachments { get; }
        
        [Option]
        [AllowedValues("low", "normal", "high", IgnoreCase = true)]
        public string Importance { get; } = "normal";
        
        [Option(Description = "The colors should be red or blue")]
        [RedOrBlue]
        public string Color { get; }

        [Option("--max-size <MB>", Description = "The maximum size of the message in MB.")]
        [Range(1, 50)]
        public int? MaxSize { get; }        
        
        [DirectoryExists]
        [Option("-d|--directory", "", CommandOptionType.SingleValue)]
        public string Directories { get; }
        
        /// <summary>
        /// Property types of ValueTuple{bool,T} translate to CommandOptionType.SingleOrNoValue.
        /// Input            | Value
        /// -----------------|--------------------------------
        /// (none)           | (false, default(TraceLevel))
        /// --trace          | (true, TraceLevel.Normal)
        /// --trace:normal   | (true, TraceLevel.Normal)
        /// --trace:verbose  | (true, TraceLevel.Verbose)
        /// </summary>
        [Option]
        public (bool HasValue, DatabaseBlockSize value) Block { get; }

        [Option("-b2|--block2")]
        public DatabaseBlockSize Block2 { get; } = DatabaseBlockSize.S4096;
        
        /// <summary>
        /// Holds the extra params given after --
        /// </summary>
        public string[] RemainingArgs { get; }

        // You can use this pattern when the parent command may have options or methods you want to
        // use from sub-commands.
        // This will automatically be set before OnExecute is invoked
        private SelfTestCommand Parent { get; }
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            Log.Info($"File = {File ?? "null"}");
            Log.Info($"GitDir = {GitDir ?? "null"}");
            Log.Info($"RequestMethod = {RequestMethod}");
            Log.Info($"Message = {Message ?? "null"}");
            Log.Info($"To = {To}");
            if (Attachments != null) {
                foreach (var attachment in Attachments) {
                    Log.Info($"Attachments = {attachment}");
                }
            }
            Log.Info($"Importance = {Importance ?? "null"}");
            Log.Info($"Color = {Color ?? "null"}");
            Log.Info($"MaxSize = {MaxSize?.ToString() ?? "null"}");
            Log.Info($"Directories = {Directories ?? "null"}");
            Log.Info($"Block = ({Block.HasValue} {Block.value})");
            Log.Info($"Block2 = {Block2}");
            if (RemainingArgs != null) {
                foreach (var remainingArg in RemainingArgs) {
                    Log.Info($"RemainingArgs = {remainingArg}");
                }
            }
            Log.Info($"Parent.Verbosity = {Parent.Verbosity}");
            
            return 1;
        }
        
        public enum HttpMethod {
            Get,
            Post
        }
        
        internal class RedOrBlueAttribute : ValidationAttribute {
            public RedOrBlueAttribute() : base("The value for {0} must be 'red' or 'blue'") { }

            protected override ValidationResult IsValid(object value, ValidationContext context) {
                if (value == null || (value is string str && str != "red" && str != "blue")) {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
                return ValidationResult.Success;
            }
        }

    }
    
    [Command(
        "prompt", "prompt",
        Description = "Subcommand that shows the usage of prompt",
        ExtendedHelpText = "sakoe selftest prompt",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class PromptSelfTestCommand : OeBaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            const string prompt = @"Which example would you like to run?
1 - Fake Git
2 - Fake Docker
3 - Fake npm
> ";
            var res = Prompt.GetInt(prompt);

            Log.Info($"= {res}");

            var args = Prompt.GetString("Enter some arguments >");

            foreach (var arg in args) {
                Log.Info($"-> {arg}");
            }
            
            var pwd = Prompt.GetPassword("Password: ");
            
            Log.Info($"password = {pwd}");
            
            return 1;
        }

    }
    
    [Command(
        "log", "l",
        Description = "Subcommand that shows the usage of log",
        ExtendedHelpText = "sakoe selftest log",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class LogSelfTestCommand : OeBaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            Log.ReportGlobalProgress(100, 5, "Logging 5% global progress");
            
            if (Log.Trace != null) {
                Log.Trace?.Write("Log.Trace?.Write");
            } else {
                Log.Info("In debug mode, you can use Log.Trace?.Write");
            }
            
            Log.Debug("Log debug");
            Log.Info("Log info");
            Log.Warn("Log warn");
            Log.Error("Log error");
            Log.Fatal("Log fatal");
            Log.Done("Log success");

            for (var i = 0; i <= 90; i++) {
                _cancelSource.Token.ThrowIfCancellationRequested();
                Log.ReportProgress(100, i, $"Executing task {i}");
                Thread.Sleep(100);
            }
            
            Log.Done("Above task did not end completely");
            
            Log.ReportGlobalProgress(100, 30, "Logging 30% global progress");

            for (var i = 0; i <= 100; i++) {
                _cancelSource.Token.ThrowIfCancellationRequested();
                Log.ReportProgress(100, i, $"Executing task {i}");
                Thread.Sleep(10);
            }

            Log.Done("Task above ended well");
            
            Log.ReportGlobalProgress(100, 100, "Logging 100% global progress");

            return 1;
        }

    }
    
    [Command(
        "progressbar", "pb",
        Description = "Subcommand that shows the usage of progress bars",
        ExtendedHelpText = "sakoe selftest progressbar",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase
    )]
    internal class ProgressSelfTestCommand : OeBaseCommand {
        
        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            
            const int totalTicks = 10;
            var options = new ProgressBarOptions {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false,
                EnableTaskBarProgress = true
            };
            var childOptions = new ProgressBarOptions {
                ForegroundColor = ConsoleColor.Magenta,
                BackgroundColor = ConsoleColor.DarkMagenta,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false,
                CollapseWhenFinished = false,
                ForegroundColorDone = ConsoleColor.DarkGreen,
            };
            using (var pbar = new ProgressBar(totalTicks, "main progressbar", options)) {
                TickToCompletion(pbar, totalTicks, sleep: 10, childAction: () => {
                    using (var child = pbar.Spawn(totalTicks, "child actions", childOptions)) {
                        TickToCompletion(child, totalTicks, sleep: 100);
                    }
                });
            }
            
            return 1;
        }
        
        private void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action childAction = null) {
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