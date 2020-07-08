using System.Collections.Generic;

namespace Gsemac.Collections {

    public interface ITreeNode<T> {

        ITreeNode<T> Parent { get; }
        int Level { get; }
        ITreeNodeCollection<T> Children { get; }

        T Value { get; }

        ITreeNode<T> GetUnderlyingNode();

    }

}