using Newtonsoft.Json;
using System;

namespace Gsemac.Text.Json.Converters {

    /// <summary>
    /// Converts an interface to and from JSON using the given concrete type.
    /// </summary>
    /// <typeparam name="ConcreteT">The concrete type used to create an instance of the interface.</typeparam>
    public sealed class ConreteTypeJsonConverter<ConcreteT> :
        JsonConverter {

        // Public members

        public override bool CanConvert(Type objectType) {

            return objectType is object &&
                objectType.IsAssignableFrom(typeof(ConcreteT));

        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

            return serializer.Deserialize<ConcreteT>(reader);

        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            serializer.Serialize(writer, value);

        }

    }

}