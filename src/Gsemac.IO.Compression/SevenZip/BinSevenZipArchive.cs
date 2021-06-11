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
        public override CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public BinSevenZipArchive(Stream stream, string sevenZipExecutablePath, IFileFormat archiveFormat, IArchiveOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                throw new ArgumentNullException(nameof(archiveFormat));

            if (string.IsNullOrWhiteSpace(sevenZipExecutablePath))
                sevenZipExecutablePath = SevenZipUtilities.SevenZipExecutablePath;

            if (!File.Exists(sevenZipExecutablePath))
                throw new ArgumentException("'7z.exe' could not be located at the given file path.");

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

                filePath = Path.GetFullPath(fileStream.Name);
                this.sevenZipExecutablePath = sevenZipExecutablePath;
                this.archiveFormat = archiveFormat;
                this.options = options;
                this.existingEntries = new Lazy<List<IArchiveEntry>>(ReadArchiveEntries);
                CompressionLevel = options.CompressionLevel;

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

                if (existingEntry is object && !options.Overwrite)
                    throw new ArchiveEntryExistsException();

                newEntries.Add(new NewEntry() {
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

        private class NewEntry :
            BinSevenZipArchiveEntry {

            public string FilePath { get; set; }

        }

        private readonly string filePath;
        private readonly string sevenZipExecutablePath;
        private readonly IFileFormat archiveFormat;
        private readonly IArchiveOptions options;
        private readonly Lazy<List<IArchiveEntry>> existingEntries;
        private readonly List<NewEntry> newEntries = new List<NewEntry>();
        private readonly List<IArchiveEntry> deletedEntries = new List<IArchiveEntry>();
        private bool archiveIsClosed = false;

        private List<IArchiveEntry> ReadArchiveEntries() {

            List<IArchiveEntry> items = new List<IArchiveEntry>();

            if (File.Exists(filePath)) {

                string arguments = new CmdArgumentsBuilder()
                    .WithArgument("l")
                    .WithArgument(filePath)
                    .ToString();

                using (Stream processStream = new ProcessStream(sevenZipExecutablePath, arguments, ProcessStreamOptions.RedirectStandardOutput))
                using (StreamReader streamReader = new StreamReader(processStream)) {

                    string output = streamReader.ReadToEnd();

                    foreach (Match match in Regex.Matches(output, @"(?<date>\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2})\s+(?<attr>[\w\.]{5})\s+(?<size>\d+)\s+(?<compressed>\d*)\s+(?<name>.+?)$", RegexOptions.Multiline)) {

                        if (match.Groups["attr"].Value.Contains("D"))
                            continue;

                        BinSevenZipArchiveEntry entry = new BinSevenZipArchiveEntry() {
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
                FileName = sevenZipExecutablePath,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            return processStartInfo;

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

                foreach (NewEntry entry in newEntries) {

                    // Add the entry to the archive.

                    ICmdArgumentsBuilder argumentsBuilder = new CmdArgumentsBuilder()
                        .WithArgument(File.Exists(filePath) ? "u" : "a");

                    AddTypeArgument(argumentsBuilder);
                    AddCompressionLevelArguments(argumentsBuilder);

                    argumentsBuilder.WithArgument(filePath)
                        .WithArgument(entry.FilePath);

                    processStartInfo.Arguments = argumentsBuilder.ToString();

                    using (Process process = Process.Start(processStartInfo))
                        process.WaitForExit();

                    // 7Zip will add each entry with only its original filename.
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

            switch (CompressionLevel) {

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

    }

}