using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class StringUtilitiesTests {

        // SplitAfter

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

        // ToProperCase

        [TestMethod]
        public void TestToProperCaseWithMixedCase() {

            Assert.AreEqual("My Title", StringUtilities.ToProperCase("mY tItLe"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCase() {

            Assert.AreEqual("My Title", StringUtilities.ToProperCase("MY TITLE"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCaseAndPreserveAcronymsOption() {

            Assert.AreEqual("MY TITLE", StringUtilities.ToProperCase("MY TITLE", ProperCaseOptions.PreserveAcronyms));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumerals() {

            Assert.AreEqual("James III Of Scotland", StringUtilities.ToProperCase("james iii of scotland", ProperCaseOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumeralsInsideOfWord() {

            Assert.AreEqual("The Liver Is An Organ", StringUtilities.ToProperCase("the liver is an organ", ProperCaseOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithPossessiveS() {

            Assert.AreEqual("John's House", StringUtilities.ToProperCase("john's house"));

        }

    }

}