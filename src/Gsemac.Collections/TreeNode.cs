using System.Collections.Generic;

namespace Gsemac.Collections {

    public class TreeNode<T> :
        ITreeNode<T> {

        // Public members

        public ITreeNode<T> Parent => null;
        public int Level => 0;
        public ICollection<ITreeNode<T>> Children { get; }
        public T Value { get; }

        ITreeNode ITreeNode.Parent => Parent;
        IEnumerable<ITreeNode> ITreeNode.Children => Children;

        public ITreeNode<T> GetUnderlyingNode() => null;

        public TreeNode(T value) {

            Value = value;
            Children = new TreeNodeCollection<T>(this);

        }

    }

}