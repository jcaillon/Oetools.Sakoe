#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (SelfTestCommand.cs) is part of Oetools.Sakoe.
//
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.ConLog;
using Oetools.Sakoe.Utilities;
using Oetools.Sakoe.Utilities.Extension;
using Oetools.Utilities.Lib.Extension;
using Oetools.Utilities.Openedge.Database;

namespace Oetools.Sakoe.Command.Oe {
#if DEBUG

    [Command(
        "selftest", "se",
        Description = "A command to test the behaviour of this tool.",
        ExtendedHelpText = "sakoe selftest"
    )]
    [Subcommand(typeof(LogSelfTestCommand))]
    [Subcommand(typeof(InputSelfTestCommand))]
    [Subcommand(typeof(PromptSelfTestCommand))]
    [Subcommand(typeof(ResponseFileSelfTestCommand))]
    [Subcommand(typeof(WrapSelfTestCommand))]
    [Subcommand(typeof(ConsoleFormatSelfTestCommand))]
    internal class SelfTestCommand : AExpectSubCommand {}

    [Command(
        "consoleformat", "cf",
        Description = "Subcommand that shows the use of CsConsoleFormat",
        ExtendedHelpText = "sakoe selftest consoleformat"
    )]
    internal class ConsoleFormatSelfTestCommand : ABaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            // The library is CsConsoleFormat

            /*
            ConsoleRenderer.RenderDocument(new Document(
                new Span("Order #") { Color = ConsoleColor.Blue }, 1, "\n",
                new Separator(),
                new Span("line"),
                new Br(),
                new Span("line"),
                new Div(),
                new Span("Customer: ") { Color = ConsoleColor.Yellow }, "name",
                new Grid {
                    Color = ConsoleColor.White,
                    Stroke = LineThickness.None,
                    Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, },
                    Children = {
                        new Cell("Id") { Stroke = LineThickness.None,  },
                        new Cell("Name") { Stroke = LineThickness.None },
                        new Cell("Count") { Stroke = LineThickness.None },
                        new Cell(1){ Stroke = LineThickness.None },
                        new Cell("name1"){ Stroke = LineThickness.None },
                        new Cell("count") { Align = Align.Right, Stroke = LineThickness.None },
                        new Cell(1){ Stroke = LineThickness.None },
                        new Cell(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas varius sapien
vel purus hendrerit vehicula. Integer hendrerit viverra turpis, ac sagittis arcu
pharetra id. Sed dapibus enim non dui posuere sit amet rhoncus tellus
consectetur. Proin blandit lacus vitae nibh tincidunt cursus. Cum sociis natoque
penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam tincidunt
purus at tortor tincidunt et aliquam dui gravida. Nulla consectetur sem vel
felis vulputate et imperdiet orci pharetra. Nam vel tortor nisi. Sed eget porta
tortor. Aliquam suscipit lacus vel odio faucibus tempor. Sed ipsum est,
condimentum eget eleifend ac, ultricies non dui. Integer tempus, nunc sed
venenatis feugiat, augue orci pellentesque risus, nec pretium lacus enim eu
nibh."){ Stroke = LineThickness.None, Margin = new Thickness(1) },
                        new Cell(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas varius sapien
vel purus hendrerit vehicula. Integer hendrerit viverra turpis, ac sagittis arcu
pharetra id. Sed dapibus enim non dui posuere sit amet rhoncus tellus
consectetur. Proin blandit lacus vitae nibh tincidunt cursus. Cum sociis natoque
penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam tincidunt
purus at tortor tincidunt et aliquam dui gravida. Nulla consectetur sem vel
felis vulputate et imperdiet orci pharetra. Nam vel tortor nisi. Sed eget porta
tortor. Aliquam suscipit lacus vel odio faucibus tempor. Sed ipsum est,
condimentum eget eleifend ac, ultricies non dui. Integer tempus, nunc sed
venenatis feugiat, augue orci pellentesque risus, nec pretium lacus enim eu
nibh.") { Align = Align.Right ,Stroke = LineThickness.None },
                    }
                }
            ));
            */

            return 1;
        }

    }

    [Command(
        "wrap", "wr",
        Description = "Subcommand that shows the word wrap",
        ExtendedHelpText = "sakoe selftest wrap"
    )]
    internal class WrapSelfTestCommand : ABaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Out.WriteResultOnNewLine("  0123456789abcdefghijklmnopsqrstu\n          0123456789abcdefghijklmnopsqrstu");
            Out.WriteResultOnNewLine("012345");
            Out.WriteResult("  01234567\n89");
            Out.WriteResultOnNewLine("012345", ConsoleColor.Gray);
            Out.WriteResultOnNewLine("012345");
            Out.WriteResultOnNewLine("0123456789abcdefghijklmnopsqrstu");

            Out.WriteResultOnNewLine("This is an output, it will still be displayed if the verbosity is set to None");
            Out.WriteResult(". ");
            Out.WriteResult("Another phrase.");
            Out.WriteResultOnNewLine("This is an output, it will still be displayed if the verbosity is set to None");
            Out.WriteResult(". ");
            Out.WriteResult("Anotherphrasemustlongerzefzefzefzefzaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaefzefezzezefzfethanexpected.");

            Out.WriteResultOnNewLine(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas varius sapien vel purus hendrerit vehicula. Integer hendrerit viverra turpis, ac sagittis arcu pharetra id. Sed dapibus enim non dui posuere sit amet rhoncus tellus consectetur. Proin blandit lacus vitae nibh tincidunt cursus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam tincidunt purus at tortor tincidunt et aliquam dui gravida. Nulla consectetur sem vel felis vulputate et imperdiet orci pharetra.
   Nam vel tortor nisi. Sed eget porta tortor. Aliquam suscipit lacus vel odio faucibus tempor. Sed ipsum est, condimentum eget eleifend ac, ultricies non dui. Integer tempus, nunc sed venenatis feugiat, augue orci pellentesque risus, nec pretium lacus enim eu nibh.");

            Out.WriteResultOnNewLine(null);
            Log.Debug("Log debug");
            Log.Info("Log info");
            Log.Warn("Log warn");
            Log.Error("Log error");
            Log.Fatal("Log fatal");
            Log.Done("Log success");

            return 1;
        }

    }

    [Command(
        "input", "in",
        Description = "Subcommand that shows the usage of options and arguments",
        ExtendedHelpText = @"sakoe st input boom.txt --attachment first --attachment ""second with spaces""
sakoe st input -b2 s1024",
        AllowArgumentSeparator = true
    )]
    internal class InputSelfTestCommand : ABaseCommand {

        [Argument(0)]
        [LegalFilePath]
        public string FileTruc { get; }

        [Argument(1)]
        public string Folder { get; }

        [Option("--git-dir")]
        [DirectoryExists]
        public string GitDir { get; }

        [Option("-X|--request", Description = "HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post.")]
        public HttpMethod RequestMethod { get; } = HttpMethod.Post;

        [Required]
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
        public string ImportanceFuck { get; } = "normal";

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
        [Description("[-- <mon truc>...]")]
        public string[] RemainingArgs { get; }

        // You can use this pattern when the parent command may have options or methods you want to
        // use from sub-commands.
        // This will automatically be set before OnExecute is invoked
        private SelfTestCommand Parent { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Log.Info($"File = {FileTruc ?? "null"}");
            Log.Info($"GitDir = {GitDir ?? "null"}");
            Log.Info($"RequestMethod = {RequestMethod}");
            Log.Info($"Message = {Message ?? "null"}");
            Log.Info($"To = {To}");
            if (Attachments != null) {
                foreach (var attachment in Attachments) {
                    Log.Info($"Attachments = {attachment}");
                }
            }
            Log.Info($"Importance = {ImportanceFuck ?? "null"}");
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
            Log.Info($"Parent.Verbosity = {Verbosity}");

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

    /// <summary>
    /// Usage : sakoe selftest responsefile -c @file.txt
    /// Will display the content of file.txt, line by line, where each line is an argument
    /// </summary>
    [Command(
        "responsefile", "rf",
        Description = "Subcommand that shows the usage of a response file",
        ExtendedHelpText = "sakoe selftest responsefile",
        AllowArgumentSeparator = true
    )]
    internal class ResponseFileSelfTestCommand : ABaseCommand {

        [Option("-c|--create", "Create the response file", CommandOptionType.NoValue)]
        public bool Create { get; }

        [Option("-f", "List of files.", CommandOptionType.MultipleValue)]
        public string[] Files { get; }

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {
            Log.Info("First do : sakoe selftest responsefile -c");
            Log.Info("Then typical usage is : sakoe @file.txt");
            if (Create) {
                File.WriteAllText("file.txt", $"{app.Parent.Name}\n{app.Name}\n-f\nnew file derp\n-f\nnew.txt\n-f\ncool");
                Log.Info("Created txt file : file.txt");
            }
            if (Files != null) {
                foreach (var file in Files) {
                    Log.Done(file);
                }
            }
            return 0;
        }

    }

    [Command(
        "prompt", "prompt",
        Description = "Subcommand that shows the usage of prompt",
        ExtendedHelpText = "sakoe selftest prompt"
    )]
    internal class PromptSelfTestCommand : ABaseCommand {

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
        ExtendedHelpText = "sakoe selftest log"
    )]
    internal class LogSelfTestCommand : ABaseCommand {

        protected override int ExecuteCommand(CommandLineApplication app, IConsole console) {

            Out.WriteOnNewLine($"WindowWidth = {ConsoleImplementation.Singleton.WindowWidth}");
            Out.WriteOnNewLine($"CursorTop = {ConsoleImplementation.Singleton.CursorTop}");
            Out.WriteOnNewLine($"CursorVisible = {ConsoleImplementation.Singleton.CursorVisible}");
            Out.WriteOnNewLine($"IsOutputRedirected = {ConsoleImplementation.Singleton.IsOutputRedirected}");
            Out.WriteOnNewLine($"IsErrorRedirected = {ConsoleImplementation.Singleton.IsErrorRedirected}");

            typeof(ConsoleColor).ForEach<ConsoleColor>((s, l) => {
                Out.WriteOnNewLine($"==== {s} ====", (ConsoleColor) l);
            });

            Log.Info("Write as result (no word wrap)");
            Out.WriteResultOnNewLine("Lorem Ipsum is simply dummy text of the printing and typesetting industry.\nLorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.", ConsoleColor.Green);
            Out.WriteResult("It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", ConsoleColor.Green);

            Log.Info("Write as normal with indentation = 2 (with word wrap)");
            Out.WriteOnNewLine("Lorem Ipsum is simply dummy text of the printing and typesetting industry.\nLorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.", ConsoleColor.Green, 2);
            Out.Write("It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", ConsoleColor.Green, 2);

            Log.Info("Write as error with indentation = 2 (with word wrap)");
            Out.WriteErrorOnNewLine("Lorem Ipsum is simply dummy text of the printing and typesetting industry.\nLorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.", ConsoleColor.Green, 2);
            Out.WriteError("It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", ConsoleColor.Green, 2);

            Log.Info("Write a tree");
            Out.PushNode(false).WriteOnNewLine("Build");
            Out.Write(": 1st");
            Out.WriteOnNewLine("very very very long line very very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long line");
            Out.PushNode(false).WriteOnNewLine("1st step");
            Out.WriteOnNewLine("item 1");
            Out.PushNode(false).WriteOnNewLine("1st task");
            Out.WriteOnNewLine("item 1");
            Out.WriteOnNewLine("item 2");
            Out.PopNode().PushNode(true).WriteOnNewLine("2nd task");
            Out.WriteOnNewLine("item 1");
            Out.WriteOnNewLine("very very very long line very very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long line");
            Out.PopNode();
            Out.WriteOnNewLine("item 2");
            Out.PopNode().PushNode(true).WriteOnNewLine("2nd step");
            Out.WriteOnNewLine("item 1");
            Out.WriteOnNewLine("item 2");
            Out.PopNode();
            Out.WriteOnNewLine("very very very long line very very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long linevery very very long line");
            Out.PopNode();

            Log.Info("Log something with new lines (it should be indented)");
            Log.Info("Log\n\nnew line  \r\n   \ndebug");
            Out.WriteOnNewLine("this is an output, it will still be displayed if the verbosity is set to None");
            Out.Write(".");

            Log.ReportGlobalProgress(100, 5, "Logging 5% global progress");

            if (Log.Trace != null) {
                Log.Trace?.Write("Log.Trace?.Write");
            } else {
                Log.Info("In debug mode, you can use Log.Trace?.Write");
            }

            Log.Debug("Log debug");
            Log.Info("Log info");
            Log.Warn("Log warn");
            Log.Error("Log error éà!€");
            Log.Fatal("Log fatal");
            Log.Done("Log success");
            Log?.Info(app.GetTypeFromCommandLine().ToString());

            for (var i = 0; i <= 90; i++) {
                CancelToken?.ThrowIfCancellationRequested();
                Log.ReportProgress(100, i, null);
                Thread.Sleep(100);
            }

            Out.WriteOnNewLine("another output!");
            Log.Done("Above task did not end completely");

            Log.ReportGlobalProgress(100, 30, "Logging 30% global progress");

            for (var i = 0; i <= 100; i++) {
                CancelToken?.ThrowIfCancellationRequested();
                Log.ReportProgress(100, i, $"Executing task {i}");
                Thread.Sleep(100);
            }

            Out.WriteOnNewLine("another output!");
            Log.Done("Task above ended well");

            Log.ReportGlobalProgress(100, 100, "Logging 100% global progress");

            return 1;
        }

    }


#endif

}
