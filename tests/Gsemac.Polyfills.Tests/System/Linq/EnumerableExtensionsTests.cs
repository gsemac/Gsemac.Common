using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gsemac.Polyfills.System.Linq.Tests {

    [TestClass]
    public class EnumerableExtensionsTests {

        // Chunk

        [TestMethod]
        public void TestChunkReturnsChunksOfCorrectLengths() {

            int[] items = new[] { 1, 2, 3, 4, 5 };
            int[][] chunkedItems = items.Chunk(2).ToArray();

            Assert.AreEqual(3, chunkedItems.Length);

            Assert.AreEqual(2, chunkedItems[0].Length);
            Assert.AreEqual(2, chunkedItems[1].Length);
            Assert.AreEqual(1, chunkedItems[2].Length);

        }
        [TestMethod]
        public void TestChunkReturnsChunksOfCorrectItems() {

            int[] items = new[] { 1, 2, 3, 4, 5 };
            int[][] chunkedItems = items.Chunk(2).ToArray();

            Assert.AreEqual(3, chunkedItems.Length);

            CollectionAssert.AreEqual(new[] { 1, 2, }, chunkedItems[0]);
            CollectionAssert.AreEqual(new[] { 3, 4, }, chunkedItems[1]);
            CollectionAssert.AreEqual(new[] { 5, }, chunkedItems[2]);

        }
        [TestMethod]
        public void TestChunkThrowsOnSizeLessThanOne() {

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new[] { 1, 2, 3, 4, 5 }.Chunk(0).ToArray());

        }

    }

}