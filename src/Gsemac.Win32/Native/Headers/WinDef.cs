using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {

        public int left;
        public int top;
        public int right;
        public int bottom;

    }

}