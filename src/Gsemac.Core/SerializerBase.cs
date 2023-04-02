using System;
using System.IO;

namespace Gsemac.Core {

    public abstract class SerializerBase<T> :
        ISerializer<T> {

        // Public members

        public abstract void Serialize(Stream outputStream, T data);
        public abstract T Deserialize(Stream inputStream);

        public virtual void Serialize(Stream outputStream, object data) {

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (data is T dataT)
                Serialize(outputStream, dataT);
            else
                throw new ArgumentException($"Attempted to serialize object of type `{data.GetType().Name}`. This serializer can only serialize objects of type `{typeof(T).Name}`.", nameof(data));

        }
        object ISerializer.Deserialize(Stream inputStream) {

            return Deserialize(inputStream);

        }

    }

}