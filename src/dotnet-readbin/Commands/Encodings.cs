namespace Alphacloud.DotNet.ReadBin.Commands
{
    using System.Text;


    /// <summary>
    ///     Text encodings supported by application.
    /// </summary>
    internal static class Encodings
    {
        /// <summary>
        ///     UTF-8 without byte order marker.
        /// </summary>
        public static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    }
}
