using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Utilities {

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

        public static ScrollBars GetVisibleScrollbars(ListBox control) {

            ScrollBars scrollBars = ScrollBars.None;

            if (Enumerable.Range(0, control.Items.Count).Sum(i => control.GetItemHeight(i)) > control.Height)
                scrollBars |= ScrollBars.Vertical;

            return scrollBars;

        }
        public static ScrollBars GetVisibleScrollbars(TextBox control) {

            return control.ScrollBars;

        }
        public static ScrollBars GetVisibleScrollbars(Control control) {

            ScrollBars scrollBars = ScrollBars.None;

            Size size = control.GetPreferredSize(Size.Empty);

            if (size.Height > control.Height)
                scrollBars |= ScrollBars.Vertical;

            if (size.Width > control.Width)
                scrollBars |= ScrollBars.Horizontal;

            return scrollBars;

        }

        public static bool GetStyle(Control control, ControlStyles styles) {

            return (bool)control.GetType()
                .GetMethod("GetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles });

        }
        public static ControlStyles GetStyles(Control control) {

            ControlStyles result = 0;

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                if (GetStyle(control, style))
                    result |= style;

            return result;

        }
        public static void SetStyle(Control control, ControlStyles styles, bool value) {

            control.GetType()
                .GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles, value });

        }
        public static void SetStyles(Control control, ControlStyles styles) {

            ClearStyles(control);

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                if (styles.HasFlag(style))
                    SetStyle(control, style, true);

        }

        public static void SetDoubleBuffered(Control control, bool value) {

            control.GetType()
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(control, value, null);

        }

        // Private members

        private static void ClearStyles(Control control) {

            foreach (ControlStyles style in Enum.GetValues(typeof(ControlStyles)))
                SetStyle(control, style, false);

        }

    }

}