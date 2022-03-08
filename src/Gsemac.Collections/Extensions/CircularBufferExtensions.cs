namespace Gsemac.Collections.Extensions {

    public static class CircularBufferExtensions {

        /// <summary>
        /// Adds an array of bytes to the queue.
        /// </summary>
        /// <param name="circularBuffer">The buffer to write to.</param>
        /// <param name="buffer">The data to copy into the buffer.</param>
        public static void Write(this CircularBuffer circularBuffer, byte[] buffer) {

            circularBuffer.Write(buffer, 0, buffer.Length);

        }
        /// <summary>
        /// Reads an array of bytes from the queue, returning the number of bytes read.
        /// </summary>
        /// <param name="circularBuffer">The buffer to read from.</param>
        /// <param name="buffer">The buffer to read into.</param>
        /// <returns>The number of bytes read.</returns>
        public static int Read(this CircularBuffer circularBuffer, byte[] buffer) {

            return circularBuffer.Read(buffer, 0, buffer.Length);

        }

    }

}