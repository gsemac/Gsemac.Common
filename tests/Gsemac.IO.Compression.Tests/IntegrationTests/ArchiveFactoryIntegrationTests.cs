using Gsemac.IO.Compression.SevenZip;
using Gsemac.IO.Compression.SystemIOCompression;
using Gsemac.IO.Compression.Tests.Properties;
using Gsemac.IO.Compression.Winrar;
using Gsemac.IO.Compression.ZipStorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.Tests.IntegrationTests {

    [TestClass, TestCategory("Integration")]
    public class ArchiveFactoryIntegrationTests {

        // Public members

        // Open

        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestReadingEntriesFromArchive(Type archiveFactoryType, string archiveFilePath) {

            IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

            using (Stream stream = File.OpenRead(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath)))
            using (IArchive archive = archiveFactory.Open(stream)) {

                Assert.AreEqual(2, archive.GetEntries().Count());

                Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                Assert.IsTrue(archive.ContainsEntry("file2.txt"));

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestReadingEntriesFromArchivePreservesArchiveIntegrity(Type archiveFactoryType, string archiveFilePath) {

            // This test is present due to a bug in ZipStorer where opening and closing an archive would corrupt it.
            // I fixed the bug myself, but we should make sure it doesn't occur again.

            IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

            // We'll open the archive with read/write privileges so that it has the opportunity to become corrupt.

            using (Stream stream = File.Open(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath), FileMode.Open, FileAccess.ReadWrite))
            using (IArchive archive = archiveFactory.Open(stream))
                archive.Close();

            // Ensure that we can still read the files back out of the archive.

            using (Stream stream = File.Open(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath), FileMode.Open, FileAccess.ReadWrite))
            using (IArchive archive = archiveFactory.Open(stream)) {

                Assert.AreEqual(2, archive.GetEntries().Count());

                Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                Assert.IsTrue(archive.ContainsEntry("file2.txt"));

            }


        }

        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestAddingEntriesToNewArchive(Type archiveFactoryType, string archiveFilePath) {

            // Archives will be created in the temporary directory and deleted afterwards.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "file1.txt"), "hello world");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "file2.txt"), "hello world again");

                string fullArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

                using (IArchive archive = archiveFactory.Open(fullArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "file1.txt"));
                    archive.AddFile(Path.Combine(tempDirectoryPath, "file2.txt"));

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                }

                Assert.IsTrue(File.Exists(fullArchiveFilePath));

                // Reopen the archive and verify that files are inside.

                using (IArchive archive = archiveFactory.OpenRead(fullArchiveFilePath)) {

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestAddingEntriesWithSubDirectoriesToNewArchive(Type archiveFactoryType, string archiveFilePath) {

            // Archives will be created in the temporary directory and deleted afterwards.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "file1.txt"), "hello world");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "file2.txt"), "hello world again");

                string fullArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

                using (IArchive archive = archiveFactory.Open(fullArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "file1.txt"), "path/to/file/file1.txt");
                    archive.AddFile(Path.Combine(tempDirectoryPath, "file2.txt"), "/path/to/other/file/file2.txt");

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("path/to/file/file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("/path/to/other/file/file2.txt"));

                    // Verify that the entries can be accessed with or without the leading directory separator.

                    Assert.IsTrue(archive.ContainsEntry("/path/to/file/file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("path/to/other/file/file2.txt"));

                }

                Assert.IsTrue(File.Exists(fullArchiveFilePath));

                // Reopen the archive and verify that files are inside.

                using (IArchive archive = archiveFactory.OpenRead(fullArchiveFilePath)) {

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("path/to/file/file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("/path/to/other/file/file2.txt"));

                    // Verify that the entries can be accessed with or without the leading directory separator.

                    Assert.IsTrue(archive.ContainsEntry("/path/to/file/file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("path/to/other/file/file2.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestAddingEntriesWithUtf8CharactersToNewArchive(Type archiveFactoryType, string archiveFilePath) {

            // Archives will be created in the temporary directory and deleted afterwards.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "ファイル1.txt"), "hello world");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "ファイル2.txt"), "hello world again");

                string fullArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

                using (IArchive archive = archiveFactory.Open(fullArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "ファイル1.txt"));
                    archive.AddFile(Path.Combine(tempDirectoryPath, "ファイル2.txt"));

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("ファイル1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("ファイル2.txt"));

                }

                Assert.IsTrue(File.Exists(fullArchiveFilePath));

                // Reopen the archive and verify that files are inside.

                using (IArchive archive = archiveFactory.OpenRead(fullArchiveFilePath)) {

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("ファイル1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("ファイル2.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestAddingEntriesToExistingArchive(Type archiveFactoryType, string archiveFilePath) {

            // Copy the sample archive to a temporary directory.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();
            string tempArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                File.Copy(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath), tempArchiveFilePath);

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "file3.txt"), "hello world #3");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "file4.txt"), "hello world #4");

                using (IArchive archive = archiveFactory.Open(tempArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "file3.txt"));
                    archive.AddFile(Path.Combine(tempDirectoryPath, "file4.txt"));

                    Assert.AreEqual(4, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file3.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file4.txt"));

                }

                // Reopen the archive and verify that files are inside.

                using (IArchive archive = archiveFactory.OpenRead(tempArchiveFilePath)) {

                    Assert.AreEqual(4, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file3.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file4.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestOverwritingEntriesInExistingArchive(Type archiveFactoryType, string archiveFilePath) {

            // Copy the sample archive to a temporary directory.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();
            string tempArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                File.Copy(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath), tempArchiveFilePath);

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "file1.txt"), "hello world #1");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "file2.txt"), "hello world #2");

                using (IArchive archive = archiveFactory.Open(tempArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "file1.txt"), overwrite: true);
                    archive.AddFile(Path.Combine(tempDirectoryPath, "file2.txt"), overwrite: true);

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                }

                // Extract the files and verify that they are the same as the ones we added.

                using (IArchive archive = archiveFactory.OpenRead(tempArchiveFilePath)) {

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                    using (FileStream fileStream = File.OpenWrite(Path.Combine(tempDirectoryPath, "file1.extracted.txt")))
                        archive.ExtractEntry("file1.txt", fileStream);

                    using (FileStream fileStream = File.OpenWrite(Path.Combine(tempDirectoryPath, "file2.extracted.txt")))
                        archive.ExtractEntry("file2.txt", fileStream);

                }

                Assert.AreEqual(FileUtilities.ComputeMD5Hash(Path.Combine(tempDirectoryPath, "file1.txt")), FileUtilities.ComputeMD5Hash(Path.Combine(tempDirectoryPath, "file1.extracted.txt")));
                Assert.AreEqual(FileUtilities.ComputeMD5Hash(Path.Combine(tempDirectoryPath, "file2.txt")), FileUtilities.ComputeMD5Hash(Path.Combine(tempDirectoryPath, "file2.extracted.txt")));

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestDeletingEntriesFromNewArchive(Type archiveFactoryType, string archiveFilePath) {

            // Archives will be created in the temporary directory and deleted afterwards.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Add files to the archive.

                File.WriteAllText(Path.Combine(tempDirectoryPath, "file1.txt"), "hello world");
                File.WriteAllText(Path.Combine(tempDirectoryPath, "file2.txt"), "hello world again");

                string fullArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

                using (IArchive archive = archiveFactory.Open(fullArchiveFilePath)) {

                    archive.AddFile(Path.Combine(tempDirectoryPath, "file1.txt"));
                    archive.AddFile(Path.Combine(tempDirectoryPath, "file2.txt"));

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                    // Remove files from the archive before it is committed to disk.

                    archive.DeleteEntry("file2.txt");

                    Assert.AreEqual(1, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsFalse(archive.ContainsEntry("file2.txt"));

                }

                Assert.IsTrue(File.Exists(fullArchiveFilePath));

                // Reopen the archive and verify that the expected files are inside.

                using (IArchive archive = archiveFactory.OpenRead(fullArchiveFilePath)) {

                    Assert.AreEqual(1, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsFalse(archive.ContainsEntry("file2.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }
        [TestMethod]
        [DataRow(null, "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        public void TestDeletingEntriesFromExistingArchive(Type archiveFactoryType, string archiveFilePath) {

            // Copy the sample archive to a temporary directory.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();
            string tempArchiveFilePath = Path.Combine(tempDirectoryPath, archiveFilePath);

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                File.Copy(Path.Combine(SamplePaths.ArchivesSamplesDirectoryPath, archiveFilePath), tempArchiveFilePath);

                IArchiveFactory archiveFactory = GetArchiveFactory(archiveFactoryType);

                // Delete entries from the archive.

                using (IArchive archive = archiveFactory.Open(tempArchiveFilePath)) {

                    Assert.AreEqual(2, archive.GetEntries().Count());

                    Assert.IsTrue(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                    archive.DeleteEntry("file1.txt");

                    Assert.AreEqual(1, archive.GetEntries().Count());

                    Assert.IsFalse(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                }

                // Reopen the archive and verify that the removed entries are gone.

                using (IArchive archive = archiveFactory.OpenRead(tempArchiveFilePath)) {

                    Assert.AreEqual(1, archive.GetEntries().Count());

                    Assert.IsFalse(archive.ContainsEntry("file1.txt"));
                    Assert.IsTrue(archive.ContainsEntry("file2.txt"));

                }

            }
            finally {

                if (Directory.Exists(tempDirectoryPath))
                    Directory.Delete(tempDirectoryPath, recursive: true);

            }

        }

        // Private members

        private static IArchiveFactory GetArchiveFactory(Type archiveFactoryType) {

            return archiveFactoryType is null ?
                ArchiveFactory.Default :
                (IArchiveFactory)Activator.CreateInstance(archiveFactoryType);

        }

    }

}