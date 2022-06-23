namespace Gsemac.Collections.Extensions {

    public static class CircularBufferExtensions {

        /// <summary>
        /// Adds an array of values to the queue.
        /// </summary>
        /// <param name="circularBuffer">The buffer to write to.</param>
        /// <param name="buffer">The data to copy into the buffer.</param>
        public static void Write<T>(this CircularBuffer<T> circularBuffer, T[] buffer) {

            circularBuffer.Write(buffer, 0, buffer.Length);

        }
        /// <summary>
        /// Reads an array of values from the queue, returning the number of values read.
        /// </summary>
        /// <param name="circularBuffer">The buffer to read from.</param>
        /// <param name="buffer">The buffer to read into.</param>
        /// <returns>The number of values read.</returns>
        public static int Read<T>(this CircularBuffer<T> circularBuffer, T[] buffer) {

            return circularBuffer.Read(buffer, 0, buffer.Length);

        }

    }

}