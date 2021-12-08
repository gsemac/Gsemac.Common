using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.FileFormats {

    public static class ArchiveFormat {

        public static IFileFormat Rar => new RarFileFormat();
        public static IFileFormat SevenZip => new SevenZipFileFormat();
        public static IFileFormat Zip => new ZipFileFormat();

        public static IEnumerable<IFileFormat> GetFormats() {

            return typeof(ArchiveFormat).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null, null))
                .Cast<IFileFormat>();

        }

    }

}