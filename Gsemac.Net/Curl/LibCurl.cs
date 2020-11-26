using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Gsemac.Net.Curl {

    public static class LibCurl {

        public static CurlCode GlobalInit(CurlGlobal flags = CurlGlobal.Default) {

            return Environment.Is64BitProcess ?
                NativeMethods.GlobalInit64(flags) :
                NativeMethods.GlobalInit32(flags);

        }
        public static void GlobalCleanup() {

            if (Environment.Is64BitProcess)
                NativeMethods.GlobalCleanup64();
            else
                NativeMethods.GlobalCleanup32();

        }

        public static CurlEasyHandle EasyInit() {

            return Environment.Is64BitProcess ?
                NativeMethods.EasyInit64() :
                NativeMethods.EasyInit32();

        }
        public static void EasyCleanup(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                NativeMethods.EasyCleanup64(handle);
            else
                NativeMethods.EasyCleanup32(handle);

        }

        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, int value) {

            return Environment.Is64BitProcess ?
                NativeMethods.EasySetOpt64(handle, option, value) :
                NativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, IntPtr value) {

            return Environment.Is64BitProcess ?
                NativeMethods.EasySetOpt64(handle, option, value) :
                NativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, string value) {

            return Environment.Is64BitProcess ?
                NativeMethods.EasySetOpt64(handle, option, value) :
                NativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, WriteCallback value) {

            return Environment.Is64BitProcess ?
                NativeMethods.EasySetOpt64(handle, option, value) :
                NativeMethods.EasySetOpt32(handle, option, value);

        }
        public static CurlCode EasySetOpt(CurlEasyHandle handle, CurlOption option, ProgressCallback value) {

            return Environment.Is64BitProcess ?
                NativeMethods.EasySetOpt64(handle, option, value) :
                NativeMethods.EasySetOpt32(handle, option, value);

        }

        public static CurlCode Perform(CurlEasyHandle handle) {

            return Environment.Is64BitProcess ?
              NativeMethods.Perform64(handle) :
              NativeMethods.Perform32(handle);

        }

        public static void EasyReset(CurlEasyHandle handle) {

            if (Environment.Is64BitProcess)
                NativeMethods.EasyReset64(handle);
            else
                NativeMethods.EasyReset32(handle);

        }

    }

}