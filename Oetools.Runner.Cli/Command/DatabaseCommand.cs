using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Runner.Cli.Command {
    
    
    [Command(
        Description = "TODO : db",
        ExtendedHelpText = "TODO : db",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    [Subcommand("createfromdf", typeof(CreateDatabaseFromDfCommand))]
    internal class DatabaseCommand : BaseCommand {
        
        private int OnExecute(CommandLineApplication app, IConsole console) {
            // this shows help even if the --help option isn't specified
            app.ShowRootCommandFullNameAndVersion();
            console.ForegroundColor = ConsoleColor.Red;
            console.Error.WriteLine("errr");
            console.ResetColor();
            app.ShowHint();
            return 1;
        }

    }
    
    [Command(
        Description = "TODO : prolint",
        ExtendedHelpText = "TODO : prolint",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class CreateDatabaseFromDfCommand : BaseCommand {
        
        [Required]
        [Argument(0, Name = "df", Description = "Path to the .df file")]
        public string DfPath { get; }
        
        [Option("-m")]
        public string Message { get; set; }

        [Option]
        public (bool HasValue, string level) Trace { get; }
        
        public string[] RemainingArgs { get; set; }

        protected int OnExecute(CommandLineApplication app, IConsole console) {
            
            const string prompt = @"Which example would you like to run?
1 - Fake Git
2 - Fake Docker
3 - Fake npm
> ";
            var fuck = Prompt.GetInt(prompt);
            
            if (RemainingArgs == null || RemainingArgs.Length == 0) {
                var args = Prompt.GetString("Enter some arguments >");
                RemainingArgs = args.Split(' ');
            }
            
            return 0;
        }

    }
}