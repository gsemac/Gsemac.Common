using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class User32 {

        // Public members

        public static IntPtr GetDC(IntPtr hwnd) {

            return GetDCNative(hwnd);

        }
        public static bool ReleaseCapture() {

            return ReleaseCaptureNative();

        }
        public static IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc) {

            return ReleaseDCNative(hwnd, hdc);

        }
        public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam) {

            return SendMessageNative(hWnd, wMsg, wParam, lParam);

        }
        public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, ref HDItemA lParam) {

            return SendMessageNative(hWnd, wMsg, wParam, ref lParam);

        }

        // Private members

        [DllImport("user32", EntryPoint = "GetDC")]
        private static extern IntPtr GetDCNative(IntPtr hwnd);
        [DllImport("user32", EntryPoint = "ReleaseCapture")]
        public static extern bool ReleaseCaptureNative();
        [DllImport("user32", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDCNative(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, ref HDItemA lParam);

    }

}