using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Extensions {

    public static class ControlExtensions {

        public static IControlState Save(this Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            return ControlState.Save(control, ControlStateOptions.Default);

        }
        public static IControlState Save(this Control control, IControlStateOptions options) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ControlState.Save(control, options);

        }
        public static void Restore(this Control control, IControlState state) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (state is null)
                throw new ArgumentNullException(nameof(state));

            state.Restore(control);

        }

    }

}