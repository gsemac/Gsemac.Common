using System;

namespace Gsemac.Net.Curl {

    public static class LibCurl {

        // Public members

        public static string CABundlePath {
            get => Environment.Is64BitProcess ? "x64/curl-ca-bundle.crt" : "x86/curl-ca-bundle.crt";
        }
        public static string CurlExecutablePath {
            get => Environment.Is64BitProcess ? "x64/curl.exe" : "x86/curl.exe";
        }
        public static string LibCurlPath {
            get => Environment.Is64BitProcess ? "x64/libcurl-x64.dll" : "x86/libcurl.dll";
        }

        public static CurlCode GlobalInit(CurlGlobal flags = CurlGlobal.Default) {

            lock (globalInitMutex) {

                // curl_global_init is reference counted, so repeated calls are okay.

                return Environment.Is64BitProcess ?
                    LibCurlNativeMethods.GlobalInit64(flags) :
                    LibCurlNativeMethods.GlobalInit32(flags);

            }

        }
        public static void GlobalCleanup() {

            lock (globalInitMutex) {

                if (Environment.Is64BitProcess)
                    LibCurlNativeMethods.GlobalCleanup64();
                else
                    LibCurlNativeMethods.GlobalCleanup32();

            }

        }

        public static CurlEasyHandle EasyInit() {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasyInit64() :
                LibCurlNativeMethods.EasyInit32();

        }
        public static void EasyCleanup(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                LibCurlNativeMethods.EasyCleanup64(handle);
            else
                LibCurlNativeMethods.EasyCleanup32(handle);

        }

        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, int value) {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasySetOpt64(handle, option, value) :
                LibCurlNativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, IntPtr value) {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasySetOpt64(handle, option, value) :
                LibCurlNativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, string value) {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasySetOpt64(handle, option, value) :
                LibCurlNativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, WriteCallback value) {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasySetOpt64(handle, option, value) :
                LibCurlNativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ProgressCallback value) {

            return Environment.Is64BitProcess ?
                LibCurlNativeMethods.EasySetOpt64(handle, option, value) :
                LibCurlNativeMethods.EasySetOpt32(handle, option, value);

        }

        public static CurlCode Perform(CurlEasyHandle handle) {

            return Environment.Is64BitProcess ?
              LibCurlNativeMethods.Perform64(handle) :
              LibCurlNativeMethods.Perform32(handle);

        }

        public static void EasyReset(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                LibCurlNativeMethods.EasyReset64(handle);
            else
                LibCurlNativeMethods.EasyReset32(handle);

        }

        // Private members

        private static readonly object globalInitMutex = new object();

    }

}