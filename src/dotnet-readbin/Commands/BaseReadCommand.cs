namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using McMaster.Extensions.CommandLineUtils;


    [HelpOption("--help")]
    internal abstract class BaseReadCommand
    {
        [Option(ShortName = "in", LongName = "input-file",
            Description = "Input file name (console, if not specified).")]
        [FileExists]
        public string InputFileName { get; set; }

        [Option(ShortName = "out", LongName = "output-file",
            Description = "Input file name (console, if not specified).")]
        public string OutputFileName { get; set; }

        protected Stream GetInputStream()
        {
            return string.IsNullOrEmpty(InputFileName)
                ? Console.OpenStandardInput()
                : File.OpenRead(InputFileName);
        }

        protected Stream GetOutputStream()
        {
            return string.IsNullOrEmpty(OutputFileName)
                ? Console.OpenStandardOutput(100)
                : new FileStream(OutputFileName, FileMode.Create);
        }

        [UsedImplicitly]
        public virtual async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            using (var input = GetInputStream())
            {
                using (var output = GetOutputStream())
                {
                    return await Dump(input, output, CancellationToken.None).ConfigureAwait(false);
                }
            }
        }

        internal abstract Task<int> Dump(Stream input, Stream output, CancellationToken cancellationToken);
    }
}
