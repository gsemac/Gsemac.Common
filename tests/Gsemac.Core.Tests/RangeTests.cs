﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class RangeTests {

        // IsAscending

        [TestMethod]
        public void TestRangeIsAscendingWithAscendingRangeReturnsTrue() {

            Assert.IsTrue(Range.Create(1, 2).IsAscending);

        }
        [TestMethod]
        public void TestRangeIsAscendingWithDescendingRangeReturnsFalse() {

            Assert.IsFalse(Range.Create(2, 1).IsAscending);

        }
        [TestMethod]
        public void TestRangeIsAscendingWithNonAscendingRangeReturnsFalse() {

            Assert.IsFalse(Range.Create(1, 1).IsAscending);

        }

        // IsDescending

        [TestMethod]
        public void TestRangeIsIsDescendingWithIsDescendingRangeReturnsTrue() {

            Assert.IsTrue(Range.Create(2, 1).IsDescending);

        }
        [TestMethod]
        public void TestRangeIsIsDescendingWithAscendingRangeReturnsFalse() {

            Assert.IsFalse(Range.Create(1, 2).IsDescending);

        }
        [TestMethod]
        public void TestRangeIsIsDescendingWithNonIsDescendingRangeReturnsFalse() {

            Assert.IsFalse(Range.Create(1, 1).IsDescending);

        }

        // Minimum

        [TestMethod]
        public void TestMinimumReturnsSmallerBoundary() {

            Assert.AreEqual(1, Range.Create(1, 5).Minimum);

        }

        // Maximum

        [TestMethod]
        public void TestMaximumReturnsLargerBoundary() {

            Assert.AreEqual(5, Range.Create(1, 5).Maximum);

        }

        // Contains

        [TestMethod]
        public void TestContainsReturnsTrueForValueBetweenBoundaries() {

            Assert.IsTrue(Range.Create(1, 5).Contains(4));

        }
        [TestMethod]
        public void TestContainsReturnsTrueForValueEqualToInclusiveStartingBoundary() {

            Assert.IsTrue(Range.Create(1, startInclusive: true, 5, endInclusive: false).Contains(1));

        }
        [TestMethod]
        public void TestContainsReturnsTrueForValueEqualToInclusiveEndingBoundary() {

            Assert.IsTrue(Range.Create(1, startInclusive: false, 5, endInclusive: true).Contains(5));

        }
        [TestMethod]
        public void TestContainsReturnsFalseForValueNotBetweenBoundaries() {

            Assert.IsFalse(Range.Create(1, 5).Contains(6));

        }
        [TestMethod]
        public void TestContainsReturnsFalseForValueEqualToExclusiveStartingBoundary() {

            Assert.IsFalse(Range.Create(1, startInclusive: false, 5, endInclusive: true).Contains(1));

        }
        [TestMethod]
        public void TestContainsReturnsFalseForValueEqualToExclusiveEndingBoundary() {

            Assert.IsFalse(Range.Create(1, startInclusive: true, 5, endInclusive: false).Contains(5));

        }

        [TestMethod]
        public void TestContainsReturnsTrueForRangeBetweenBoundaries() {

            Assert.IsTrue(Range.Create(1, 5).Contains(Range.Create(1, 3)));

        }
        [TestMethod]
        public void TestContainsReturnsFalseForRangePartiallyBetweenBoundaries() {

            Assert.IsFalse(Range.Create(1, 5).Contains(Range.Create(0, 3)));

        }
        [TestMethod]
        public void TestContainsReturnsFalseForRangeNotBetweenBoundaries() {

            Assert.IsFalse(Range.Create(1, 5).Contains(Range.Create(6, 10)));

        }

        // IntersectsWith

        [TestMethod]
        public void TestIntersectsWithReturnsTrueForRangeBetweenBoundaries() {

            Assert.IsTrue(Range.Create(1, 5).IntersectsWith(Range.Create(1, 3)));

        }
        [TestMethod]
        public void TestIntersectsWithReturnsTrueForRangePartiallyBetweenBoundaries() {

            Assert.IsTrue(Range.Create(1, 5).IntersectsWith(Range.Create(0, 3)));

        }
        [TestMethod]
        public void TestIntersectsWithReturnsFalseForRangeNotBetweenBoundaries() {

            Assert.IsFalse(Range.Create(1, 5).IntersectsWith(Range.Create(6, 10)));

        }

        // CompareTo

        [TestMethod]
        public void TestRangeWithSmallerMinimumIsOrderedFirst() {

            Assert.IsTrue(Range.Create(1, 5).CompareTo(Range.Create(0, 3)) > 0);

        }
        [TestMethod]
        public void TestRangeWithEqualMinimumAndSmallerMaximumIsOrderedFirst() {

            Assert.IsTrue(Range.Create(1, 5).CompareTo(Range.Create(1, 3)) > 0);

        }
        [TestMethod]
        public void TestRangesWithSameStartAndEndValuesAreEqual() {

            Assert.IsTrue(Range.Create(1, 5).CompareTo(Range.Create(1, 5)) == 0);

        }

        // TryParse

        [TestMethod]
        public void TestTryParseSucceedsWithBoundedIntegerValues() {

            Assert.IsTrue(Range<int>.TryParse("[1, 5)", out var result));
            Assert.AreEqual(Range.Create(1, startInclusive: true, 5, endInclusive: false), result);

        }
        [TestMethod]
        public void TestTryParseSucceedsWithSingleIntegerValue() {

            Assert.IsTrue(Range<int>.TryParse("1", out var result));
            Assert.AreEqual(Range.Create(1), result);

        }
        [TestMethod]
        public void TestTryParseSucceedsWithSingleNegativeValue() {

            Assert.IsTrue(Range<int>.TryParse("-1", out var result));
            Assert.AreEqual(Range.Create(-1), result);

        }
        [TestMethod]
        public void TestTryParseSucceedsWithDashedIntegerValues() {

            Assert.IsTrue(Range<int>.TryParse("1-3", new RangeParsingOptions() {
                AllowDashedRanges = true,
            }, out var result));

            Assert.AreEqual(Range.Create(1, 3), result);

        }
        [TestMethod]
        public void TestTryParseSucceedsWithBoundedNegativeIntegerValues() {

            Assert.IsTrue(Range<int>.TryParse("[-5, -1)", new RangeParsingOptions() {
                AllowDashedRanges = true,
            }, out var result));

            Assert.AreEqual(Range.Create(-5, startInclusive: true, -1, endInclusive: false), result);

        }
        [TestMethod]
        public void TestTryParseSucceedsWithDashedNegativeIntegerValues() {

            Assert.IsTrue(Range<int>.TryParse("-5--1", new RangeParsingOptions() {
                AllowDashedRanges = true,
            }, out var result));

            Assert.AreEqual(Range.Create(-5, -1), result);

        }
        [TestMethod]
        public void TestTryParseFailsWithTooFewValues() {

            Assert.IsFalse(Range<int>.TryParse("[)", out _));

        }
        [TestMethod]
        public void TestTryParseFailsWithTooManyValues() {

            Assert.IsFalse(Range<int>.TryParse("[1, 1, 3)", out _));

        }
        [TestMethod]
        public void TestTryParseFailWithEmptyString() {

            Assert.IsFalse(Range<int>.TryParse(string.Empty, out _));

        }
        [TestMethod]
        public void TestTryParseFailsWithNullString() {

            Assert.IsFalse(Range<int>.TryParse(null, out _));

        }

        // Combine

        [TestMethod]
        public void TestCombineRangesWithOverlappingRangesReturnsRangeContainingAllCommonValues() {

            var combinedRange = Range.Combine(Range.Create(1, 5), Range.Create(10, 3));

            Assert.AreEqual(Range.Create(1, 10), combinedRange);

        }
        [TestMethod]
        public void TestCombineRangesPreservesInclusivity() {

            var combinedRange = Range.Combine(Range.Create(1, 5), Range.Create(10, startInclusive: false, 3, endInclusive: true));

            Assert.AreEqual(Range.Create(1, startInclusive: true, 10, endInclusive: false), combinedRange);

        }

        // Subtract

        [TestMethod]
        public void TestSubtractRangesWithOverlappingRangesWitLhsRangeBeforeRhsRange() {

            Assert.AreEqual(Range.Create(1, startInclusive: true, 4, endInclusive: false), Range.Subtract(Range.Create(1, 5), Range.Create(4, 8)));

        }
        [TestMethod]
        public void TestSubtractRangesWithOverlappingRangesWitLhsRangeAfterRhsRange() {

            Assert.AreEqual(Range.Create(5, startInclusive: false, 8, endInclusive: true), Range.Subtract(Range.Create(4, 8), Range.Create(1, 5)));

        }
        [TestMethod]
        public void TestSubtractRangesWithEqualRanges() {

            Assert.AreEqual(Range.Create(1, startInclusive: false, 1, endInclusive: false), Range.Subtract(Range.Create(1, 5), Range.Create(1, 5)));

        }
        [TestMethod]
        public void TestSubtractRangesWithNonOverlappingRanges() {

            Assert.AreEqual(Range.Create(1, 5), Range.Subtract(Range.Create(1, 5), Range.Create(6, 10)));

        }

        // Normalize

        [TestMethod]
        public void TestNormalizeWithDescendingRangeReturnsEquivalentAscendingRange() {

            Assert.AreEqual(Range<int>.Parse("[4, 10)"), Range.Normalize(Range<int>.Parse("(10,4]")));

        }

    }

}