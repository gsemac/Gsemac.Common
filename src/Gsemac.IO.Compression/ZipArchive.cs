using Gsemac.IO.Compression.Implementations;
using Gsemac.Reflection;
using System;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class ZipArchive {

        // Public members

        public static IArchive Open(string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            return FromStream(new FileStream(filePath, FileMode.OpenOrCreate, fileAccess), fileAccess, leaveOpen: false, options);

        }
        public static IArchive FromStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

#if NET45_OR_NEWER
            return new SystemIOCompressionZipArchive(stream, fileAccess, leaveOpen, options);
#else

            if (IsSharpCompressAvailable.Value)
                return new SharpCompressZipArchive(stream, fileAccess, leaveOpen, options);
            else
                return new ZipStorerZipArchive(stream, fileAccess, leaveOpen, options);

#endif

        }

        // Private members

#if !NET45_OR_NEWER

        private static Lazy<bool> IsSharpCompressAvailable { get; } = new Lazy<bool>(GetIsSharpCompressAvailable);

        private static bool GetIsSharpCompressAvailable() {

            AnyCpuFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver();

            // Check for the presence of the "SharpCompress.Archives.Zip.ZipArchive" class (in case something like ilmerge was used and the assembly is not present on disk).

            bool sharpCompressExists = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType("SharpCompress.Archives.Zip.ZipArchive") != null)
                .FirstOrDefault();

            // Check for SharpCompress on disk.

            if (!sharpCompressExists)
                sharpCompressExists = assemblyResolver.AssemblyExists("SharpCompress");

            return sharpCompressExists;

        }

#endif

    }

}