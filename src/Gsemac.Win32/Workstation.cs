using Gsemac.Win32.Native;
using System;

namespace Gsemac.Win32 {

    public static class Workstation {

        // Public members

        public static bool Lock() {

            return User32.LockWorkStation();

        }
        public static bool LogOff() {

            return User32.ExitWindowsEx(Defines.EWX_LOGOFF, Defines.SHTDN_REASON_MAJOR_OTHER);

        }

        public static bool Sleep() {

            return PowrProf.SetSuspendState(false, true, true) != 0;

        }
        public static bool Hibernate() {

            return PowrProf.SetSuspendState(true, true, true) != 0;

        }
        public static bool Shutdown() {

            IntPtr processHandle = Kernel32.GetCurrentProcess();

            if (Advapi32.OpenProcessToken(processHandle, Defines.TOKEN_ADJUST_PRIVILEGES | Defines.TOKEN_QUERY, out IntPtr tokenHandle)) {

                TOKEN_PRIVILEGES tokenPrivileges = new TOKEN_PRIVILEGES {
                    PrivilegeCount = 1,
                    Privileges = new LUID_AND_ATTRIBUTES[] {
                        new LUID_AND_ATTRIBUTES() {
                            Luid = new LUID {
                                HighPart = 0,
                                LowPart = 0,
                            },
                            Attributes = Defines.SE_PRIVILEGE_ENABLED,
                        }
                    }
                };

                return Advapi32.LookupPrivilegeValue(null, Defines.SE_SHUTDOWN_NAME, ref tokenPrivileges.Privileges[0].Luid) &&
                    Advapi32.AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges) &&
                    User32.ExitWindowsEx(Defines.EWX_SHUTDOWN, Defines.SHTDN_REASON_MAJOR_OTHER);

            }

            // The process did not complete successfully.

            return false;

        }

    }

}