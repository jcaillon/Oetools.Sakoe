using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using ShellProgressBar;

namespace Oetools.Runner.Cli {
    class Program {
        static void Main(string[] args) {

            Packager.Main.Run();

            var app = new CommandLineApplication();

            app.Description = "Creates files with the names of given strings and containing the coresponding hashcode.";
            app.HelpOption("-?|-h|--help");
            app.ExtendedHelpText = "Following argument is required -add and options are case sensitive.";

            var stringsOpt = app.Option("-add|--add <string>",
                "Defines one of the strings you wish to create a file for. Ex \" -add hello \"",
                CommandOptionType.MultipleValue);

            var pathOpt = app.Option("-path|--path <path>",
                "Defines the path of the files to write to. Ex \" -path C:\\myDirectory\\\". Defaults to the relative directory",
                CommandOptionType.SingleValue);

            var enableTimestampOpt = app.Option("-timestamp|--timestamp",
                "Option for the filenames to be extended by the current timestamp",
                CommandOptionType.NoValue);
            app.OnExecute(() => {
                if (!stringsOpt.HasValue()) {
                    Console.WriteLine("The -add options is required to be used at least ones. Please use the --help option for more info.");
                    return 0;
                } else {
                    List<String> strings = stringsOpt.Values;

                    String path = "";
                    if (pathOpt.HasValue()) path = pathOpt.Value();

                    if (enableTimestampOpt.HasValue()) path += DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss ");

                    CreateHashFiles(strings, path);
                    return 1;
                }
            });

            try {
                int result = app.Execute(args);
                Environment.Exit(result);
            } catch (CommandParsingException ex) {
                Console.WriteLine("Couldn't pass your command. Please use the --help option for more info.");
                Console.WriteLine(ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("An unexpected error occured. Please use the --help option for more info.");
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateHashFiles(List<String> strings, String path) {
            var options = new ProgressBarOptions {
                ProgressCharacter = '#',
                ProgressBarOnBottom = false,
                ForegroundColorDone = ConsoleColor.Green,
                ForegroundColor = ConsoleColor.White
            };

            using (var pbar = new ShellProgressBar.ProgressBar(strings.Count, "Writing hashes to files", options)) {
                foreach (String s in strings) {
                    String filename = path + s + ".txt";
                    if (File.Exists(filename)) File.Delete(filename);
                    using (StreamWriter writer = File.AppendText(filename)) {
                        pbar.Tick("Writing file for string \"" + s + "\"");
                        writer.WriteLine(s.GetHashCode());
                    }
                }
            }
        }
    }
}