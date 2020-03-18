namespace Alphacloud.DotNet.ReadBin.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Commands;
    using FluentAssertions;
    using Xunit;


    public class HexTests : TextBasedConverterTestsBase<ReadHexCommand>
    {
        [Fact]
        public async Task CanConvertHex()
        {
            SetInputStream("01 02 F3 FF" + Environment.NewLine, Encoding.ASCII);
            await Run();

            var result = Output.ToArray();
            result.Should().HaveCount(4);
            result[0].Should().Be(1);
            result[1].Should().Be(2);
            result[2].Should().Be(0xF3);
            result[3].Should().Be(0xFF);
        }

        [Fact]
        public async Task Should_Fail_On_Incomplete_Data()
        {
            SetInputStream("F3 F", Encoding.ASCII);
            var error = (await this.Awaiting(x => x.Run())
                .Should().ThrowAsync<InvalidOperationException>()).Which;
            error.Message.Should().Be("Unexpected end of input at offset 4 (0x04).");
        }
    }
}
