using Gsemac.IO.Compression.Extensions;
using Gsemac.Reflection;
using System;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class ZipArchive {

        // Public members

        public static IArchive OpenFile(string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            return CompressionPluginLoader.GetArchiveReaders().First().OpenFile(filePath, fileAccess, options);

        }
        public static IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return CompressionPluginLoader.GetArchiveReaders().First().OpenStream(stream, fileAccess, leaveOpen, options);
        }

    }

}