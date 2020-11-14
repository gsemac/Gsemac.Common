namespace Gsemac.IO {

    public static class DirectoryUtilities {

        public static string FindFileByFileName(string directoryPath, string fileName, FindFileOptions options = FindFileOptions.Default) {

            string fullPath = string.Empty;

            if (!string.IsNullOrEmpty(fileName) && System.IO.Directory.Exists(directoryPath)) {

                if (options.HasFlag(FindFileOptions.IgnoreCase))
                    fileName = fileName.ToLowerInvariant();

                foreach (string filePath in System.IO.Directory.EnumerateFiles(directoryPath, "*", System.IO.SearchOption.AllDirectories)) {

                    string candidateFileName = options.HasFlag(FindFileOptions.IgnoreExtension) ?
                        System.IO.Path.GetFileNameWithoutExtension(filePath) :
                        System.IO.Path.GetFileName(filePath);

                    if (options.HasFlag(FindFileOptions.IgnoreCase))
                        candidateFileName = candidateFileName.ToLowerInvariant();

                    if (candidateFileName.Equals(fileName)) {

                        fullPath = filePath;

                        break;

                    }

                }

            }

            return fullPath;

        }

    }

}