using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFormat {

        // Public members

        public static IFileFormat Jpeg => new JpegFileFormat();
        public static IFileFormat Png => new PngFileFormat();

    }

}