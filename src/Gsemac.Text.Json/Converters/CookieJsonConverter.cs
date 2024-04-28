using Newtonsoft.Json;
using System;
using System.Net;

namespace Gsemac.Text.Json.Converters {

    public sealed class CookieJsonConverter :
        JsonConverter<Cookie> {

        // Public members

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override Cookie ReadJson(JsonReader reader, Type objectType, Cookie existingValue, bool hasExistingValue, JsonSerializer serializer) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (objectType is null)
                throw new ArgumentNullException(nameof(objectType));

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            if (hasExistingValue && existingValue is null)
                throw new ArgumentNullException(nameof(existingValue));

            Cookie cookie = new Cookie();

            while (reader.TokenType != JsonToken.EndObject) {

                if (reader.TokenType == JsonToken.PropertyName) {

                    string propertyName = reader.Value.ToString()
                        .ToLowerInvariant();

                    switch (propertyName) {

                        case "domain":

                            cookie.Domain = reader.ReadAsString();

                            break;

                        case "expires": {

                                DateTime? expiryTime = reader.ReadAsDateTime();

                                if (expiryTime.HasValue)
                                    cookie.Expires = expiryTime.Value;

                            }

                            break;

                        case "name":

                            cookie.Name = reader.ReadAsString();

                            break;

                        case "value":

                            cookie.Value = reader.ReadAsString();

                            break;

                        case "path":

                            cookie.Path = reader.ReadAsString();

                            break;

                    }

                }

                reader.Read();

            }

            // Eat the "EndObject" token.

            reader.Read();

            return cookie;

        }
        public override void WriteJson(JsonWriter writer, Cookie value, JsonSerializer serializer) {

            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            writer.WriteStartObject();

            writer.WritePropertyName("Domain");
            serializer.Serialize(writer, value.Domain);

            if (value.Expires != default) {

                writer.WritePropertyName("Expires");
                serializer.Serialize(writer, value.Expires);

            }

            writer.WritePropertyName("Name");
            serializer.Serialize(writer, value.Name);

            writer.WritePropertyName("Value");
            serializer.Serialize(writer, value.Value);

            writer.WritePropertyName("Path");
            serializer.Serialize(writer, value.Path);

            writer.WriteEndObject();

        }

    }

}