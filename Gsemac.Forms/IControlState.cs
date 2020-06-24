using System;
using System.Windows.Forms;

namespace Gsemac.Forms {

    [Flags]
    public enum ControlStateOptions {
        None = 0,
        StoreLayoutProperties = 1,
        StoreVisualProperties = 2,
        Default = StoreLayoutProperties | StoreVisualProperties,
    }

    public interface IControlState {

        void Restore(Control control);

    }

}