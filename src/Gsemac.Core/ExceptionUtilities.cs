using System;
using System.Reflection;

namespace Gsemac.Core {

    public static class ExceptionUtilities {

        // Public members

        public static int GetHResult(Exception exception) {

            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

            return GetHResultPropertyValue(exception);

        }
        public static int GetWin32ErrorCode(Exception exception) {

            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

            int hResult = GetHResult(exception);

            return hResult & 0xFFFF;

        }

        // Private members

        private static int GetHResultPropertyValue(Exception exception) {

            // Prior to .NET 4.5, the HResult property getter was protected.
            // For earlier versions of .NET, we'll need to use reflection to get its value.

            // Marshal.GetHRForException exists, but has significant side effects:
            // https://docs.microsoft.com/en-us/archive/blogs/yizhang/marshal-gethrforexception-does-more-than-just-get-hr-for-exception

            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

#if NET40_OR_LESSER

            PropertyInfo hResultProperty = exception.GetType().GetProperty("HResult", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (hResultProperty is null)
                return 0;

            return (int)hResultProperty.GetValue(exception, null);

#else
            return exception.HResult;
#endif

        }

    }

}