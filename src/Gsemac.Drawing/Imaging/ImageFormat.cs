using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFormat {

        // Public members

        public static IFileFormat Avif => new AvifFileFormat();
        public static IFileFormat Bmp => new BmpFileFormat();
        public static IFileFormat Gif => new GifFileFormat();
        public static IFileFormat Ico => new IcoFileFormat();
        public static IFileFormat Jpeg => new JpegFileFormat();
        public static IFileFormat Jxl => new JxlFileFormat();
        public static IFileFormat Png => new PngFileFormat();
        public static IFileFormat Tiff => new TiffFileFormat();
        public static IFileFormat WebP => new WebPFileFormat();
        public static IFileFormat Wmf => new WmfFileFormat();

    }

}