using Gsemac.Net.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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

            string nextPageUrl = new Repository(url).ReleasesUrl;

            using (IWebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                while (!string.IsNullOrWhiteSpace(nextPageUrl)) {

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
            string branchName = gitHubUrl.Tree ?? GitHubUtilities.DefaultBranchName;

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

            IEnumerable<HtmlNode> releaseNodes = releasesNode.SelectNodes(@"//div[@class='release-entry']");

            foreach (IRelease release in releaseNodes.Select(node => ParseRelease(node)))
                yield return release;

        }
        private IRelease ParseRelease(HtmlNode releaseNode) {

            DateTimeOffset.TryParse(releaseNode.SelectNodes(@".//*[@datetime]")?.FirstOrDefault()?.GetAttributeValue("datetime", string.Empty), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset creationTime);

            string description = releaseNode.SelectNodes(@".//div[@class='markdown-body']")?.FirstOrDefault()?.InnerText;
            string tag = releaseNode.SelectNodes(@".//svg[contains(@class,'octicon-tag')]/following-sibling::span")?.FirstOrDefault()?.InnerText;
            string title = releaseNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div")?.FirstOrDefault()?.InnerText;
            string url = releaseNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div/a")?.FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            return new Release() {
                Published = creationTime,
                Description = Uri.UnescapeDataString(description?.Trim() ?? ""),
                Tag = tag,
                Title = Uri.UnescapeDataString(title?.Trim() ?? ""),
                Url = GitHubUtilities.GitHubRootUrl.TrimEnd('/') + url,
                Assets = ParseReleaseAssets(releaseNode),
            };

        }
        private IEnumerable<IReleaseAsset> ParseReleaseAssets(HtmlNode releaseNode) {

            return releaseNode.SelectNodes(@".//summary[contains(.,'Assets')]/following-sibling::div/div/div")
                ?.Select(node => ParseReleaseAsset(node))
                .Where(asset => !string.IsNullOrWhiteSpace(asset.Name) && !string.IsNullOrWhiteSpace(asset.DownloadUrl));

        }
        private IReleaseAsset ParseReleaseAsset(HtmlNode releaseAssetNode) {

            string name = releaseAssetNode.SelectNodes(@".//a").FirstOrDefault()?.InnerText;
            string downloadUrl = releaseAssetNode.SelectNodes(@".//a").FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            return new ReleaseAsset() {
                Name = Uri.UnescapeDataString(name?.Trim() ?? ""),
                DownloadUrl = GitHubUtilities.GitHubRootUrl.TrimEnd('/') + downloadUrl,
            };

        }
        private string GetFileListUrl(string url, string branchName) {

            IGitHubUrl gitHubUrl = GitHubUrl.Parse(url);

            StringBuilder sb = new StringBuilder();

            sb.Append(GitHubUtilities.GitHubRootUrl);
            sb.Append(Uri.EscapeUriString(gitHubUrl.Owner));
            sb.Append("/");
            sb.Append(Uri.EscapeUriString(gitHubUrl.RepositoryName));
            sb.Append("/file-list/");
            sb.Append(Uri.EscapeUriString(branchName ?? GitHubUtilities.DefaultBranchName));

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
                fileUrl = GitHubUtilities.GitHubRootUrl.TrimEnd('/') + fileUrl;

            return new FileNode(fileUrl) {
                CommitHash = commitHash,
                CommitMessage = Uri.UnescapeDataString(commitMessage),
                IsDirectory = isDirectory,
                LastModified = lastModified,
            };

        }

    }

}