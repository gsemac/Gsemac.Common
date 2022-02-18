using System;

namespace Gsemac.Text {

    public class HammingDistanceStrategy :
        IStringDistanceStrategy {

        // Public members

        public double ComputeDistance(string first, string second, bool normalizeResult = false) {

            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
                return 0;

            // Hamming distance assumes that both strings are the same length.
            // To allow for strings of different lengths, we'll consider any extraneous characters non-matching.

            int difference = Math.Abs(first.Length - second.Length);
            int maxLength = Math.Max(first.Length, second.Length);

            for (int i = 0; i < Math.Min(first.Length, second.Length); ++i)
                if (first[i] != second[i])
                    ++difference;

            return normalizeResult ?
                 difference / (double)maxLength :
                 difference;

        }

    }

}
