using System.IO;
using System.Text;

namespace Gsemac.Text.Extensions {

    public static class SerializerExtensions {

        public static string Serialize<T>(this ISerializer<T> serializer, T data) {

            using (MemoryStream stream = new MemoryStream()) {

                serializer.Serialize(stream, data);

                return Encoding.UTF8.GetString(stream.ToArray());

            }

        }
        public static T Deserialize<T>(this ISerializer<T> serializer, string serializedData) {

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedData)))
                return serializer.Deserialize(stream);

        }

    }

}