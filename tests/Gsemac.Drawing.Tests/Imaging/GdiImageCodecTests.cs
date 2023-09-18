using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Tests.Properties;
using Gsemac.IO.FileFormats;
using Gsemac.Win32.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging.Tests {

    [TestClass]
    public class GdiImageCodecTests {

        // Public members

        [TestMethod]
        public void TestImageIsValidAfterDisposingSourceStream() {

            // Image.FromStream creates Bitmap instances that load the source stream lazily.
            // We want images produced by GdiImageCodec.Decode to release the source stream immediately without invaliding the bitmap data.
            // This is done by copying the bitmap data to a new Bitmap so that the source stream is released.

            // Note that Bitmap instances do not ALWAYS defer reading the stream, depending on the image content.
            // The test image used here (ICO with multiple sizes) was experimentally constructed specifically to cause this problem.

            Bitmap bitmap;

            using (IImage image = ImageFromFile("static.ico"))
                bitmap = image.ToBitmap();

            IntPtr hObject = IntPtr.Zero;

            try {

                // This call will throw "System.Runtime.InteropServices.ExternalException (0x80004005): A generic error occurred in GDI+"
                // if the underlying stream is disposed while the Bitmap has a reference to it.

                hObject = bitmap.GetHbitmap();

                Assert.AreNotEqual(IntPtr.Zero, hObject);

            }
            finally {

                Gdi32.DeleteObject(hObject);

            }

        }

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
        public void TestIsAnimatedReturnsTrueWithAnimatedGif() {

            using (IImage image = ImageFromFile("animated.gif"))
                Assert.IsTrue(image.IsAnimated());

        }
        [TestMethod]
        public void TestIsAnimatedReturnsTrueWithAnimatedGifAndMetadataModeEnabled() {

            IImageDecoderOptions options = new ImageDecoderOptions() {
                Mode = ImageDecoderMode.Metadata,
            };

            using (IImage image = ImageFromFile("animated.gif", options))
                Assert.IsTrue(image.IsAnimated());

        }
        [TestMethod]
        public void TestIsAnimatedReturnsFalseWithStaticGif() {

            using (IImage image = ImageFromFile("static.gif"))
                Assert.IsFalse(image.IsAnimated());

        }
        [TestMethod]
        public void TestIsAnimatedReturnsFalseWithImageWithMultipleResolutions() {

            using (IImage image = ImageFromFile("static.ico"))
                Assert.IsFalse(image.IsAnimated());
        }

        [TestMethod]
        public void TestImageFormatIsValidAfterConvertingToDifferentImageFormat() {

            using (Stream outputStream = new MemoryStream()) {

                using (IImage image = ImageFromFile("static.png"))
                    image.Save(outputStream, ImageFormat.Jpeg);

                outputStream.Seek(0, SeekOrigin.Begin);

                using (IImage image = ImageFromStream(outputStream)) {

                    Assert.IsNotNull(image);

                    Assert.AreEqual(ImageFormat.Jpeg, image.Format);

                }

            }

        }

        // Private members

        private static IImage ImageFromStream(Stream stream) {

            IImageCodec codec = new GdiImageCodec();

            return codec.Decode(stream);

        }
        private static IImage ImageFromFile(string fileName) {

            return ImageFromFile(fileName, ImageDecoderOptions.Default);

        }
        private static IImage ImageFromFile(string fileName, IImageDecoderOptions options) {

            IImageCodec codec = new GdiImageCodec();

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, fileName)))
                return codec.Decode(stream, options);

        }

    }

}