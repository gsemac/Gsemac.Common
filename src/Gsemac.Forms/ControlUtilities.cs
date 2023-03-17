using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class ControlUtilities {

        // Public members

        public static bool MouseIntersectsWith(Control control) {

            return MouseIntersectsWith(control, control.ClientRectangle);

        }
        public static bool MouseIntersectsWith(Control control, Rectangle rect) {

            Point mousePos = control.PointToClient(Cursor.Position);
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

            return rect.IntersectsWith(mouseRect);

        }

        public static bool ContainsText(Control control, string text, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (control.Text.IndexOf(text, comparisonType) != -1)
                return true;

            foreach (Control childControl in control.Controls)
                if (ContainsText(childControl, text, comparisonType))
                    return true;

            return false;

        }

        public static string EscapeLabelText(string text) {

            // The "UseMnemonic" property can be set to "false" instead of escaping the text.
            // However, this property is not available on the ToolStripLabel control.

            if (string.IsNullOrWhiteSpace(text))
                return text;

            return text.Replace("&", "&&");

        }
        public static string UnescapeLabelText(string text) {

            if (string.IsNullOrWhiteSpace(text))
                return text;

            return text.Replace("&&", "&");

        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment contentAlignment) {

            TextFormatFlags flags = TextFormatFlags.Default;

            if ((contentAlignment & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
                flags |= TextFormatFlags.Top;

            if ((contentAlignment & (ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft)) != 0)
                flags |= TextFormatFlags.Left;

            if ((contentAlignment & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
                flags |= TextFormatFlags.Right;

            if ((contentAlignment & (ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft)) != 0)
                flags |= TextFormatFlags.Bottom;

            if ((contentAlignment & (ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft)) != 0)
                flags |= TextFormatFlags.VerticalCenter;

            if ((contentAlignment & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
                flags |= TextFormatFlags.HorizontalCenter;

            return flags;

        }
        public static TextFormatFlags GetTextFormatFlags(HorizontalAlignment horizontalAlignment) {

            TextFormatFlags flags = TextFormatFlags.Default;

            if (horizontalAlignment == HorizontalAlignment.Left)
                flags |= TextFormatFlags.Left;
            else if (horizontalAlignment == HorizontalAlignment.Center)
                flags |= TextFormatFlags.HorizontalCenter;
            else if (horizontalAlignment == HorizontalAlignment.Right)
                flags |= TextFormatFlags.Right;

            return flags;

        }

        public static ScrollBars GetVisibleScrollBars(ListBox control) {

            ScrollBars scrollBars = ScrollBars.None;

            if (Enumerable.Range(0, control.Items.Count).Sum(i => control.GetItemHeight(i)) > control.Height)
                scrollBars |= ScrollBars.Vertical;

            return scrollBars;

        }
        public static ScrollBars GetVisibleScrollBars(TextBox control) {

            ScrollBars scrollBars = ScrollBars.None;

            if (control.Multiline) {

                scrollBars = control.ScrollBars;

                // If WordWrap is enabled, the horizontal scroll bar never appears, even if it is enabled.

                if (control.WordWrap)
                    scrollBars &= ~ScrollBars.Horizontal;

            }

            return scrollBars;

        }
        public static ScrollBars GetVisibleScrollBars(Control control) {

            ScrollBars scrollBars = ScrollBars.None;

            Size size = control.GetPreferredSize(Size.Empty);

            if (size.Height > control.Height)
                scrollBars |= ScrollBars.Vertical;

            if (size.Width > control.Width)
                scrollBars |= ScrollBars.Horizontal;

            return scrollBars;

        }

        public static bool GetStyle(Control control, ControlStyles styles) {

            MethodInfo getStyleMethod = control.GetType()
                .GetMethod("GetStyle", BindingFlags.Instance | BindingFlags.NonPublic);

            if (getStyleMethod is null)
                return false;

            return (bool)getStyleMethod.Invoke(control, new object[] { styles });

        }
        public static bool SetStyle(Control control, ControlStyles styles, bool value) {

            MethodInfo setStyleMethod = control.GetType()
                .GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);

            if (setStyleMethod is null)
                return false;

            setStyleMethod.Invoke(control, new object[] { styles, value });

            return true;

        }
        public static ControlStyles GetStyles(Control control) {

            ControlStyles result = 0;

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                if (GetStyle(control, style))
                    result |= style;

            return result;

        }
        public static bool SetStyles(Control control, ControlStyles styles) {

            ClearStyles(control);

            bool success = true;

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                if (styles.HasFlag(style))
                    success &= SetStyle(control, style, true);

            return success;

        }

        public static bool SetDoubleBuffered(Control control, bool value) {

            PropertyInfo doubleBufferedProperty = control.GetType()
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            if (doubleBufferedProperty is null)
                return false;

            doubleBufferedProperty.SetValue(control, value, null);

            return true;

        }
        public static bool SetResizeRedraw(Control control, bool value) {

            // ResizeRedraw needs to be set to true to prevent smearing when custom-painting controls.
            // https://stackoverflow.com/a/39419274/5383169

            PropertyInfo drawModeProperty = control.GetType()
                .GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance);

            if (drawModeProperty is null)
                return false;

            drawModeProperty.SetValue(control, value, null);

            return true;

        }

        public static IEnumerable<IComponent> GetComponents(Control control) {

            // Returns components added via the designer (e.g. ToolTips).

            FieldInfo fieldInfo = control.GetType()
                .GetField("components", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldInfo is object && fieldInfo.GetValue(control) is IContainer container) {

                return container.Components.OfType<IComponent>();

            }
            else {

                return Enumerable.Empty<IComponent>();

            }

        }
        public static IEnumerable<ToolTip> GetToolTips(Control control) {

            return GetComponents(control).OfType<ToolTip>();

        }

        // Private members

        private static void ClearStyles(Control control) {

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                SetStyle(control, style, false);

        }

    }

}