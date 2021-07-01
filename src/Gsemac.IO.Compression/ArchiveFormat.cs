namespace Gsemac.IO.Compression {

    public static class ArchiveFormat {

        public static IFileFormat Rar => new RarFileFormat();
        public static IFileFormat SevenZip => new SevenZipFileFormat();
        public static IFileFormat Zip => new ZipFileFormat();

    }

}