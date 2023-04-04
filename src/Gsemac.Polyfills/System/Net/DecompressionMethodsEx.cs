using System;
using System.Net;

namespace Gsemac.Polyfills.System.Net {

    /// <inheritdoc cref="DecompressionMethods"/>
    [Flags]
    public enum DecompressionMethodsEx {
        /// <summary>
        /// Use all compression-decompression algorithms.
        /// </summary>
        All = -1,
        /// <inheritdoc cref="DecompressionMethods.None"/>
        None = 0,
        /// <inheritdoc cref="DecompressionMethods.GZip"/>
        GZip = 1,
        /// <inheritdoc cref="DecompressionMethods.Deflate"/>
        Deflate = 2,
        /// <summary>
        /// Use the Brotli compression-decompression algorithm.
        /// </summary>
        Brotli = 4,
    }

}