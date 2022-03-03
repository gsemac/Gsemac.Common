using Gsemac.Drawing.Imaging.ImageMagick.Tests.Properties;
using Gsemac.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Gsemac.Drawing.Imaging.ImageMagick.Tests {

    [TestClass]
    public class MagickImageCodecTests {

        // Public members

        [TestMethod]
        public void TestAnimationInfoIsValidWithAnimatedGif() {

            IAnimationInfo animationInfo;

            using (IImage image = ImageFromFile("animated.gif"))
                animationInfo = image.Animation;

            Assert.IsNotNull(animationInfo);

            Assert.AreEqual(TimeSpan.FromMilliseconds(200), animationInfo.Delay);
            Assert.AreEqual(3, animationInfo.FrameCount);
            Assert.AreEqual(0, animationInfo.Iterations);

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithStaticGif() {

            IAnimationInfo animationInfo;

            using (IImage image = ImageFromFile("static.gif"))
                animationInfo = image.Animation;

            Assert.IsNotNull(animationInfo);

            Assert.AreEqual(TimeSpan.Zero, animationInfo.Delay);
            Assert.AreEqual(1, animationInfo.FrameCount);
            Assert.IsTrue(animationInfo.Iterations <= 1);

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithImageWithMultipleResolutions() {

            // Some file formats can have multiple "frames" that do not pertain to animation (e.g. resolutions in an ICO image).
            // This should not be reflected in the animation info.

            IAnimationInfo animationInfo;

            using (IImage image = ImageFromFile("static.ico"))
                animationInfo = image.Animation;

            Assert.IsNotNull(animationInfo);
            Assert.AreEqual(TimeSpan.Zero, animationInfo.Delay);
            Assert.AreEqual(1, animationInfo.FrameCount);
            Assert.AreEqual(0, animationInfo.Iterations);

        }

        // Private members

        private static IImage ImageFromFile(string fileName) {

            IFileFormat fileFormat = FileFormatFactory.Default.FromFileExtension(fileName);
            IImageCodec codec = new MagickImageCodec(fileFormat);

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, fileName)))
                return codec.Decode(stream);

        }

    }

}