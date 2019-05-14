using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Alphacloud.DotNet.ReadBin.Commands
{
    [Command("bson", Description = "Dump BSON as JSON")]
    internal class ReadBsonCommand : BaseReadCommand
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);

        [Option(ShortName = "arr", LongName = "array", Description = "Read object as an array")]
        public bool IsArray { get; set; }

        [Option(ShortName = "dt", LongName = "date-time",
            Description = "DateTime kind to use when reading dates (Local,Utc).")]
        public DateTimeKind DateTimeKindHandling { get; set; } = DateTimeKind.Local;

        [Option(ShortName = "b", LongName = "bom",
            Description = "Add UTF8 identifier to output")]
        public bool AddBom { get; set; }

        [Option(ShortName = "indent", LongName = "indent-output",
            Description = "Ident output")]
        public bool IndentOutput { get; set; }

        internal override async Task<int> Dump(Stream input, Stream output, CancellationToken cancellationToken)
        {
            JObject obj;
            using (var reader = new BsonDataReader(input, IsArray, DateTimeKindHandling))
            {
                obj = await JObject.LoadAsync(reader, cancellationToken)
                    .ConfigureAwait(false);
            }

            var enc = AddBom ? Encoding.UTF8 : Utf8NoBom;
            var writer = new JsonTextWriter(new StreamWriter(output, enc, 1024, true));
            if (IndentOutput)
                writer.Formatting = Formatting.Indented;

            using (writer)
            {
                await obj.WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
            }

            return ExitCodes.Ok;
        }
    }
}
