namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using McMaster.Extensions.CommandLineUtils;


    /// <summary>
    ///     Base class for read commands.
    /// </summary>
    [HelpOption("--help")]
    public abstract class BaseReadCommand
    {
        /// <summary>
        ///     Input file name.
        /// </summary>
        [Option(ShortName = "in", LongName = "input-file",
            Description = "Input file name (console, if not specified).")]
        [FileExists]
        public string InputFileName { get; set; }

        /// <summary>
        ///     Output file name.
        /// </summary>
        [Option(ShortName = "out", LongName = "output-file",
            Description = "Input file name (console, if not specified).")]
        public string OutputFileName { get; set; }

        /// <summary>
        ///     Returns stream for input data.
        /// </summary>
        /// <returns></returns>
        protected Stream GetInputStream()
        {
            return string.IsNullOrEmpty(InputFileName)
                ? Console.OpenStandardInput()
                : File.OpenRead(InputFileName);
        }

        /// <summary>
        ///     Returns stream to output data.
        /// </summary>
        /// <returns></returns>
        protected Stream GetOutputStream()
        {
            return string.IsNullOrEmpty(OutputFileName)
                ? Console.OpenStandardOutput(100)
                : new FileStream(OutputFileName, FileMode.Create);
        }

        /// <summary>
        ///     Executes transformation.
        /// </summary>
        /// <returns>Exit code to return to calling process, <see cref="ExitCodes" />.</returns>
        [UsedImplicitly]
        public virtual async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            var input = GetInputStream();
            await using (input.ConfigureAwait(false))
            {
                var output = GetOutputStream();
                await using (output.ConfigureAwait(false))
                {
                    return await Transform(input, output, CancellationToken.None).ConfigureAwait(false);
                }
            }
        }

        internal abstract Task<int> Transform(Stream input, Stream output, CancellationToken cancellationToken);
    }
}
