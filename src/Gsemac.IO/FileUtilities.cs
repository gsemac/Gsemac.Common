using System;
using System.IO;
using System.Security.Cryptography;

namespace Gsemac.IO {

    public static class FileUtilities {

        public static string ComputeMD5Hash(string filePath) {

            using (MD5 md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath)) {

                var hash = md5.ComputeHash(stream);

                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            }

        }

        public static long GetFileSize(string filePath) {

            return new FileInfo(filePath).Length;

        }
        public static bool TryGetFileSize(string filePath, out long fileSize) {

            fileSize = default;

            if (!File.Exists(filePath))
                return false;

            try {

                fileSize = GetFileSize(filePath);

                return true;

            }
            catch (UnauthorizedAccessException) {

                return false;

            }

        }

        public static bool TryDelete(string filePath) {

            if (!File.Exists(filePath))
                return true;

            try {

                File.Delete(filePath);

                return true;

            }
            catch (Exception) {

                return false;

            }
        }

    }

}