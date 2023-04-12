using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class SplitContainerUtilities {

        // Public members

        public static void SetSmoothResizingEnabled(SplitContainer splitContainer, bool value) {

            if (value) {

                splitContainer.MouseDown += SmoothResizingMouseDownEventHandler;
                splitContainer.MouseUp += SmoothResizingMouseUpEventHandler;
                splitContainer.MouseMove += SmoothResizingMouseMoveEventHandler;

            }
            else {

                splitContainer.MouseDown -= SmoothResizingMouseDownEventHandler;
                splitContainer.MouseUp -= SmoothResizingMouseUpEventHandler;
                splitContainer.MouseMove -= SmoothResizingMouseMoveEventHandler;

            }

        }

        // Private members

        private static void SmoothResizingMouseDownEventHandler(object sender, MouseEventArgs e) {

            // Temporarily disable normal resizing behavior.

            if (sender is SplitContainer splitContainer)
                splitContainer.IsSplitterFixed = true;

        }
        private static void SmoothResizingMouseUpEventHandler(object sender, MouseEventArgs e) {

            // Restore normal resizing behavior.

            if (sender is SplitContainer splitContainer)
                splitContainer.IsSplitterFixed = false;

        }
        private static void SmoothResizingMouseMoveEventHandler(object sender, MouseEventArgs e) {

            if (sender is SplitContainer splitContainer && splitContainer.IsSplitterFixed) {

                if (e.Button == MouseButtons.Left) {

                    if (splitContainer.Orientation == Orientation.Vertical && e.X > 0 && e.X < splitContainer.Width) {

                        splitContainer.SplitterDistance = e.X;

                    }
                    else if (e.Y > 0 && e.Y < splitContainer.Height) {

                        splitContainer.SplitterDistance = e.Y;

                    }

                    splitContainer.Refresh();

                }
                else {

                    splitContainer.IsSplitterFixed = false;

                }

            }

        }

    }

}