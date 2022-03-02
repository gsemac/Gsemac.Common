using System;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing.Imaging.Native {

    internal static class LibWebPDemux {

        // Public members

        public static WebPDemuxerHandle WebPDemux(ref WebPData data) {

            return Environment.Is64BitProcess ?
                WebPDemuxInternalNative64(ref data, 0, IntPtr.Zero, WebPDemuxAbiVersion) :
                WebPDemuxInternalNative32(ref data, 0, IntPtr.Zero, WebPDemuxAbiVersion);

        }
        public static void WebPDemuxDelete(WebPDemuxerHandle dmux) {

            if (Environment.Is64BitProcess)
                WebPDemuxDeleteNative64(dmux);
            else
                WebPDemuxDeleteNative32(dmux);

        }
        public static int WebPDemuxGetFrame(WebPDemuxerHandle dmux, int frameNumber, ref WebPIterator iter) {

            return Environment.Is64BitProcess ?
                WebPDemuxGetFrameNative64(dmux, frameNumber, ref iter) :
                WebPDemuxGetFrameNative32(dmux, frameNumber, ref iter);

        }
        public static uint WebPDemuxGetI(WebPDemuxerHandle dmux, WebPFormatFeature feature) {

            return Environment.Is64BitProcess ?
                WebPDemuxGetINative64(dmux, feature) :
                WebPDemuxGetINative32(dmux, feature);

        }
        public static int WebPDemuxNextFrame(ref WebPIterator iter) {

            return Environment.Is64BitProcess ?
                WebPDemuxNextFrameNative64(ref iter) :
                WebPDemuxNextFrameNative32(ref iter);

        }
        public static int WebPDemuxPrevFrame(ref WebPIterator iter) {

            return Environment.Is64BitProcess ?
                WebPDemuxPrevFrameNative64(ref iter) :
                WebPDemuxPrevFrameNative32(ref iter);

        }
        public static void WebPDemuxReleaseIterator(ref WebPIterator iter) {

            if (Environment.Is64BitProcess)
                WebPDemuxReleaseIteratorNative64(ref iter);
            else
                WebPDemuxReleaseIteratorNative32(ref iter);

        }
        public static int WebPGetDemuxVersion() {

            return Environment.Is64BitProcess ?
                WebPGetDemuxVersionNative64() :
                WebPGetDemuxVersionNative32();

        }

        // Private members

        public const int WebPDemuxAbiVersion = 0x0107; // WEBP_DEMUX_ABI_VERSION

        private const string X86DllPath = @"x86\\libwebpdemux";
        private const string X64DllPath = @"x64\\libwebpdemux";

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxDelete")]
        private static extern int WebPDemuxDeleteNative32(WebPDemuxerHandle dmux);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxDelete")]
        private static extern int WebPDemuxDeleteNative64(WebPDemuxerHandle dmux);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxGetFrame")]
        private static extern int WebPDemuxGetFrameNative32(WebPDemuxerHandle dmux, int frameNumber, ref WebPIterator iter);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxGetFrame")]
        private static extern int WebPDemuxGetFrameNative64(WebPDemuxerHandle dmux, int frameNumber, ref WebPIterator iter);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxGetI")]
        private static extern uint WebPDemuxGetINative32(WebPDemuxerHandle dmux, WebPFormatFeature feature);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxGetI")]
        private static extern uint WebPDemuxGetINative64(WebPDemuxerHandle dmux, WebPFormatFeature feature);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxInternal")]
        private static extern WebPDemuxerHandle WebPDemuxInternalNative32(ref WebPData data, int copy, IntPtr state, int version);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxInternal")]
        private static extern WebPDemuxerHandle WebPDemuxInternalNative64(ref WebPData data, int copy, IntPtr state, int version);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxNextFrame")]
        private static extern int WebPDemuxNextFrameNative32(ref WebPIterator iter);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxNextFrame")]
        private static extern int WebPDemuxNextFrameNative64(ref WebPIterator iter);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxPrevFrame")]
        private static extern int WebPDemuxPrevFrameNative32(ref WebPIterator iter);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxPrevFrame")]
        private static extern int WebPDemuxPrevFrameNative64(ref WebPIterator iter);

        [DllImport(X86DllPath, EntryPoint = "WebPDemuxReleaseIterator")]
        private static extern void WebPDemuxReleaseIteratorNative32(ref WebPIterator iter);
        [DllImport(X64DllPath, EntryPoint = "WebPDemuxReleaseIterator")]
        private static extern void WebPDemuxReleaseIteratorNative64(ref WebPIterator iter);

        [DllImport(X86DllPath, EntryPoint = "WebPGetDemuxVersion")]
        private static extern int WebPGetDemuxVersionNative32();
        [DllImport(X64DllPath, EntryPoint = "WebPGetDemuxVersion")]
        private static extern int WebPGetDemuxVersionNative64();

    }

}