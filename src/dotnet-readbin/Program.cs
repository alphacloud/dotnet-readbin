using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

[assembly: InternalsVisibleTo("ReadBin.Tests")]

namespace Alphacloud.DotNet.ReadBin
{
    class Program
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
                Console.Error.WriteLine("Unexpected error: " + ex.ToString());
                Console.ResetColor();
                return ExitCodes.Exception;
            }
        }
    }
}
