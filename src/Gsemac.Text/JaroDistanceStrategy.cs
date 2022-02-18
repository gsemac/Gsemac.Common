using System;

namespace Gsemac.Text {

    public class JaroDistanceStrategy :
        IStringDistanceStrategy {

        // Public members

        public double ComputeDistance(string first, string second, bool normalizeResult = false) {

            // An implementation for Jaro–Winkler can be found here: https://stackoverflow.com/a/19165108/5383169 (leebickmtu)
            // Another implementation with addditional commentary can be found here: https://www.geeksforgeeks.org/jaro-and-jaro-winkler-similarity/ (andrew1234)
            // This article explains the algorithm pretty clearly: https://javascript.plainenglish.io/what-is-jaro-winkler-similarity-7448cad41a6d

            // We will only implement the Jaro distance here.

            if (first is null)
                throw new ArgumentNullException(nameof(first));

            if (second is null)
                throw new ArgumentNullException(nameof(second));

            if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
                return 0.0;

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
                return 1.0;

            int length1 = first.Length;
            int length2 = second.Length;

            // Find matching characters.

            // Two characters are only considered matching if they are no more than the below search range apart.
            // https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance#Jaro_Similarity

            int searchRange = Math.Max(length1, length2) / 2 - 1;

            bool[] matches1 = new bool[length1];
            bool[] matches2 = new bool[length2];
            int totalMatches = 0;

            for (int i = 0; i < length1; ++i) {

                // Calculate the boundaries in which we will search for a match (i +- searchRange).

                int searchStartIndex = Math.Max(i - searchRange, 0);
                int searchEndIndex = Math.Min(i + searchRange + 1, length2);

                for (int j = searchStartIndex; j < searchEndIndex; ++j) {

                    // If we find a match, we will flag the character as found for both strings.
                    // Only flag the match if we haven't already matched that character already.

                    if (first[i] == second[j] && !matches2[j]) {

                        matches1[i] = true;
                        matches2[j] = true;

                        ++totalMatches;

                        break;

                    }

                }

            }

            // If we didn't find any matching characters, the strings are entirely dissimilar.

            if (totalMatches <= 0)
                return 1.0;

            // Find number of transpositions.

            // The number of transpositions is defined as the number of matching characters that are in the wrong order.
            // In other words, the matches are there, but they may appear in a different order in the other string.

            int totalTranspositions = 0;
            int k = 0;

            for (int i = 0; i < Math.Min(matches1.Length, matches2.Length); ++i) {

                if (!matches1[i])
                    continue;

                // Find the corresponding match in the second array.

                while (!matches2[k])
                    ++k;

                if (first[i] != second[k++])
                    ++totalTranspositions;

            }

            // The result is already normalized between 0 and 1.

            double jaroSimilarity = ((totalMatches / (double)length1) + (totalMatches / (double)length2) + ((totalMatches - (totalTranspositions / 2.0)) / totalMatches)) / 3.0;

            return 1.0 - jaroSimilarity;

        }

    }

}