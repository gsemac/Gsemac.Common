using Gsemac.Collections;
using System;
using System.IO;

namespace Gsemac.IO {

    public sealed class SeekableStream :
        Stream {

        // Public members

        public override bool CanRead => wrappedStream.CanRead;
        public override bool CanSeek => true;
        public override bool CanWrite => wrappedStream.CanWrite;
        public override long Length => bufferStream.Length + (wrappedStream.Length - wrappedStream.Position);
        public override long Position {
            get => bufferStream.Position;
            set => Seek(value, SeekOrigin.Begin);
        }

        public SeekableStream(Stream stream) :
            this(stream, disposeStream: true) {
        }
        public SeekableStream(Stream stream, bool disposeStream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            bufferStream = new MemoryStream();
            wrappedStream = stream;
            this.disposeStream = disposeStream;

        }
        public SeekableStream(Stream stream, int bufferSize) :
            this(stream, bufferSize, disposeStream: true) {
        }
        public SeekableStream(Stream stream, int bufferSize, bool disposeStream) :
            this(stream, disposeStream: disposeStream) {

            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), Properties.ExceptionMessages.CapacityMustBePositive);

            this.bufferSize = bufferSize;
            bufferStream = new MemoryStream(bufferSize);

        }

        public override void Flush() {

            bufferStream.Flush();
            wrappedStream.Flush();

        }

        public override int Read(byte[] buffer, int offset, int count) {

            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            if (bufferSize == 0) {

                // If the buffer has no capacity, simply read from the underlying stream.

                return wrappedStream.Read(buffer, offset, count);

            }
            else {

                int bytesToRead = count;

                while (bytesToRead > 0) {

                    // ALWAYS read bytes into the read buffer before we copy them into the output buffer.
                    // The underlying stream might not give us the same number of bytes as requested even if the requested number is greater than the read buffer size.

                    int bytesToMakeAvailable = bufferSize == InfiniteBufferSize ? bytesToRead : Math.Min(bytesToRead, bufferSize);

                    MakeBytesAvailableInBuffer(bytesToMakeAvailable);

                    // Read the bytes from the buffer.

                    int bytesReadFromBuffer = bufferStream.Read(buffer, offset, bytesToMakeAvailable);

                    bytesToRead -= bytesReadFromBuffer;
                    offset += bytesReadFromBuffer;

                    if (bytesReadFromBuffer <= 0)
                        break;

                }

                return count - bytesToRead;

            }

        }
        public override long Seek(long offset, SeekOrigin origin) {

            if (origin == SeekOrigin.Current) {

                return Seek(Position + offset, SeekOrigin.Begin);

            }
            else if (origin == SeekOrigin.End) {

                return Seek(Length + offset, SeekOrigin.Begin);

            }
            else if (origin == SeekOrigin.Begin) {

                if (offset < 0)
                    throw new IOException(Properties.ExceptionMessages.CannotMovePositionBeforeBeginningOfStream);

                if (bufferSize == 0) {

                    // If the buffer has no capacity, simply skip bytes in the wrapped stream.

                    EatBytesFromWrappedStream((int)offset);

                }
                else {

                    // If the seek position is within the buffer stream, just seek the buffer stream.

                    if (offset <= bufferStream.Length)
                        return bufferStream.Seek(offset, SeekOrigin.Begin);

                    // Otherwise, we need to seek foward in the underlying stream.
                    // Assuming the underlying stream doesn't support seeking, we need to read until we reach the desired position.
                    // We want as much available in the buffer as possible, so seek such that the desired position is at the very end of the buffer.

                    while (offset > 0) {

                        bufferStream.Seek(0, SeekOrigin.End);

                        int bytesToSkip = Math.Min((int)offset, bufferSize == InfiniteBufferSize ? SeekBufferSize : bufferSize);

                        MakeBytesAvailableInBuffer(bytesToSkip);

                        if (bufferStream.Position == bufferStream.Length)
                            break;

                        offset -= bytesToSkip;

                    }

                }

                return bufferStream.Seek(0, SeekOrigin.End);

            }
            else {

                throw new ArgumentException(Properties.ExceptionMessages.InvalidSeekOrigin, nameof(origin));

            }

        }
        public override void SetLength(long value) {

            wrappedStream.SetLength(value - bufferStream.Length);

        }
        public override void Write(byte[] buffer, int offset, int count) {

            // TODO: Implement writing

            throw new NotImplementedException();

        }

        public override void Close() {

            bufferStream.Close();

            if (disposeStream)
                wrappedStream.Close();

            base.Close();

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                bufferStream.Dispose();

                if (disposeStream)
                    wrappedStream.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private const int InfiniteBufferSize = -1;
        private const int SeekBufferSize = 4096;

        private readonly int bufferSize = InfiniteBufferSize;
        private readonly MemoryStream bufferStream;
        private readonly Stream wrappedStream;
        private readonly bool disposeStream;

        private void EatBytesFromWrappedStream(int count) {

            byte[] tempBuffer = new byte[SeekBufferSize];

            while (count > 0) {

                int bytesRead = wrappedStream.Read(tempBuffer, 0, Math.Min(count, tempBuffer.Length));

                if (bytesRead <= 0)
                    break;

                count -= bytesRead;

            }

        }
        private void MakeBytesAvailableInBuffer(int count) {

            int uninitializedBytesLeft = (int)(bufferStream.Capacity - bufferStream.Length);
            int bytesLeft = (int)(bufferStream.Length - bufferStream.Position) + uninitializedBytesLeft;
            int additionalBytesRequired = count - bytesLeft;
            long bufferPosition = bufferStream.Position;

            // Read the requested number of bytes from the underlying stream.

            byte[] tempBuffer = new byte[count];

            int bytesRead = wrappedStream.Read(tempBuffer, 0, tempBuffer.Length);

            if (bufferSize != InfiniteBufferSize && additionalBytesRequired > 0) {

                // Shift the existing buffer content to make room for the new content.

                byte[] underlyingBuffer = bufferStream.GetBuffer();

                ArrayUtilities.Shift(underlyingBuffer, -bytesRead);

                // Make room to write the new bytes in the buffer stream.

                bufferStream.SetLength(Math.Max(0, bufferStream.Length - bytesRead));

            }

            // Write the new bytes to the end of the stream.

            bufferStream.Seek(0, SeekOrigin.End);

            bufferStream.Write(tempBuffer, 0, bytesRead);

            if (additionalBytesRequired > 0)
                bufferPosition = Math.Max(0, bufferPosition - bytesRead);

            bufferStream.Position = bufferPosition;

        }

    }

}