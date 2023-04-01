using Gsemac.IO.FileFormats;

namespace Gsemac.IO {

    public static class FileFormat {

        public static IFileFormat Any => new WildcardFileFormat();

    }

}