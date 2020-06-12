using System;

namespace Gsemac.Utilities {

    public static class PathUtilities {

        public static string AnonymizePath(string path) {

            // Remove the current user's username from the path (if the path is inside the user directory).

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrWhiteSpace(userDirectory) && path.StartsWith(userDirectory, StringComparison.OrdinalIgnoreCase)) {

                path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "%USERNAME%", StringUtilities.After(path, userDirectory, StringComparison.OrdinalIgnoreCase));

            }

            return path;

        }

    }

}