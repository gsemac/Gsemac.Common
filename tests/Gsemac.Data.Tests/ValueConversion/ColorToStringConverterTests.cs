using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class ColorToStringConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertFromArgbToNamedColor() {

            // ColorTranslator's ToHtml method won't return the name for colors that weren't created by name.

            Assert.AreEqual("red", new ColorToStringConverter().Convert(Color.FromArgb(255, 0, 0)));

        }
        [TestMethod]
        public void TestConvertFromColorConstantToNamedColor() {

            Assert.AreEqual("red", new ColorToStringConverter().Convert(Color.Red));

        }
        [TestMethod]
        public void TestConvertToHexString() {

            Assert.AreEqual("#7289da", new ColorToStringConverter().Convert(Color.FromArgb(114, 137, 218)));

        }
        [TestMethod]
        public void TestConvertToHexStringWithAlpha() {

            // Colors with alpha should be converted to #RRGGBBAA.
            // https://css-tricks.com/8-digit-hex-codes/

            Assert.AreEqual("#00ff0080", new ColorToStringConverter().Convert(Color.FromArgb(128, 0, 255, 0)));

        }

        [TestMethod]
        public void TestConvertFromArgbPrefersFuchsiaOverMagenta() {

            // "Magenta" is alias for "Fuchsia", but the latter was standardized first in CSS1.
            // If the color name isn't given explicitly, we'll prefer to call it fuchsia.

            Assert.AreEqual("fuchsia", new ColorToStringConverter().Convert(Color.FromArgb(255, 0, 255)));

        }
        [TestMethod]
        public void TestConvertFromColorConstantPrefersColorName() {

            // When dealing with aliased colors, prefer the name that matches the name in the struct.

            Assert.AreEqual("fuchsia", new ColorToStringConverter().Convert(Color.Fuchsia));
            Assert.AreEqual("magenta", new ColorToStringConverter().Convert(Color.Magenta));

        }
        [TestMethod]
        public void TestConvertPrefersGrayToGrey() {

            // The "gray" spelling should always be returned instead of "grey".

            Assert.AreEqual("gray", new ColorToStringConverter().Convert(Color.Gray));
            Assert.AreEqual("darkgray", new ColorToStringConverter().Convert(Color.DarkGray));

        }
        [TestMethod]
        public void TestConvertEmptyColorReturnsTransparent() {

            Assert.AreEqual("transparent", new ColorToStringConverter().Convert(Color.Empty));

        }

    }

}