namespace Gsemac.Win32.Native {

    public static partial class Constants {

        public const int CCH_RM_MAX_APP_NAME = 255;
        public const int CCH_RM_MAX_SVC_NAME = 63;

        public const int RmRebootReasonNone = 0;

        public const int RmUnknownApp = 0;
        public const int RmMainWindow = 1;
        public const int RmOtherWindow = 2;
        public const int RmService = 3;
        public const int RmExplorer = 4;
        public const int RmConsole = 5;
        public const int RmCritical = 1000;

        // _RM_SHUTDOWN_TYPE 

        public const int RmForceShutdown = 0x1;
        public const int RmShutdownOnlyRegistered = 0x10;

    }

}