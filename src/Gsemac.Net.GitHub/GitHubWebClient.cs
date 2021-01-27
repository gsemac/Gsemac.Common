using Gsemac.Net.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

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

        public IRepository GetRepository(IRepositoryUrl repositoryUrl) {

            using (WebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(webClient.DownloadString(repositoryUrl.CodeUrl));

                string defaultDownloadUrl = $"{GitHubUtilities.RootUrl}{repositoryUrl.Owner}/{repositoryUrl.RepositoryName}/archive/{(string.IsNullOrWhiteSpace(repositoryUrl.BranchName) ? "master" : repositoryUrl.BranchName)}.zip";
                string downloadUrl = GitHubUtilities.RootUrl.TrimEnd('/') + htmlDocument.DocumentNode.SelectNodes(@"//a[contains(.,'Download ZIP')]").FirstOrDefault()?.GetAttributeValue("href", string.Empty);

                return new Repository() {
                    Url = repositoryUrl,
                    DownloadUrl = downloadUrl.EndsWith("/") ? defaultDownloadUrl : downloadUrl,
                };

            }

        }
        public IEnumerable<IRelease> GetReleases(IRepositoryUrl repositoryUrl) {

            string nextPageUrl = repositoryUrl.ReleasesUrl;

            List<IRelease> releases = new List<IRelease>();

            using (WebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                while (!string.IsNullOrWhiteSpace(nextPageUrl)) {

                    htmlDocument.LoadHtml(webClient.DownloadString(nextPageUrl));

                    IEnumerable<HtmlNode> releaseEntryNodes = htmlDocument.DocumentNode.SelectNodes(@"//div[@class='release-entry']");

                    if (releaseEntryNodes.Count() <= 0)
                        break;

                    foreach (IRelease release in releaseEntryNodes.Select(node => ParseReleaseEntry(node)))
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

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private WebClient CreateWebClient() {

            return webRequestFactory.ToWebClientFactory().Create();

        }

        private IRelease ParseReleaseEntry(HtmlNode releaseEntryNode) {

            DateTimeOffset creationTime = DateTimeOffset.Parse(releaseEntryNode.SelectNodes(@".//*[@datetime]")?.FirstOrDefault()?.GetAttributeValue("datetime", string.Empty), CultureInfo.InvariantCulture);
            string description = releaseEntryNode.SelectNodes(@".//div[@class='markdown-body']")?.FirstOrDefault()?.InnerText;
            string tag = releaseEntryNode.SelectNodes(@".//svg[contains(@class,'octicon-tag')]/following-sibling::span")?.FirstOrDefault()?.InnerText;
            string title = releaseEntryNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div")?.FirstOrDefault()?.InnerText;
            string url = releaseEntryNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div/a")?.FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            return new Release() {
                CreationTime = creationTime,
                Description = Uri.UnescapeDataString(description?.Trim() ?? ""),
                Tag = tag,
                Title = Uri.UnescapeDataString(title?.Trim() ?? ""),
                Url = GitHubUtilities.RootUrl.TrimEnd('/') + url,
                Assets = ParseReleaseAssets(releaseEntryNode),
            };

        }
        private IEnumerable<IReleaseAsset> ParseReleaseAssets(HtmlNode releaseEntryNode) {

            return releaseEntryNode.SelectNodes(@".//summary[contains(.,'Assets')]/following-sibling::div/div/div")
                ?.Select(node => ParseReleaseAsset(node))
                .Where(asset => !string.IsNullOrWhiteSpace(asset.Name) && !string.IsNullOrWhiteSpace(asset.DownloadUrl));

        }
        private IReleaseAsset ParseReleaseAsset(HtmlNode releaseAssetNode) {

            string name = releaseAssetNode.SelectNodes(@".//a").FirstOrDefault()?.InnerText;
            string downloadUrl = releaseAssetNode.SelectNodes(@".//a").FirstOrDefault()?.GetAttributeValue("href", string.Empty);

            return new ReleaseAsset() {
                Name = Uri.UnescapeDataString(name?.Trim() ?? ""),
                DownloadUrl = GitHubUtilities.RootUrl.TrimEnd('/') + downloadUrl,
            };

        }

    }

}