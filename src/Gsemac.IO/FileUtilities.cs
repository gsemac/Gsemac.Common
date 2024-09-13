using System;
using System.IO;
using System.Linq;
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

        public static long GetSize(string filePath) {

            if (!File.Exists(filePath))
                return 0;

            return new FileInfo(filePath).Length;

        }
        public static bool TryGetSize(string filePath, out long fileSize) {

            fileSize = default;

            if (!File.Exists(filePath))
                return false;

            try {

                fileSize = GetSize(filePath);

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

        public static string FormatBytes(long bytes) {

            return FormatBytes(bytes, ByteFormattingOptions.Default);

        }
        public static string FormatBytes(long bytes, IByteFormattingOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Based on the answer given here: https://stackoverflow.com/a/14488941 (JLRishe)

            string[] suffixes = GetSuffixesForBinaryPrefix(options.Prefix);
            long power = GetPowerForBinaryPrefix(options.Prefix);

            // Convert to bits if we're using a bit-based units.

            if (bytes < 0) {

                return $"-{FormatBytes(Math.Abs(bytes), options)}";

            }
            else if (bytes == 0) {

                return $"{bytes} {suffixes[0]}";

            }
            else {

                double bitMultiplier = options.Prefix == BinaryPrefix.BinaryBits || options.Prefix == BinaryPrefix.DecimalBits ? 8.0 : 1.0;
                double adjustedBytes = bytes * bitMultiplier;

                int order = Math.Min((int)Math.Log(adjustedBytes, power), suffixes.Length - 1); // 0 = bytes, 1 = KiB, 2 = MiB, ...

                adjustedBytes /= Math.Pow(power, order);

                // If rounding the value puts us over the threshold (it will be rounded by String.Format), move up to the next order.

                if (order < suffixes.Length - 1 && Math.Round(adjustedBytes, Math.Max(0, options.Precision)) >= (power * options.Threshold)) {

                    order += 1;
                    adjustedBytes /= power;

                }

                string fractionPartFormatting = new string('#', options.Precision);

                return string.Format("{0:0." + fractionPartFormatting + "} {1}", adjustedBytes, suffixes[order]);

            }

        }

        public static bool Exists(string filePath) {
            return File.Exists(filePath);
        }
        public static bool Exists(string filePath, bool ignoreFileExension) {

            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            if (!ignoreFileExension)
                return Exists(filePath);

            if (File.Exists(filePath))
                return true;

            string fullFilePath = Path.GetFullPath(filePath);
            string parentDirectoryPath = Path.GetDirectoryName(fullFilePath);
            string fileName = Path.GetFileNameWithoutExtension(fullFilePath);

            if (string.IsNullOrWhiteSpace(parentDirectoryPath) || !Directory.Exists(parentDirectoryPath))
                return false;

            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(parentDirectoryPath);

            string foundFilePath = parentDirectoryInfo
                .EnumerateFiles($"{fileName}.*", SearchOption.TopDirectoryOnly)
                .FirstOrDefault()?.FullName ?? string.Empty;

            return !string.IsNullOrWhiteSpace(foundFilePath);

        }

        public static FileStream Open(string filePath, FileMode fileMode, TimeSpan timeout) {

            return InternalOpen(() => File.Open(filePath, fileMode), timeout);

        }
        public static FileStream Open(string filePath, FileMode fileMode, FileAccess fileAccess, TimeSpan timeout) {

            return InternalOpen(() => File.Open(filePath, fileMode, fileAccess), timeout);

        }
        public static FileStream Open(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, TimeSpan timeout) {

            return InternalOpen(() => File.Open(filePath, fileMode, fileAccess, fileShare), timeout);

        }
        public static bool TryOpen(string filePath, FileMode fileMode, out FileStream stream) {

            stream = null;

            if (!File.Exists(filePath))
                return false;

            return InternalTryOpen(() => File.Open(filePath, fileMode), out stream);

        }
        public static bool TryOpen(string filePath, FileMode fileMode, FileAccess fileAccess, out FileStream stream) {

            stream = null;

            if (!File.Exists(filePath))
                return false;

            return InternalTryOpen(() => File.Open(filePath, fileMode, fileAccess), out stream);

        }
        public static bool TryOpen(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, out FileStream stream) {

            stream = null;

            if (!File.Exists(filePath))
                return false;

            return InternalTryOpen(() => File.Open(filePath, fileMode, fileAccess, fileShare), out stream);

        }

        public static bool Copy(string filePath, string newFilePath, bool overwrite = false, TimeSpan timeout = default) {

            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format(Properties.ExceptionMessages.FileNotFound, filePath), filePath);

            if (File.Exists(newFilePath) && !overwrite)
                throw new IOException(string.Format(Properties.ExceptionMessages.FileAlreadyExists, newFilePath));

            // We won't attempt to perform the copy if the two paths refer to the same destination.

            if (!PathsPointToSameFile(filePath, newFilePath)) {

                CreateDestinationDirectory(newFilePath);

                // Copying files using streams can be significantly slower than using File.Copy, but it ensures that the operation is synchronous.
                // Another way to do this might be using File.Exists with WaitForFile to verify that the destination file is fully written and unlocked.

                using (Stream sourceStream = Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, timeout))
                using (Stream destinationStream = Open(newFilePath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None, timeout))
                    sourceStream.CopyTo(destinationStream);

            }

            return true;

        }
        public static bool TryCopy(string filePath, string newFilePath, bool overwrite = false, TimeSpan timeout = default) {

            if (!File.Exists(filePath))
                return false;

            if (File.Exists(newFilePath) && !overwrite)
                return false;

            return TryWithTimeout(() => Copy(filePath, newFilePath, overwrite, timeout), timeout);

        }

        public static bool Move(string filePath, string newFilePath, bool overwrite = false, TimeSpan timeout = default) {

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
        public static bool TryMove(string filePath, string newFilePath, bool overwrite = false, TimeSpan timeout = default) {

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

        public static bool Rename(string filePath, string newFileName, bool overwrite = false, TimeSpan timeout = default) {

            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            string newFilePath = Path.Combine(directoryPath, newFileName);

            return Move(filePath, newFilePath, overwrite, timeout);

        }
        public static bool TryRename(string filePath, string newFileName, bool overwrite = false, TimeSpan timeout = default) {

            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            string newFilePath = Path.Combine(directoryPath, newFileName);

            return TryMove(filePath, newFilePath, overwrite, timeout);

        }

        public static bool Delete(string filePath, TimeSpan timeout = default) {

            // File.Delete doesn't wait for the operation to finish before returning, so this is a workaround.
            // More info: https://stackoverflow.com/questions/9370012/waiting-for-system-to-delete-file

            if (File.Exists(filePath)) {

                // Wait until nothing is locking the file.

                if (timeout > TimeSpan.Zero)
                    using (var stream = Open(filePath, FileMode.Open, timeout))
                        stream.Close();

                DateTimeOffset startTime = DateTimeOffset.Now;

                // Attempt to delete the file.

                File.Delete(filePath);

                // Block until the file system reports that the file no longer exists.

                if (timeout > TimeSpan.Zero && !WaitForFileToBeDeleted(filePath, startTime, timeout))
                    throw new TimeoutException(Properties.ExceptionMessages.OperationTimedOut);

            }

            // I return true even if WaitForFileToBeDeleted happens to return false.
            // This is because a successful call to File.Delete /will/ delete the file even if it's not deleted yet.
            // If this method returns false, it should guarantee that no deletion has taken place.

            return true;

        }
        public static bool TryDelete(string filePath, TimeSpan timeout = default) {

            if (!File.Exists(filePath))
                return true;

            return TryWithTimeout(() => Delete(filePath, timeout), timeout);

        }

        public static string ReadAllText(string filePath, TimeSpan timeout = default) {

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

        private static void CreateDestinationDirectory(string filePath) {

            // The user may have specified a complicated path for the output path (e.g. "/one/two/file.txt").
            // Create the full output directory if doesn't already exist.

            string newFileDirectory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(newFileDirectory) && !Directory.Exists(newFileDirectory))
                Directory.CreateDirectory(newFileDirectory);

        }
        private static bool WaitForFileToBeDeleted(string filePath, DateTimeOffset startTime, TimeSpan timeout) {

            while (File.Exists(filePath) && (DateTimeOffset.Now - startTime) < timeout)
                Thread.Sleep(DefaultSleep);

            return !File.Exists(filePath);

        }
        private static bool TryWithTimeout(Func<bool> action, TimeSpan timeout) {

            DateTimeOffset startTime = DateTimeOffset.Now;
            bool success;

            do {

                try {

                    success = action();

                }
                catch (Exception) {

                    success = false;

                    if ((DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(DefaultSleep);

                }

            } while (!success && (DateTimeOffset.Now - startTime) < timeout);

            return success;

        }
        private static bool PathsPointToSameFile(string filePath1, string filePath2) {

            return PathUtilities.AreEqual(Path.GetFullPath(filePath1), Path.GetFullPath(filePath2));

        }
        private static long GetPowerForBinaryPrefix(BinaryPrefix byteFormat) {

            switch (byteFormat) {

                case BinaryPrefix.Binary:
                case BinaryPrefix.BinaryBits:
                    return 1024;

                case BinaryPrefix.Decimal:
                case BinaryPrefix.DecimalBits:
                    return 1000;

                default:
                    throw new ArgumentOutOfRangeException(nameof(byteFormat));

            }

        }
        private static string[] GetSuffixesForBinaryPrefix(BinaryPrefix byteFormat) {

            switch (byteFormat) {

                case BinaryPrefix.Binary:
                    return new[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB", };

                case BinaryPrefix.BinaryBits:
                    return new[] { "b", "Kib", "Mib", "Gib", "Tib", "Pib", "Eib", "Zib", "Yib", };

                case BinaryPrefix.Decimal:
                    return new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", };

                case BinaryPrefix.DecimalBits:
                    return new[] { "b", "Kb", "Mb", "Gb", "Tb", "Pb", "Eb", "Zb", "Yb", };

                default:
                    throw new ArgumentOutOfRangeException(nameof(byteFormat));

            }

        }
        private static FileStream InternalOpen(Func<FileStream> fileStreamFactory, TimeSpan timeout) {

            if (fileStreamFactory is null)
                throw new ArgumentNullException(nameof(fileStreamFactory));

            DateTimeOffset startTime = DateTimeOffset.Now;
            Exception lastException;

            do {

                try {

                    // Attempt to open the file path with exclusive access.

                    return fileStreamFactory();

                }
                catch (Exception ex) {

                    lastException = ex;

                    if ((DateTimeOffset.Now - startTime) < timeout)
                        Thread.Sleep(DefaultSleep);

                }

            } while ((DateTimeOffset.Now - startTime) < timeout);

            throw new TimeoutException(Properties.ExceptionMessages.OperationTimedOut, lastException);

        }
        private static bool InternalTryOpen(Func<FileStream> fileStreamFactory, out FileStream stream) {

            if (fileStreamFactory is null)
                throw new ArgumentNullException(nameof(fileStreamFactory));

            stream = null;

            try {

                stream = fileStreamFactory();

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

    }

}