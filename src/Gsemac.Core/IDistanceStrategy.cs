namespace Gsemac.Core {

    public interface IDistanceStrategy<T> {

        double ComputeDistance(T first, T second, bool normalizeResult = false);

    }

}