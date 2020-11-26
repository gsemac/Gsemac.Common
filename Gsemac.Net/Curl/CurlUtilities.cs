using System;

namespace Gsemac.Net.Curl {

    public static class CurlUtilities {

        // Public members

        public static string CABundlePath {
            get => Environment.Is64BitProcess ? "x64/curl-ca-bundle.crt" : "x86/curl-ca-bundle.crt";
        }
        public static string CurlExecutablePath {
            get => Environment.Is64BitProcess ? "x64/curl.exe" : "x86/curl.exe";
        }

        public static void ThreadSafeGlobalInit() {

            lock (globalInitMutex) {

                // curl_global_init is reference counted, so repeated calls are okay.

                LibCurl.GlobalInit();

            }

        }
        public static void ThreadSafeGlobalCleanup() {

            lock (globalInitMutex) {

                LibCurl.GlobalCleanup();

            }

        }

        // Private members

        private static readonly object globalInitMutex = new object();

    }

}