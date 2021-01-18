using System;

namespace Gsemac.IO {

    [Flags]
    public enum OpenDirectoryOptions {
        None,
        Default = None,
        /// <summary>
        /// Open the path a new window even if it is already open.
        /// </summary>
        NewWindow
    }

}
