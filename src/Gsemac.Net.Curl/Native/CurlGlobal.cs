using System;

namespace Gsemac.Net.Curl.Native {

    [Flags]
    public enum CurlGlobal {

        /// <summary>
        /// Initialize everything possible. This sets all known bits except <see cref="AckEintr"/>.
        /// </summary>
        All = Ssl | Win32,
        /// <summary>
        /// Initialize SSL.
        /// </summary>
        Ssl = 1 << 0,
        /// <summary>
        /// Initialize the Win32 socket libraries.
        /// </summary>
        Win32 = 1 << 1,
        /// <summary>
        /// Initialize nothing extra. This sets no bit.
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// A sensible default. It initializes both SSL and Win32. Right now, this equals the functionality of the <see cref="All"/> mask.
        /// </summary>
        Default = All,
        /// <summary>
        /// When this flag is set, curl acknowledges EINTR condition when connecting or when waiting for data. Otherwise, curl waits until full timeout elapses.
        /// </summary>
        AckEintr = 1 << 2,

    }

}