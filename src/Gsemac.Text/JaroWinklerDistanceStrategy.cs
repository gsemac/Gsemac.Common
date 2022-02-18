using Gsemac.Core.Extensions;
using System;

namespace Gsemac.Text {

    internal class JaroWinklerDistanceStrategy :
        IStringDistanceStrategy {

        // Public members

        public double ComputeDistance(string first, string second, bool normalizeResult = false) {

            // An implementation for Jaro–Winkler can be found here: https://stackoverflow.com/a/19165108/5383169 (leebickmtu)
            // Another implementation with addditional commentary can be found here: https://www.geeksforgeeks.org/jaro-and-jaro-winkler-similarity/ (andrew1234)

            double jaroSimilarity = new JaroDistanceStrategy().ComputeSimilarity(first, second);

            if (jaroSimilarity <= WeightThreshold)
                return 1.0 - jaroSimilarity;

            // Calculate the length of the common prefix up to NumChars.

            int maxPrefixLength = Math.Min(NumChars, Math.Min(first.Length, second.Length));
            int prefixLength = 0;

            for (int i = 0; i < maxPrefixLength; ++i) {

                if (first[i] != second[i])
                    break;

                ++prefixLength;

            }

            if (prefixLength <= 0)
                return 1.0 - jaroSimilarity;

            double jaroWinklerSimilarity = jaroSimilarity + ScaleFactor * prefixLength * (1.0 - jaroSimilarity);

            return 1.0 - jaroWinklerSimilarity;

        }

        // Private members

        // The following constants are taken from Winkler's paper.

        private const double WeightThreshold = 0.7;
        private const int NumChars = 4;
        private const double ScaleFactor = 0.1;

    }

}