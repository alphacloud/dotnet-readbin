namespace Alphacloud.DotNet.ReadBin
{
    using System.Threading.Tasks;
    using Commands;
    using JetBrains.Annotations;
    using McMaster.Extensions.CommandLineUtils;


    /// <summary>
    ///     Main command.
    /// </summary>
    [Command(
        Name = "dotnet readbin",
        FullName = "dotnet-readbin",
        Description = "Displays binary encoded data in human readable format.",
        ExtendedHelpText = "Application reads console input")]
    [Subcommand(
        typeof(ReadBsonCommand), typeof(ReadMessagePackCommand), typeof(ReadHexCommand),
        typeof(ReadBase64Command)
    )]
    [HelpOption]
    public class ReadBinCommand
    {
        [UsedImplicitly]
        public Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (app.Arguments.Count == 0) app.ShowHelp();

            return Task.FromResult(ExitCodes.Error);
        }
    }
}
