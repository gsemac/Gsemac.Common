using Gsemac.Core;
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

        public IEnumerable<IRelease> GetReleases(IRepositoryUrl repositoryUrl, int numberOfReleases) {

            string nextPageUrl = repositoryUrl.ReleasesUrl;

            List<IRelease> releases = new List<IRelease>();

            using (WebClient webClient = CreateWebClient()) {

                HtmlDocument htmlDocument = new HtmlDocument();

                while (releases.Count() < numberOfReleases) {

                    htmlDocument.LoadHtml(webClient.DownloadString(repositoryUrl.ReleasesUrl));

                    IEnumerable<HtmlNode> releaseEntryNodes = htmlDocument.DocumentNode.SelectNodes(@"//div[@class='release-entry']");

                    if (releaseEntryNodes.Count() <= 0)
                        break;

                    releases.AddRange(releaseEntryNodes.Select(node => ParseReleaseEntry(node)));

                    // Go the next page of releases.

                    if (releases.Count() < numberOfReleases) {

                        nextPageUrl = htmlDocument.DocumentNode.SelectNodes(@"//a[text()='Next']").FirstOrDefault()?.GetAttributeValue("href", string.Empty);

                        if (string.IsNullOrWhiteSpace(nextPageUrl))
                            break;

                    }

                }

            }

            return releases.Take(numberOfReleases);

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private WebClient CreateWebClient() {

            return webRequestFactory.ToWebClientFactory().Create();

        }

        private IRelease ParseReleaseEntry(HtmlNode releaseEntryNode) {

            DateTimeOffset creationTime = DateTimeOffset.Parse(releaseEntryNode.SelectNodes(@".//*[@datetime]").FirstOrDefault()?.GetAttributeValue("datetime", string.Empty), CultureInfo.InvariantCulture);
            string description = releaseEntryNode.SelectNodes(@".//div[@class='markdown-body']").FirstOrDefault()?.InnerText;
            string tag = releaseEntryNode.SelectNodes(@".//svg[contains(@class,'octicon-tag')]/following-sibling::span").FirstOrDefault()?.InnerText;
            string title = releaseEntryNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div").FirstOrDefault()?.InnerText;
            string url = releaseEntryNode.SelectNodes(@".//div[contains(@class,'release-header')]/div/div/a").FirstOrDefault()?.GetAttributeValue("href", string.Empty);

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
                .Select(node => ParseReleaseAsset(node))
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