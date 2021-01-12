using System;

namespace Gsemac.IO {

    public static class DirectoryUtilities {

        public static string FindFileByFilename(string directoryPath, string filename, FindFileOptions options = FindFileOptions.Default) {

            string fullPath = string.Empty;

            if (!string.IsNullOrEmpty(filename) && System.IO.Directory.Exists(directoryPath)) {

                if (options.HasFlag(FindFileOptions.IgnoreCase))
                    filename = filename.ToLowerInvariant();

                foreach (string filePath in System.IO.Directory.EnumerateFiles(directoryPath, "*", System.IO.SearchOption.AllDirectories)) {

                    string candidateFileName = options.HasFlag(FindFileOptions.IgnoreExtension) ?
                        System.IO.Path.GetFileNameWithoutExtension(filePath) :
                        System.IO.Path.GetFileName(filePath);

                    if (options.HasFlag(FindFileOptions.IgnoreCase))
                        candidateFileName = candidateFileName.ToLowerInvariant();

                    if (candidateFileName.Equals(filename)) {

                        fullPath = filePath;

                        break;

                    }

                }

            }

            return fullPath;

        }

        public static bool TryCreateDirectory(string directoryPath) {

            try {

                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

    }

}