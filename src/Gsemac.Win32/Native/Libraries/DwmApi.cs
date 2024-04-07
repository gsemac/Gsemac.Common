using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class DwmApi {

        // Public members

        public static void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute) {

            DwmSetWindowAttributeNative(hwnd, attribute, ref pvAttribute, cbAttribute);

        }

        // Private members

        [DllImport("dwmapi", EntryPoint = "DwmSetWindowAttribute", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void DwmSetWindowAttributeNative(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

    }

}