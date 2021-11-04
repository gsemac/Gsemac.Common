using System.IO;
using System.Text;

namespace Gsemac.IO {

    public static class StreamUtilities {

        // Public members

        public static string StreamToString(Stream stream) {

            return StreamToString(stream, Encoding.UTF8);

        }
        public static string StreamToString(Stream stream, Encoding encoding) {

            using (StreamReader sr = new StreamReader(stream, encoding))
                return sr.ReadToEnd();

        }

    }

}