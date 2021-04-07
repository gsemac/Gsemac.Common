using System;
using System.IO;

namespace Gsemac.Text {

    public abstract class SerializerBase<T> :
        ISerializer<T> {

        // Public members

        public abstract void Serialize(Stream outputStream, T data);
        public abstract T Deserialize(Stream inputStream);

        public virtual void Serialize(Stream outputStream, object data) {

            if (data is T dataT)
                Serialize(outputStream, dataT);
            else
                throw new ArgumentException($"Cannot serialize objects of type `{data.GetType().Name}`.", nameof(data));

        }
        object ISerializer.Deserialize(Stream inputStream) {

            return Deserialize(inputStream);

        }

    }

}