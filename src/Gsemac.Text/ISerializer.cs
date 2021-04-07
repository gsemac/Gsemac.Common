using System.IO;

namespace Gsemac.Text {

    public interface ISerializer {

        object Deserialize(Stream inputStream);
        void Serialize(Stream outputStream, object data);

    }

    public interface ISerializer<T> :
        ISerializer {

        new T Deserialize(Stream inputStream);
        void Serialize(Stream outputStream, T data);

    }

}