using System;

namespace Gsemac.IO.Compression.Implementations {

    internal static class ArchiveExceptions {

        public static Exception ArchiveIsReadOnly => new UnauthorizedAccessException("This archive is read-only.");
        public static Exception ReadingCommentsIsNotSupported => new NotSupportedException("This archive does not support reading comments.");
        public static Exception WritingCommentsIsNotSupported => new NotSupportedException("This archive does not support writing comments.");
        public static Exception EntryDoesNotBelongToThisArchive => new ArgumentException("The entry does not belong to this archive.");

    }

}