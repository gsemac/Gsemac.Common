using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class RangeCollectionTests {

        // Contains

        [TestMethod]
        public void TestContainsWithValueContainedInRangesReturnsTrue() {

            Assert.IsTrue(new RangeCollection<int> {
                Range.Create(1, 3),
                Range.Create(4, 10),
            }.Contains(5));

        }
        [TestMethod]
        public void TestContainsWithValueNotContainedInRangesReturnsFalse() {

            Assert.IsFalse(new RangeCollection<int> {
                Range.Create(1, 3),
                Range.Create(4, 10),
            }.Contains(11));

        }

        [TestMethod]
        public void TestContainsWithRangePresentInCollectionReturnsTrue() {

            Assert.IsTrue(new RangeCollection<int> {
                Range.Create(1, 3),
                Range.Create(4, 10),
            }.Contains(Range.Create(1, 3)));

        }
        [TestMethod]
        public void TestContainsWithRangeNotPresentInCollectionReturnsFalse() {

            Assert.IsFalse(new RangeCollection<int> {
                Range.Create(1, 3),
                Range.Create(4, 10),
            }.Contains(Range.Create(5, 6)));

        }

        // Sort

        [TestMethod]
        public void TestSortSortsRangesInAscendingOrder() {

            IRangeCollection<int> unsorted = new RangeCollection<int> {
                Range.Create(1, 10),
                Range.Create(1, 3),
                Range.Create(4, 0),
                Range.Create(6, 10),
                Range.Create(10, 6),
                Range.Create(7),
                Range.Create(5, 11),
            };

            IRangeCollection<int> sorted = new RangeCollection<int> {
                Range.Create(4, 0),
                Range.Create(1, 3),
                Range.Create(1, 10),
                Range.Create(5, 11),
                Range.Create(6, 10),
                Range.Create(10, 6),
                Range.Create(7),
            };

            unsorted.Sort();

            Assert.IsTrue(sorted.SequenceEqual(unsorted));

        }

        // Condense

        [TestMethod]
        public void TestCondenseCombinesEquivalentRanges() {

            IRangeCollection<int> unsorted = new RangeCollection<int> {
                Range.Create(1, 10),
                Range.Create(1, 3),
                Range.Create(4, 0),
                Range.Create(6, 10),
                Range.Create(10, 6),
                Range.Create(7),
                Range.Create(5, 11),
            };

            IRangeCollection<int> condensed = new RangeCollection<int> {
                Range.Create(0, 11),
            };

            unsorted.Condense();

            Assert.IsTrue(condensed.SequenceEqual(unsorted));

        }

        // ToString

        [TestMethod]
        public void TestToStringWithCustomRangeFormatter() {

            IRangeCollection<int> ranges = new RangeCollection<int> {
                Range.Create(1, 10),
                Range.Create(1, 3),
                Range.Create(4, 0),
            };

            Assert.AreEqual("1-10, 1-3, 4-0", ranges.ToString(RangeFormatter.Dashed));

        }

        // TryParse

        [TestMethod]
        public void TestTryParseSucceedsWithSingleValidBoundedRanges() {

            Assert.IsTrue(RangeCollection<int>.TryParse("[1, 5]", out var parsedResult));

            IRangeCollection<int> validResult = new RangeCollection<int> {
                Range.Create(1, 5),
            };

            Assert.IsTrue(parsedResult.SequenceEqual(validResult));

        }
        [TestMethod]
        public void TestTryParseSucceedsWithMultipleValidBoundedRanges() {

            Assert.IsTrue(RangeCollection<int>.TryParse("[1, 5],[2,6],,[6,7],5,", out var parsedResult));

            IRangeCollection<int> validResult = new RangeCollection<int> {
                Range.Create(1, 5),
                Range.Create(2, 6),
                Range.Create(6, 7),
                Range.Create(5),
            };

            Assert.IsTrue(parsedResult.SequenceEqual(validResult));

        }
        [TestMethod]
        public void TestTryParseSucceedsWithValidAndInvalidBoundedRangesWithIgnoreInvalidRangesEnabled() {

            Assert.IsTrue(RangeCollection<int>.TryParse("[1, 5],[2,,],,,[5],xdg,w,[2.5],[2,6]", new RangeParsingOptions() {
                IgnoreInvalidRanges = true,
            }, out var parsedResult));

            IRangeCollection<int> validResult = new RangeCollection<int> {
                Range.Create(1, 5),
                Range.Create(2, 6),
            };

            Assert.IsTrue(parsedResult.SequenceEqual(validResult));

        }
        [TestMethod]
        public void TestTryParseFailsWithValidAndInvalidBoundedRangesWithIgnoreInvalidRangesDisabled() {

            Assert.IsFalse(RangeCollection<int>.TryParse("[1, 5],[2,,],,,[5],xdg,w,[2.5],[2,6]", new RangeParsingOptions() {
                IgnoreInvalidRanges = false,
            }, out var parsedResult));

        }
        [TestMethod]
        public void TestTryParseFailsWithNullString() {

            Assert.IsFalse(RangeCollection<int>.TryParse(null, out _));

        }
        [TestMethod]
        public void TestTryParseFailsWithEmptyString() {

            Assert.IsFalse(RangeCollection<int>.TryParse(string.Empty, out _));

        }

    }

}