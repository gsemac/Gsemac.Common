using System.Linq;

namespace Gsemac.Core.Extensions {

    public static class VersionExtensions {

        public static System.Version ToVersion(this IVersion version) {

            int[] revisionNumbers = version.RevisionNumbers.ToArray();

            if (revisionNumbers.Length <= 0)
                return new System.Version(0, 0);

            if (revisionNumbers.Length == 1)
                return new System.Version(revisionNumbers[0], 0);

            if (revisionNumbers.Length == 2)
                return new System.Version(revisionNumbers[0], revisionNumbers[1]);

            if (revisionNumbers.Length == 3)
                return new System.Version(revisionNumbers[0], revisionNumbers[1], revisionNumbers[2]);

            return new System.Version(revisionNumbers[0], revisionNumbers[1], revisionNumbers[2], revisionNumbers[3]);

        }

    }

}