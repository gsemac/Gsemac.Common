using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Collections.Extensions {

    public static class TreeNodeExtensions {

        // Public members

        public static void DepthFirstTraversal<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> visitor) {

            InOrderDepthFirstTraversal(root, visitor);

        }
        public static void InOrderDepthFirstTraversal<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> visitor) {

            bool earlyReturn = false;

            int leftCount = root.Children.Count / 2;

            foreach (ITreeNode<T> childNode in root.Children.Take(leftCount)) {

                if (!visitor(childNode))
                    earlyReturn = true;

                if (earlyReturn)
                    break;

            }

            if (!earlyReturn)
                earlyReturn = visitor(root);

            if (!earlyReturn) {

                foreach (ITreeNode<T> childNode in root.Children.Skip(leftCount))
                    if (!visitor(childNode))
                        break;

            }

        }
        public static void PreOrderDepthFirstTraversal<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> visitor) {

            if (visitor(root))
                foreach (ITreeNode<T> childNode in root.Children)
                    if (!visitor(childNode))
                        break;

        }
        public static void PostOrderDepthFirstTraversal<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> visitor) {

            bool earlyReturn = false;

            foreach (ITreeNode<T> childNode in root.Children) {

                if (!visitor(childNode))
                    earlyReturn = true;

                if (earlyReturn)
                    break;

            }

            if (!earlyReturn)
                visitor(root);

        }

        public static ITreeNode<T> Find<T>(this ITreeNode<T> root, T value) {

            return root.DepthFirstSearch(value);

        }
        public static ITreeNode<T> Find<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> condition) {

            return root.DepthFirstSearch(condition);

        }

        public static ITreeNode<T> DepthFirstSearch<T>(this ITreeNode<T> root, T value) {

            return DepthFirstSearch(root, node => node.Value.Equals(value));

        }
        public static ITreeNode<T> DepthFirstSearch<T>(this ITreeNode<T> root, Func<ITreeNode<T>, bool> condition) {

            ITreeNode<T> result = null;

            root.DepthFirstTraversal(node => {

                bool valueFound = condition(node);

                if (valueFound)
                    result = node;

                return !valueFound;

            });

            return result;

        }

    }

}