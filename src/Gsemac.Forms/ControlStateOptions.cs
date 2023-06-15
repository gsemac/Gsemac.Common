namespace Gsemac.Forms {

    public class ControlStateOptions :
        IControlStateOptions {

        // Public members

        public bool IncludeLayoutProperties { get; set; } = true;
        public bool IncludeVisualProperties { get; set; } = true;

        public static ControlStateOptions Default => new ControlStateOptions();

    }

}