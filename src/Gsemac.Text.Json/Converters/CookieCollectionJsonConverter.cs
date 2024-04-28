using Newtonsoft.Json;
using System;
using System.Net;

namespace Gsemac.Text.Json.Converters {

    public sealed class CookieCollectionJsonConverter :
    JsonConverter<CookieCollection> {

        // Public members

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override CookieCollection ReadJson(JsonReader reader, Type objectType, CookieCollection existingValue, bool hasExistingValue, JsonSerializer serializer) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (objectType is null)
                throw new ArgumentNullException(nameof(objectType));

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            if (hasExistingValue && existingValue is null)
                throw new ArgumentNullException(nameof(existingValue));

            CookieCollection result = hasExistingValue ? existingValue : new CookieCollection();
            CookieJsonConverter cookieJsonConverter = new CookieJsonConverter();

            while (reader.TokenType != JsonToken.EndArray && reader.TokenType != JsonToken.EndObject) {

                Cookie cookie = cookieJsonConverter.ReadJson(reader, typeof(Cookie), null, false, serializer);

                if (cookie is object)
                    result.Add(cookie);

            }

            // Eat the "EndArray" token.

            if (reader.TokenType == JsonToken.EndArray)
                reader.Read();

            return result;

        }
        public override void WriteJson(JsonWriter writer, CookieCollection value, JsonSerializer serializer) {

            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            CookieJsonConverter cookieJsonConverter = new CookieJsonConverter();

            writer.WriteStartArray();

            foreach (Cookie cookie in value)
                cookieJsonConverter.WriteJson(writer, cookie, serializer);

            writer.WriteEndArray();

        }

    }

}