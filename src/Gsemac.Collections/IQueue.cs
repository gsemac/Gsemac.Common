using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public interface IQueue<T> :
       IEnumerable,
       IEnumerable<T> {

        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        T Dequeue();
        void Enqueue(T item);
        T Peek();
        T[] ToArray();

    }

}