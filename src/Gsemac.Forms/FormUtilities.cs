using Gsemac.Win32;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class FormUtilities {

        // Public members

        public static bool IsFormOnScreen(Form form) {

            Screen[] screens = Screen.AllScreens;

            foreach (Screen screen in screens) {

                Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

                if (screen.WorkingArea.IntersectsWith(formRectangle))
                    return true;

            }

            return false;

        }
        public static void EnsureFormIsOnScreen(Form form) {

            if (!IsFormOnScreen(form)) {

                Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                int centerX = workingArea.Width / 2 - form.Width / 2;
                int centerY = workingArea.Height / 2 - form.Height / 2;

                form.Location = new Point(centerX, centerY);

            }

        }

        public static Form ShowSingleInstanceForm<T>(Form parent = null) where T : Form, new() {

            Form existingForm = Application.OpenForms.OfType<T>().FirstOrDefault();

            if (existingForm is null) {

                Form newForm = new T();

                newForm.Show(parent);

                if (newForm.StartPosition == FormStartPosition.CenterParent)
                    CenterFormToParent(newForm, parent);

                return newForm;

            }
            else {

                existingForm.Focus();

                return existingForm;

            }

        }
        public static void CenterFormToParent(Form form, Form parent) {

            if (form != null && parent != null) {

                int x = parent.Location.X + (parent.Width - form.Width) / 2;
                int y = parent.Location.Y + (parent.Height - form.Height) / 2;

                form.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));

            }

        }

        public static void SetDraggingEnabled(Form form, bool enabled) {

            if (enabled)
                form.MouseDown += DraggingMouseDownEventHandler;
            else
                form.MouseDown -= DraggingMouseDownEventHandler;

        }

        // Private members

        private static void DraggingMouseDownEventHandler(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left) {

                User32.ReleaseCapture();

                User32.SendMessage((sender as Form).Handle, User32.WM_NCLBUTTONDOWN, (IntPtr)User32.HTCAPTION, (IntPtr)0);

            }

        }

    }

}