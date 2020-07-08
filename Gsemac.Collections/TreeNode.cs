using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Collections {

    class TreeNode<T> :
        ITreeNode<T> {

        // Public members

        public ITreeNode<T> Parent => null;
        public int Level => 0;
        public ICollection<ITreeNode<T>> Children { get; }
        public T Value { get; }

        public ITreeNode<T> GetUnderlyingNode() => null;

        public TreeNode(T value) {

            Value = value;
            Children = new TreeNodeCollection<T>(this);

        }

    }

}