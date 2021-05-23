using System;

namespace Gsemac.Collections {

    /// <summary>
    /// Provides a queue interface over a byte buffer.
    /// </summary>
    public class CircularBuffer {

        // Public members

        /// <summary>
        /// Returns the number of unread bytes in the queue.
        /// </summary>
        public int Length {
            get {

                // The length of the queue is the amount of data between the read head and the write head.
                // (i.e. the amount of data left available for reading.)

                if (buffer is null)
                    return 0;

                if (wpos == rpos)
                    return 0;
                else if (wpos > rpos)
                    return wpos - rpos;
                else
                    return buffer.Length - rpos + wpos;

            }
        }
        /// <summary>
        /// Returns the size of the underlying buffer.
        /// </summary>
        public int Capacity {
            get => buffer is null ? 0 : buffer.Length;
            set => SetCapacity(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer"/> class.
        /// </summary>
        public CircularBuffer() {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer"/> class.
        /// </summary>
        /// <param name="initialCapacity">Starting capacity of the underlying buffer.</param>
        public CircularBuffer(int initialCapacity) {

            Capacity = initialCapacity;

        }
        public CircularBuffer(byte[] buffer) :
            this(buffer.Length) {

            this.fixedCapacity = true;
            this.buffer = buffer;

        }

        /// <summary>
        /// Adds a single byte to the queue.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void WriteByte(byte value) {

            Write(new byte[] { value }, 0, 1);

        }
        /// <summary>
        /// Adds an array of bytes to the queue.
        /// </summary>
        /// <param name="buffer">Values to add.</param>
        /// <param name="offset">Starting offset in buffer.</param>
        /// <param name="count">Number of bytes to write from the buffer.</param>
        public void Write(byte[] buffer, int offset, int count) {

            int bytesToWrite = count;

            // Make sure the capacity is large enough to store the new bytes.

            EnsureCapacity(bytesToWrite);

            if (wpos < rpos) {

                // New capacity will have been provided between the write and read heads, so we can write guaranteed that there is enough space.

                Buffer.BlockCopy(buffer, offset, this.buffer, wpos, count);

                wpos += count;

            }
            else {

                int writeCapacityLeft = this.buffer.Length - wpos;

                // Copy as much as we can to the end of the buffer.

                int bytesWritten = Math.Min(bytesToWrite, writeCapacityLeft);

                Buffer.BlockCopy(buffer, offset, this.buffer, wpos, bytesWritten);

                bytesToWrite -= bytesWritten;

                wpos = (wpos + bytesWritten) % this.buffer.Length;

                if (bytesToWrite > 0) {

                    // Go back to the front of the buffer to continue writing.

                    Buffer.BlockCopy(buffer, offset + bytesWritten, this.buffer, wpos, bytesToWrite);

                    wpos = bytesToWrite;

                }

            }

        }
        /// <summary>
        /// Reads a single byte from the queue.
        /// </summary>
        /// <returns>A single byte from the queue.</returns>
        public byte ReadByte() {

            if (Length <= 0)
                throw new Exception("Queue was empty.");

            byte[] buffer = new byte[1];

            Read(buffer, 0, 1);

            return buffer[0];

        }
        /// <summary>
        /// Reads an array of bytes from the queue, returning the number of bytes read.
        /// </summary>
        /// <param name="buffer">Buffer to read into.</param>
        /// <param name="offset">Offset at which to begin writing.</param>
        /// <param name="count">Maximum number of bytes to read.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count) {

            int bytesToRead = count;
            int bytesRead = 0;

            if (rpos < wpos) {

                // Read head is behind the write head, so reading the bytes is trivial.
                // All we need to do is read up the requested byte count or the write head-- Whichever comes first.

                bytesRead = Math.Min(bytesToRead, wpos - rpos);

                Buffer.BlockCopy(this.buffer, rpos, buffer, offset, Math.Min(bytesToRead, bytesRead));

                rpos += Math.Min(bytesToRead, bytesRead);

            }
            else if (rpos > wpos) {

                // Read head is in fron of the write head.

                int capacityLeft = this.buffer.Length - rpos;

                bytesRead = Math.Min(capacityLeft, count);

                Buffer.BlockCopy(this.buffer, rpos, buffer, offset, bytesRead);

                rpos = (rpos + bytesRead) % this.buffer.Length;

                bytesToRead -= bytesRead;

                if (bytesToRead > 0) {

                    bytesToRead = Math.Min(bytesToRead, wpos - rpos);

                    Buffer.BlockCopy(this.buffer, rpos, buffer, offset + bytesRead, bytesToRead);

                    bytesRead += bytesToRead;

                    rpos += bytesToRead;

                }

            }

            return bytesRead;

        }

        /// <summary>
        /// Clears the underlying buffer.
        /// </summary>
        public void Clear() {

            Array.Clear(buffer, 0, buffer.Length);

            wpos = 0;
            rpos = 0;

        }
        /// <summary>
        /// Returns the next byte in the queue without dequeueing it.
        /// </summary>
        /// <returns>The next byte in the queue.</returns>
        public byte Peek() {
            return buffer[rpos];
        }

        // Private members

        private const int growthFactor = 2;
        private const int minCapacity = 4;

        private readonly bool fixedCapacity = false;
        private byte[] buffer;
        private int wpos = 0; // write position
        private int rpos = 0; // read position

        int GetNumberOfBytesAvailableForWriting() {

            // Calculate the amount of space left in the buffer for writing new data.

            if (buffer is null)
                return 0;

            if (wpos > rpos)
                return rpos + buffer.Length - wpos;
            else if (wpos < rpos)
                return rpos - wpos;
            else
                return buffer.Length;

        }
        void EnsureCapacity(int spaceRequired) {

            // Grow the buffer by increasing it by the growth factor, ensuring a minimum of our required capacity + 1.
            // The extra 1 is added so that there's always at least one empty byte in the buffer, so the write and read heads are never equal.
            // If they were, we wouldn't be able to tell that there was new data in the buffer.

            int spaceLeft = GetNumberOfBytesAvailableForWriting();

            if (spaceLeft < spaceRequired + 1) {

                if (!fixedCapacity)
                    Capacity = Math.Max(Capacity + spaceRequired + 1 - spaceLeft, Math.Max(Capacity * growthFactor, minCapacity));
                else
                    throw new InvalidOperationException(Properties.ExceptionMessages.BufferHasNoSpaceLeft);

            }

        }
        void SetCapacity(int capacity) {

            if (fixedCapacity)
                throw new InvalidOperationException(Properties.ExceptionMessages.BufferIsNotExpandable);

            byte[] newBuffer = new byte[capacity];

            if (rpos < wpos) {

                // If the read head is behind the write head, just copy in the data that we still have to read (between the heads).
                // This places all of the new capacity after the write head.

                Buffer.BlockCopy(buffer, rpos, newBuffer, 0, wpos - rpos);

                wpos -= rpos;
                rpos = 0;

            }
            else if (rpos > wpos) {

                // If the write head is behind the read head, we need to put the new capacity between them.

                Buffer.BlockCopy(buffer, 0, newBuffer, 0, wpos);

                int unreadSegmentLength = buffer.Length - rpos;

                Buffer.BlockCopy(buffer, rpos, newBuffer, newBuffer.Length - unreadSegmentLength, unreadSegmentLength);

                rpos = newBuffer.Length - unreadSegmentLength;

            }
            else {

                // If they're equal, there's no data to read in the buffer, so we can just reset both heads.

                wpos = 0;
                rpos = 0;

            }

            buffer = newBuffer;

        }

    }

}