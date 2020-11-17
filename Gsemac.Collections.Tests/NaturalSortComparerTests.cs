using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gsemac.Collections.Tests {

    [TestClass]
    public class NaturalSortComparerTests {

        [TestMethod]
        public void TestSortWithAlphabeticSequences() {

            string[] items = new[] {
                "abbc",
                "b",
                "abc",
                "cdf",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "abbc",
                "abc",
                "b",
                "cdf",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithNumericSequences() {

            // StrCmpLogicalW ignores leading dashes (of any length) when comparing numbers.

            string[] items = new[] {
                "11",
                "-5",
                "--4",
                "1",
                "2",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "1",
                "2",
                "--4",
                "-5",
                "11",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithNumericSequencesWithLeadingZeros() {

            string[] items = new[] {
                "--0000001",
                "000001",
                "0000000001",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "0000000001",
                "--0000001",
                "000001",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithAlphanumericSequences() {

            string[] items = new[] {
                "f2",
                "f00",
                "f11",
                "f--3",
                "f4",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "f00",
                "f2",
                "f--3",
                "f4",
                "f11",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithMixedSequences() {

            // Test inputs taken from http://archives.miloush.net/michkap/archive/2006/10/01/778990.html

            string[] items = new[] {
                "My File (4).txt",
                "My File (200).txt",
                "My File (10).txt",
                "My File.txt",
                "My File (3000).txt",
                "My File (5).txt",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "My File.txt",
                "My File (4).txt",
                "My File (5).txt",
                "My File (10).txt",
                "My File (200).txt",
                "My File (3000).txt",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithEqualNumericSequencesWithDifferingHyphens() {

            string[] items = new[] {
                "a-1b-c",
                "a--1b-c",
                "a1b-c",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "a1b-c",
                "a-1b-c",
                "a--1b-c",
            }, sortedItems);

        }
        [TestMethod]
        public void TestSortWithMixedCaseAlphabeticSequences() {

            string[] items = new[] {
                "ABC3",
                "AbC1",
                "aBc2",
            };

            string[] sortedItems = items.OrderBy(item => item, new NaturalSortComparer(CultureInfo.InvariantCulture)).ToArray();

            CollectionAssert.AreEqual(new[] {
                "AbC1",
                "aBc2",
                "ABC3",
            }, sortedItems);

        }

    }

}