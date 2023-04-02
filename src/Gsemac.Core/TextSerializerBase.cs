using System;
using System.IO;
using System.Text;

namespace Gsemac.Core {

    public abstract class TextSerializerBase<T> :
        SerializerBase<T> {

        // Public members

        public abstract T Deserialize(string data);
        public abstract string Serialize(T data);

        public sealed override T Deserialize(Stream inputStream) {

            if (inputStream is null)
                throw new ArgumentNullException(nameof(inputStream));

            using (StreamReader reader = new StreamReader(inputStream, encoding))
                return Deserialize(reader.ReadToEnd());

        }
        public sealed override void Serialize(Stream outputStream, T data) {

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            byte[] serializedBytes = encoding.GetBytes(Serialize(data));

            outputStream.Write(serializedBytes, 0, serializedBytes.Length);

        }

        // Protected members

        protected TextSerializerBase() :
            this(Encoding.UTF8) {
        }
        protected TextSerializerBase(Encoding encoding) {

            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            this.encoding = encoding;

        }

        // Private members

        private readonly Encoding encoding = Encoding.UTF8;

    }

}