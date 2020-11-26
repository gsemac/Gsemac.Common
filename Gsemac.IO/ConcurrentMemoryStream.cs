using Gsemac.Collections;
using System;
using System.IO;
using System.Threading;

namespace Gsemac.IO {

    /// <summary>
    /// Provides a view of a sequence of bytes that supports concurrent read/write operations.
    /// </summary>
    public class ConcurrentMemoryStream :
        Stream {

        // Public members

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override bool CanTimeout => true;
        public override long Length {
            get {

                lock (bufferMutex) {

                    return data.Length;

                }

            }
        }
        public override long Position {
            get => throw new NotSupportedException("Stream does not support seeking.");
            set => throw new NotSupportedException("Stream does not support seeking.");
        }
        public override int ReadTimeout { get; set; } = Timeout.Infinite;

        /// <summary>
        /// If set to true, reads will block until data is available.
        /// </summary>
        public bool Blocking {
            get => blocking;
            set => blocking = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentMemoryStream"/> class.
        /// </summary>
        public ConcurrentMemoryStream() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentMemoryStream"/> class.
        /// </summary>
        /// <param name="bufferSize">Initial size of the underlying buffer.</param>
        public ConcurrentMemoryStream(int bufferSize) {
            data.Capacity = bufferSize;
        }

        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count) {

            // Allow closed streams to be read (but not written to).
            // This allows a stream to be closed to indicate that it is finished being written to.

            int bytesRead = 0;

            lock (bufferMutex) {

                bytesRead = data.Dequeue(buffer, offset, count);

                // If we didn't read anything from the stream, reset the lock so we can wait on it.

                if (bytesRead <= 0 && blocking && !isClosed)
                    bufferDataNotifier.Reset();

            }

            if (bytesRead <= 0 && blocking) {

                if (!isClosed)
                    bufferDataNotifier.Wait(ReadTimeout);

                lock (bufferMutex) {

                    bytesRead = data.Dequeue(buffer, offset, count);

                    // If we didn't read anything from the stream, reset the lock so we can wait on it.

                    if (bytesRead <= 0 && blocking && !isClosed)
                        bufferDataNotifier.Reset();

                }

            }

            return bytesRead;

        }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException("Stream does not support seeking.");
        public override void SetLength(long value) => throw new NotSupportedException("Stream does not support this operation.");
        public override void Write(byte[] buffer, int offset, int count) {

            lock (bufferMutex) {

                if (isClosed)
                    throw new ObjectDisposedException("Cannot access a closed Stream.");

                data.Enqueue(buffer, offset, count);

                bufferDataNotifier.Set();

            }

        }
        public override void Close() {

            if (!isClosed) {

                // Unblock read if currently blocked.

                blocking = false;

                bufferDataNotifier.Set();

                bufferDataNotifier.Dispose();

            }

            isClosed = true;

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing)
                Close();

            base.Dispose(disposing);

        }

        // Private members

        private readonly ByteQueue data = new ByteQueue(4096);
        private readonly object bufferMutex = new object();
        private readonly ManualResetEventSlim bufferDataNotifier = new ManualResetEventSlim(true);
        private volatile bool blocking = false;
        private volatile bool isClosed = false;

    }

}