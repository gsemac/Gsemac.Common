using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class Shlwapi {

        // Public members

        public static int StrCmpLogicalW(string psz1, string psz2) {

            return StrCmpLogicalWNative(psz1, psz2);

        }

        // Private members

        [DllImport("shlwapi", EntryPoint = "StrCmpLogicalW", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalWNative(string psz1, string psz2);

    }

}