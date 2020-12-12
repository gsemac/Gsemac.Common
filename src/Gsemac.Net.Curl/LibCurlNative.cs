using System;
using System.Runtime.InteropServices;

namespace Gsemac.Net.Curl {

    internal static class LibCurlNative {

        // Public members

        [DllImport(X86DllPath, EntryPoint = "curl_global_init")]
        public static extern CurlCode GlobalInit32(CurlGlobal flags = CurlGlobal.Default);
        [DllImport(X64DllPath, EntryPoint = "curl_global_init")]
        public static extern CurlCode GlobalInit64(CurlGlobal flags = CurlGlobal.Default);

        [DllImport(X86DllPath, EntryPoint = "curl_global_cleanup")]
        public static extern void GlobalCleanup32();
        [DllImport(X64DllPath, EntryPoint = "curl_global_cleanup")]
        public static extern void GlobalCleanup64();

        [DllImport(X86DllPath, EntryPoint = "curl_easy_init")]
        public static extern CurlEasyHandle EasyInit32();
        [DllImport(X64DllPath, EntryPoint = "curl_easy_init")]
        public static extern CurlEasyHandle EasyInit64();

        [DllImport(X86DllPath, EntryPoint = "curl_easy_cleanup")]
        public static extern void EasyCleanup32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_cleanup")]
        public static extern void EasyCleanup64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, int value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, int value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, IntPtr value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, IntPtr value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, string value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, string value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, WriteFunctionDelegate value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, WriteFunctionDelegate value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, ProgressFunctionDelegate value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, ProgressFunctionDelegate value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, ReadFunctionDelegate value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, ReadFunctionDelegate value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt32(CurlEasyHandle handle, CurlOption option, SListHandle value);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_setopt")]
        public static extern CurlCode EasySetOpt64(CurlEasyHandle handle, CurlOption option, SListHandle value);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_perform")]
        public static extern CurlCode EasyPerform32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_perform")]
        public static extern CurlCode EasyPerform64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_easy_reset")]
        public static extern void EasyReset32(CurlEasyHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_easy_reset")]
        public static extern void EasyReset64(CurlEasyHandle handle);

        [DllImport(X86DllPath, EntryPoint = "curl_slist_append")]
        public static extern SListHandle SListAppend32(SListHandle handle, string @string);
        [DllImport(X64DllPath, EntryPoint = "curl_slist_append")]
        public static extern SListHandle SListAppend64(SListHandle handle, string @string);

        [DllImport(X86DllPath, EntryPoint = "curl_slist_free_all")]
        public static extern void SListFreeAll32(SListHandle handle);
        [DllImport(X64DllPath, EntryPoint = "curl_slist_free_all")]
        public static extern void SListFreeAll64(SListHandle handle);

        // Private members

        // Note: .NET Framework can't find the DLL with forward slashes, but finds it with backslashes.

        private const string X86DllPath = @"x86\\libcurl";
        private const string X64DllPath = @"x64\\libcurl-x64";

    }

}