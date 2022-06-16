using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class PowrProf {

        // Public members

        public static uint SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent) {

            return SetSuspendStateNative(hibernate, forceCritical, disableWakeEvent);

        }

        // Private members

        [DllImport("Powrprof", EntryPoint = "SetSuspendState", SetLastError = true)]
        static extern uint SetSuspendStateNative(bool hibernate, bool forceCritical, bool disableWakeEvent);

    }

}