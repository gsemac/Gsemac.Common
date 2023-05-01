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
        [DataRow(typeof(ZipStorerArchiveFactory), "archive.zip")]
        [DataRow(typeof(SystemIOCompressionArchiveFactory), "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.zip")]
        [DataRow(typeof(SevenZipExeArchiveFactory), "archive.7z")]
        [DataRow(typeof(WinrarExeArchiveFactory), "archive.rar")]
        public void TestReadFilesFromArchive(Type archiveFactoryType, string archiveFilePath) {

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

    }

}