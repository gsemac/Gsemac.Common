using Gsemac.Drawing.Imaging.Extensions;
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

            using (IImage image = ImageFromFile("animated.gif")) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.FromMilliseconds(200), image.AnimationDelay);
                Assert.AreEqual(3, image.FrameCount);
                Assert.AreEqual(0, image.AnimationIterations);

            }

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithStaticGif() {

            using (IImage image = ImageFromFile("static.gif")) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.Zero, image.AnimationDelay);
                Assert.AreEqual(1, image.FrameCount);
                Assert.IsTrue(image.AnimationIterations <= 1);

            }

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithImageWithMultipleResolutions() {

            // Some file formats can have multiple "frames" that do not pertain to animation (e.g. resolutions in an ICO image).
            // This should not be reflected in the animation info.

            using (IImage image = ImageFromFile("static.ico")) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.Zero, image.AnimationDelay);
                Assert.AreEqual(1, image.FrameCount);
                Assert.AreEqual(0, image.AnimationIterations);

            }

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