// The "Architecture" enum was added in .NET Framework 4.7.1.
// The "Wasm" value was added in .NET 5.
// The "S390x" value was added in .NET 6.

namespace Gsemac.Polyfills.System.Runtime.InteropServices {

    public enum Architecture {
        /// <summary>
        /// An Intel-based 32-bit processor architecture.
        /// </summary>
        X86,
        /// <summary>
        /// An Intel-based 64-bit processor architecture.
        /// </summary>
        X64,
        /// <summary>
        /// A 32-bit ARM processor architecture.
        /// </summary>
        Arm,
        /// <summary>
        /// A 64-bit ARM processor architecture.
        /// </summary>
        Arm64,
        /// <summary>
        /// The WebAssembly platform.
        /// </summary>
        Wasm,
        /// <summary>
        /// The S390x platform architecture.
        /// </summary>
        S390x,
    }

}