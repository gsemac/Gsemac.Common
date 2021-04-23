using Gsemac.IO;
using System;
using System.Text;

namespace Gsemac.Net.GitHub {

    public class FileNode :
        IFileNode {

        // Public members

        public string Url => GetUrl();
        public string CommitHash { get; set; }
        public string CommitMessage { get; set; }
        public string Name => Uri.UnescapeDataString(PathUtilities.GetFilename(Url));
        public string Path => Uri.UnescapeDataString(gitHubUrl.Path);
        public bool IsDirectory { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string RawUrl => GetRawUrl();

        public FileNode(string url) {

            gitHubUrl = GitHubUrl.Parse(url);

        }

        // Private members

        private readonly IGitHubUrl gitHubUrl;

        private string GetUrl() {

            StringBuilder sb = new StringBuilder();

            sb.Append(GitHubUtilities.GitHubRootUrl);
            sb.Append(Uri.EscapeUriString(gitHubUrl.Owner));
            sb.Append("/");
            sb.Append(Uri.EscapeUriString(gitHubUrl.RepositoryName));
            sb.Append("/blob/");

            if (!string.IsNullOrWhiteSpace(gitHubUrl.Tree))
                sb.Append(Uri.EscapeUriString(gitHubUrl.Tree));
            else
                sb.Append(GitHubUtilities.DefaultBranchName);

            sb.Append(Path);

            return sb.ToString();

        }
        private string GetRawUrl() {

            StringBuilder sb = new StringBuilder();

            sb.Append(GitHubUtilities.RawRootUrl);
            sb.Append(Uri.EscapeUriString(gitHubUrl.Owner));
            sb.Append("/");
            sb.Append(Uri.EscapeUriString(gitHubUrl.RepositoryName));
            sb.Append("/");

            if (!string.IsNullOrWhiteSpace(gitHubUrl.Tree))
                sb.Append(Uri.EscapeUriString(gitHubUrl.Tree));
            else
                sb.Append(GitHubUtilities.DefaultBranchName);

            sb.Append(Path);

            return sb.ToString();

        }

    }

}