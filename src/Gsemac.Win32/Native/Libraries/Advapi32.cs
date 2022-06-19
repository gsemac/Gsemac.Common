using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class Advapi32 {

        // Public members

        public static bool AdjustTokenPrivileges(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, ref TOKEN_PRIVILEGES newState) {

            return AdjustTokenPrivilegesNativeWithoutPreviousState(tokenHandle, disableAllPrivileges, ref newState, 0, IntPtr.Zero, IntPtr.Zero);

        }
        public static bool AdjustTokenPrivileges(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, ref TOKEN_PRIVILEGES newState, uint bufferLengthInBytes, ref TOKEN_PRIVILEGES previousState, out uint returnLengthInBytes) {

            return AdjustTokenPrivilegesNative(tokenHandle, disableAllPrivileges, ref newState, bufferLengthInBytes, ref previousState, out returnLengthInBytes);

        }
        public static bool InitiateSystemShutdownEx(string lpMachineName, string lpMessage, uint dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown, uint dwReason) {

            return InitiateSystemShutdownExNative(lpMachineName, lpMessage, dwTimeout, bForceAppsClosed, bRebootAfterShutdown, dwReason);

        }
        public static bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle) {

            return OpenProcessTokenNative(processHandle, desiredAccess, out tokenHandle);

        }
        public static bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid) {

            return LookupPrivilegeValueNative(lpSystemName, lpName, ref lpLuid);

        }

        // Private members

        [DllImport("advapi32", EntryPoint = "AdjustTokenPrivileges", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivilegesNative(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, ref TOKEN_PRIVILEGES newState, uint bufferLengthInBytes, ref TOKEN_PRIVILEGES previousState, out uint returnLengthInBytes);
        [DllImport("advapi32", EntryPoint = "AdjustTokenPrivileges", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivilegesNativeWithoutPreviousState(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, ref TOKEN_PRIVILEGES newState, uint zero, IntPtr null1, IntPtr null2);
        [DllImport("advapi32", EntryPoint = "InitiateSystemShutdownEx", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InitiateSystemShutdownExNative(string lpMachineName, string lpMessage, uint dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown, uint dwReason);
        [DllImport("advapi32", EntryPoint = "LookupPrivilegeValue")]
        static extern bool LookupPrivilegeValueNative(string lpSystemName, string lpName, ref LUID lpLuid);
        [DllImport("advapi32", EntryPoint = "OpenProcessToken", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessTokenNative(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

    }

}