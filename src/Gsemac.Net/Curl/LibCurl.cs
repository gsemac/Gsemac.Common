using System;
using System.IO;
using System.Linq;

namespace Gsemac.Net.Curl {

    public static class LibCurl {

        // Public members

        public static string CABundlePath {
            get => GetCABundlePath();
            set => caBundlePath = value;
        }
        public static string CurlExecutablePath {
            get => GetCurlExecutablePath();
            set => curlExecutablePath = value;
        }
        public static string LibCurlPath {
            get => GetLibCurlPath();
            set => libCurlPath = value;
        }

        public static bool IsInitialized {
            get {

                lock (globalInitMutex)
                    return globalInitRefCount > 0;

            }
        }

        public static CurlCode GlobalInit(CurlGlobal flags = CurlGlobal.Default) {

            lock (globalInitMutex) {

                // curl_global_init is reference counted, but I do manual reference counting in order to implement IsInitialized.

                ++globalInitRefCount;

                return Environment.Is64BitProcess ?
                    LibCurlNative.GlobalInit64(flags) :
                    LibCurlNative.GlobalInit32(flags);

            }

        }
        public static void GlobalCleanup() {

            lock (globalInitMutex) {

                if (globalInitRefCount == 1) {

                    if (Environment.Is64BitProcess)
                        LibCurlNative.GlobalCleanup64();
                    else
                        LibCurlNative.GlobalCleanup32();

                }

                if (globalInitRefCount > 0)
                    --globalInitRefCount;

            }

        }

        public static CurlEasyHandle EasyInit() {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasyInit64() :
                LibCurlNative.EasyInit32();

        }
        public static void EasyCleanup(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                LibCurlNative.EasyCleanup64(handle);
            else
                LibCurlNative.EasyCleanup32(handle);

        }

        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, int value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, IntPtr value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, string value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, WriteFunctionDelegate value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ProgressFunctionDelegate value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ReadFunctionDelegate value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, SListHandle value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }

        public static CurlCode EasyPerform(CurlEasyHandle handle) {

            return Environment.Is64BitProcess ?
              LibCurlNative.EasyPerform64(handle) :
              LibCurlNative.EasyPerform32(handle);

        }

        public static void EasyReset(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                LibCurlNative.EasyReset64(handle);
            else
                LibCurlNative.EasyReset32(handle);

        }

        public static SListHandle SListAppend(SListHandle handle, string @string) {

            return Environment.Is64BitProcess ?
                   LibCurlNative.SListAppend64(handle, @string) :
                   LibCurlNative.SListAppend32(handle, @string);


        }
        public static void SListFreeAll(SListHandle handle) {

            if (Environment.Is64BitProcess)
                LibCurlNative.SListFreeAll64(handle);
            else
                LibCurlNative.SListFreeAll32(handle);

        }

        // Private members

        private static readonly object globalInitMutex = new object();
        private static int globalInitRefCount = 0;
        private static string caBundlePath;
        private static string curlExecutablePath;
        private static string libCurlPath;

        private static string GetCABundlePath() {

            if (string.IsNullOrEmpty(caBundlePath)) {

                string caBundleFilename = "curl-ca-bundle.crt";
                string libDirectory = "lib";

                caBundlePath = new string[] {
                    caBundleFilename,
                    Path.Combine(libDirectory, caBundleFilename),
                    Environment.Is64BitProcess ? Path.Combine("x64", caBundleFilename) : Path.Combine("x86", caBundleFilename),
                    Environment.Is64BitProcess ? Path.Combine(libDirectory, "x64", caBundleFilename) : Path.Combine(libDirectory, "x86", caBundleFilename),
                }.FirstOrDefault(file => File.Exists(file));

            }

            return caBundlePath;

        }
        private static string GetCurlExecutablePath() {

            if (string.IsNullOrEmpty(curlExecutablePath)) {

                string caExecutableFilename = "curl.exe";
                string binDirectory = "bin";

                curlExecutablePath = new string[] {
                    caExecutableFilename,
                    Path.Combine(binDirectory, caExecutableFilename),
                    Environment.Is64BitProcess ? Path.Combine("x64", caExecutableFilename) : Path.Combine("x86", caExecutableFilename),
                    Environment.Is64BitProcess ? Path.Combine(binDirectory, "x64", caExecutableFilename) : Path.Combine(binDirectory, "x86", caExecutableFilename),
                }.FirstOrDefault(file => File.Exists(file));

            }

            return curlExecutablePath;

        }
        private static string GetLibCurlPath() {

            if (string.IsNullOrEmpty(libCurlPath)) {

                string libCurlFilename = Environment.Is64BitProcess ? "libcurl-x64.dll" : "libcurl.dll";
                string binDirectory = "lib";

                libCurlPath = new string[] {
                    libCurlFilename,
                    Path.Combine(binDirectory, libCurlFilename),
                    Environment.Is64BitProcess ? Path.Combine("x64", libCurlFilename) : Path.Combine("x86", libCurlFilename),
                    Environment.Is64BitProcess ? Path.Combine(binDirectory, "x64", libCurlFilename) : Path.Combine(binDirectory, "x86", libCurlFilename),
                }.FirstOrDefault(file => File.Exists(file));

            }

            return libCurlPath;

        }

    }

}