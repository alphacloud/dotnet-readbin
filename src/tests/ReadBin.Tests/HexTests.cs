namespace Alphacloud.DotNet.ReadBin.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using FluentAssertions;
    using JetBrains.Annotations;
    using Xunit;


    public class HexTests: IDisposable
    {
        private readonly MemoryStream _input;
        private readonly MemoryStream _output;
        private ReadHexCommand _readHexCommand;

        public HexTests()
        {
            _input = new MemoryStream();
            _output = new MemoryStream();
            _readHexCommand = new ReadHexCommand();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _input?.Dispose();
            _output?.Dispose();
        }

        [Fact]
        public async Task CanConvertHex()
        {
            var streamWriter = new StreamWriter(_input, Encoding.ASCII, leaveOpen: true);
            streamWriter.WriteLine("01 02 F3 FF");
            streamWriter.Flush();
            _input.Position = 0;
            await _readHexCommand.Dump(_input, _output, CancellationToken.None);
            var result = _output.ToArray();
            result.Should().HaveCount(4);
            result[0].Should().Be(1);
            result[1].Should().Be(2);
            result[2].Should().Be(0xF3);
            result[3].Should().Be(0xFF);
        }

        [Fact] 
        public async Task Should_Fail_On_Incomplete_Data()
        {
            var streamWriter = new StreamWriter(_input, Encoding.ASCII, leaveOpen: true);
            streamWriter.Write("F3 F");
            streamWriter.Flush();
            _input.Position = 0;
            var error = (await _readHexCommand.Awaiting(x=>x.Dump(_input, _output, CancellationToken.None))
                .Should().ThrowAsync<InvalidOperationException>()).Which;
            error.Message.Should().Be("Unexpected end of input at offset 4 (0x04).");
        }
    }
}
