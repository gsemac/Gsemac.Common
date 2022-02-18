namespace Gsemac.Text {

    public interface IStringDistanceStrategy {

        double ComputeDistance(string first, string second, bool normalizeResult = true);

    }

}