using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Alphacloud.DotNet.ReadBin.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Alphacloud.DotNet.ReadBin.Tests
{
    public class BsonTests : IDisposable
    {
        private Stream _input;
        private readonly MemoryStream _output;
        private readonly JsonTextReader _outputReader;
        private ReadBsonCommand _command;

        public BsonTests()
        {
            _output = new MemoryStream(1024);
            _outputReader = new JsonTextReader(new StreamReader(_output));

            _command = new ReadBsonCommand();
        }

        public void Dispose()
        {
            _input?.Dispose();
            _output?.Dispose();
            _outputReader?.Close();
        }

        [Fact]
        public async Task CanDump()
        {
            _input = GetType().Assembly.GetManifestResourceStream("Alphacloud.DotNet.ReadBin.Tests.Resources.SimplePayload.bson");
            var res = await _command.Dump(_input, _output, CancellationToken.None).ConfigureAwait(false);
            res.Should().Be(0);

            _output.Position = 0;

            var dump = JObject.Load(_outputReader);

            dump["string"].Value<string>().Should().Be("string");
            dump["int"].Value<int>().Should().Be(100);
            dump["boolean"].Value<bool>().Should().BeTrue();

        }
    }
}
