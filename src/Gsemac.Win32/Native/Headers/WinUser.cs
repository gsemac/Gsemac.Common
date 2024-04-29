namespace Gsemac.Win32.Native {

    public static partial class Constants {

        public const int HTCAPTION = 0x2;

        /// <summary>
        /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
        /// </summary>
        public const int HWND_NOTOPMOST = -2;
        /// <summary>
        /// Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        public const int HWND_TOPMOST = -1;
        /// <summary>
        /// Places the window at the top of the Z order.
        /// </summary>
        public const int HWND_TOP = 0;
        /// <summary>
        /// Places the window at the bottom of the Z order.
        /// </summary>
        public const int HWND_BOTTOM = 1;

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
        /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        /// <summary>
        /// Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        public const int SWP_DEFERERASE = 0x2000;
        /// <summary>
        /// Draws a frame (defined in the window's class description) around the window.
        /// </summary>
        public const int SWP_DRAWFRAME = 0x0020;
        /// <summary>
        /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        public const int SWP_FRAMECHANGED = 0x0020;
        /// <summary>
        /// Hides the window.
        /// </summary>
        public const int SWP_HIDEWINDOW = 0x0080;
        /// <summary>
        /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOACTIVATE = 0x0010;
        /// <summary>
        /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        public const int SWP_NOCOPYBITS = 0x0100;
        /// <summary>
        /// Retains the current position (ignores X and Y parameters).
        /// </summary>
        public const int SWP_NOMOVE = 0x0002;
        /// <summary>
        /// Does not change the owner window's position in the Z order.
        /// </summary>
        public const int SWP_NOOWNERZORDER = 0x0200;
        /// <summary>
        /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        public const int SWP_NOREDRAW = 0x0008;
        /// <summary>
        /// Same as the <see cref="SWP_NOOWNERZORDER"/> flag.
        /// </summary>
        public const int SWP_NOREPOSITION = 0x0200;
        /// <summary>
        /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        public const int SWP_NOSENDCHANGING = 0x0400;
        /// <summary>
        /// Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        public const int SWP_NOSIZE = 0x0001;
        /// <summary>
        /// Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOZORDER = 0x0004;
        /// <summary>
        /// Displays the window.
        /// </summary>
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int WM_NCLBUTTONDOWN = 0xA1;

        public const int EWX_LOGOFF = 0x00000000;
        public const int EWX_SHUTDOWN = 0x00000001;
        public const int EWX_REBOOT = 0x00000002;
        public const int EWX_FORCE = 0x00000004;
        public const int EWX_POWEROFF = 0x00000008;
        public const int EWX_FORCEIFHUNG = 0x00000010;

        public const int SHTDN_REASON_MAJOR_OTHER = 0x00000000;

        /// <summary>
        /// Sets the position of the scroll box in a window's standard horizontal scroll bar.
        /// </summary>
        public const int SB_HORZ = 0;
        /// <summary>
        /// Sets the position of the scroll box in a window's standard vertical scroll bar.
        /// </summary>
        public const int SB_VERT = 1;
        /// <summary>
        /// Sets the position of the scroll box in a scroll bar control. The hwnd parameter must be the handle to the scroll bar control.
        /// </summary>
        public const int SB_CTL = 2;
        public const int SB_BOTH = 3;

        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;

    }

}