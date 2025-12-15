using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class ListUtilitiesTests {

        // Move

        [TestMethod]
        public void TestMoveItemFromLowerIndexToHigherIndex() {

            List<int> list = new() {
                1, 2, 3, 4, 5,
            };

            ListUtilities.Move(list, 1, 3);

            CollectionAssert.AreEqual(list, new List<int>() { 1, 3, 4, 2, 5 });

        }
        [TestMethod]
        public void TestMoveItemFromHigherIndexToLowerIndex() {

            List<int> list = new() {
                1, 2, 3, 4, 5,
            };

            ListUtilities.Move(list, 3, 1);

            CollectionAssert.AreEqual(list, new List<int>() { 1, 4, 2, 3, 5 });

        }
        [TestMethod]
        public void TestMoveItemToFirstIndex() {

            List<int> list = new() {
                1, 2, 3, 4, 5,
            };

            ListUtilities.Move(list, 3, 0);

            CollectionAssert.AreEqual(list, new List<int>() { 4, 1, 2, 3, 5 });

        }
        [TestMethod]
        public void TestMoveItemToLastIndex() {

            List<int> list = new() {
                1, 2, 3, 4, 5,
            };

            ListUtilities.Move(list, 3, 4);

            CollectionAssert.AreEqual(list, new List<int>() { 1, 2, 3, 5, 4 });

        }
        [TestMethod]
        public void TestMoveThrowsExceptionWithNegativeDestinationIndex() {

            Assert.Throws<ArgumentOutOfRangeException>(() => {

                ListUtilities.Move(new List<int>() { 1, 2, 3 }, 0, -1);

            });

        }
        [TestMethod]
        public void TestMoveThrowsExceptionWithNegativeSourceIndex() {

            Assert.Throws<ArgumentOutOfRangeException>(() => {

                ListUtilities.Move(new List<int>() { 1, 2, 3 }, -1, 0);

            });

        }
        [TestMethod]
        public void TestMoveThrowsExceptionWithDestinationIndexGreaterThanListSize() {

            Assert.Throws<ArgumentOutOfRangeException>(() => {

                ListUtilities.Move(new List<int>() { 1, 2, 3 }, 3, 0);

            });

        }
        [TestMethod]
        public void TestMoveThrowsExceptionWithSourceIndexGreaterThanListSize() {

            Assert.Throws<ArgumentOutOfRangeException>(() => {

                ListUtilities.Move(new List<int>() { 1, 2, 3 }, 0, 3);

            });

        }

    }

}