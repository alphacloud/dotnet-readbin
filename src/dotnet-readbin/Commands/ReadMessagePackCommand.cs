namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;
    using MessagePack;


    [Command("msgpack", Description = "Dump MessagePack as JSON")]
    internal class ReadMessagePackCommand : BaseReadCommand
    {
        /// <inheritdoc />
        internal override async Task<int> Dump(Stream input, Stream output, CancellationToken cancellationToken)
        {
            var bytes = new byte[input.Length];
            await input.ReadAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);


            var json = MessagePackSerializer.ConvertToJson(bytes);
            using (var writer = new StreamWriter(output, Encodings.Utf8NoBom, 512, true))
            {
                await writer.WriteAsync(json).ConfigureAwait(false);
            }

            return ExitCodes.Ok;
        }
    }
}
