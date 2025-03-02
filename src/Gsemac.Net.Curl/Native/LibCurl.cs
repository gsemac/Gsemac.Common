using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl.Native {

    public static class LibCurl {

        // Public members

        public static CurlCode GlobalInit(CurlGlobal flags = CurlGlobal.Default) {

            lock (globalInitMutex) {

                // Note that "curl_global_init" is already reference counted since version 7.19.0.
                // However, I ran into an issue where repeated calls to "curl_global_cleanup" would cause a crash.
                // We'll do manual reference counting just to be safe.

                ++globalInitRefCount;

                return Environment.Is64BitProcess ?
                    GlobalInit64(flags) :
                    GlobalInit32(flags);

            }

        }
        public static void GlobalCleanup() {

            lock (globalInitMutex) {

                if (globalInitRefCount == 1) {

                    if (Environment.Is64BitProcess)
                        GlobalCleanup64();
                    else
                        GlobalCleanup32();

                }

                if (globalInitRefCount > 0)
                    --globalInitRefCount;

            }

        }

        public static CurlEasyHandle EasyInit() {

            return Environment.Is64BitProcess ?
                EasyInit64() :
                EasyInit32();

        }
        public static void EasyCleanup(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                EasyCleanup64(handle);
            else
                EasyCleanup32(handle);

        }

        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, int value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, IntPtr value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, string value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, WriteFunctionCallback value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ProgressFunctionCallback value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ReadFunctionCallback value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, SListHandle value) {

            return Environment.Is64BitProcess ?
                EasySetOpt64(handle, option, value) :
                EasySetOpt32(handle, option, value);

        }

        public static CurlCode EasyPerform(CurlEasyHandle handle) {

            return Environment.Is64BitProcess ?
              EasyPerform64(handle) :
              EasyPerform32(handle);

        }

        public static void EasyReset(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                EasyReset64(handle);
            else
                EasyReset32(handle);

        }

        public static SListHandle SListAppend(SListHandle handle, string @string) {

            return Environment.Is64BitProcess ?
                   SListAppend64(handle, @string) :
                   SListAppend32(handle, @string);


        }
        public static void SListFreeAll(SListHandle handle) {

            if (Environment.Is64BitProcess)
                SListFreeAll64(handle);
            else
                SListFreeAll32(handle);

        }

        // Private members

        private static readonly object globalInitMutex = new object();
        private static int globalInitRefCount = 0;

        [DllImport(X86DllPath, EntryPoint = "curl_global_init")]
        private static extern CurlCode GlobalInit32(CurlGlobal flags = CurlGlobal.Default);
        [DllImport(X64DllPath, EntryPoint = "curl_global_init")]
        private static extern CurlCode GlobalInit64(CurlGlobal flags = CurlGlobal.Default);

        [DllImport(X86DllPath, EntryPoint = "curl_global_cleanup")]
        private static extern void GlobalCleanup32();
        [DllImport(X64DllPath, EntryPoint = "curl_global_cleanup")]
        private static extern void GlobalCleanup64();

        [DllImport(X86DllPath, EntryPoint = "curl_easy_init")]
        private static extern CurlEasyHandle EasyInit32();
        [DllImport(X64DllPath, EntryPoint = "curl_easy_init")]
        private static extern CurlEasyHandle EasyInit64();

        [DllImport(X86DllPath, EntryPoint = "curl_easy_cleanup")]
        private static extern void EasyCleanup32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_cleanup")]
        private static extern void EasyCleanup64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, int value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, int value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, IntPtr value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, IntPtr value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, string value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, string value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, WriteFunctionCallback value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, WriteFunctionCallback value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, ProgressFunctionCallback value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, ProgressFunctionCallback value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, ReadFunctionCallback value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, ReadFunctionCallback value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, SListHandle value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        private static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, SListHandle value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_perform")]
        private static extern CurlCode EasyPerform32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_perform")]
        private static extern CurlCode EasyPerform64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_reset")]
        private static extern void EasyReset32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_reset")]
        private static extern void EasyReset64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_slist_append", CharSet = CharSet.Ansi)]
        private static extern SListHandle SListAppend32(SListHandle handle, string @string);
        [DllImport(X64DllPath, EntryPoint = "curl_slist_append", CharSet = CharSet.Ansi)]
        private static extern SListHandle SListAppend64(SListHandle handle, string @string);

        [DllImport(X86DllPath, EntryPoint = "curl_slist_free_all")]
        private static extern void SListFreeAll32(SListHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_slist_free_all")]
        private static extern void SListFreeAll64(SListHandle handle);

        // Note: .NET Framework can't find the DLL with forward slashes, but finds it with backslashes.

        private const string X86DllPath = @"x86\\libcurl";
        private const string X64DllPath = @"x64\\libcurl-x64";

    }

}