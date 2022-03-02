using Gsemac.Drawing.Extensions;
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

            IAnimationInfo animationInfo;

            using (IImage image = ImageFromFile("animated.webp"))
                animationInfo = image.Animation;

            Assert.IsNotNull(animationInfo);

            Assert.AreEqual(TimeSpan.FromMilliseconds(200), animationInfo.Delay);
            Assert.AreEqual(3, animationInfo.FrameCount);
            Assert.AreEqual(0, animationInfo.Iterations);

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithStaticWebP() {

            IAnimationInfo animationInfo;

            using (IImage image = ImageFromFile("static.webp"))
                animationInfo = image.Animation;

            Assert.IsNotNull(animationInfo);

            Assert.AreEqual(TimeSpan.Zero, animationInfo.Delay);
            Assert.AreEqual(1, animationInfo.FrameCount);
            Assert.IsTrue(animationInfo.Iterations <= 1);

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