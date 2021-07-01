using Gsemac.Core;
using Gsemac.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.IO.Compression.SevenZip {

    internal class BinSevenZipArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => options.FileAccess.HasFlag(FileAccess.Read);
        public override bool CanWrite => options.FileAccess.HasFlag(FileAccess.Write);
        public override string Comment {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportReadingComments);
            set => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportWritingComments);
        }
        public override CompressionLevel CompressionLevel {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportGettingCompressionLevel);
            set => compressionLevel = value;
        }

        public BinSevenZipArchive(Stream stream, string sevenZipDirectoryPath, IFileFormat archiveFormat, IArchiveOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                throw new ArgumentNullException(nameof(archiveFormat));

            if (string.IsNullOrWhiteSpace(sevenZipDirectoryPath))
                sevenZipDirectoryPath = SevenZipUtilities.SevenZipDirectoryPath;

            this.sevenZipDirectoryPath = sevenZipDirectoryPath;

            if (!File.Exists(GetSevenZipExecutablePath()))
                throw new FileNotFoundException(Properties.ExceptionMessages.SevenZipExecutableNotFound, sevenZipDirectoryPath);

            if (stream is FileStream fileStream) {

                // When interfacing with 7-Zip using the command line interface, we need to work file paths (I couldn't get the "-si" flag to work).
                // We'll extract the file information from the FileStream object to pass to 7-Zip.             

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
                this.compressionLevel = options.CompressionLevel;

                // We must close the file stream to ensure that 7-Zip can access the archive.

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

                NewArchiveEntry newEntry = new NewArchiveEntry() {
                    Name = SanitizeEntryName(entryName),
                    LastModified = DateTimeOffset.Now,
                    Comment = options.Comment,
                    FilePath = Path.GetFullPath(fileStream.Name),
                };

                if (existingEntry is object) {

                    if (!options.Overwrite) {

                        throw new ArchiveEntryExistsException();

                    }
                    else if (newEntry.RenameRequired) {

                        // While 7-Zip will happily overwrite old entries when adding new entries to the archive with the same name, it will
                        // not delete an older entry when renaming a new entry to the same name. Instead, it creates a duplicate.
                        // We need to make sure that we delete the old entry to avoid creating a duplicate.

                        DeleteEntry(existingEntry);

                    }

                }

                newEntries.Add(newEntry);

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

            if (!existingEntries.Value.Contains(entry))
                throw new ArchiveEntryDoesNotExistException();

            deletedEntries.Add(entry);

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

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            processStartInfo.Arguments = new CmdArgumentsBuilder()
                .WithArgument("e")
                .WithArgument(filePath)
                .WithArgument("-so")
                .WithArgument(SanitizeEntryName(entry.Name))
                .ToString();

            using (Stream processStream = new ProcessStream(processStartInfo, ProcessStreamOptions.RedirectStandardOutput))
                processStream.CopyTo(outputStream);

        }
        public override IEnumerable<IArchiveEntry> GetEntries() {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            // Include all existing and new entries minus deleted and overwritten entries.

            return existingEntries.Value
                .Concat(newEntries)
                .Except(deletedEntries)
                .Except(existingEntries.Value.Where(entry => newEntries.Any(newEntry => newEntry.Name.Equals(entry.Name))));

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
            public bool RenameRequired => !Name.Equals(PathUtilities.GetFilename(FilePath));

        }

        private readonly string filePath;
        private readonly string sevenZipDirectoryPath;
        private readonly IFileFormat archiveFormat;
        private readonly IArchiveOptions options;
        private readonly Lazy<List<IArchiveEntry>> existingEntries;
        private readonly List<NewArchiveEntry> newEntries = new List<NewArchiveEntry>();
        private readonly List<IArchiveEntry> deletedEntries = new List<IArchiveEntry>();
        private CompressionLevel compressionLevel = CompressionLevel.Maximum;
        private bool archiveIsClosed = false;

        private List<IArchiveEntry> ReadArchiveEntries() {

            List<IArchiveEntry> items = new List<IArchiveEntry>();

            if (File.Exists(filePath) && FileUtilities.GetFileSize(filePath) > 0) {

                ProcessStartInfo processStartInfo = CreateProcessStartInfo();

                processStartInfo.Arguments = new CmdArgumentsBuilder()
                    .WithArgument("l")
                    .WithArgument(filePath)
                    .ToString();

                using (Stream processStream = new ProcessStream(processStartInfo, ProcessStreamOptions.RedirectStandardOutput))
                using (StreamReader streamReader = new StreamReader(processStream)) {

                    string output = streamReader.ReadToEnd();

                    foreach (Match match in Regex.Matches(output, @"(?<date>\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2})\s+(?<attr>[\w\.]{5})\s+(?<size>\d+)\s+(?<compressed>\d*)\s+(?<name>.+?)$", RegexOptions.Multiline)) {

                        if (match.Groups["attr"].Value.Contains("D"))
                            continue;

                        GenericArchiveEntry entry = new GenericArchiveEntry() {
                            Name = SanitizeEntryName(match.Groups["name"].Value.Trim()),
                        };

                        if (DateTimeOffset.TryParseExact(match.Groups["date"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTimeOffset date))
                            entry.LastModified = date;

                        if (int.TryParse(match.Groups["size"].Value, out int size))
                            entry.Size = size;

                        if (int.TryParse(match.Groups["compressed"].Value, out int compressed))
                            entry.CompressedSize = compressed;

                        items.Add(entry);

                    }

                }

            }

            return items;

        }
        private ProcessStartInfo CreateProcessStartInfo() {

            ProcessStartInfo processStartInfo = new ProcessStartInfo() {
                FileName = GetSevenZipExecutablePath(),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            return processStartInfo;

        }
        private string GetSevenZipExecutablePath() {

            string executablePath = Path.Combine(sevenZipDirectoryPath, "7z.exe");

            if (!File.Exists(executablePath))
                throw new FileNotFoundException(Properties.ExceptionMessages.SevenZipExecutableNotFound, executablePath);

            return executablePath;

        }

        private void AddTypeArgument(ICmdArgumentsBuilder argumentsBuilder) {

            if (archiveFormat.Equals(ArchiveFormat.Zip)) {

                argumentsBuilder.WithArgument("-tzip");

            }
            else if (archiveFormat.Equals(ArchiveFormat.SevenZip)) {

                argumentsBuilder.WithArgument("-t7z");

            }

        }
        private void AddCompressionLevelArguments(ICmdArgumentsBuilder argumentsBuilder) {

            switch (compressionLevel) {

                case CompressionLevel.Store:

                    argumentsBuilder.WithArgument("-mx0");

                    if (archiveFormat.Equals(ArchiveFormat.Zip)) {

                        argumentsBuilder.WithArgument("-mmCopy");

                    }

                    break;

                case CompressionLevel.Maximum:

                    // https://superuser.com/a/742034 (kenorb)

                    argumentsBuilder.WithArgument("-mx9");

                    if (archiveFormat.Equals(ArchiveFormat.Zip)) {

                        argumentsBuilder.WithArgument("-mm=Deflate")
                            .WithArgument("-mfb=258")
                            .WithArgument("-mpass=15");

                    }
                    else if (archiveFormat.Equals(ArchiveFormat.SevenZip)) {

                        argumentsBuilder.WithArgument("-m0=lzma")
                            .WithArgument("-mfb=64")
                            .WithArgument("-md=32m");

                    }

                    break;

            }

        }

        private void CommitDeletedEntries(ProcessStartInfo processStartInfo) {

            if (deletedEntries.Any()) {

                string tempFilePath = null;

                try {

                    // Save the names of the deleted entries to a text file.

                    tempFilePath = PathUtilities.GetUniqueTemporaryFilePath();

                    File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, deletedEntries.Select(entry => entry.Name)));

                    processStartInfo.Arguments = new CmdArgumentsBuilder()
                        .WithArgument("d")
                        .WithArgument(filePath)
                        .WithArguments($"@{tempFilePath}")
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
        private void CommitNewEntries(ProcessStartInfo processStartInfo) {

            if (newEntries.Any()) {

                string tempFilePath = null;
                ICmdArgumentsBuilder argumentsBuilder;

                try {

                    // Save the names of the new entries to a text file.

                    tempFilePath = PathUtilities.GetUniqueTemporaryFilePath();

                    // Add the entries to the archive.
                    // 7-Zip does not allow us to add a file to the archive and rename it in one action.

                    // Start by adding the files that don't need to be renamed.

                    IEnumerable<string> filesToAdd = newEntries.Where(entry => !entry.RenameRequired).Select(entry => entry.FilePath);

                    if (filesToAdd.Any()) {

                        File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, filesToAdd));

                        argumentsBuilder = new CmdArgumentsBuilder()
                            .WithArgument(File.Exists(filePath) ? "u" : "a");

                        AddTypeArgument(argumentsBuilder);
                        AddCompressionLevelArguments(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
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
                            .WithArgument("-spf") // use fully-qualified path
                            .WithArgument($"@{tempFilePath}");

                        processStartInfo.Arguments = argumentsBuilder.ToString();

                        using (Process process = Process.Start(processStartInfo))
                            process.WaitForExit();

                        // Finally, rename the files that need to be renamed.
                        // The listfile format has the old name followed by the new name on the following line for each file to be renamed.
                        // https://sourceforge.net/p/sevenzip/discussion/45798/thread/3a1961c0/#453b

                        File.WriteAllText(tempFilePath, string.Join(Environment.NewLine, newEntries.Where(entry => entry.RenameRequired).Select(entry => entry.FilePath + Environment.NewLine + entry.Name)));

                        argumentsBuilder = new CmdArgumentsBuilder()
                           .WithArgument("rn");

                        AddTypeArgument(argumentsBuilder);

                        argumentsBuilder.WithArgument(filePath)
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
        private void CommitChanges() {

            ProcessStartInfo processStartInfo = CreateProcessStartInfo();

            if (File.Exists(filePath) && FileUtilities.GetFileSize(filePath) <= 0)
                File.Delete(filePath);

            // Remove deleted entries from the archive.

            CommitDeletedEntries(processStartInfo);

            // Add new entries to the archive.

            CommitNewEntries(processStartInfo);

        }

    }

}