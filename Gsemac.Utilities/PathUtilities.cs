using System;

namespace Gsemac.Utilities {

    public static class PathUtilities {

        public static string GetRelativePath(string fullPath, string relativePath) {

            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentNullException(nameof(fullPath));

            if (string.IsNullOrEmpty(relativePath))
                return fullPath;

            string fullInputPath = System.IO.Path.GetFullPath(fullPath);
            string fullRelativeToPath = TrimDirectorySeparators(System.IO.Path.GetFullPath(relativePath)) + System.IO.Path.DirectorySeparatorChar;

            int index = fullInputPath.IndexOf(fullRelativeToPath);

            if (index >= 0)
                return fullInputPath.Substring(index + fullRelativeToPath.Length, fullInputPath.Length - (index + fullRelativeToPath.Length));
            else
                return fullPath;

        }
        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }

        public static string AnonymizePath(string path) {

            // Remove the current user's username from the path (if the path is inside the user directory).

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrWhiteSpace(userDirectory) && path.StartsWith(userDirectory)) {

                string userDirectoryParentDirectory = System.IO.Path.GetDirectoryName(userDirectory);

                path = System.IO.Path.Combine(userDirectoryParentDirectory, "%USERNAME%", GetRelativePath(path, userDirectory));

            }

            return path;

        }

    }

}