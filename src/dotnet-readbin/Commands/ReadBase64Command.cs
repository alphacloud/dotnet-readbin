namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;


    [Command("base64", Description = "Convert Base64 encoded data to binary.")]
    public class ReadBase64Command : BaseReadCommand
    {
        /// <inheritdoc />
        internal override async Task<int> Transform(Stream input, Stream output, CancellationToken cancellationToken)
        {
            const int bufferSize = 512;
            var inputBuffer = new byte[bufferSize];
            var outputBuffer = new byte[bufferSize];
            using (var transform = new FromBase64Transform())
            {
                int bytesRead;
                while ((bytesRead = await input.ReadAsync(inputBuffer, 0, inputBuffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
                {
                    int bytesTransformed;
                    try
                    {
                        bytesTransformed = transform.TransformBlock(inputBuffer, 0, bytesRead, outputBuffer, 0);
                    }
                    catch (Exception ex)
                    {
                        var offset = input.Position - bufferSize;
                        throw new InvalidOperationException(
                            $"Error decoding input block of {bufferSize} bytes starting at offset {offset} ({offset:x2}).",
                            ex);
                    }

                    await output.WriteAsync(outputBuffer, 0, bytesTransformed, cancellationToken).ConfigureAwait(false);
                }
            }

            return ExitCodes.Ok;
        }
    }
}

