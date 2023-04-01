using Gsemac.IO.FileFormats;
using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileFormatFactory :
        FileFormatFactoryBase {

        // Public members

        public static FileFormatFactory Default => new FileFormatFactory();

        public FileFormatFactory() { }
        public FileFormatFactory(IEnumerable<IFileFormat> knownFileFormats) {

            if (knownFileFormats is null)
                throw new ArgumentNullException(nameof(knownFileFormats));

            fileFormats = knownFileFormats;

        }

        // Internal members

        internal const int DefaultReadBufferSize = 1024;

        // Protected members

        protected override IEnumerable<IFileFormat> GetKnownFileFormats() {

            if (fileFormats is object)
                return fileFormats;

            return globalFileFormats.Value;

        }

        // Private members

        private readonly IEnumerable<IFileFormat> fileFormats;

        private static readonly Lazy<IEnumerable<IFileFormat>> globalFileFormats = new Lazy<IEnumerable<IFileFormat>>(InitializeKnownFileFormats);

        private static IEnumerable<IFileFormat> InitializeKnownFileFormats() {

            return TypeUtilities.GetTypesImplementingInterface<IFileFormat>()
                .Where(type => type != typeof(WildcardFileFormat))
                .Where(type => type.IsDefaultConstructable())
                .Select(type => Activator.CreateInstance(type))
                .OfType<IFileFormat>();

        }

    }

}