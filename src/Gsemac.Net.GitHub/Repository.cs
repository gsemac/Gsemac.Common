using System;
using System.Text;

namespace Gsemac.Net.GitHub {

    public class Repository :
        IRepository {

        // Public members

        public string DefaultBranchName { get; set; }
        public string Url => GetUrl();
        public string Name { get; }
        public string Owner { get; }
        public string Tree { get; }

        public string ArchiveUrl => GetArchiveUrl();
        public string ReleasesUrl => GetReleasesUrl();

        public Repository(string url) {

            IGitHubUrl gitHubUrl = GitHubUrl.Parse(url);

            Name = gitHubUrl.RepositoryName;
            Owner = gitHubUrl.Owner;
            Tree = gitHubUrl.Tree;

        }
        public Repository(Uri uri) :
            this(uri.AbsoluteUri) {
        }
        public Repository(string owner, string name) {

            Owner = owner;
            Name = name;

        }

        // Private members

        private string GetBaseUrl() {

            StringBuilder sb = new StringBuilder();

            sb.Append(GitHubUtilities.GitHubRootUrl);
            sb.Append(Uri.EscapeUriString(Owner));
            sb.Append("/");
            sb.Append(Uri.EscapeUriString(Name));

            return sb.ToString();

        }
        private string GetUrl() {

            StringBuilder sb = new StringBuilder();

            sb.Append(GetBaseUrl());

            if (!string.IsNullOrWhiteSpace(Tree)) {

                sb.Append("/tree/");
                sb.Append(Uri.EscapeUriString(Tree));

            }

            return sb.ToString();

        }
        private string GetArchiveUrl() {

            StringBuilder sb = new StringBuilder();

            sb.Append(GetBaseUrl());
            sb.Append("/archive/");

            if (!string.IsNullOrWhiteSpace(Tree))
                sb.Append(Uri.EscapeUriString(Tree));
            else
                sb.Append(GitHubUtilities.DefaultBranchName);

            sb.Append(".zip");

            return sb.ToString();

        }
        private string GetReleasesUrl() {

            return $"{GetBaseUrl()}/releases";

        }

    }

}