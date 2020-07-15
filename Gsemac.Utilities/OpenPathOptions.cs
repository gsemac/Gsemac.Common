using System;

namespace Gsemac.Utilities {

    [Flags]
    public enum OpenPathOptions {
        None,
        Default = None,
        /// <summary>
        /// Open the path a new window even if it is already open.
        /// </summary>
        NewWindow
    }

}
