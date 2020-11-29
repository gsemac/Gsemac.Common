using System;

namespace Gsemac.Net.Curl {

    public class SList :
        IDisposable {

        // Public members

        public SListHandle Handle => handle;

        public SList() :
            this(new SListHandle()) {
        }
        public SList(SListHandle handle) {

            this.handle = handle;

        }

        public void Append(string item) {

            handle = LibCurl.SListAppend(handle, item);

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    handle.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        private SListHandle handle;
        private bool disposedValue = false;

    }

}