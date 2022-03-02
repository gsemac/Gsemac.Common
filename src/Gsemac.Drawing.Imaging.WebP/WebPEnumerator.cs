using Gsemac.Drawing.Imaging.Native;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public sealed class WebPEnumerator :
        IWebPEnumerator {

        // Public members

        public IWebPFrame Current => GetCurrentFrame();

        object IEnumerator.Current => Current;

        public bool MoveNext() {

            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebPEnumerator));

            // Note that WebP frame indices are 1-based.

            int result = isIteratorInitialized ?
                LibWebPDemux.WebPDemuxNextFrame(ref iterator) :
                LibWebPDemux.WebPDemuxGetFrame(demuxerHandle, frameNumber: 1, ref iterator);

            isIteratorInitialized = true;

            return result > 0;

        }
        public void Reset() {

            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebPEnumerator));

            ReleaseIterator();

            isIteratorInitialized = false;

        }

        public void Dispose() {

            if (!isDisposed) {

                ReleaseIterator();

                isDisposed = true;

            }

        }

        // Internal members

        internal WebPEnumerator(WebPDemuxerHandle demuxerHandle) {

            if (demuxerHandle is null)
                throw new ArgumentNullException(nameof(demuxerHandle));

            this.demuxerHandle = demuxerHandle;
            this.iterator = new WebPIterator();

        }

        // Private members

        private readonly WebPDemuxerHandle demuxerHandle;
        private bool isDisposed;
        private bool isIteratorInitialized;
        private WebPIterator iterator;

        private WebPFrame GetCurrentFrame() {

            return isIteratorInitialized ?
                new WebPFrame(iterator) :
                null;

        }
        private void ReleaseIterator() {

            LibWebPDemux.WebPDemuxReleaseIterator(ref iterator);

        }

    }

}