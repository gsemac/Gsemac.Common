using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class SplitContainerUtilities {

        // Public members

        public static void SetRealTimeResizingEnabled(SplitContainer splitContainer, bool value) {

            if (value) {

                splitContainer.MouseDown += RealTimeResizingMouseDownEventHandler;
                splitContainer.MouseUp += RealTimeResizingMouseUpEventHandler;
                splitContainer.MouseMove += RealTimeResizingMouseMoveEventHandler;

            }
            else {

                splitContainer.MouseDown -= RealTimeResizingMouseDownEventHandler;
                splitContainer.MouseUp -= RealTimeResizingMouseUpEventHandler;
                splitContainer.MouseMove -= RealTimeResizingMouseMoveEventHandler;

            }

        }

        // Private members

        private static void RealTimeResizingMouseDownEventHandler(object sender, MouseEventArgs e) {

            // Temporarily disable normal resizing behavior.

            if (sender is SplitContainer splitContainer)
                splitContainer.IsSplitterFixed = true;

        }
        private static void RealTimeResizingMouseUpEventHandler(object sender, MouseEventArgs e) {

            // Restore normal resizing behavior.

            if (sender is SplitContainer splitContainer)
                splitContainer.IsSplitterFixed = false;

        }
        private static void RealTimeResizingMouseMoveEventHandler(object sender, MouseEventArgs e) {

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