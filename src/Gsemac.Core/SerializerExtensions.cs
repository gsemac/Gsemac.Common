using System;
using System.IO;
using System.Text;

namespace Gsemac.Core {

    public static class SerializerExtensions {

        public static string Serialize<T>(this ISerializer<T> serializer, T data) {

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            using (MemoryStream stream = new MemoryStream()) {

                serializer.Serialize(stream, data);

                return Encoding.UTF8.GetString(stream.ToArray());

            }

        }
        public static T Deserialize<T>(this ISerializer<T> serializer, string data) {

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                return serializer.Deserialize(stream);

        }

    }

}