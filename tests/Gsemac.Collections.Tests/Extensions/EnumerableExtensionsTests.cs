using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Collections.Extensions.Tests {

    [TestClass]
    public class EnumerableExtensionsTests {

        // Public members

        // IsSorted

        [TestMethod]
        public void TestIsSortedReturnsTrueForSortedItems() {

            int[] items = { 1, 2, 4, 8, 16, 32 };

            Assert.IsTrue(items.IsSorted());

        }
        [TestMethod]
        public void TestIsSortedReturnsFalseForNonSortedItems() {

            int[] items = { 1, 2, 4, 8, 32, 16 };

            Assert.IsFalse(items.IsSorted());

        }
        [TestMethod]
        public void TestIsSortedReturnsTrueForEmptyArray() {

            Assert.IsTrue(Array.Empty<int>().IsSorted());

        }
        [TestMethod]
        public void TestIsSortedReturnsTrueForArrayWithSingleItem() {

            Assert.IsTrue(new int[] { 0 }.IsSorted());

        }

    }

}