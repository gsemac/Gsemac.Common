using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Extensions.Tests {

    [TestClass]
    public class ArrayExtensionsTests {

        [TestMethod]
        public void TestRotateRight() {

            int[] array = { 1, 2, 3, 4, 5 };

            array.Rotate(2);

            CollectionAssert.AreEqual(new[] { 4, 5, 1, 2, 3 }, array);

        }
        [TestMethod]
        public void TestRotateLeft() {

            int[] array = { 1, 2, 3, 4, 5 };

            array.Rotate(-2);

            CollectionAssert.AreEqual(new[] { 3, 4, 5, 1, 2 }, array);

        }

    }

}