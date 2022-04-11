using Gsemac.Collections.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class OrderedDictionaryTests {

        // Public members

        [TestMethod]
        public void TestKeysOrderingRespectsInsertionOrder() {

            IDictionary<int, int> dict = new OrderedDictionary<int, int>(1);

            for (int i = 0; i < 100; ++i)
                dict.Add(i, i);

            Assert.IsTrue(dict.Keys.IsSorted());

        }
        [TestMethod]
        public void TestValuesOrderingRespectsInsertionOrder() {

            IDictionary<int, int> dict = new OrderedDictionary<int, int>(1);

            for (int i = 0; i < 100; ++i)
                dict.Add(i, i);

            Assert.IsTrue(dict.Values.IsSorted());

        }

    }

}