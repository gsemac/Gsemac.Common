using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class DwmApi {

        // Public members

        public static void DwmSetWindowAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute) {

            DwmSetWindowAttributeNative(hWnd, attribute, ref pvAttribute, cbAttribute);

        }

        // Private members

        [DllImport("dwmapi", EntryPoint = "DwmSetWindowAttribute", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void DwmSetWindowAttributeNative(IntPtr hWnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

    }

}