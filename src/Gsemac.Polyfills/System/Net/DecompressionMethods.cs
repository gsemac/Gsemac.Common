using System;

namespace Gsemac.Polyfills.System.Net {

    /// <inheritdoc cref="global::System.Net.DecompressionMethods"/>
    [Flags]
    public enum DecompressionMethods {
        /// <summary>
        /// Use all compression-decompression algorithms.
        /// </summary>
        All = -1,
        /// <inheritdoc cref="global::System.Net.DecompressionMethods.None"/>
        None = global::System.Net.DecompressionMethods.None,
        /// <inheritdoc cref="global::System.Net.DecompressionMethods.GZip"/>
        GZip = global::System.Net.DecompressionMethods.GZip,
        /// <inheritdoc cref="global::System.Net.DecompressionMethods.Deflate"/>
        Deflate = global::System.Net.DecompressionMethods.Deflate,
        /// <summary>
        /// Use the Brotli compression-decompression algorithm.
        /// </summary>
        Brotli = 4,
    }

}