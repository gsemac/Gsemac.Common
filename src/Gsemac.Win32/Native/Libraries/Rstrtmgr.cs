using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Gsemac.Win32.Native {

    public delegate void RM_WRITE_STATUS_CALLBACK(int progress);

    [StructLayout(LayoutKind.Sequential)]
    public struct RM_UNIQUE_PROCESS {

        public int dwProcessId;
        public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RM_PROCESS_INFO {

        public RM_UNIQUE_PROCESS Process;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.CCH_RM_MAX_APP_NAME + 1)]
        public string strAppName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.CCH_RM_MAX_SVC_NAME + 1)]
        public string strServiceShortName;
        public int ApplicationType;
        public uint AppStatus;
        public uint TSSessionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bRestartable;

    }

    public static class Rstrtmgr {

        // Public members

        public static int RmEndSession(uint pSessionHandle) {

            return RmEndSessionNative(pSessionHandle);

        }
        public static int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons) {

            return RmGetListNative(dwSessionHandle, out pnProcInfoNeeded, ref pnProcInfo, rgAffectedApps, ref lpdwRebootReasons);

        }
        public static int RmRegisterResources(uint dwSessionHandle, uint nFiles, string[] rgsFileNames, uint nApplications, RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames) {

            return RmRegisterResourcesNative(dwSessionHandle, nFiles, rgsFileNames, nApplications, rgApplications, nServices, rgsServiceNames);

        }
        public static int RmShutdown(uint dwSessionHandle, int lActionFlags, RM_WRITE_STATUS_CALLBACK fnStatus) {

            return RmShutdownNative(dwSessionHandle, lActionFlags, fnStatus);

        }
        public static int RmStartSession(out uint pSessionHandle, uint dwSessionFlags, StringBuilder strSessionKey) {

            return RmStartSessionNative(out pSessionHandle, dwSessionFlags, strSessionKey);

        }

        // Private members

        [DllImport("rstrtmgr", EntryPoint = "RmEndSession", SetLastError = true)]
        static extern int RmEndSessionNative(uint pSessionHandle);
        [DllImport("rstrtmgr", EntryPoint = "RmGetList", SetLastError = false)]
        static extern int RmGetListNative(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);
        [DllImport("rstrtmgr", EntryPoint = "RmRegisterResources", CharSet = CharSet.Unicode, SetLastError = false)]
        static extern int RmRegisterResourcesNative(uint dwSessionHandle, [Optional] uint nFiles, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.LPWStr)] string[] rgsFileNames, [Optional] uint nApplications, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] RM_UNIQUE_PROCESS[] rgApplications, [Optional] uint nServices, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5, ArraySubType = UnmanagedType.LPWStr)] string[] rgsServiceNames);
        [DllImport("rstrtmgr", EntryPoint = "RmShutdown", SetLastError = false)]
        static extern int RmShutdownNative(uint dwSessionHandle, [Optional] int lActionFlags, [In, Optional][MarshalAs(UnmanagedType.FunctionPtr)] RM_WRITE_STATUS_CALLBACK fnStatus);
        [DllImport("rstrtmgr", EntryPoint = "RmStartSession", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int RmStartSessionNative(out uint pSessionHandle, [Optional] uint dwSessionFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder strSessionKey);

    }

}