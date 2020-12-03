using System.Drawing;

namespace Gsemac.Drawing {

    public interface IColorDistanceStrategy {

        double GetDistance(Color first, Color second, bool normalize);

    }

}