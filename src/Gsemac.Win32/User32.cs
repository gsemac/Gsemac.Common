﻿using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class User32 {

        // Public members

        public const int HTCAPTION = 0x2;

        public const int HWND_TOPMOST = -1;

        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        public const int SW_HIDE = 0;
        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        public const int SW_SHOWNORMAL = 1;
        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        public const int SW_NORMAL = 1;
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        public const int SW_MAXIMIZE = 3;
        /// <summary>
        /// Displays a window in its most recent size and position. This value is similar to <see cref="SW_SHOWNORMAL"/>, except that the window is not activated.
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;
        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        public const int SW_SHOW = 5;
        /// <summary>
        /// Minimizes the specified window and activates the next top-level window in the Z order.
        /// </summary>
        public const int SW_MINIMIZE = 6;
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to <see cref="SW_SHOWMINIMIZED"/>, except the window is not activated.
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;
        /// <summary>
        /// Displays the window in its current size and position. This value is similar to <see cref="SW_SHOW"/>, except that the window is not activated.
        /// </summary>
        public const int SW_SHOWNA = 8;
        /// <summary>
        /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
        /// </summary>
        public const int SW_RESTORE = 9;
        /// <summary>
        /// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
        /// </summary>
        public const int SW_SHOWDEFAULT = 10;
        /// <summary>
        /// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;

        /// <summary>
        /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOACTIVATE = 0x10;

        public const int WM_NCLBUTTONDOWN = 0xA1;

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
        public static bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags) {

            return SetWindowPosNative(hWnd, hWndInsertAfter, x, y, cX, cY, uFlags);

        }
        public static bool ShowWindow(IntPtr hWnd, int nCmdShow) {

            return ShowWindowNative(hWnd, nCmdShow);

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
        [DllImport("user32", EntryPoint = "SetWindowPos", SetLastError = true)]
        public static extern bool SetWindowPosNative(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int uFlags);
        [DllImport("user32", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindowNative(IntPtr hWnd, int nCmdShow);

    }

}