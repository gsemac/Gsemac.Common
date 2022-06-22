using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.PatternMatching.Tests {

    [TestClass]
    public class WildcardPatternTests {

        [TestMethod]
        public void TestIsMatchWithEmptyPatternAndEmptyInput() {

            Assert.IsTrue(new WildcardPattern("").IsMatch(""));

        }
        [TestMethod]
        public void TestIsMatchWithEmptyPatternAndNullInput() {

            Assert.IsTrue(new WildcardPattern("").IsMatch(null));

        }
        [TestMethod]
        public void TestIsMatchWithSingleWildcardPatternAndMatchingInput() {

            Assert.IsTrue(new WildcardPattern("*").IsMatch("hello"));

        }
        [TestMethod]
        public void TestIsMatchWithMultipleWildcardPatternAndMatchingInput() {

            Assert.IsTrue(new WildcardPattern("***").IsMatch("hello"));

        }
        [TestMethod]
        public void TestIsMatchWithStartingWildcardPatternAndMatchingInput() {

            Assert.IsTrue(new WildcardPattern("*llo").IsMatch("hello"));

        }
        [TestMethod]
        public void TestIsMatchWithEndingWildcardPatternAndMatchingInput() {

            Assert.IsTrue(new WildcardPattern("hell*").IsMatch("hello"));

        }
        [TestMethod]
        public void TestIsMatchWithNonMatchingInput() {

            Assert.IsFalse(new WildcardPattern("h*llo").IsMatch("world"));

        }

    }

}