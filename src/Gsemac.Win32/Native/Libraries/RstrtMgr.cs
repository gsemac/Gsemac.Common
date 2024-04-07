using System.Runtime.InteropServices;
using System.Text;

namespace Gsemac.Win32.Native {

    public static class RstrtMgr {

        // Public members

        public static int RmEndSession(uint pSessionHandle) {

            return RmEndSessionNative(pSessionHandle);

        }
        public static int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, RM_PROCESS_INFO[] rgAffectedApps, out RM_REBOOT_REASON lpdwRebootReasons) {

            return RmGetListNative(dwSessionHandle, out pnProcInfoNeeded, ref pnProcInfo, rgAffectedApps, out lpdwRebootReasons);

        }
        public static int RmRegisterResources(uint dwSessionHandle, uint nFiles, string[] rgsFileNames, uint nApplications, RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames) {

            return RmRegisterResourcesNative(dwSessionHandle, nFiles, rgsFileNames, nApplications, rgApplications, nServices, rgsServiceNames);

        }
        public static int RmShutdown(uint dwSessionHandle, RM_SHUTDOWN_TYPE lActionFlags, RM_WRITE_STATUS_CALLBACK fnStatus) {

            return RmShutdownNative(dwSessionHandle, lActionFlags, fnStatus);

        }
        public static int RmStartSession(out uint pSessionHandle, uint dwSessionFlags, StringBuilder strSessionKey) {

            return RmStartSessionNative(out pSessionHandle, dwSessionFlags, strSessionKey);

        }

        // Private members

        [DllImport("rstrtmgr", EntryPoint = "RmEndSession", SetLastError = true)]
        static extern int RmEndSessionNative(uint pSessionHandle);
        [DllImport("rstrtmgr", EntryPoint = "RmGetList", SetLastError = false)]
        static extern int RmGetListNative(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, out RM_REBOOT_REASON lpdwRebootReasons);
        [DllImport("rstrtmgr", EntryPoint = "RmRegisterResources", CharSet = CharSet.Unicode, SetLastError = false)]
        static extern int RmRegisterResourcesNative(uint dwSessionHandle, [Optional] uint nFiles, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.LPWStr)] string[] rgsFileNames, [Optional] uint nApplications, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] RM_UNIQUE_PROCESS[] rgApplications, [Optional] uint nServices, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5, ArraySubType = UnmanagedType.LPWStr)] string[] rgsServiceNames);
        [DllImport("rstrtmgr", EntryPoint = "RmShutdown", SetLastError = false)]
        static extern int RmShutdownNative(uint dwSessionHandle, [Optional] RM_SHUTDOWN_TYPE lActionFlags, [In, Optional][MarshalAs(UnmanagedType.FunctionPtr)] RM_WRITE_STATUS_CALLBACK fnStatus);
        [DllImport("rstrtmgr", EntryPoint = "RmStartSession", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int RmStartSessionNative(out uint pSessionHandle, [Optional] uint dwSessionFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder strSessionKey);

    }

}