using System.Windows.Forms;

namespace Gsemac.Forms {

    public static class ToolStripUtilities {

        public static void SetRoundedEdgesEnabled(ToolStrip control, bool roundedEdges) {

            if (control.Renderer is ToolStripProfessionalRenderer professionalRenderer)
                professionalRenderer.RoundedEdges = roundedEdges;

        }

    }

}