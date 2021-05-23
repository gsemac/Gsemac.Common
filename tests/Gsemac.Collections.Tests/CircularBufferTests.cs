using Gsemac.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class CircularBufferTests {

        [TestMethod]
        public void TestLengthOfEmptyBufferIsZero() {

            Assert.AreEqual(0, new CircularBuffer().Length);

        }
        [TestMethod]
        public void TestLengthOfEmptyBufferWithInitialCapacityIsZero() {

            Assert.AreEqual(0, new CircularBuffer(32).Length);

        }
        [TestMethod]
        public void TestLengthAfterWrite() {

            CircularBuffer queue = new CircularBuffer();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Write(input, 0, input.Length);

            Assert.AreEqual(input.Length, queue.Length);

        }
        [TestMethod]
        public void TestWriteThenRead() {

            CircularBuffer queue = new CircularBuffer();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Write(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Read(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }
        [TestMethod]
        public void TestWriteThenReadWithWrapAround() {

            CircularBuffer queue = new CircularBuffer(10);

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

            CircularBuffer queue = new CircularBuffer(20);
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