namespace Gsemac.Win32.Native {

    public enum DWMWINDOWATTRIBUTE {
        /// <summary>
        /// Is non-client rendering enabled/disabled.
        /// </summary>
        DWMWA_NCRENDERING_ENABLED = 1,
        /// <summary>
        /// Non-client rendering policy.
        /// </summary>
        DWMWA_NCRENDERING_POLICY,
        /// <summary>
        /// Potentially enable/forcibly disable transitions.
        /// </summary>
        DWMWA_TRANSITIONS_FORCEDISABLED,
        /// <summary>
        /// Allow contents rendered in the non-client area to be visible on the DWM-drawn frame.
        /// </summary>
        DWMWA_ALLOW_NCPAINT,
        /// <summary>
        /// Bounds of the caption button area in window-relative space.
        /// </summary>
        DWMWA_CAPTION_BUTTON_BOUNDS,
        /// <summary>
        ///  Is non-client content RTL mirrored.
        /// </summary>
        DWMWA_NONCLIENT_RTL_LAYOUT,
        /// <summary>
        /// Force this window to display iconic thumbnails.
        /// </summary>
        DWMWA_FORCE_ICONIC_REPRESENTATION,
        /// <summary>
        /// Designates how Flip3D will treat the window.
        /// </summary>
        DWMWA_FLIP3D_POLICY,
        /// <summary>
        /// Gets the extended frame bounds rectangle in screen space.
        /// </summary>
        DWMWA_EXTENDED_FRAME_BOUNDS,
        /// <summary>
        /// Indicates an available bitmap when there is no better thumbnail representation.
        /// </summary>
        DWMWA_HAS_ICONIC_BITMAP,
        /// <summary>
        /// Don't invoke Peek on the window.
        /// </summary>
        DWMWA_DISALLOW_PEEK,
        /// <summary>
        /// LivePreview exclusion information.
        /// </summary>
        DWMWA_EXCLUDED_FROM_PEEK,
        /// <summary>
        /// Cloak or uncloak the window.
        /// </summary>
        DWMWA_CLOAK,
        /// <summary>
        /// Gets the cloaked state of the window.
        /// </summary>
        DWMWA_CLOAKED,
        /// <summary>
        /// Force this window to freeze the thumbnail without live update.
        /// </summary>
        DWMWA_FREEZE_REPRESENTATION,
        DWMWA_PASSIVE_UPDATE_MODE,
        DWMWA_USE_HOSTBACKDROPBRUSH,
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_WINDOW_CORNER_PREFERENCE = 33,
        DWMWA_BORDER_COLOR,
        DWMWA_CAPTION_COLOR,
        DWMWA_TEXT_COLOR,
        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
        DWMWA_SYSTEMBACKDROP_TYPE,
        DWMWA_LAST,
    };

}