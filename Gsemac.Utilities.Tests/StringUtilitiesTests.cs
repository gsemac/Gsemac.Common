using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class StringUtilitiesTests {

        [TestMethod]
        public void TestSplitAfter() {

            string input = "a/b/c";
            string[] expectedResult = { "a/", "b/", "c" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }
        [TestMethod]
        public void TestSplitAfterWithStartingDelimiter() {

            string input = "/a/b/c";
            string[] expectedResult = { "/", "a/", "b/", "c" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }
        [TestMethod]
        public void TestSplitAfterWithEndingDelimiter() {

            string input = "a/b/c/";
            string[] expectedResult = { "a/", "b/", "c/", "" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }

    }

}