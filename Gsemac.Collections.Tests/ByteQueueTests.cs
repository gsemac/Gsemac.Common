using Gsemac.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class ByteQueueTests {

        [TestMethod]
        public void TestLengthAfterWrite() {

            ByteQueue queue = new ByteQueue();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Enqueue(input, 0, input.Length);

            Assert.AreEqual(input.Length, queue.Length);

        }
        [TestMethod]
        public void TestWriteThenRead() {

            ByteQueue queue = new ByteQueue();

            byte[] input = Encoding.ASCII.GetBytes("hello world!");
            queue.Enqueue(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Dequeue(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }
        [TestMethod]
        public void TestWriteThenReadWithWrapAround() {

            ByteQueue queue = new ByteQueue(10);

            byte[] input = Encoding.ASCII.GetBytes("123456");
            queue.Enqueue(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Dequeue(output, 0, output.Length);

            queue.Enqueue(input, 0, input.Length);

            Array.Clear(output, 0, output.Length);

            queue.Dequeue(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }
        [TestMethod]
        public void TestReadAfterCapacityChanged() {

            ByteQueue queue = new ByteQueue(20);
            int capacity = queue.Capacity;

            byte[] input = Encoding.ASCII.GetBytes("hello world!");

            // Write then read so the read head != 0.

            queue.Enqueue(input, 0, input.Length);

            byte[] output = new byte[input.Length];
            queue.Dequeue(output, 0, output.Length);

            // Write until the capacity changes.

            while (queue.Capacity == capacity)
                queue.Enqueue(input, 0, input.Length);

            queue.Dequeue(output, 0, output.Length);

            Assert.AreEqual(Encoding.ASCII.GetString(input), Encoding.ASCII.GetString(output));

        }

    }

}