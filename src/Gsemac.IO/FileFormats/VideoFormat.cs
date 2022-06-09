using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

    public static class VideoFormat {

        // Public members

        public static IFileFormat MP4 => new MP4FileFormat();

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(VideoFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}