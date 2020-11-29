using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public class TreeNodeCollection<T> :
        ICollection<ITreeNode<T>> {

        // Public members

        public int Count => nodes.Count;
        public bool IsReadOnly => false;

        public TreeNodeCollection() {
        }
        public TreeNodeCollection(ITreeNode<T> parentNode) {

            this.parentNode = parentNode;

        }

        public void Add(ITreeNode<T> item) {

            nodes.Add(GetUnderlyingNode(item));

        }
        public void Add(T item) {

            Add(new TreeNode<T>(item));

        }
        public void Clear() {

            nodes.Clear();

        }
        public bool Contains(ITreeNode<T> item) {

            return nodes.Contains(GetUnderlyingNode(item));

        }
        public void CopyTo(ITreeNode<T>[] array, int arrayIndex) {

            nodes.CopyTo(array, arrayIndex);

        }
        public IEnumerator<ITreeNode<T>> GetEnumerator() {

            IEnumerable<ITreeNode<T>> result;

            if (parentNode != null)
                result = nodes.Select(node => new NodeWrapper(node, parentNode));
            else
                result = nodes;

            return nodes.GetEnumerator();

        }
        public bool Remove(ITreeNode<T> item) {

            return nodes.Remove(GetUnderlyingNode(item));

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private class NodeWrapper :
            ITreeNode<T> {

            // Public members

            public ITreeNode<T> Parent { get; }
            public int Level { get; }
            public ICollection<ITreeNode<T>> Children => underlyingNode.Children;
            public T Value => underlyingNode.Value;

            ITreeNode ITreeNode.Parent => Parent;
            IEnumerable<ITreeNode> ITreeNode.Children => Children;

            public NodeWrapper(ITreeNode<T> underlyingNode, ITreeNode<T> parentNode) {

                Parent = parentNode;
                Level = parentNode.Level + 1;

                this.underlyingNode = underlyingNode;

            }

            public ITreeNode<T> GetUnderlyingNode() {

                return underlyingNode;

            }

            // Private members

            private readonly ITreeNode<T> underlyingNode;

        }

        private readonly ITreeNode<T> parentNode;
        private readonly IList<ITreeNode<T>> nodes = new List<ITreeNode<T>>();

        private ITreeNode<T> GetUnderlyingNode(ITreeNode<T> node) {

            if (node.GetUnderlyingNode() is null)
                return node;
            else
                return GetUnderlyingNode(node.GetUnderlyingNode());

        }

    }

}