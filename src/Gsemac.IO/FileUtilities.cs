using System;
using System.IO;
using System.Security.Cryptography;

namespace Gsemac.IO {

    public static class FileUtilities {

        public static string CalculateMD5Hash(string filePath) {

            using (MD5 md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath)) {

                var hash = md5.ComputeHash(stream);

                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            }

        }

        public static bool TryDelete(string filePath) {

            if (!File.Exists(filePath))
                return true;

            try {

                File.Delete(filePath);

                return true;

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) {

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

    }

}