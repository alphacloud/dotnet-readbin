namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;


    [Command("hex", Description = "Convert hex encoded text to binary")]
    internal class ReadHexCommand : BaseReadCommand
    {
        private static readonly char[] _whiteSpaces = new[]
        {
            ' ', '\x0D', '\x0A', '\x09'
        }.OrderBy(x => x).ToArray();

        private static readonly IReadOnlyDictionary<char, byte> Hex = new Dictionary<char, byte>(16)
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['a'] = 10,
            ['b'] = 11,
            ['c'] = 12,
            ['d'] = 13,
            ['e'] = 14,
            ['f'] = 15,
        };

        /// <inheritdoc />
        /// <exception cref="T:System.InvalidOperationException">Unexpected character in input</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        internal override async Task<int> Dump(Stream input, Stream output, CancellationToken cancellationToken)
        {
            var buf = new byte[2];
            int charIndex = 0;
            int offset = 0;

            var outStream = new BinaryWriter(output, Encoding.ASCII, true);
            await using (outStream.ConfigureAwait(false))
            {
                int nextByte = 0;
                while ((nextByte = input.ReadByte()) != -1)
                {
                    var c = char.ToLowerInvariant((char) (nextByte & 0xFF));
                    offset++;
                    if (Array.BinarySearch(_whiteSpaces, c) >= 0) continue;

                    if (!Hex.TryGetValue(c, out var hexValue))
                        throw new InvalidOperationException($"Incorrect hex character '{c}' (0x{(byte) c:X}) at offset {offset}.");

                    buf[charIndex++] = hexValue;
                    if (charIndex == 2)
                    {
                        charIndex = 0;
                        byte b = (byte) ((byte) (buf[0] << 4) + buf[1]);
                        outStream.Write(b);
                    }
                }

                if (charIndex != 0)
                    throw new InvalidOperationException($"Unexpected end of input at offset {offset} (0x{offset:x2}).");
            }

            return ExitCodes.Ok;
        }
    }
}
