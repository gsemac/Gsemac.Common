using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class Kernel32 {

        // Public members

        public static bool AddDllDirectory(string newDirectory) {

            return AddDllDirectoryNative(newDirectory);

        } // Requires Windows 8+, or Windows Vista+ with KB2533623
        public static bool AllocConsole() {

            return AllocConsoleNative();

        }
        public static bool AttachConsole(int dwProcessId) {

            return AttachConsoleNative(dwProcessId);

        }
        public static bool FreeConsole() {

            return FreeConsoleNative();

        }
        public static IntPtr GetCurrentProcess() {

            return GetCurrentProcessNative();

        }
        public static bool MoveFileW(string lpExistingFileName, string lpNewFileName) {

            return MoveFileWNative(lpExistingFileName, lpNewFileName);

        }
        public static bool SetDllDirectory(string lpPathName) {

            return SetDllDirectoryNative(lpPathName);

        }

        // Private members

        [DllImport("kernel32", EntryPoint = "AddDllDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool AddDllDirectoryNative(string newDirectory);
        [DllImport("kernel32", EntryPoint = "AllocConsole", SetLastError = true)]
        internal static extern bool AllocConsoleNative();
        [DllImport("kernel32", EntryPoint = "AttachConsole", SetLastError = true)]
        private static extern bool AttachConsoleNative(int dwProcessId);
        [DllImport("kernel32", EntryPoint = "FreeConsole", SetLastError = true)]
        private static extern bool FreeConsoleNative();
        [DllImport("kernel32", EntryPoint = "GetCurrentProcess", SetLastError = true)]
        private static extern IntPtr GetCurrentProcessNative();
        [DllImport("kernel32", EntryPoint = "MoveFileW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool MoveFileWNative(string lpExistingFileName, string lpNewFileName);
        [DllImport("kernel32", EntryPoint = "SetDllDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectoryNative(string lpPathName);

    }

}