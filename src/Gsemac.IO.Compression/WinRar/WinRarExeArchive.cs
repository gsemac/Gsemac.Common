using Gsemac.Core;
using Gsemac.IO.FileFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.IO.Compression.WinRar {

    internal sealed class WinRarExeArchive :
        DeferredCreationArchiveBase {

        // Public members

        public override string Comment {
            get => newComment is null ? existingComment.Value : newComment;
            set => newComment = value;
        }
        public override CompressionLevel CompressionLevel {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportGettingCompressionLevel);
            set => compressionLevel = value;
        }

        public WinRarExeArchive(Stream stream, string winrarDirectoryPath, IFileFormat archiveFormat, IArchiveOptions options) :
            base(options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                throw new ArgumentNullException(nameof(archiveFormat));

            this.winrarDirectoryPath = winrarDirectoryPath;

            if (!File.Exists(GetWinrarExecutablePath()))
                throw new FileNotFoundException(Properties.ExceptionMessages.WinrarExecutableNotFound, winrarDirectoryPath);

            if (stream is FileStream fileStream) {

                if (options is null) {

                    FileAccess fileAccess = 0;

                    if (fileStream.CanRead)
                        fileAccess |= FileAccess.Read;

                    if (fileStream.CanWrite)
                        fileAccess |= FileAccess.Write;

                    options = new ArchiveOptions() {
                        FileAccess = fileAccess,
                    };

                }

                this.filePath = Path.GetFullPath(fileStream.Name);
                this.archiveFormat = archiveFormat;
                this.options = options;
                this.existingComment = new Lazy<string>(ReadArchiveComment);
                this.compressionLevel = options.CompressionLevel;

                if (options.Comment is object)
                    newComment = options.Comment;

                // We must close the file stream to ensure that WinRAR can access the archive.

                fileStream.Close();

            }
            else {

                throw new ArgumentException(Properties.ExceptionMessages.ArchiveOnlySupportsFileStreams);

            }

        }

        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (!GetEntries().Contains(entry))
                throw new ArchiveEntryDoesNotExistException();

            if (entry is NewArchiveEntry newArchiveEntry) {

                // If the archive is a newly-added entry that hasn't already been committed to the archive, just copy it.

                using (Stream fileStream = File.OpenRead(newArchiveEntry.FilePath))
                    fileStream.CopyTo(outputStream);

            }
            else {

                // Extract the given entry.

                ProcessStartInfo processStartInfo = CreateProcessStartInfo();

                processStartInfo.Arguments = new CmdArgumentsBuilder()
                    .WithArgument("p")
                    .WithArgument("-inul") // disable header and progress bar
                    .WithArgument(filePath)
                    .WithArgument(GetPasswordArgument())
                    .WithArgument(SanitizeEntryName(entry.Name))
                    .ToString();

                using (Stream processStream = new ProcessStream(processStartInfo, new ProcessStreamOptions(redirectStandardOutput: true)))
                    processStream.CopyTo(outputStream);

            }

        }

        // Protected members

        protected override List<IArchiveEntry> ReadArchiveEntries() {

            List<IArchiveEntry> items = new List<IArchiveEntry>();

            if (File.Exists(filePath) && FileUtilities.GetSize(filePath) > 0) {

                ProcessStartInfo processStartInfo = CreateProcessStartInfo();

                processStartInfo.Arguments = new CmdArgumentsBuilder()
                    .WithArgument("l")
                    .WithArgument(filePath)
                    .WithArgument("-scf") // output filenames in UTF-8 instead of Windows' default charset
                    .WithArgument(GetPasswordArgument(includeEmptyPassword: true))
                    .ToString();

                using (Stream processStream = new ProcessStream(processStartInfo, new ProcessStreamOptions(redirectStandardOutput: true, redirectStandardError: true) { RedirectStandardErrorToStandardOutput = true }))
                using (StreamReader streamReader = new StreamReader(processStream)) {

                    string output = streamReader.ReadToEnd();

                    foreach (Match match in Regex.Matches(output, @"(?<attr>[\w\.]{7})\s+(?<compressed>\d+)\s+(?<date>\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2})\s+(?<name>.+?)$", RegexOptions.Multiline)) {

                        if (match.Groups["attr"].Value.Contains("D"))
                            continue;

                        GenericArchiveEntry entry = new GenericArchiveEntry() {
                            Name = SanitizeEntryName(match.Groups["name"].Value.Trim()),
                        };

                        if (DateTimeOffset.TryParseExact(match.Groups["date"].Value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTimeOffset date))
                            entry.LastModified = date;

                        if (int.TryParse(match.Groups["compressed"].Value, out int compressed))
                            entry.CompressedSize = compressed;

                        // We have no means of getting the original file size.

                        entry.Size = entry.CompressedSize;

                        items.Add(entry);

                    }

                    if (!items.Any() && output.Contains("Incorrect password"))
                        throw new PasswordException();

                }

            }

            return items;

        }

        protected override void CommitChanges(IEnumerable<NewArchiveEntry> newEntries, IEnumerable<IArchiveEntry> deletedEntries) {

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            if (File.Exists(filePath) && FileUtilities.GetSize(filePath) <= 0)
                File.Delete(filePath);

            // Remove deleted entries from the archive.

            CommitDeletedEntries(processStartInfo, deletedEntries);

            // Add new entries to the archive.

            CommitNewEntries(processStartInfo, newEntries);

            // Update archive comment.

            CommitComment(processStartInfo);

        }

        // Private members

        private readonly string filePath;
        private readonly string winrarDirectoryPath;
        private readonly IFileFormat archiveFormat;
        private readonly IArchiveOptions options;
        private readonly Lazy<string> existingComment;
        private CompressionLevel compressionLevel = CompressionLevel.Maximum;
        private string newComment;

        private static string ConvertToEp3Path(string filePath) {

            return Regex.Replace(filePath, @"^\/+|:", m => "".PadLeft(m.Length, '_'));

        }

        private string ReadArchiveComment() {

            if (!File.Exists(filePath) || FileUtilities.GetSize(filePath) <= 0)
                return string.Empty;

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            processStartInfo.Arguments = new CmdArgumentsBuilder()
                .WithArgument("cw")
                .WithArgument(filePath)
                .WithArgument(GetPasswordArgument(includeEmptyPassword: true))
                .WithArgument(GetEncodingArgument())
                .ToString();

            using (Stream processStream = new ProcessStream(processStartInfo, new ProcessStreamOptions(redirectStandardOutput: true)))
            using (StreamReader streamReader = new StreamReader(processStream, options.Encoding)) {

                string output = streamReader.ReadToEnd().Trim();

                if (output.Contains("Program aborted"))
                    throw new PasswordException();

                // Skip the first two lines that are displayed before the comment content.

                output = string.Join(Environment.NewLine, output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(2));

                return output;

            }

        }

        private ProcessStartInfo CreateProcessStartInfo() {

            ProcessStartInfo processStartInfo = new ProcessStartInfo() {
                FileName = GetWinrarExecutablePath(),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            return processStartInfo;

        }
        private string GetWinrarExecutablePath() {

            string executablePath = WinRarUtilities.GetWinRarExecutablePath(winrarDirectoryPath);

            if (!File.Exists(executablePath))
                throw new FileNotFoundException(Properties.ExceptionMessages.WinrarExecutableNotFound, executablePath);

            return executablePath;

        }

        private void AddCompressionLevelArguments(ICommandLineArgumentsBuilder argumentsBuilder) {

            switch (compressionLevel) {

                case CompressionLevel.Store:

                    argumentsBuilder.WithArgument("-m0");

                    break;

                case CompressionLevel.Maximum:

                    argumentsBuilder.WithArgument("-m5");

                    break;

            }

        }
        private void AddTypeArgument(ICommandLineArgumentsBuilder argumentsBuilder) {

            if (archiveFormat.Equals(ArchiveFormat.Zip)) {

                argumentsBuilder.WithArgument("-tzip");

            }
            else if (archiveFormat.Equals(ArchiveFormat.SevenZip)) {

                argumentsBuilder.WithArgument("-t7z");

            }

        }
        private string GetEncodingArgument() {

            if (options.Encoding.Equals(Encoding.Unicode)) {

                return "-scu";

            }
            else if (options.Encoding.Equals(Encoding.ASCII)) {

                return "-sca";

            }
            else {

                // Default to UTF-8.

                return "-scf";

            }

        }
        private string GetPasswordArgument(bool includeEmptyPassword = false) {

            // A default password can optionally be included to prevent WinRAR from prompting for one.
            // This should only be enabled for non-constructive operations like reading files from the archive.

            if (string.IsNullOrEmpty(options.Password) && !includeEmptyPassword)
                return "";

            string switchStr = options.EncryptHeaders ? "hp" : "p";
            string password = string.IsNullOrEmpty(options.Password) ? "_" : options.Password;

            return $"-{switchStr}{password}";

        }

        private void CommitComment(ProcessStartInfo processStartInfo) {

            if (newComment is object) {

                // WinRAR will read the new comment from stdin, but I need to get that working properly with ProcessStream first.
                // For now, save the comment to a temporary file and add the comment from there.

                string tempFilePath = PathUtilities.GetTemporaryFilePath(new TemporaryPathOptions() { EnsureUnique = true, });

                try {

                    File.WriteAllText(tempFilePath, newComment, options.Encoding);

                    processStartInfo.Arguments = new CmdArgumentsBuilder()
                         .WithArgument("c")
                         .WithArgument(GetPasswordArgument(includeEmptyPassword: true))
                         .WithArgument(GetEncodingArgument())
                         .WithArgument($"-z{tempFilePath}")
                         .WithArgument(filePath)
                         .ToString();

                    using (Process process = Process.Start(processStartInfo))
                        process.WaitForExit();

                }
                finally {

                    File.Delete(tempFilePath);

                }

            }

        }
        private void CommitDeletedEntries(ProcessStartInfo processStartInfo, IEnumerable<IArchiveEntry> deletedEntries) {

            if (deletedEntries.Any()) {

                string tempFilePath = null;

                try {

                    // Save the names of the deleted entries to a text file.

                    tempFilePath = PathUtilities.GetTemporaryFilePath(new TemporaryPathOptions() { EnsureUnique = true, });

                    File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, deletedEntries.Select(entry => entry.Name)));

                    processStartInfo.Arguments = new CmdArgumentsBuilder()
                        .WithArgument("d")
                        .WithArgument(filePath)
                        .WithArgument(GetPasswordArgument())
                        .WithArgument(GetEncodingArgument()) // We must specify the encoding for the list file
                        .WithArgument($"@{tempFilePath}")
                        .ToString();

                    using (Process process = Process.Start(processStartInfo))
                        process.WaitForExit();

                }
                finally {

                    // Delete the temporary file that we created.

                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);

                }

            }

        }
        private void CommitNewEntries(ProcessStartInfo processStartInfo, IEnumerable<NewArchiveEntry> newEntries) {

            if (newEntries.Any()) {

                string tempFilePath = null;
                ICommandLineArgumentsBuilder argumentsBuilder;

                try {

                    tempFilePath = PathUtilities.GetTemporaryFilePath(new TemporaryPathOptions() { EnsureUnique = true, });

                    // Add the entries to the archive.
                    // WinRAR does not allow us to add a file to the archive and rename it in one action.

                    // Start by adding the files that don't need to be renamed.

                    IEnumerable<string> filesToAdd = newEntries.Where(entry => !entry.RenameRequired).Select(entry => entry.FilePath);

                    if (filesToAdd.Any()) {

                        File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, filesToAdd));

                        argumentsBuilder = new CmdArgumentsBuilder()
                            .WithArgument(File.Exists(filePath) ? "u" : "a");

                        AddTypeArgument(argumentsBuilder);
                        AddCompressionLevelArguments(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
                            .WithArgument("-ep") // Add files to root of archive
                            .WithArgument(GetPasswordArgument())
                            .WithArgument(GetEncodingArgument()) // We must specify the encoding for the list file
                            .WithArgument($"@{tempFilePath}");

                        processStartInfo.Arguments = argumentsBuilder.ToString();

                        using (Process process = Process.Start(processStartInfo))
                            process.WaitForExit();

                    }

                    // Next, add the files that do need to be renamed using their fully-qualified paths.
                    // After the files have been added, they will be renamed.

                    filesToAdd = newEntries.Where(entry => entry.RenameRequired).Select(entry => entry.FilePath);

                    if (filesToAdd.Any()) {

                        File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, filesToAdd));

                        argumentsBuilder = new CmdArgumentsBuilder()
                            .WithArgument(File.Exists(filePath) ? "u" : "a");

                        AddTypeArgument(argumentsBuilder);
                        AddCompressionLevelArguments(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
                            .WithArgument("-ep3") // use fully-qualified path
                            .WithArgument(GetPasswordArgument())
                            .WithArgument(GetEncodingArgument())
                            .WithArgument($"@{tempFilePath}");

                        processStartInfo.Arguments = argumentsBuilder.ToString();

                        using (Process process = Process.Start(processStartInfo))
                            process.WaitForExit();

                        // Finally, rename the files that need to be renamed.

                        File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, newEntries.Where(entry => entry.RenameRequired).Select(entry => ConvertToEp3Path(entry.FilePath) + Environment.NewLine + entry.Name)));

                        argumentsBuilder = new CmdArgumentsBuilder()
                           .WithArgument("rn");

                        AddTypeArgument(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
                            .WithArgument(GetPasswordArgument())
                            .WithArgument(GetEncodingArgument())
                            .WithArgument($"@{tempFilePath}");

                        processStartInfo.Arguments = argumentsBuilder.ToString();

                        using (Process process = Process.Start(processStartInfo))
                            process.WaitForExit();

                    }

                }
                finally {

                    // Delete the temporary file that we created.

                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);

                }

            }

        }

    }

}