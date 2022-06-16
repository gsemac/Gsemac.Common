using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class User32 {

        // Public members

        public static IntPtr GetDC(IntPtr hwnd) {

            return GetDCNative(hwnd);

        }
        public static bool DestroyIcon(IntPtr hIcon) {

            return DestroyIconNative(hIcon);

        }
        public static bool ExitWindowsEx(uint uFlags, uint dwReason) {

            return ExitWindowsExNative(uFlags, dwReason);

        }
        public static bool LockWorkStation() {

            return LockWorkStationNative();

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
        public static bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags) {

            return SetWindowPosNative(hWnd, hWndInsertAfter, x, y, cX, cY, uFlags);

        }
        public static bool ShowWindow(IntPtr hWnd, int nCmdShow) {

            return ShowWindowNative(hWnd, nCmdShow);

        }

        // Private members

        [DllImport("user32", EntryPoint = "GetDC")]
        private static extern IntPtr GetDCNative(IntPtr hwnd);
        [DllImport("user32", EntryPoint = "DestroyIcon", SetLastError = true)]
        private static extern bool DestroyIconNative(IntPtr hIcon);
        [DllImport("user32", EntryPoint = "ExitWindowsEx", SetLastError = true)]
        private static extern bool ExitWindowsExNative(uint uFlags, uint dwReason);
        [DllImport("user32", EntryPoint = "LockWorkStation", SetLastError = true)]
        private static extern bool LockWorkStationNative();
        [DllImport("user32", EntryPoint = "ReleaseCapture")]
        public static extern bool ReleaseCaptureNative();
        [DllImport("user32", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDCNative(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, ref HDItemA lParam);
        [DllImport("user32", EntryPoint = "SetWindowPos", SetLastError = true)]
        public static extern bool SetWindowPosNative(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags);
        [DllImport("user32", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindowNative(IntPtr hWnd, int nCmdShow);

    }

}