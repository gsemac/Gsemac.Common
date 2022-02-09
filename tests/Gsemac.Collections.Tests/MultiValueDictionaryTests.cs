using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class MultiValueDictionaryTests {

        [TestMethod]
        public void TestGetValueByKeyAfterAddingSingleValue() {

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int> {
                { 1, 2 }
            };

            Assert.AreEqual(1, dict[1].Count);
            Assert.AreEqual(2, dict[1].First());

        }
        [TestMethod]
        public void TestGetValueByKeyAfterAddingMultipleValues() {

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int> {
                { 1, 2 },
                { 1, 3 }
            };

            Assert.AreEqual(2, dict[1].Count);
            Assert.AreEqual(2, dict[1].First());
            Assert.AreEqual(3, dict[1].Skip(1).First());

        }
        [TestMethod]
        public void TestGetNonExistantValueByKeyThrowsException() {

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int>();

            Assert.ThrowsException<KeyNotFoundException>(() => dict[1]);

        }

        [TestMethod]
        public void TestRemoveByKeyReturnsTrueWhenKeyExists() {

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int> {
                { 1, 2 }
            };

            Assert.IsTrue(dict.Remove(1));

        }
        [TestMethod]
        public void TestRemoveByKeyReturnsFalseWhenKeyDoesNotExist() {

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int>();

            Assert.IsFalse(dict.Remove(1));

        }

        [TestMethod]
        public void TestCountReturnsNumberOfCollections() {

            // The Count property should return the number of collections, NOT the number of items.
            // This is in line with the MultiValueDictionary implementation provided by Microsoft.Experimental.Collections.

            MultiValueDictionary<int, int> dict = new MultiValueDictionary<int, int> {
                { 1, 1 },
                { 1, 2 },
                { 1, 3 },
                { 2, 1 },
            };

            Assert.AreEqual(2, dict.Count);

        }

    }

}