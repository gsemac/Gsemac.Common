using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class StringToColorConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertFromName() {

            Assert.AreEqual(Color.Red, new StringToColorConverter().Convert("red"));

        }
        [TestMethod]
        public void TestConvertFromNameIsCaseInsensitive() {

            Assert.AreEqual(Color.Red, new StringToColorConverter().Convert("ReD"));

        }
        [TestMethod]
        public void TestConvertFromNameNormalizesGraySpelling() {

            Assert.AreEqual(Color.Gray, new StringToColorConverter().Convert("gray"));
            Assert.AreEqual(Color.Gray, new StringToColorConverter().Convert("grey"));
            Assert.AreEqual(Color.DarkGray, new StringToColorConverter().Convert("darkgray"));
            Assert.AreEqual(Color.DarkGray, new StringToColorConverter().Convert("darkgrey"));

        }
        [TestMethod]
        public void TestConvertFromHexString() {

            Assert.AreEqual(Color.Fuchsia, new StringToColorConverter().Convert("#ff00ff"));

        }
        [TestMethod]
        public void TestConvertFromHexStringIsCaseInsensitive() {

            Assert.AreEqual(Color.Orange, new StringToColorConverter().Convert("#fFA500"));

        }
        [TestMethod]
        public void TestConvertFromHexStringWithAlpha() {

            Assert.AreEqual(Color.FromArgb(128, 0, 255, 0), new StringToColorConverter().Convert("#00ff0080"));

        }
        [TestMethod]
        public void TestConvertFromShorthandHexString() {

            Assert.AreEqual(Color.Fuchsia, new StringToColorConverter().Convert("#f0f"));

        }
        [TestMethod]
        public void TestConvertFromShorthandHexStringWithAlpha() {

            Assert.AreEqual(Color.FromArgb(187, 0, 255, 0), new StringToColorConverter().Convert("#00ff00bb"));

        }

    }

}