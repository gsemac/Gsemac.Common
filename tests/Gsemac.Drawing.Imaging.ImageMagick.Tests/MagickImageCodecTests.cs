using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.ImageMagick.Tests.Properties;
using Gsemac.IO;
using Gsemac.IO.FileFormats;
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
        public void TestAnimationInfoIsValidWithAnimatedGifAndStaticDecoderMode() {

            IDecoderOptions options = new DecoderOptions() {
                Mode = DecoderMode.Static,
            };

            using (IImage image = ImageFromFile("animated.gif", options)) {

                Assert.IsNotNull(image);

                Assert.AreEqual(TimeSpan.FromMilliseconds(200), image.AnimationDelay);
                Assert.AreEqual(3, image.FrameCount);
                Assert.AreEqual(0, image.AnimationIterations);

            }

        }
        [TestMethod]
        public void TestAnimationInfoIsValidWithAnimatedGifAndMetadataDecoderMode() {

            IDecoderOptions options = new DecoderOptions() {
                Mode = DecoderMode.Metadata,
            };

            using (IImage image = ImageFromFile("animated.gif", options)) {

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
        [TestMethod]
        public void TestAnimationInfoIsValidAfterConvertingAnimatedWebPToAnimatedGif() {

            // Animation info should remain the same after converting an animated image to another format.

            using (Stream animatedGifStream = new MemoryStream()) {

                using (IImage image = ImageFromFile("animated.webp"))
                    image.Save(animatedGifStream, ImageFormat.Gif);

                animatedGifStream.Seek(0, SeekOrigin.Begin);

                using (IImage image = ImageFromStream(animatedGifStream)) {

                    Assert.IsNotNull(image);

                    Assert.AreEqual(ImageFormat.Gif, image.Format);
                    Assert.AreEqual(TimeSpan.FromMilliseconds(200), image.AnimationDelay);
                    Assert.AreEqual(3, image.FrameCount);
                    Assert.AreEqual(0, image.AnimationIterations);

                }

            }

        }

        [TestMethod]
        public void TestImageFormatIsValidAfterConvertingToDifferentImageFormat() {

            using (Stream outputStream = new MemoryStream()) {

                using (IImage image = ImageFromFile("static.webp"))
                    image.Save(outputStream, ImageFormat.Gif);

                outputStream.Seek(0, SeekOrigin.Begin);

                using (IImage image = ImageFromStream(outputStream)) {

                    Assert.IsNotNull(image);

                    Assert.AreEqual(ImageFormat.Gif, image.Format);

                }

            }

        }

        // Private members

        private static IImage ImageFromStream(Stream stream) {

            IImageCodec codec = new MagickImageCodec();

            return codec.Decode(stream);

        }
        private static IImage ImageFromFile(string fileName) {

            return ImageFromFile(fileName, DecoderOptions.Default);

        }
        private static IImage ImageFromFile(string fileName, IDecoderOptions options) {

            IFileFormat fileFormat = FileFormatFactory.Default.FromFileExtension(fileName);
            IImageCodec codec = new MagickImageCodec(fileFormat);

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, fileName)))
                return codec.Decode(stream, options);

        }


    }

}