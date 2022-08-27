using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Gsemac.IO {

    public static class FileUtilities {

        // Public members

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

        public static string FormatBytes(long totalBytes, int decimalPlaces = 1, ByteFormat byteFormat = ByteFormat.Binary) {

            // Based on the answer given here: https://stackoverflow.com/a/14488941 (JLRishe)

            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(string.Format(Properties.ExceptionMessages.MustBeGreaterThanZero, nameof(decimalPlaces)), nameof(decimalPlaces));

            string[] suffixes;
            long power;

            switch (byteFormat) {

                case ByteFormat.Binary:

                    power = 1024;
                    suffixes = new[] { "bytes", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB", };

                    break;

                case ByteFormat.Decimal:

                    power = 1000;
                    suffixes = new[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", };

                    break;

                case ByteFormat.BinaryBits:

                    power = 1024;
                    totalBytes *= 8; // Convert to bits
                    suffixes = new[] { "bits", "Kib", "Mib", "Gib", "Tib", "Pib", "Eib", "Zib", "Yib", };

                    break;

                case ByteFormat.DecimalBits:

                    power = 1000;
                    totalBytes *= 8; // Convert to bits
                    suffixes = new[] { "bits", "Kb", "Mb", "Gb", "Tb", "Pb", "Eb", "Zb", "Yb", };

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(byteFormat));

            }

            string formatStr = "{0:n" + decimalPlaces.ToString(CultureInfo.InvariantCulture) + "} {1}";

            if (totalBytes < 0) {

                return "-" + FormatBytes(Math.Abs(totalBytes), decimalPlaces);

            }
            else if (totalBytes == 0) {

                return string.Format(formatStr, totalBytes, suffixes[0]);

            }
            else {

                int order = Math.Min((int)Math.Log(totalBytes, power), suffixes.Length - 1); // 0 = bytes, 1 = KiB, 2 = MiB, ...
                double adjustedQuantity = totalBytes / Math.Pow(power, order);

                // If rounding the value puts us over the threshold (it will be rounded by String.Format), move up to the next order.

                double threshold = 0.97;

                if (order < suffixes.Length - 1 && Math.Round(adjustedQuantity, decimalPlaces) >= (power * threshold)) {

                    order += 1;
                    adjustedQuantity /= power;

                }

                return string.Format(formatStr, adjustedQuantity, suffixes[order]);

            }

        }

        public static Stream WaitForFile(string filePath, TimeSpan timeout) {

            return WaitForFile(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, timeout);

        }
        public static Stream WaitForFile(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, TimeSpan timeout) {

            DateTimeOffset startTime = DateTimeOffset.Now;
            Exception lastException;

            do {

                try {

                    // Attempt to open the file path with exclusive access.

                    return File.Open(filePath, fileMode, fileAccess, fileShare);

                }
                catch (Exception ex) {

                    lastException = ex;

                    if ((DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(DefaultSleep);

                }

            } while ((DateTimeOffset.Now - startTime) < timeout);

            throw new TimeoutException(Properties.ExceptionMessages.OperationTimedOut, lastException);

        }

        public static bool Copy(string filePath, string newFilePath, bool overwrite = false, TimeSpan? timeout = null) {

            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format(Properties.ExceptionMessages.FileNotFound, filePath), filePath);

            if (File.Exists(newFilePath) && !overwrite)
                throw new IOException(string.Format(Properties.ExceptionMessages.FileAlreadyExists, newFilePath));

            // We won't attempt to perform the copy if the two paths refer to the same destination.

            if (!PathsPointToSameFile(filePath, newFilePath)) {

                CreateDestinationDirectory(newFilePath);

                using (Stream sourceStream = WaitForFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, timeout ?? DefaultTimeout))
                using (Stream destinationStream = WaitForFile(newFilePath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None, timeout ?? DefaultTimeout))
                    sourceStream.CopyTo(destinationStream);

            }

            return true;

        }
        public static bool TryCopy(string filePath, string newFilePath, bool overwrite = false, TimeSpan? timeout = null) {

            if (!File.Exists(filePath))
                return false;

            if (File.Exists(newFilePath) && !overwrite)
                return false;

            return TryWithTimeout(() => Copy(filePath, newFilePath, overwrite, timeout), timeout);

        }
        public static bool Move(string filePath, string newFilePath, bool overwrite = false, TimeSpan? timeout = null) {

            // Be careful that we don't delete the original file if the two paths point to the same file.
            // If the paths are equivalent, the move is considered to be a success.

            if (PathsPointToSameFile(filePath, newFilePath))
                return true;

            bool success = Copy(filePath, newFilePath, overwrite, timeout);

            if (success) {

                // Delete the original file if it was successfully copied.

                success = Delete(filePath, timeout);

                // If we fail to delete the original file, delete the copied file.

                if (!success)
                    Delete(newFilePath, timeout);

            }

            return success;

        }
        public static bool TryMove(string filePath, string newFilePath, bool overwrite = false, TimeSpan? timeout = null) {

            if (!File.Exists(filePath))
                return false;

            if (File.Exists(newFilePath) && !overwrite)
                return false;

            // Be careful that we don't delete the original file if the two paths point to the same file.
            // If the paths are equivalent, the move is considered to be a success.

            if (PathsPointToSameFile(filePath, newFilePath))
                return true;

            bool success = TryCopy(filePath, newFilePath, overwrite, timeout);

            if (success) {

                // Delete the original file if it was successfully copied.

                success = TryDelete(filePath, timeout);

                // If we fail to delete the original file, delete the copied file.

                if (!success)
                    TryDelete(newFilePath, timeout);

            }

            return success;

        }
        public static bool Rename(string filePath, string newFilename, bool overwrite = false, TimeSpan? timeout = null) {

            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            string newFilePath = Path.Combine(directoryPath, newFilename);

            return Move(filePath, newFilePath, overwrite, timeout);

        }
        public static bool TryRename(string filePath, string newFilename, bool overwrite = false, TimeSpan? timeout = null) {

            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            string newFilePath = Path.Combine(directoryPath, newFilename);

            return TryMove(filePath, newFilePath, overwrite, timeout);

        }
        public static bool Delete(string filePath, TimeSpan? timeout = null) {

            // File.Delete doesn't wait for the operation to finish before returning, so this is a workaround.
            // More info: https://stackoverflow.com/questions/9370012/waiting-for-system-to-delete-file

            if (File.Exists(filePath)) {

                // Wait until nothing is locking the file.

                if (timeout.HasValue)
                    using (var stream = WaitForFile(filePath, timeout.Value))
                        stream.Close();

                DateTimeOffset startTime = DateTimeOffset.Now;

                // Attempt to delete the file.

                File.Delete(filePath);

                // Block until the file system reports that the file no longer exists.

                if (timeout.HasValue && !WaitForFileToBeDeleted(filePath, startTime, timeout))
                    throw new TimeoutException(Properties.ExceptionMessages.OperationTimedOut);

            }

            // I return true even if WaitForFileToBeDeleted happens to return false.
            // This is because a successful call to File.Delete /will/ delete the file even if it's not deleted yet.
            // If this method returns false, it should guarantee that no deletion has taken place.

            return true;

        }
        public static bool TryDelete(string filePath, TimeSpan? timeout = null) {

            if (!File.Exists(filePath))
                return true;

            return TryWithTimeout(() => Delete(filePath, timeout), timeout);

        }

        public static string ReadAllText(string filePath, TimeSpan? timeout = null) {

            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format(Properties.ExceptionMessages.FileNotFound, filePath), filePath);

            DateTimeOffset startTime = DateTimeOffset.Now;
            Exception lastException;

            do {

                try {

                    return File.ReadAllText(filePath);

                }
                catch (Exception ex) {

                    lastException = ex;

                    if ((DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(DefaultSleep);

                }

            } while ((DateTimeOffset.Now - startTime) < timeout);

            throw lastException;

        }
        public static void WriteAllText(string filePath, string contents) {

            CreateDestinationDirectory(filePath);

            File.WriteAllText(filePath, contents);

        }

        // Private members

        private static readonly TimeSpan DefaultSleep = TimeSpan.FromMilliseconds(250);
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(1);

        private static void CreateDestinationDirectory(string filePath) {

            // The user may have specified a complicated path for the output path (e.g. "/one/two/file.txt").
            // Create the full output directory if doesn't already exist.

            string newFileDirectory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(newFileDirectory) && !Directory.Exists(newFileDirectory))
                Directory.CreateDirectory(newFileDirectory);

        }
        private static bool WaitForFileToBeDeleted(string filePath, DateTimeOffset startTime, TimeSpan? timeout) {

            while (File.Exists(filePath) && (DateTimeOffset.Now - startTime) < timeout)
                Thread.Sleep(DefaultSleep);

            return !File.Exists(filePath);

        }
        private static bool TryWithTimeout(Func<bool> action, TimeSpan? timeout) {

            DateTimeOffset startTime = DateTimeOffset.Now;
            bool success;
            do {

                try {

                    success = action();

                }
                catch (Exception) {

                    success = false;

                    if (timeout.HasValue && (DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(DefaultSleep);

                }

            } while (!success && timeout.HasValue && (DateTimeOffset.Now - startTime) < timeout);

            return success;

        }
        private static bool PathsPointToSameFile(string filePath1, string filePath2) {

            return PathUtilities.AreEqual(Path.GetFullPath(filePath1), Path.GetFullPath(filePath2));

        }

    }

}