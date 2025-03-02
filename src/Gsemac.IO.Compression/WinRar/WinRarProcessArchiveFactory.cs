using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.WinRar {

    [RequiresWinRarExecutable]
    public class WinRarProcessArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public WinRarProcessArchiveFactory() :
            this(WinRarProcessArchiveFactoryOptions.Default) {
        }
        public WinRarProcessArchiveFactory(IWinRarProcessArchiveFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            // While WinRAR supports numerous archive formats, the command-line programs ("Rar.exe" and "UnRAR.exe") only support RAR archives.
            // As well, only the former is capable of writing to archives.

            bool canWrite = WinRarUtilities.GetWinRarExecutablePath(options.WinRarDirectoryPath)
                .EndsWith(WinRarUtilities.WinRarExecutableFileName);

            return new IFileFormat[] {
                ArchiveFormat.Rar,
            }
            .OrderBy(f => f.Extensions.First())
            .Distinct()
            .Select(f => new CodecCapabilities(f, canRead: true, canWrite: canWrite));

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new UnsupportedFileFormatException(archiveFormat);

            return new WinRarProcessArchive(stream, options.WinRarDirectoryPath, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly IWinRarProcessArchiveFactoryOptions options;

    }

}