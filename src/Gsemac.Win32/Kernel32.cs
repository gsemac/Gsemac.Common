using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class Kernel32 {

        // Public members

        public static bool AddDllDirectory(string newDirectory) {

            return AddDllDirectoryNative(newDirectory);

        } // Requires Windows 8+, or Windows Vista+ with KB2533623 
        public static bool SetDllDirectory(string lpPathName) {

            return SetDllDirectoryNative(lpPathName);

        }

        // Private members

        [DllImport("kernel32", EntryPoint = "AddDllDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool AddDllDirectoryNative(string newDirectory);
        [DllImport("kernel32", EntryPoint = "SetDllDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectoryNative(string lpPathName);

    }

}