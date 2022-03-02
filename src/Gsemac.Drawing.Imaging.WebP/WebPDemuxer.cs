using Gsemac.Drawing.Imaging.Native;
using Gsemac.Drawing.Imaging.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing.Imaging {

    public sealed class WebPDemuxer :
        IWebPDemuxer {

        // Public members

        public WebPDemuxer(byte[] data) {

            webpDataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

            webPData = new WebPData() {
                bytes = webpDataHandle.AddrOfPinnedObject(),
                size = new UIntPtr((uint)data.Length),
            };

            demuxerHandle = LibWebPDemux.WebPDemux(ref webPData);

        }

        public IWebPFrame GetFrame(int frameIndex) {

            WebPIterator iter = new WebPIterator();
            bool success = false;

            try {

                success = LibWebPDemux.WebPDemuxGetFrame(demuxerHandle, frameIndex, ref iter) > 0;

                if (!success)
                    throw new ArgumentOutOfRangeException(nameof(frameIndex), ExceptionMessages.IndexOutOfRange);

                return new WebPFrame(iter);

            }
            finally {

                if (success)
                    LibWebPDemux.WebPDemuxReleaseIterator(ref iter);

            }

        }
        public IEnumerable<IWebPFrame> GetFrames() {

            using (IWebPEnumerator frameIterator = new WebPEnumerator(demuxerHandle)) {

                while (frameIterator.MoveNext())
                    yield return frameIterator.Current;

            }

        }

        public int GetI(WebPFormatFeature feature) {

            return (int)LibWebPDemux.WebPDemuxGetI(demuxerHandle, feature);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Private members

        private readonly GCHandle webpDataHandle;
        private readonly WebPData webPData;
        private readonly WebPDemuxerHandle demuxerHandle;
        private bool disposedValue;

        private void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    demuxerHandle.Dispose();

                    if (webpDataHandle.IsAllocated)
                        webpDataHandle.Free();

                }

                disposedValue = true;

            }
        }

    }

}