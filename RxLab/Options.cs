using System;
using CommandLine;
using CommandLine.Text;

namespace RxLab
{
    public class Options
    {
        [Option('i', "input", Required = false, HelpText = "Input file to read commands in batch.")]
        public string InputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
                            {
                                Heading = new HeadingInfo("Console Application Base", "1.0"),
                                Copyright = new CopyrightInfo("Ruben Delange", 2014),
                                AdditionalNewLineAfterOption = true,
                                AddDashesToOption = true
                            };

            HandleParsingErrorsInHelp(help);

            help.AddPreOptionsLine("Usage: ConsoleApplicationBase [-i batchCommands.txt]");
            help.AddOptions(this);

            return help;
        }

        private void HandleParsingErrorsInHelp(HelpText help)
        {
            string errors = help.RenderParsingErrorsText(this, 2);
            if (!string.IsNullOrEmpty(errors))
            {
                help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR: ", errors, Environment.NewLine));
            }
        }

    }
}
