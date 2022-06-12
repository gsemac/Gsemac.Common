using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.WebP.Tests.Properties;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Gsemac.Drawing.Imaging.WebP.Tests {

    [TestClass]
    public class WebPImageCodecTests {

        // Public members

        [TestMethod]
        public void TestAnimationInfoIsValidWithAnimatedWebP() {

            using (IImage image = ImageFromFile("animated.webp")) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.FromMilliseconds(200), image.AnimationDelay);
                Assert.AreEqual(3, image.FrameCount);
                Assert.AreEqual(0, image.AnimationIterations);

            }

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithStaticWebP() {

            using (IImage image = ImageFromFile("static.webp")) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.Zero, image.AnimationDelay);
                Assert.AreEqual(1, image.FrameCount);
                Assert.IsTrue(image.AnimationIterations <= 1);

            }

        }
        [TestMethod]
        public void TestConvertWebPToDifferentImageFormat() {

            using (IImage image = ImageFromFile("static.webp"))
            using (Stream outputStream = new MemoryStream()) {

                image.Save(outputStream, ImageFormat.Png);

                Assert.AreEqual(ImageFormat.Png, FileFormatFactory.Default.FromStream(outputStream));

            }

        }

        // Private members

        private static IImage ImageFromFile(string fileName) {

            IImageCodec codec = new WebPImageCodec();

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, fileName)))
                return codec.Decode(stream);

        }

    }

}