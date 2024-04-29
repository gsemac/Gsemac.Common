using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class User32 {

        // Public members

        public static IntPtr GetDC(IntPtr hWnd) {

            return GetDCNative(hWnd);

        }
        public static bool DestroyIcon(IntPtr hIcon) {

            return DestroyIconNative(hIcon);

        }
        public static bool ExitWindowsEx(uint uFlags, uint dwReason) {

            return ExitWindowsExNative(uFlags, dwReason);

        }
        public static IntPtr GetForegroundWindow() {

            return GetForegroundWindowNative();

        }
        public static int GetScrollPos(IntPtr hWnd, int nBar) {

            return GetScrollPosNative(hWnd, nBar);

        }
        public static bool GetWindowRect(IntPtr hWnd, ref RECT rectangle) {

            return GetWindowRectNative(hWnd, ref rectangle);

        }
        public static bool LockWindowUpdate(IntPtr hWndLock) {

            return LockWindowUpdateNative(hWndLock);

        }
        public static bool LockWorkStation() {

            return LockWorkStationNative();

        }
        public static bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint) {

            return MoveWindowNative(hWnd, x, y, nWidth, nHeight, bRepaint);

        }
        public static bool ReleaseCapture() {

            return ReleaseCaptureNative();

        }
        public static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc) {

            return ReleaseDCNative(hWnd, hdc);

        }
        public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam) {

            return SendMessageNative(hWnd, wMsg, wParam, lParam);

        }
        public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, ref HDItemA lParam) {

            return SendMessageNative(hWnd, wMsg, wParam, ref lParam);

        }
        public static bool SetForegroundWindow(IntPtr hWnd) {

            return SetForegroundWindowNative(hWnd);

        }
        public static bool SetProcessDPIAware() {

            return SetProcessDPIAwareNative();

        }
        public static int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw) {

            return SetScrollPosNative(hWnd, nBar, nPos, bRedraw);

        }
        public static bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags) {

            return SetWindowPosNative(hWnd, hWndInsertAfter, x, y, cX, cY, uFlags);

        }
        public static bool ShowWindow(IntPtr hWnd, int nCmdShow) {

            return ShowWindowNative(hWnd, nCmdShow);

        }

        // Private members

        [DllImport("user32", EntryPoint = "GetDC")]
        private static extern IntPtr GetDCNative(IntPtr hWnd);
        [DllImport("user32", EntryPoint = "DestroyIcon", SetLastError = true)]
        private static extern bool DestroyIconNative(IntPtr hIcon);
        [DllImport("user32", EntryPoint = "ExitWindowsEx", SetLastError = true)]
        private static extern bool ExitWindowsExNative(uint uFlags, uint dwReason);
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindowNative();
        [DllImport("user32", EntryPoint = "GetScrollPos", SetLastError = true)]
        private static extern int GetScrollPosNative(IntPtr hWnd, int nBar);
        [DllImport("user32")]
        public static extern bool GetWindowRectNative(IntPtr hWnd, ref RECT rectangle);
        [DllImport("user32", EntryPoint = "LockWindowUpdate", SetLastError = true)]
        private static extern bool LockWindowUpdateNative(IntPtr hWndLock);
        [DllImport("user32", EntryPoint = "LockWorkStation", SetLastError = true)]
        private static extern bool LockWorkStationNative();
        [DllImport("user32", SetLastError = true)]
        public static extern bool MoveWindowNative(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32", EntryPoint = "ReleaseCapture")]
        public static extern bool ReleaseCaptureNative();
        [DllImport("user32", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDCNative(IntPtr hWnd, IntPtr hdc);
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        private static extern bool SetForegroundWindowNative(IntPtr hWnd);
        [DllImport("user32", EntryPoint = "SetProcessDPIAware", SetLastError = true)]
        private static extern bool SetProcessDPIAwareNative();
        [DllImport("user32", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, ref HDItemA lParam);
        [DllImport("user32", EntryPoint = "SetScrollPos", SetLastError = true)]
        private static extern int SetScrollPosNative(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32", EntryPoint = "SetWindowPos", SetLastError = true)]
        private static extern bool SetWindowPosNative(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags);
        [DllImport("user32", EntryPoint = "ShowWindow", SetLastError = true)]
        private static extern bool ShowWindowNative(IntPtr hWnd, int nCmdShow);

    }

}