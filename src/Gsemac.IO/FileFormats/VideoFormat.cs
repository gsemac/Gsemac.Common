using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

    public static class VideoFormat {

        // Public members

        public static IFileFormat Mkv => new MkvFileFormat();
        public static IFileFormat Mp4 => new Mp4FileFormat();
        public static IFileFormat WebM => new WebMFileFormat();

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(VideoFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}