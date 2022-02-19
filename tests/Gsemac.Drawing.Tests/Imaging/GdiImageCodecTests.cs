#if NETFRAMEWORK

using Gsemac.Drawing.Tests.Properties;
using Gsemac.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging.Tests {

    [TestClass]
    public class GdiImageCodecTests {

        [TestMethod]
        public void TestImageIsValidAfterDisposingSourceStream() {

            // Image.FromStream creates Bitmap instances that load the source stream lazily.
            // We want images produced by GdiImageCodec.Decode to release the source stream immediately without invaliding the bitmap data.
            // This is done by copying the bitmap data to a new Bitmap so that the source stream is released.

            // Note that Bitmap instances do not ALWAYS defer reading the stream, depending on the image content.
            // The test image used here (ICO with multiple sizes) was experimentally constructed specifically to cause this problem.

            Bitmap bitmap;
            IImageCodec codec = new GdiImageCodec();

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, "static.ico")))
            using (IImage image = codec.Decode(stream))
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

    }

}

#endif