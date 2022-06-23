using System;

namespace Gsemac.IO.Extensions {

    public static class PeekBufferStreamReaderExtensions {

        // Public members

        public static string PeekString(this PeekBufferStreamReader streamReader, int count) {

            if (streamReader is null)
                throw new ArgumentNullException(nameof(streamReader));

            char[] buffer = new char[count];

            int charsRead = streamReader.Peek(buffer, 0, count);

            if (charsRead <= 0)
                return string.Empty;

            return new string(buffer, 0, charsRead);

        }

    }

}