using System;

namespace Gsemac.IO.Compression.Implementations {

    internal static class ArchiveExceptions {

        public static Exception ArchiveIsReadOnly => new UnauthorizedAccessException("This archive is read-only.");
        public static Exception CommentsNotSupported => new NotSupportedException("This archive does not support comments.");
        public static Exception EntryDoesNotBelongToThisArchive => new ArgumentException("The entry does not belong to this archive.");

    }

}