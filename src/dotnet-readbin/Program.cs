using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ReadBin.Tests")]

namespace Alphacloud.DotNet.ReadBin
{
    using System;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;


    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                return await CommandLineApplication.ExecuteAsync<ReadBinCommand>(args)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Unexpected error: " + ex);
                Console.ResetColor();
                return ExitCodes.Exception;
            }
        }
    }
}
