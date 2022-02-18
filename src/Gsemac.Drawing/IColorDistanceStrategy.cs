using System.Drawing;

namespace Gsemac.Drawing {

    public interface IColorDistanceStrategy {

        double ComputeDistance(Color first, Color second, bool normalizeResult = true);

    }

}