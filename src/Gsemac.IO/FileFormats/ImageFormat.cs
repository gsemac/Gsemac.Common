using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

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

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(ImageFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}