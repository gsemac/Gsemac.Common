using Gsemac.Text.Json.Properties;
using Newtonsoft.Json;
using System;

namespace Gsemac.Text.Json.Converters {

    /// <summary>
    /// Used to prevent circular references when using <see cref="ConcreteTypeJsonConverter{ConcreteT}"/>.
    /// </summary>
    public sealed class NoJsonConverter :
        JsonConverter {

        // Public members

        public override bool CanRead => false;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) {

            return false;

        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

            throw new NotSupportedException(ExceptionMessages.JsonConverterDoesNotSupportReading);

        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            throw new NotSupportedException(ExceptionMessages.JsonConverterDoesNotSupportWriting);

        }

    }

}