namespace Gsemac.Win32.Native {

    public static partial class Constants {

        public const int SE_PRIVILEGE_ENABLED = 0x00000002;

        public const int TOKEN_QUERY = 0x00000008;
        public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

    }

}