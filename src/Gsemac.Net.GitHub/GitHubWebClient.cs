using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using Gsemac.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.GitHub {

    public class GitHubWebClient :
        IGitHubClient {

        // Public members

        public GitHubWebClient() :
            this(new HttpWebRequestFactory()) {
        }
        public GitHubWebClient(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        public IRepository GetRepository(string url) {

            using (IWebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(webClient.DownloadString(new Repository(url).Url));

                return ParseRepository(url, htmlDocument.DocumentNode);

            }

        }
        public IEnumerable<IRelease> GetReleases(string url) {

            string releasesUrl = new Repository(url).ReleasesUrl;
            string nextPageUrl = releasesUrl;

            using (IWebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                while (!string.IsNullOrWhiteSpace(nextPageUrl)) {

                    nextPageUrl = Url.Combine(releasesUrl, nextPageUrl);

                    htmlDocument.LoadHtml(webClient.DownloadString(nextPageUrl));

                    IEnumerable<IRelease> nextReleases = ParseReleases(htmlDocument.DocumentNode);

                    if (!nextReleases.Any())
                        break;

                    foreach (IRelease release in nextReleases)
                        yield return release;

                    // Go the next page of releases.

                    nextPageUrl = htmlDocument.DocumentNode.SelectNodes(@"//a[text()='Next']")
                        ?.FirstOrDefault()
                        ?.GetAttributeValue("href", string.Empty);

                    if (string.IsNullOrWhiteSpace(nextPageUrl))
                        break;

                }

            }

        }
        public IEnumerable<IFileNode> GetFiles(string url, SearchOption searchOption) {

            IGitHubUrl gitHubUrl = GitHubUrl.Parse(url);
            string branchName = gitHubUrl.Tree ?? Properties.GitHub.DefaultBranchName;

            if (string.IsNullOrWhiteSpace(gitHubUrl.Tree))
                branchName = GetRepository(url).DefaultBranchName;

            using (IWebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                string fileListUrl = GetFileListUrl(url, branchName);

                htmlDocument.LoadHtml(webClient.DownloadString(fileListUrl));

                IEnumerable<IFileNode> nextFiles = ParseFiles(htmlDocument.DocumentNode);

                foreach (IFileNode file in nextFiles) {

                    yield return file;

                    if (searchOption == SearchOption.AllDirectories && file.IsDirectory)
                        foreach (IFileNode nestedFile in GetFiles(file.Url, searchOption))
                            yield return nestedFile;

                }

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private IWebClient CreateWebClient() {

            return webRequestFactory.ToWebClientFactory().Create();

        }

        private IRepository ParseRepository(string url, HtmlNode repositoryNode) {

            string defaultBranchName = repositoryNode.SelectNodes("//*[@id='branch-select-menu']//*[@data-menu-button]")?.FirstOrDefault()?.InnerText;

            return new Repository(url) {
                DefaultBranchName = defaultBranchName,
            };

        }
        private IEnumerable<IRelease> ParseReleases(HtmlNode releasesNode) {

            IEnumerable<HtmlNode> releaseNodes = releasesNode.SelectNodes(Properties.QueryStrings.ReleasesXPath);

            if (releaseNodes is object) {

                foreach (IRelease release in releaseNodes.Select(node => ParseRelease(node)))
                    yield return release;

            }

        }
        private IRelease ParseRelease(HtmlNode releaseNode) {

            DateTimeOffset.TryParse(releaseNode.SelectNodes(Properties.QueryStrings.ReleaseDateTimeXPath)?.FirstOrDefault()?.GetAttributeValue("datetime", string.Empty), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset creationTime);

            string description = releaseNode.SelectNodes(Properties.QueryStrings.ReleaseDescriptionXPath)?.FirstOrDefault()?.InnerText?.Trim() ?? "";
            string tag = releaseNode.SelectNodes(Properties.QueryStrings.ReleaseTagXPath)?.FirstOrDefault()?.InnerText?.Trim() ?? "";
            string title = releaseNode.SelectNodes(Properties.QueryStrings.ReleaseTitleXPath)?.FirstOrDefault()?.InnerText?.Trim() ?? "";
            string url = releaseNode.SelectNodes(Properties.QueryStrings.ReleaseTitleXPath)?.FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            return new Release() {
                Published = creationTime,
                Description = Uri.UnescapeDataString(description),
                Tag = tag,
                Title = Uri.UnescapeDataString(title),
                Url = Url.Combine(Properties.GitHub.RootUrl, url),
                Assets = ParseReleaseAssets(releaseNode),
            };

        }
        private IEnumerable<IReleaseAsset> ParseReleaseAssets(HtmlNode releaseNode) {

            return releaseNode.SelectNodes(Properties.QueryStrings.ReleaseAssetsXPath)?
                .Select(node => ParseReleaseAsset(node))
                .ToArray() // The HTML document will be reused, so we need these assets to be evaluated immediately
                .Where(asset => !string.IsNullOrWhiteSpace(asset.Name) && !string.IsNullOrWhiteSpace(asset.DownloadUrl)) ?? Enumerable.Empty<IReleaseAsset>();

        }
        private IReleaseAsset ParseReleaseAsset(HtmlNode releaseAssetNode) {

            string name = GetNodeText(releaseAssetNode, Properties.QueryStrings.ReleaseAssetTitleXPath);
            string downloadUrl = GetNodeHref(releaseAssetNode, Properties.QueryStrings.ReleaseAssetTitleXPath);

            return new ReleaseAsset() {
                Name = StringUtilities.NormalizeWhiteSpace(Uri.UnescapeDataString(name)),
                DownloadUrl = Url.Combine(Properties.GitHub.RootUrl, downloadUrl),
            };

        }
        private string GetFileListUrl(string url, string branchName) {

            IGitHubUrl gitHubUrl = GitHubUrl.Parse(url);

            StringBuilder sb = new StringBuilder();

            sb.Append(Properties.GitHub.RootUrl);
            sb.Append(Uri.EscapeUriString(gitHubUrl.Owner));
            sb.Append("/");
            sb.Append(Uri.EscapeUriString(gitHubUrl.RepositoryName));
            sb.Append("/file-list/");
            sb.Append(Uri.EscapeUriString(branchName ?? Properties.GitHub.DefaultBranchName));

            if (!string.IsNullOrWhiteSpace(gitHubUrl.Path))
                sb.Append(Uri.EscapeUriString(gitHubUrl.Path));

            return sb.ToString();

        }
        private IEnumerable<IFileNode> ParseFiles(HtmlNode filesNode) {

            IEnumerable<HtmlNode> fileNodes = filesNode.SelectNodes(@"//div[contains(@class,'js-navigation-item')]");

            foreach (IFileNode file in fileNodes.Select(node => ParseFile(node)).Where(file => file is object))
                yield return file;

        }
        private IFileNode ParseFile(HtmlNode fileNode) {

            bool isDirectory = fileNode.SelectNodes(".//svg")?.FirstOrDefault()?.GetAttributeValue("aria-label", string.Empty)?.Equals("Directory", StringComparison.OrdinalIgnoreCase) ?? false;
            string fileUrl = fileNode.SelectNodes(".//a[contains(@class,'Link--primary')]")?.FirstOrDefault()?.GetAttributeValue("href", string.Empty);
            string commitUrl = fileNode.SelectNodes(".//a[contains(@class,'Link--secondary')]")?.FirstOrDefault()?.GetAttributeValue("href", string.Empty);
            string commitMessage = fileNode.SelectNodes(".//a[contains(@class,'Link--secondary')]")?.FirstOrDefault()?.GetAttributeValue("title", string.Empty);

            DateTimeOffset.TryParse(fileNode.SelectNodes(@".//*[@datetime]")?.FirstOrDefault()?.GetAttributeValue("datetime", string.Empty), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset lastModified);

            // If we encounter something like the ".." link to return to a parent directory, we won't have a file URL.
            // This is fine, because we don't want this to be considered a file anyway.

            if (string.IsNullOrEmpty(fileUrl))
                return null;

            // If we can get a commit hash for this file, change the tree to the commit hash.
            // This ensures that when we get the raw URL it always points to the latest version of the file (otherwise the file is cached by GitHub for up to 5 minutes and can be stale).

            string commitHash = string.Empty;

            if (!string.IsNullOrWhiteSpace(commitUrl)) {

                Match commitHashMatch = Regex.Match(commitUrl, @"\/commit\/([^\/]+)");

                if (commitHashMatch.Success) {

                    commitHash = commitHashMatch.Groups[1].Value;

                    fileUrl = Regex.Replace(fileUrl, @"\/tree\/[^\/]+", $@"/tree/{commitHash}");

                }

            }

            if (fileUrl.StartsWith("/"))
                fileUrl = Url.Combine(Properties.GitHub.RootUrl, fileUrl);

            return new FileNode(fileUrl) {
                CommitHash = commitHash,
                CommitMessage = Uri.UnescapeDataString(commitMessage),
                IsDirectory = isDirectory,
                LastModified = lastModified,
            };

        }

        private static string GetNodeText(HtmlNode node, string xPath) {

            return node.SelectNodes(xPath).FirstOrDefault()?.InnerText?.Trim() ?? "";

        }
        private static string GetNodeHref(HtmlNode node, string xPath) {

            return node.SelectNodes(xPath).FirstOrDefault()?.GetAttributeValue("href", "");

        }

    }

}