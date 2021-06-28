using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFormat {

        // Public members

        public static IFileFormat Avif => new AvifFileFormat();
        public static IFileFormat Gif => new GifFileFormat();
        public static IFileFormat Jpeg => new JpegFileFormat();
        public static IFileFormat Png => new PngFileFormat();
        public static IFileFormat WebP => new WebPFileFormat();

    }

}