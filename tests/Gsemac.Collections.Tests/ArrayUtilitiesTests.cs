using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class ArrayUtilitiesTests {

        // Rotate

        [TestMethod]
        public void TestRotateWithPositiveOffset() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 3, 4, 5, 1, 2 };

            ArrayUtilities.Rotate(beforeItems, 3);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestRotateWithNegativeOffset() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 4, 5, 1, 2, 3 };

            ArrayUtilities.Rotate(beforeItems, -3);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestRotateWithPositiveOffsetGreaterThanArrayLength() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 3, 4, 5, 1, 2 };

            ArrayUtilities.Rotate(beforeItems, 8);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestRotateWithNegativeOffsetGreaterThanArrayLength() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 4, 5, 1, 2, 3 };

            ArrayUtilities.Rotate(beforeItems, -8);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }

        // Shift

        [TestMethod]
        public void TestShiftWithPositiveOffset() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 0, 0, 0, 1, 2 };

            ArrayUtilities.Shift(beforeItems, 3);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestShiftWithNegativeOffset() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 4, 5, 0, 0, 0 };

            ArrayUtilities.Shift(beforeItems, -3);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestShiftWithPositiveOffsetGreaterThanArrayLength() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 0, 0, 0, 0, 0 };

            ArrayUtilities.Shift(beforeItems, 100);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }
        [TestMethod]
        public void TestShiftWithNegativeOffsetGreaterThanArrayLength() {

            int[] beforeItems = new int[] { 1, 2, 3, 4, 5 };
            int[] afterItems = new int[] { 0, 0, 0, 0, 0 };

            ArrayUtilities.Shift(beforeItems, -100);

            CollectionAssert.AreEqual(afterItems, beforeItems);

        }

    }

}