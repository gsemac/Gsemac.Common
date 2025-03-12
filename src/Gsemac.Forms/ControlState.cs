using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public class ControlState :
    IControlState {

        // Public members

        public void Restore(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            // Layout state is saved before visual state, and they are restored in the opposite order.
            // This is because visual state can affect the layout properties (e.g. the "Size" property).

            if (visualState is object)
                RestoreVisualState(control);

            if (layoutState is object)
                RestoreLayoutState(control);

        }

        public static IControlState Save(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            return Save(control, ControlStateOptions.Default);

        }
        public static IControlState Save(Control control, IControlStateOptions options) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return new ControlState(control, options);

        }

        // Protected members

        protected static bool TryGetResizeRedraw(Control control, out bool value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty != null) {

                value = (bool)drawModeProperty.GetValue(control, null);

                return true;

            }
            else {

                value = false;

                return false;

            }

        }
        protected static bool TrySetResizeRedraw(Control control, bool value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty != null) {

                drawModeProperty.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        protected static bool TryGetDrawMode(Control control, out DrawMode value) {

            PropertyInfo property = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (DrawMode)property.GetValue(control, null);

                return true;

            }
            else {

                value = DrawMode.Normal;

                return false;

            }

        }
        protected static bool TrySetDrawMode(Control control, DrawMode value) {

            PropertyInfo property = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        protected static bool TryGetBorderStyle(Control control, out BorderStyle value) {

            PropertyInfo property = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (BorderStyle)property.GetValue(control, null);

                return true;

            }
            else {

                value = BorderStyle.None;

                return false;

            }

        }
        protected static bool TrySetBorderStyle(Control control, BorderStyle value) {

            PropertyInfo property = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        protected static bool TryGetFlatStyle(Control control, out FlatStyle value) {

            PropertyInfo property = control.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (FlatStyle)property.GetValue(control, null);

                return true;

            }
            else {

                value = FlatStyle.Standard;

                return false;

            }

        }
        protected static bool TrySetFlatStyle(Control control, FlatStyle value) {

            PropertyInfo property = control.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        protected static bool TryGetOwnerDraw(Control control, out bool value) {

            PropertyInfo property = control.GetType().GetProperty("OwnerDraw", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (bool)property.GetValue(control, null);

                return true;

            }
            else {

                value = false;

                return false;

            }

        }
        protected static bool TrySetOwnerDraw(Control control, bool value) {

            PropertyInfo property = control.GetType().GetProperty("OwnerDraw", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        protected static bool TryGetUseVisualStyleBackColor(Control control, out bool value) {

            PropertyInfo property = control.GetType().GetProperty("UseVisualStyleBackColor", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (bool)property.GetValue(control, null);

                return true;

            }
            else {

                value = true;

                return false;

            }

        }
        protected static bool TrySetUseVisualStyleBackColor(Control control, bool value) {

            PropertyInfo property = control.GetType().GetProperty("UseVisualStyleBackColor", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }

        // Private members

        private class LayoutState {

            public AnchorStyles Anchor { get; set; }
            public bool AutoSize { get; set; }
            public DockStyle Dock { get; set; }
            public Point Location { get; set; }
            public Size Size { get; set; }
            public Point? BottomRightPadding { get; set; }

        }

        private class VisualState {

            public Color BackColor { get; set; }
            public BorderStyle BorderStyle { get; set; }
            public DrawMode DrawMode { get; set; }
            public bool EnableHeadersVisualStyles { get; set; }
            public FlatStyle FlatStyle { get; set; }
            public Color ForeColor { get; set; }
            public bool GridLines { get; set; }
            public bool OwnerDraw { get; set; }
            public bool ResizeRedraw { get; set; }
            public ControlStyles Styles { get; set; }
            public ToolStripRenderer ToolStripRenderer { get; set; }
            public TreeViewDrawMode TreeViewDrawMode { get; set; }
            public bool UseVisualStyleBackColor { get; set; }

        }

        private readonly LayoutState layoutState = null;
        private readonly VisualState visualState = null;

        private ControlState(Control control, IControlStateOptions options) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.IncludeLayoutProperties) {

                layoutState = new LayoutState() {
                    Anchor = control.Anchor,
                    AutoSize = control.AutoSize,
                    Dock = control.Dock,
                    Location = control.Location,
                    Size = control.Size,
                };

                if (control.Parent is object) {

                    layoutState.BottomRightPadding = new Point(
                        control.Parent.Width - (control.Location.X + control.Width),
                        control.Parent.Height - (control.Location.Y + control.Height));

                }

            }

            if (options.IncludeVisualProperties) {

                visualState = new VisualState() {
                    BackColor = control.BackColor,
                    ForeColor = control.ForeColor,
                    Styles = ControlUtilities.GetStyles(control)
                };

                if (TryGetResizeRedraw(control, out bool resizeRedraw))
                    visualState.ResizeRedraw = resizeRedraw;

                if (TryGetDrawMode(control, out DrawMode drawMode))
                    visualState.DrawMode = drawMode;

                if (TryGetBorderStyle(control, out BorderStyle borderStyle))
                    visualState.BorderStyle = borderStyle;

                if (TryGetFlatStyle(control, out FlatStyle flatStyle))
                    visualState.FlatStyle = flatStyle;

                if (TryGetOwnerDraw(control, out bool ownerDraw))
                    visualState.OwnerDraw = ownerDraw;

                // This property is important for restoring the visual style of controls like Buttons and TabPages.

                if (TryGetUseVisualStyleBackColor(control, out bool useVisualStyleBackColor))
                    visualState.UseVisualStyleBackColor = useVisualStyleBackColor;

                switch (control) {

                    case DataGridView dataGridView:

                        visualState.BackColor = dataGridView.BackgroundColor;
                        visualState.EnableHeadersVisualStyles = dataGridView.EnableHeadersVisualStyles;

                        break;

                    case ListView listView:

                        visualState.GridLines = listView.GridLines;

                        break;

                    case ToolStrip toolStrip:

                        visualState.ToolStripRenderer = toolStrip.Renderer;

                        break;

                    case TreeView treeView:

                        visualState.TreeViewDrawMode = treeView.DrawMode;

                        break;

                }

            }

        }

        private void RestoreLayoutState(Control control) {

            // Temporarily remove anchoring so it does not interfere with resizing/repositioning the control.

            control.Anchor = AnchorStyles.None;

            control.Location = layoutState.Location;
            control.Size = layoutState.Size;

            if (layoutState.Dock == DockStyle.None && layoutState.BottomRightPadding.HasValue && control.Parent is object) {

                Point bottomRightPadding = layoutState.BottomRightPadding.Value;

                // Resize the control such that the original relative distances from the sides of its parent control are retained.

                if (!layoutState.Anchor.HasFlag(AnchorStyles.Left) && layoutState.Anchor.HasFlag(AnchorStyles.Right)) {

                    // If the control is anchored to the right but not the left, we need to adjust its location so that it has the same relative position to the right side of its parent.
                    // The size of the control should be equivalent to its original size.

                    control.Location = new Point(control.Parent.Width - (control.Width + bottomRightPadding.X), control.Location.Y);

                }
                else if (layoutState.Anchor.HasFlag(AnchorStyles.Left) && layoutState.Anchor.HasFlag(AnchorStyles.Right)) {

                    // If the control is anchored to both sides of its parent, we need to adjust its width to compensate for both its current position and its distance from the right side of the parent.

                    control.Width = control.Parent.Width - control.Location.X - bottomRightPadding.X;

                }
                else {

                    control.Width = layoutState.Size.Width;

                }

                if (!layoutState.Anchor.HasFlag(AnchorStyles.Top) && layoutState.Anchor.HasFlag(AnchorStyles.Bottom)) {

                    // If the control is anchored to the bottom but not the top, we need to adjust its location so that it has the same relative position to the bottom side of its parent.
                    // The size of the control should be equivalent to its original size.

                    control.Location = new Point(control.Location.X, control.Parent.Height - (control.Height + bottomRightPadding.Y));

                }
                else if (layoutState.Anchor.HasFlag(AnchorStyles.Top) && layoutState.Anchor.HasFlag(AnchorStyles.Bottom)) {

                    // If the control is anchored to both sides of its parent, we need to adjust its width to compensate for both its current position and its distance from the bottom side of the parent.

                    control.Height = control.Parent.Height - control.Location.Y - bottomRightPadding.Y;

                }
                else {

                    control.Height = layoutState.Size.Height;

                }

            }
            else if (layoutState.Dock == DockStyle.Left || layoutState.Dock == DockStyle.Right) {

                control.Width = layoutState.Size.Width;

            }
            else if (layoutState.Dock == DockStyle.Top || layoutState.Dock == DockStyle.Bottom) {

                control.Height = layoutState.Size.Height;

            }
            else {

                control.Size = layoutState.Size;

            }

            control.Anchor = layoutState.Anchor;
            control.AutoSize = layoutState.AutoSize;
            control.Dock = layoutState.Dock;

        }
        private void RestoreVisualState(Control control) {

            // Set the colors to their defaults before attempting to apply the stored colors.
            // This is so we can avoid changing the colors if they are already the defaults, which makes sure the ToolStripRenderer appears correctly.

            // DataGrid is a special case where we can't set BackColor to default (Color.Empty).
            // This is because it will throw an exception if we try to set BackColor to a color with transparency (ForeColor is unaffected).

            control.BackColor = control is DataGrid ? SystemColors.Window : default;
            control.ForeColor = default;

            if (visualState.BackColor != control.BackColor)
                control.BackColor = visualState.BackColor;

            if (visualState.ForeColor != control.ForeColor)
                control.ForeColor = visualState.ForeColor;

            ControlUtilities.SetStyles(control, visualState.Styles);

            TrySetResizeRedraw(control, visualState.ResizeRedraw);
            TrySetDrawMode(control, visualState.DrawMode);
            TrySetBorderStyle(control, visualState.BorderStyle);
            TrySetFlatStyle(control, visualState.FlatStyle);
            TrySetOwnerDraw(control, visualState.OwnerDraw);
            TrySetUseVisualStyleBackColor(control, visualState.UseVisualStyleBackColor);

            switch (control) {

                case DataGridView dataGridView:

                    dataGridView.BackgroundColor = visualState.BackColor;
                    dataGridView.EnableHeadersVisualStyles = visualState.EnableHeadersVisualStyles;

                    break;

                case ListView listView:

                    listView.GridLines = visualState.GridLines;

                    break;

                case ToolStrip toolStrip:

                    toolStrip.Renderer = visualState.ToolStripRenderer;

                    break;

                case TreeView treeView:

                    treeView.DrawMode = visualState.TreeViewDrawMode;

                    break;

            }

        }

    }

}