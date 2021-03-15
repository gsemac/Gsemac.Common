using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.GitHub.Extensions {

    public static class GithubClientExtensions {

        public static IRepository GetRepository(this IGitHubClient gitHubClient, Uri uri) {

            return gitHubClient.GetRepository(uri.AbsoluteUri);

        }
        public static IRepository GetRepository(this IGitHubClient gitHubClient, IRepository repository) {

            return gitHubClient.GetRepository(repository.Url);

        }
        public static IRepository GetRepository(this IGitHubClient gitHubClient, string owner, string name) {

            return gitHubClient.GetRepository(new Repository(owner, name));

        }

        public static IEnumerable<IRelease> GetReleases(this IGitHubClient gitHubClient, Uri uri) {

            return gitHubClient.GetReleases(uri.AbsoluteUri);

        }
        public static IEnumerable<IRelease> GetReleases(this IGitHubClient gitHubClient, IRepository repository) {

            return gitHubClient.GetReleases(repository.Url);

        }
        public static IEnumerable<IRelease> GetReleases(this IGitHubClient gitHubClient, string owner, string name) {

            return gitHubClient.GetReleases(new Repository(owner, name).Url);

        }

        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, string url) {

            return GetLatestRelease(gitHubClient, new Repository(url));

        }
        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, Uri uri) {

            return GetLatestRelease(gitHubClient, new Repository(uri));

        }
        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, IRepository repository) {

            return gitHubClient.GetReleases(repository.Url).FirstOrDefault();

        }
        public static IRelease GetLatestRelease(this IGitHubClient gitHubClient, string owner, string name) {

            return GetLatestRelease(gitHubClient, new Repository(owner, name));

        }

        public static IEnumerable<IFileNode> GetFiles(this IGitHubClient gitHubClient, Uri uri, SearchOption searchOption = SearchOption.TopDirectoryOnly) {

            return gitHubClient.GetFiles(uri.AbsoluteUri, searchOption);

        }
        public static IEnumerable<IFileNode> GetFiles(this IGitHubClient gitHubClient, IRepository repository, SearchOption searchOption = SearchOption.TopDirectoryOnly) {

            return gitHubClient.GetFiles(repository.Url, searchOption);

        }
        public static IEnumerable<IFileNode> GetFiles(this IGitHubClient gitHubClient, string owner, string name, SearchOption searchOption = SearchOption.TopDirectoryOnly) {

            return gitHubClient.GetFiles(new Repository(owner, name).Url, searchOption);

        }

    }

}