using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32.Native {

    public static class WinINet {

        // Public members

        /// <summary>
        /// Creates a cookie associated with the specified URL.
        /// </summary>
        /// <param name="lpszUrlName">Pointer to a null-terminated string that specifies the URL for which the cookie should be set.</param>
        /// <param name="lpszCookieName">Pointer to a null-terminated string that specifies the name to be associated with the cookie data. If this parameter is <see langword="null"/>, no name is associated with the cookie.</param>
        /// <param name="lpszCookieData">Pointer to the actual data to be associated with the URL.</param>
        /// <returns>Returns <see langword="true"/> if successful, or <see langword="false"/> otherwise. To get a specific error message, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        public static bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData) {

            return InternetSetCookieNative(lpszUrlName, lpszCookieName, lpszCookieData);

        }
        /// <summary>
        /// The InternetSetCookieEx function creates a cookie with a specified name that is associated with a specified URL. This function differs from the <see cref="InternetSetCookie"/> function by being able to create third-party cookies.
        /// </summary>
        /// <param name="lpszUrlName">Pointer to a null-terminated string that specifies the URL for which the cookie should be set.</param>
        /// <param name="lpszCookieName">Pointer to a null-terminated string that specifies the name to be associated with the cookie data. If this parameter is <see langword="null"/>, no name is associated with the cookie.</param>
        /// <param name="lpszCookieData">Pointer to a null-terminated string that contains the data to be associated with the new cookie.<para></para>If this pointer is <see langword="null"/>, <see cref="InternetSetCookieEx"/> fails with an ERROR_INVALID_PARAMETER error.</param>
        /// <param name="dwFlags">Flags that control how the function retrieves cookie data.</param>
        /// <param name="dwReserved"><see langword="null"/>, or contains a pointer to a Platform-for-Privacy-Protection (P3P) header to be associated with the cookie.</param>
        /// <returns>Returns a member of the InternetCookieState enumeration if successful, or <see langword="false"/> if the function fails. On failure, if a call to GetLastError returns ERROR_NOT_ENOUGH_MEMORY, insufficient system memory was available.</returns>
        public static bool InternetSetCookieEx(string lpszUrlName, string lpszCookieName, string lpszCookieData, uint dwFlags, IntPtr dwReserved) {

            return InternetSetCookieExNative(lpszUrlName, lpszCookieName, lpszCookieData, dwFlags, dwReserved);

        }

        // Private members

        [DllImport("wininet", EntryPoint = "InternetSetCookie", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookieNative(string lpszUrlName, string lpszCookieName, string lpszCookieData);
        [DllImport("wininet", EntryPoint = "InternetSetCookieEx", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookieExNative(string lpszUrlName, string lpszCookieName, string lpszCookieData, uint dwFlags, IntPtr dwReserved);

    }

}