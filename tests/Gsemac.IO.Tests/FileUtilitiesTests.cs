using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class FileUtilitiesTests {

        // FormatBytes

        [TestMethod]
        public void TestFormatBytesWithZeroBytes() {

            Assert.AreEqual("0 B", FileUtilities.FormatBytes(0));

        }
        [TestMethod]
        public void TestFormatBytesWithNegativeBytes() {

            Assert.AreEqual("-10 B", FileUtilities.FormatBytes(-10));

        }
        [TestMethod]
        public void TestFormatBytesAsBits() {

            Assert.AreEqual("16 b", FileUtilities.FormatBytes(2, new ByteFormattingOptions() {
                Prefix = BinaryPrefix.BinaryBits,
            }));

        }
        [TestMethod]
        public void TestFormatBytesWithMaxValue() {

            Assert.AreEqual("8 EiB", FileUtilities.FormatBytes(long.MaxValue));

        }
        [TestMethod]
        public void TestFormatBytesAsBitsWithMaxValue() {

            Assert.AreEqual("64 Eib", FileUtilities.FormatBytes(long.MaxValue, new ByteFormattingOptions() {
                Prefix = BinaryPrefix.BinaryBits,
            }));

        }
        [TestMethod]
        public void TestFormatBytesWithFractionalAmount() {

            Assert.AreEqual("3.5 KiB", FileUtilities.FormatBytes((long)(1024 * 3.5)));

        }
        [TestMethod]
        public void TestFormatBytesWithWholeAmount() {

            Assert.AreEqual("3 KiB", FileUtilities.FormatBytes(1024 * 3));

        }
        [TestMethod]
        public void TestFormatBytesWithThreshold() {

            Assert.AreEqual("0.1 GiB", FileUtilities.FormatBytes(107400000, new ByteFormattingOptions() {
                Threshold = 0.1,
            }));

        }

    }

}