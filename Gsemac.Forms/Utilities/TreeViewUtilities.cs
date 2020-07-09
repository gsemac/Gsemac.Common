using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Utilities {

    public static class TreeViewUtilities {

        // Public members

        public static IEnumerable<TreeNode> GetVisibleNodes(TreeView treeView) {

            return treeView.Nodes.Cast<TreeNode>()
                .SelectMany(node => GetVisibleNodes(node));

        }

        public static void SetAlternatingNodeColors(TreeView treeView, Color evenColor, Color oddColor) {

            treeView.BeginUpdate();

            bool even = true;

            foreach (TreeNode node in GetVisibleNodes(treeView)) {

                node.BackColor = even ? evenColor : oddColor;

                even = !even;

            }

            treeView.EndUpdate();

        }

        // Private members

        private static IEnumerable<TreeNode> GetVisibleNodes(TreeNode node) {

            if (node.Parent == null || node.Parent.IsExpanded)
                yield return node;

            foreach (TreeNode childNode in node.Nodes)
                foreach (TreeNode visibleNode in GetVisibleNodes(childNode))
                    yield return visibleNode;

        }

    }

}