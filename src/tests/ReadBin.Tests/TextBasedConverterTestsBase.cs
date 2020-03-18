namespace Alphacloud.DotNet.ReadBin.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;


    public class TextBasedConverterTestsBase<TCommand> : IDisposable
        where TCommand: BaseReadCommand, new()
    {
        protected MemoryStream Input { get; }
        protected MemoryStream Output { get; }
        protected TCommand Command { get; }

        public TextBasedConverterTestsBase()
        {
            Input = new MemoryStream();
            Output = new MemoryStream();
            Command = new TCommand();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Input?.Dispose();
            Output?.Dispose();
        }

        protected void SetInputStream(string str, Encoding encoding)
        {
            Input.Position = 0;
            Input.SetLength(0);
            using (var streamWriter = new StreamWriter(Input, encoding, leaveOpen: true))
            {
                streamWriter.Write(str);
                streamWriter.Flush();
            }
            Input.Position = 0;
        }

        protected async Task Run()
        {
            await Command.Transform(Input, Output, CancellationToken.None);
            Output.SetLength(Output.Position);
            Output.Position = 0;
        }
    }
}
