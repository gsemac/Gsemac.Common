using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public sealed class CodecCapabilities :
        ICodecCapabilities {

        // Public members

        public IFileFormat Format { get; }
        public bool CanRead { get; }
        public bool CanWrite { get; }

        public CodecCapabilities(IFileFormat format, bool canRead, bool canWrite) {

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            Format = format;
            CanRead = canRead;
            CanWrite = canWrite;

        }

        public static IEnumerable<ICodecCapabilities> Flatten(IEnumerable<ICodecCapabilities> formatCapabilities) {

            if (formatCapabilities is null)
                throw new ArgumentNullException(nameof(formatCapabilities));

            // We want to group formats together so that CanRead and CanWrite are true if there is at least one instance of the format for which they are true.

            return formatCapabilities.GroupBy(f => f.Format)
                .Select(group => new CodecCapabilities(group.Key, group.Any(f => f.CanRead), group.Any(f => f.CanWrite)))
                .Where(f => f.CanRead || f.CanWrite);

        }

    }

}