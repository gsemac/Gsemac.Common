using System;
using System.Threading;

namespace Gsemac.Polyfills.System.Threading {

    public static class TimeoutEx {

        // Public members

        /// <summary>
        /// A constant used to specify an infinite waiting period, for methods that accept a <see cref="TimeSpan" /> parameter.
        /// </summary>
#if NET40_OR_LESSER
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1); // Added in .NET Framework 4.5
#else
        public static readonly TimeSpan InfiniteTimeSpan = Timeout.InfiniteTimeSpan;
#endif

    }

}