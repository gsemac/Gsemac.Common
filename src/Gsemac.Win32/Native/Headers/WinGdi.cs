namespace Gsemac.Win32.Native {

    public static partial class Constants {

        /// <summary>
        /// Logical units are mapped to arbitrary units with equally scaled axes; that is, one unit along the x-axis is equal to one unit along the y-axis. Use the SetWindowExtEx and SetViewportExtEx functions to specify the units and the orientation of the axes. Graphics device interface (GDI) makes adjustments as necessary to ensure the x and y units remain the same size (When the window extent is set, the viewport will be adjusted to keep the units isotropic).
        /// </summary>
        public const int MM_ISOTROPIC = 7;
        /// <summary>
        /// Logical units are mapped to arbitrary units with arbitrarily scaled axes. Use the SetWindowExtEx and SetViewportExtEx functions to specify the units, orientation, and scaling.
        /// </summary>
        public const int MM_ANISOTROPIC = 8;

    }

}