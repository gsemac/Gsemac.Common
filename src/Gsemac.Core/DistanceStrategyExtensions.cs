using System;

namespace Gsemac.Core {

    public static class DistanceStrategyExtensions {

        // Public members

        public static double ComputeSimilarity<T>(this IDistanceStrategy<T> distanceStrategy, T first, T second) {

            if (distanceStrategy is null)
                throw new ArgumentNullException(nameof(distanceStrategy));

            double distance = distanceStrategy.ComputeDistance(first, second, normalize: true);
            double similarity = 1.0 - distance;

            return Math.Max(0.0, Math.Min(similarity, 1.0));

        }

    }

}