namespace Gsemac.IO.Compression {

    public static class ArchiveFormat {

        public static IFileFormat Zip => new ZipFileFormat();
        public static IFileFormat SevenZip => new SevenZipFileFormat();

    }

}