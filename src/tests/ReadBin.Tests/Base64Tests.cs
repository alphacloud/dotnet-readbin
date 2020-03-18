namespace Alphacloud.DotNet.ReadBin.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Commands;
    using FluentAssertions;
    using Xunit;


    public class Base64Tests : TextBasedConverterTestsBase<ReadBase64Command>
    {
        [Fact]
        public async Task CanDecode()
        {
            var base64String = Convert.ToBase64String(new byte[] {1, 2, 0xF3, 0xFF}, Base64FormattingOptions.None);

            SetInputStream(base64String, Encoding.ASCII);
            await Run();

            var result = Output.ToArray();
            result.Should().HaveCount(4);
            result[0].Should().Be(1);
            result[1].Should().Be(2);
            result[2].Should().Be(0xF3);
            result[3].Should().Be(0xFF);
        }

        [Fact]
        public async Task CanProcessLineBreaks()
        {
            var arr = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                arr[i] = (byte) i;
            }

            SetInputStream(Convert.ToBase64String(arr, Base64FormattingOptions.InsertLineBreaks), Encoding.ASCII);
            await Run();

            var result = Output.ToArray();
            result.Should().HaveCount(128);
            for (int i = 0; i < 128; i++)
            {
                result[i].Should().Be((byte) i, "error at index {0}", i);
            }
        }
    }
}
