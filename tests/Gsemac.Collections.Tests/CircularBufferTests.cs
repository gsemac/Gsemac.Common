using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class CircularBufferTests {

        // this[]

        [TestMethod]
        public void TestGetValueAtIndexAfterRead() {

            CircularBuffer<char> queue = new();

            foreach (char c in "hello world!")
                queue.Write(c);

            queue.Read();

            Assert.AreEqual('o', queue[3]);

        }
        [TestMethod]
        public void TestSetValueAtIndexAfterRead() {

            CircularBuffer<char> queue = new();

            foreach (char c in "hello world!")
                queue.Write(c);

            queue.Read();

            queue[3] = 'x';

            Assert.AreEqual('x', queue[3]);

        }
        [TestMethod]
        public void TestGetValueAtOutOfBoundsIndexThrowsException() {

            CircularBuffer<char> queue = new();

            Assert.ThrowsException<IndexOutOfRangeException>(() => queue[5]);

        }

        // Length

        [TestMethod]
        public void TestLengthOfEmptyBufferIsZero() {

            Assert.AreEqual(0, new CircularBuffer<byte>().Length);

        }
        [TestMethod]
        public void TestLengthOfEmptyBufferWithInitialCapacityIsZero() {

            Assert.AreEqual(0, new CircularBuffer<byte>(32).Length);

        }
        [TestMethod]
        public void TestLengthAfterWrite() {

            CircularBuffer<byte> queue = new();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Write(input, 0, input.Length);

            Assert.AreEqual(input.Length, queue.Length);

        }

        // Read

        [TestMethod]
        public void TestWriteThenRead() {

            CircularBuffer<byte> queue = new();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Write(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Read(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }
        [TestMethod]
        public void TestWriteThenReadWithWrapAround() {

            CircularBuffer<byte> queue = new CircularBuffer<byte>(10);

            byte[] input = Encoding.ASCII.GetBytes("123456");
            queue.Write(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Read(output, 0, output.Length);

            queue.Write(input, 0, input.Length);

            Array.Clear(output, 0, output.Length);

            queue.Read(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }
        [TestMethod]
        public void TestReadAfterCapacityChanged() {

            CircularBuffer<byte> queue = new CircularBuffer<byte>(20);
            int capacity = queue.Capacity;

            byte[] input = Encoding.ASCII.GetBytes("hello world!");

            // Write then read so the read head != 0.

            queue.Write(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Read(output, 0, output.Length);

            // Write until the capacity changes.

            while (queue.Capacity == capacity)
                queue.Write(input, 0, input.Length);

            queue.Read(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }

    }

}