using System;

namespace Gsemac.Win32.Native {

    [Flags]
    public enum EmfToWmfBitsFlags {
        EmfToWmfBitsFlagsDefault = 0x00000000,
        EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
        EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
        EmfToWmfBitsFlagsNoXORClip = 0x00000004,
    }

}