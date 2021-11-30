using Gsemac.IO.Properties;
using System;
using System.Threading;

namespace Gsemac.IO {

    public class ProducerConsumerStream :
        ConcurrentMemoryStream {

        // Public members

        public ProducerConsumerStream(int capacity) :
            base(capacity) {

            this.capacity = capacity;

            Blocking = true;

        }

        public override int Read(byte[] buffer, int offset, int count) {

            int bytesRead = base.Read(buffer, offset, count);

            lock (writeLock) {

                if (bytesRead > 0)
                    Monitor.PulseAll(writeLock);

            }

            return bytesRead;

        }
        public override void Write(byte[] buffer, int offset, int count) {

            int bytesLeftToWrite = count;

            lock (writeLock) {

                while (bytesLeftToWrite > 0) {

                    // We use Length + 1 instead of just Length because the underlying buffer class resizes when it reaches Capacity - 1.
                    // This ensures that the buffer capacity is never increased.

                    while (Length + 1 >= capacity && !isClosed)
                        if (!Monitor.Wait(writeLock, WriteTimeout))
                            throw new TimeoutException();

                    if (isClosed)
                        throw new ObjectDisposedException(null, ExceptionMessages.CannotAccessAClosedStream);

                    int bytesAvailableInBuffer = capacity - ((int)Length + 1);
                    int bytesWritten = Math.Min(bytesLeftToWrite, bytesAvailableInBuffer);

                    base.Write(buffer, offset, bytesWritten);

                    offset += bytesWritten;
                    bytesLeftToWrite -= bytesWritten;

                }

            }

        }

        public override void Close() {

            lock (writeLock) {

                isClosed = true;

                Monitor.PulseAll(writeLock);

            }

            base.Close();

        }

        // Private members

        private readonly int capacity;
        private readonly object writeLock = new object();
        private volatile bool isClosed = false;

    }

}