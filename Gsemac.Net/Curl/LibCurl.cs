﻿using System;

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
                    LibCurlNative.GlobalInit64(flags) :
                    LibCurlNative.GlobalInit32(flags);

            }

        }
        public static void GlobalCleanup() {

            lock (globalInitMutex) {

                if (Environment.Is64BitProcess)
                    LibCurlNative.GlobalCleanup64();
                else
                    LibCurlNative.GlobalCleanup32();

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
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, WriteCallback value) {

            return Environment.Is64BitProcess ?
                LibCurlNative.EasySetOpt64(handle, option, value) :
                LibCurlNative.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ProgressCallback value) {

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

    }

}