using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public delegate void RM_WRITE_STATUS_CALLBACK(int progress);

    public static partial class Constants {

        public const int CCH_RM_MAX_APP_NAME = 255;
        public const int CCH_RM_MAX_SVC_NAME = 63;

    }

    public enum RM_APP_TYPE {
        /// <summary>
        /// The application cannot be classified as any other type. An application of this type can only be shut down by a forced shutdown.
        /// </summary>
        RmUnknownApp = 0,
        /// <summary>
        /// A Windows application run as a stand-alone process that displays a top-level window.
        /// </summary>
        RmMainWindow = 1,
        /// <summary>
        /// A Windows application that does not run as a stand-alone process and does not display a top-level window.
        /// </summary>
        RmOtherWindow = 2,
        /// <summary>
        /// The application is a Windows service.
        /// </summary>
        RmService = 3,
        /// <summary>
        /// The application is Windows Explorer.
        /// </summary>
        RmExplorer = 4,
        /// <summary>
        /// The application is a stand-alone console application.
        /// </summary>
        RmConsole = 5,
        /// <summary>
        /// A system restart is required to complete the installation because a process cannot be shut down. The process cannot be shut down because of the following reasons. The process may be a critical process. The current user may not have permission to shut down the process. The process may belong to the primary installer that started the Restart Manager.
        /// </summary>
        RmCritical = 1000,
    }

    [Flags]
    public enum RM_REBOOT_REASON {
        /// <summary>
        /// A system restart is not required.
        /// </summary>
        RmRebootReasonNone = 0,
        /// <summary>
        /// The current user does not have sufficient privileges to shut down one or more processes.
        /// </summary>
        RmRebootReasonPermissionDenied = 0x1,
        /// <summary>
        /// One or more processes are running in another Terminal Services session.
        /// </summary>
        RmRebootReasonSessionMismatch = 0x2,
        /// <summary>
        /// A system restart is needed because one or more processes to be shut down are critical processes.
        /// </summary>
        RmRebootReasonCriticalProcess = 0x4,
        /// <summary>
        /// A system restart is needed because one or more services to be shut down are critical services.
        /// </summary>
        RmRebootReasonCriticalService = 0x8,
        /// <summary>
        /// A system restart is needed because the current process must be shut down.
        /// </summary>
        RmRebootReasonDetectedSelf = 0x10,
    }

    public enum RM_SHUTDOWN_TYPE {
        /// <summary>
        /// Forces unresponsive applications and services to shut down after the timeout period. An application that does not respond to a shutdown request by the Restart Manager is forced to shut down after 30 seconds. A service that does not respond to a shutdown request is forced to shut down after 20 seconds.
        /// </summary>
        RmForceShutdown = 0x1,
        /// <summary>
        /// Shuts down applications if and only if all the applications have been registered for restart using the RegisterApplicationRestart function. If any processes or services cannot be restarted, then no processes or services are shut down.
        /// </summary>
        RmShutdownOnlyRegistered = 0x10,
    }

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
        public RM_APP_TYPE ApplicationType;
        public uint AppStatus;
        public uint TSSessionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bRestartable;

    }

}