using System;
using System.Runtime.InteropServices;

namespace Gsemac.Win32 {

    public static class WinINet {

        // Public members

        public static bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData) {

            return InternetSetCookieNative(lpszUrlName, lpszCookieName, lpszCookieData);

        }
        public static bool InternetSetCookieEx(string lpszUrlName, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved) {

            return InternetSetCookieExNative(lpszUrlName, lpszCookieName, lpszCookieData, dwFlags, dwReserved);

        }

        // Private members

        [DllImport("wininet", EntryPoint = "InternetSetCookie", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookieNative(string lpszUrlName, string lpszCookieName, string lpszCookieData);
        [DllImport("wininet", EntryPoint = "InternetSetCookieEx", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookieExNative(string lpszUrlName, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

    }

}