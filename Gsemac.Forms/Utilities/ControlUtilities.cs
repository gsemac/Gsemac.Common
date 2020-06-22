using System.Reflection;
using System.Windows.Forms;

namespace Gsemac.Forms.Utilities {

    public static class ControlUtilities {

        public static void SetDoubleBuffered(Control control, bool value) {

            control.GetType()
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(control, value, null);

        }

    }

}