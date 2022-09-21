using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class DashedRangeFormatterTests {

        // Format

        [TestMethod]
        public void TestFormatWithRangeWithDifferentStartAndEndValues() {

            Assert.AreEqual("1-5", RangeFormatter.Dashed.Format(Range.Create(1, 5)));

        }
        [TestMethod]
        public void TestFormatWithRangeWithSameStartAndEndValues() {

            Assert.AreEqual("1", RangeFormatter.Dashed.Format(Range.Create(1, 1)));

        }
        [TestMethod]
        public void TestFormatWithDescendingRangeWithNormalizeEnabled() {

            Assert.AreEqual("1-5", RangeFormatter.Dashed.Format(Range.Create(5, 1), new RangeFormattingOptions() {
                Normalize = true,
            }));

        }
        [TestMethod]
        public void TestFormatWithDescendingRangeWithNormalizeDisabled() {

            Assert.AreEqual("5-1", RangeFormatter.Dashed.Format(Range.Create(5, 1), new RangeFormattingOptions() {
                Normalize = false,
            }));

        }

    }

}