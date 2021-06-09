using Gsemac.Collections;
using Gsemac.IO.Properties;
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

                lock (streamBuffer)
                    return streamBuffer.Length;

            }
        }
        public override long Position {
            get => throw new NotSupportedException(ExceptionMessages.StreamDoesNotSupportSeeking);
            set => throw new NotSupportedException(ExceptionMessages.StreamDoesNotSupportSeeking);
        }
        public override int ReadTimeout { get; set; } = Timeout.Infinite;
        public override int WriteTimeout { get; set; } = Timeout.Infinite;

        /// <summary>
        /// If set to true, reads will block until data is available.
        /// </summary>
        public bool Blocking {
            get => isBlocking;
            set => isBlocking = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentMemoryStream"/> class.
        /// </summary>
        public ConcurrentMemoryStream() {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentMemoryStream"/> class.
        /// </summary>
        /// <param name="bufferSize">Initial size of the underlying buffer.</param>
        public ConcurrentMemoryStream(int bufferSize) {

            streamBuffer.Capacity = bufferSize;

        }

        public override void Flush() {
        }
        public override int Read(byte[] buffer, int offset, int count) {

            // Closed streams can be read from, but cannot be written to.
            // A closed stream indicates that there is no more data to write it.

            lock (streamBuffer) {

                while (streamBuffer.Length <= 0 && isBlocking && !isClosed)
                    if (!Monitor.Wait(streamBuffer, ReadTimeout))
                        throw new TimeoutException();

                return streamBuffer.Read(buffer, offset, count);

            }

        }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(ExceptionMessages.StreamDoesNotSupportSeeking);
        public override void SetLength(long value) => throw new NotSupportedException(ExceptionMessages.StreamDoesNotSupportThisOperation);
        public override void Write(byte[] buffer, int offset, int count) {

            lock (streamBuffer) {

                if (isClosed)
                    throw new ObjectDisposedException(ExceptionMessages.CannotAccessAClosedStream);

                streamBuffer.Write(buffer, offset, count);

                // PulseAll is used instead of Pulse because a single reading thread may not read all of the available data.

                if (count > 0)
                    Monitor.PulseAll(streamBuffer);

            }

        }
        public override void Close() {

            lock (streamBuffer) {

                if (!isClosed) {

                    isClosed = true;

                    Monitor.PulseAll(streamBuffer);

                }

            }

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing)
                Close();

            base.Dispose(disposing);

        }

        // Private members

        private readonly CircularBuffer streamBuffer = new CircularBuffer();
        private volatile bool isBlocking = false;
        private volatile bool isClosed = false;

    }

}