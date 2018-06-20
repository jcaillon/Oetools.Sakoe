﻿using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Runner.Cli.Command {
    
    
    [Command(
        Description = "TODO : prolint",
        ExtendedHelpText = "TODO : prolint",
        OptionsComparison = StringComparison.CurrentCultureIgnoreCase,
        ThrowOnUnexpectedArgument = false
    )]
    internal class ProlintCommand : BaseCommand {
        [Option("-m")]
        public string Message { get; set; }

        // This will automatically be set before OnExecute is invoked.
        private MainCommand Parent { get; set; }
        
        [Required]
        [Argument(0, Description = "Main command ", ShowInHelpText = true)]
        public CommandType Command { get; } = CommandType.Deploy;

        [Option]
        public (bool HasValue, CommandType level) Trace { get; }
        
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
    enum CommandType {
        Deploy,
        Prolint
    }
}