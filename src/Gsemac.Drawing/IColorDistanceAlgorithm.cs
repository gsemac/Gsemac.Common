using System.Drawing;

namespace Gsemac.Drawing {

    public interface IColorDistanceAlgorithm {

        double GetDistance(Color first, Color second, bool normalize);

    }

}