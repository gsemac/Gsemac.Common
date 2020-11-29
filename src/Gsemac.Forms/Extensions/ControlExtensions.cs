using System.Windows.Forms;

namespace Gsemac.Forms.Extensions {

    public static class ControlExtensions {

        public static IControlState Save(this Control control, ControlStateOptions options = ControlStateOptions.Default) {

            return ControlState.Save(control, options);

        }
        public static void Restore(this Control control, IControlState state) {

            state.Restore(control);

        }

    }

}