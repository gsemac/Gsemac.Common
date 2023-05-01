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
        public void TestFilesReadMatchFilesInArchive(Type archiveFactoryType, string archiveFilePath) {

            IArchiveFactory archiveFactory = archiveFactoryType is null ?
                ArchiveFactory.Default :
                (IArchiveFactory)Activator.CreateInstance(archiveFactoryType);

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
        public void TestAddingFilesToNewArchiveProducesAValidArchive(Type archiveFactoryType, string archiveFilePath) {

            // Archives will be created in the temporary directory and deleted afterwards.

            string tempDirectoryPath = PathUtilities.GetTemporaryDirectoryPath();

            Assert.IsTrue(Directory.Exists(tempDirectoryPath));
            Assert.IsTrue(DirectoryUtilities.IsDirectoryEmpty(tempDirectoryPath));

            try {

                IArchiveFactory archiveFactory = archiveFactoryType is null ?
                    ArchiveFactory.Default :
                    (IArchiveFactory)Activator.CreateInstance(archiveFactoryType);

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

    }

}