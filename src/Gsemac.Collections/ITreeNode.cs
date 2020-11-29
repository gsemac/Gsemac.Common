using System.Collections.Generic;

namespace Gsemac.Collections {

    public interface ITreeNode {

        ITreeNode Parent { get; }
        int Level { get; }
        IEnumerable<ITreeNode> Children { get; }

    }

    public interface ITreeNode<T> :
        ITreeNode {

        new ITreeNode<T> Parent { get; }
        new ICollection<ITreeNode<T>> Children { get; }

        T Value { get; }

        ITreeNode<T> GetUnderlyingNode();

    }

}