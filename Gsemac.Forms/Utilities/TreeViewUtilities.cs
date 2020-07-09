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

        public static void SetAlternatingNodeColorsEnabled(TreeView treeView, bool value) {

            // When we set the back color of TreeNodes to a custom color, selection turns the node white, making it look like it's flickering.
            // The following event handlers help to reduce the appearance of flicker.

            if (value) {

                treeView.AfterExpand += AlternatingNodeColorsAfterExpandEventHandler;
                treeView.AfterCollapse += AlternatingNodeColorsAfterExpandEventHandler;
                treeView.MouseDown += AlternatingNodeColorsMouseDownEventHandler;
                treeView.MouseUp += AlternatingNodeColorsMouseUpEventHandler;


            }
            else {

                treeView.AfterExpand -= AlternatingNodeColorsAfterExpandEventHandler;
                treeView.AfterCollapse -= AlternatingNodeColorsAfterExpandEventHandler;
                treeView.MouseDown -= AlternatingNodeColorsMouseDownEventHandler;
                treeView.MouseUp -= AlternatingNodeColorsMouseUpEventHandler;

            }

            ApplyAlternatingNodeColors(treeView);

        }

        // Private members

        private static IEnumerable<TreeNode> GetVisibleNodes(TreeNode node) {

            if (node.Parent == null || node.Parent.IsExpanded)
                yield return node;

            foreach (TreeNode childNode in node.Nodes)
                foreach (TreeNode visibleNode in GetVisibleNodes(childNode))
                    yield return visibleNode;

        }

        private static void ApplyAlternatingNodeColors(TreeView treeView) {

            treeView.BeginUpdate();

            bool even = true;

            foreach (TreeNode node in GetVisibleNodes(treeView)) {

                node.BackColor = even ? default : SystemColors.ControlLight;

                even = !even;

            }

            treeView.EndUpdate();

        }

        private static void AlternatingNodeColorsAfterExpandEventHandler(object sender, TreeViewEventArgs e) {

            if (sender is TreeView treeView)
                ApplyAlternatingNodeColors(treeView);

        }
        private static void AlternatingNodeColorsMouseDownEventHandler(object sender, MouseEventArgs e) {

            if (sender is TreeView treeView) {

                treeView.SelectedNode = null;

                ApplyAlternatingNodeColors(treeView);

                TreeViewHitTestInfo hitTestInfo = treeView.HitTest(e.Location);

                if (hitTestInfo.Node != null)
                    hitTestInfo.Node.BackColor = default;

            }

        }
        private static void AlternatingNodeColorsMouseUpEventHandler(object sender, MouseEventArgs e) {

            if (sender is TreeView treeView)
                ApplyAlternatingNodeColors(treeView);

        }

    }

}