using Gsemac.Core;
using Gsemac.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.IO.Compression.Winrar {

    internal class BinWinrarArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => options.FileAccess.HasFlag(FileAccess.Read);
        public override bool CanWrite => options.FileAccess.HasFlag(FileAccess.Write);
        public override string Comment {
            get => newComment is null ? existingComment.Value : newComment;
            set => newComment = value;
        }
        public override CompressionLevel CompressionLevel {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportGettingCompressionLevel);
            set => compressionLevel = value;
        }

        public BinWinrarArchive(Stream stream, string winrarDirectoryPath, IFileFormat archiveFormat, IArchiveOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                throw new ArgumentNullException(nameof(archiveFormat));

            if (string.IsNullOrWhiteSpace(winrarDirectoryPath))
                winrarDirectoryPath = WinrarUtilities.WinrarDirectoryPath;

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
                this.existingEntries = new Lazy<List<IArchiveEntry>>(ReadArchiveEntries);
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

        public override IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (stream is FileStream fileStream) {

                if (options is null)
                    options = ArchiveEntryOptions.Default;

                IArchiveEntry existingEntry = GetEntry(entryName);

                if (existingEntry is object && !options.Overwrite)
                    throw new ArchiveEntryExistsException();

                newEntries.Add(new NewArchiveEntry() {
                    Name = SanitizeEntryName(entryName),
                    LastModified = DateTimeOffset.Now,
                    Comment = options.Comment,
                    FilePath = Path.GetFullPath(fileStream.Name),
                });

                if (!options.LeaveStreamOpen)
                    stream.Close();

                return newEntries.Last();

            }
            else {

                throw new ArgumentException(Properties.ExceptionMessages.ArchiveOnlySupportsFileStreams);

            }

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            bool entryRemoved = false;

            // If this is an entry that we added previously, remove it.

            if (entry is NewArchiveEntry newArchiveEntry)
                entryRemoved = newEntries.Remove(newArchiveEntry);

            // If this is an existing entry, remove it.

            if (existingEntries.Value.Contains(entry)) {

                deletedEntries.Add(entry);

                entryRemoved = true;

            }

            if (!entryRemoved)
                throw new ArchiveEntryDoesNotExistException();

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (!existingEntries.Value.Contains(entry))
                throw new ArchiveEntryDoesNotExistException();

            // Extract the given entry.

            ProcessStartInfo processStartInfo = CreateProcessStartInfo(unrar: true);

            processStartInfo.Arguments = new CmdArgumentsBuilder()
                .WithArgument("p")
                .WithArgument("-inul") // disable header and progress bar
                .WithArgument(filePath)
                .WithArgument(SanitizeEntryName(entry.Name))
                .ToString();

            using (Stream processStream = new ProcessStream(processStartInfo, ProcessStreamOptions.RedirectStandardOutput))
                processStream.CopyTo(outputStream);

        }
        public override IEnumerable<IArchiveEntry> GetEntries() {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            return existingEntries.Value
                .Concat(newEntries)
                .Except(deletedEntries);

        }
        public override void Close() {

            if (!archiveIsClosed)
                CommitChanges();

            archiveIsClosed = true;

        }

        // Private members

        private class NewArchiveEntry :
            GenericArchiveEntry {

            public string FilePath { get; set; }

        }

        private readonly string filePath;
        private readonly string winrarDirectoryPath;
        private readonly IFileFormat archiveFormat;
        private readonly IArchiveOptions options;
        private readonly Lazy<List<IArchiveEntry>> existingEntries;
        private readonly Lazy<string> existingComment;
        private readonly List<NewArchiveEntry> newEntries = new List<NewArchiveEntry>();
        private readonly List<IArchiveEntry> deletedEntries = new List<IArchiveEntry>();
        private CompressionLevel compressionLevel = CompressionLevel.Maximum;
        private string newComment;
        private bool archiveIsClosed = false;

        private string ReadArchiveComment() {

            if (!File.Exists(filePath) || FileUtilities.GetFileSize(filePath) <= 0)
                return string.Empty;

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            processStartInfo.Arguments = new CmdArgumentsBuilder()
                .WithArgument("cw")
                .WithArgument(filePath)
                .WithArgument(GetEncodingArgument())
                .ToString();

            using (Stream processStream = new ProcessStream(processStartInfo, ProcessStreamOptions.RedirectStandardOutput))
            using (StreamReader streamReader = new StreamReader(processStream, options.Encoding)) {

                string output = streamReader.ReadToEnd().Trim();

                // Skip the first two lines that are displayed before the comment content.

                output = string.Join(Environment.NewLine, output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(2));

                return output;

            }

        }
        private List<IArchiveEntry> ReadArchiveEntries() {

            List<IArchiveEntry> items = new List<IArchiveEntry>();

            if (File.Exists(filePath) && FileUtilities.GetFileSize(filePath) > 0) {

                ProcessStartInfo processStartInfo = CreateProcessStartInfo(unrar: true);

                processStartInfo.Arguments = new CmdArgumentsBuilder()
                    .WithArgument("l")
                    .WithArgument(filePath)
                    .WithArgument("-scf") // output filenames in UTF-8 instead of Windows' default charset
                    .ToString();

                using (Stream processStream = new ProcessStream(processStartInfo, ProcessStreamOptions.RedirectStandardOutput))
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

                }

            }

            return items;

        }

        private ProcessStartInfo CreateProcessStartInfo(bool unrar = false) {

            ProcessStartInfo processStartInfo = new ProcessStartInfo() {
                FileName = unrar ? GetUnrarExecutablePath() : GetWinrarExecutablePath(),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            return processStartInfo;

        }
        private string GetWinrarExecutablePath() {

            string executablePath = Path.Combine(winrarDirectoryPath, WinrarUtilities.WinrarExecutableFilename);

            if (!File.Exists(executablePath))
                throw new FileNotFoundException(Properties.ExceptionMessages.WinrarExecutableNotFound, executablePath);

            return executablePath;

        }
        private string GetUnrarExecutablePath() {

            string executablePath = Path.Combine(winrarDirectoryPath, "UnRAR.exe");

            if (!File.Exists(executablePath))
                throw new FileNotFoundException(Properties.ExceptionMessages.UnrarExecutableNotFound, executablePath);

            return executablePath;

        }

        private void AddCompressionLevelArguments(ICmdArgumentsBuilder argumentsBuilder) {

            switch (compressionLevel) {

                case CompressionLevel.Store:

                    argumentsBuilder.WithArgument("-m0");

                    break;

                case CompressionLevel.Maximum:

                    argumentsBuilder.WithArgument("-m5");

                    break;

            }

        }
        private void AddTypeArgument(ICmdArgumentsBuilder argumentsBuilder) {

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

        private void CommitComment(ProcessStartInfo processStartInfo) {

            if (newComment is object) {

                // WinRAR will read the new comment from stdin, but I need to get that working properly with ProcessStream first.
                // For now, save the comment to a temporary file and add the comment from there.

                string tempFilePath = PathUtilities.GetUniqueTemporaryFilePath();

                try {

                    File.WriteAllText(tempFilePath, newComment, options.Encoding);

                    processStartInfo.Arguments = new CmdArgumentsBuilder()
                         .WithArgument("c")
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
        private void CommitChanges() {

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            if (File.Exists(filePath) && FileUtilities.GetFileSize(filePath) <= 0)
                File.Delete(filePath);

            // Remove deleted entries from the archive.

            if (deletedEntries.Any()) {

                processStartInfo.Arguments = new CmdArgumentsBuilder()
                    .WithArgument("d")
                    .WithArgument(filePath)
                    .WithArguments(deletedEntries.Select(entry => entry.Name).ToArray())
                    .ToString();

                using (Process process = Process.Start(processStartInfo))
                    process.WaitForExit();

            }

            // Add new entries to the archive.

            if (newEntries.Any()) {

                foreach (NewArchiveEntry entry in newEntries) {

                    // Add the entry to the archive.

                    ICmdArgumentsBuilder argumentsBuilder = new CmdArgumentsBuilder()
                        .WithArgument(File.Exists(filePath) ? "u" : "a")
                        .WithArgument("-ep");

                    AddTypeArgument(argumentsBuilder);
                    AddCompressionLevelArguments(argumentsBuilder);

                    argumentsBuilder.WithArgument(filePath)
                        .WithArgument(entry.FilePath);

                    processStartInfo.Arguments = argumentsBuilder.ToString();

                    using (Process process = Process.Start(processStartInfo))
                        process.WaitForExit();

                    // WinRAR will add each entry with only its original filename.
                    // Rename the entry if needed.

                    if (!PathUtilities.GetFilename(entry.FilePath).Equals(entry.Name)) {

                        argumentsBuilder.Clear();

                        argumentsBuilder.WithArgument("rn");

                        AddTypeArgument(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
                            .WithArgument(PathUtilities.GetFilename(entry.FilePath))
                            .WithArgument(entry.Name);

                        processStartInfo.Arguments = argumentsBuilder.ToString();

                        using (Process process = Process.Start(processStartInfo))
                            process.WaitForExit();

                    }

                }

            }

            // Update archive comment.

            CommitComment(processStartInfo);

        }

    }

}