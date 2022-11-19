using Gsemac.Net.GitHub.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.GitHub.Tests.IntegrationTests {

    [TestClass, TestCategory("Integration")]
    public class GitHubWebClientIntegrationTests {

        // GetRepository

        [TestMethod]
        public void TestGetRepositoryReturnsCorrectMetadata() {

            IRepository repositoryInfo = new GitHubWebClient()
                .GetRepository(GitHubUrls.GitHubRepositoryUrl);

            Assert.AreEqual("master", repositoryInfo.DefaultBranchName);
            Assert.AreEqual(GitHubUrls.GitHubRepositoryUrl, repositoryInfo.Url);
            Assert.AreEqual("Gsemac.Common", repositoryInfo.Name);
            Assert.AreEqual("gsemac", repositoryInfo.Owner);

            Assert.AreEqual($"{GitHubUrls.GitHubRepositoryUrl}/releases", repositoryInfo.ReleasesUrl);
            Assert.AreEqual($"{GitHubUrls.GitHubRepositoryUrl}/archive/{repositoryInfo.DefaultBranchName}.zip", repositoryInfo.ArchiveUrl);

        }

        // GetReleases

        [TestMethod]
        public void TestGetReleasesContainsReleaseWithCorrectMetadata() {

            // We'll look for the presence of a specific release, and verify its metadata.

            const string releaseTag = "v2.2.10";

            IRelease release = new GitHubWebClient()
                .GetReleases(GitHubUrls.GitHubRepositoryReleasesUrl)
                .Where(release => release.Tag.Equals(releaseTag, StringComparison.OrdinalIgnoreCase))
                .First();

            Assert.AreEqual($"{GitHubUrls.GitHubRepositoryReleasesUrl}/tag/{releaseTag}", release.Url);
            Assert.AreEqual(releaseTag, release.Tag);
            Assert.AreEqual("v2.2.10", release.Title);
            Assert.AreEqual(DateTimeOffset.Parse("2022-10-22T13:41:37Z"), release.Published);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(release.Description));

        }
        [TestMethod]
        public void TestReleaseHasReleaseAssets() {

            const string releaseTag = "v2.2.10";

            IRelease release = new GitHubWebClient()
                .GetReleases(GitHubUrls.GitHubRepositoryReleasesUrl)
                .Where(release => release.Tag.Equals(releaseTag, StringComparison.OrdinalIgnoreCase))
                .First();

            Assert.IsTrue(release.Assets.Any());

            Assert.IsTrue(release.Assets.Where(asset => asset.Name.Equals("flaresolverr-v2.2.10-linux-x64.zip", StringComparison.OrdinalIgnoreCase)).Any());
            Assert.IsTrue(release.Assets.Where(asset => asset.Name.Equals("flaresolverr-v2.2.10-windows-x64.zip", StringComparison.OrdinalIgnoreCase)).Any());
            Assert.IsTrue(release.Assets.Where(asset => asset.Name.Equals("Source code (zip)", StringComparison.OrdinalIgnoreCase)).Any());
            Assert.IsTrue(release.Assets.Where(asset => asset.Name.Equals("Source code (tar.gz)", StringComparison.OrdinalIgnoreCase)).Any());

        }

        // GetFiles

        [TestMethod]
        public void TestGetFilesReturnsCorrectMetadata() {

            IEnumerable<IFileNode> files = new GitHubWebClient()
                .GetFiles(GitHubUrls.GitHubDirectoryUrl, SearchOption.TopDirectoryOnly);

            Assert.IsTrue(files.Any());

            // Test for the presence of a couple directories and files.

            Assert.IsTrue(files.Where(file => file.IsDirectory && file.Name.Equals("Extensions", StringComparison.OrdinalIgnoreCase)).Any());
            Assert.IsTrue(files.Where(file => file.IsDirectory && file.Name.Equals("Properties", StringComparison.OrdinalIgnoreCase)).Any());

            Assert.IsTrue(files.Where(file => !file.IsDirectory && file.Name.Equals("FileNode.cs", StringComparison.OrdinalIgnoreCase)).Any());
            Assert.IsTrue(files.Where(file => !file.IsDirectory && file.Name.Equals("Repository.cs", StringComparison.OrdinalIgnoreCase)).Any());

            // Make sure that we were able to read the commit message.

            Assert.IsTrue(files.Any(file => !string.IsNullOrWhiteSpace(file.CommitMessage)));

            // Make sure that we were able to read the last-modified date.

            Assert.IsTrue(files.Any(file => !file.LastModified.Equals(default)));

        }

        [ClassInitialize]
        public static void ClassInit(TestContext _) {

            // We need to enable TLS 1.2 support in order to access GitHub.

            ServicePointManagerUtilities.SetSecurityProtocolEnabled(ServicePointManagerUtilities.Net45SecurityProtocols, enabled: true);

        }

    }

}