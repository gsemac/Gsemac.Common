using System.Runtime.InteropServices;

namespace Gsemac.Native {

    public static class Kernel32 {

        // Public members

        public static bool SetDllDirectory(string lpPathName) {

            return SetDllDirectoryNative(lpPathName);

        }

        // Private members

        [DllImport("kernel32", EntryPoint = "SetDllDirectory", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectoryNative(string lpPathName);

    }

}