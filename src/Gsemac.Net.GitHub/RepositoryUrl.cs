using System;
using System.Text.RegularExpressions;

namespace Gsemac.Net.GitHub {

    public class RepositoryUrl :
        IRepositoryUrl {

        // Public members

        public string Owner { get; set; } = string.Empty;
        public string RepositoryName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;

        public string CodeUrl => ToString();
        public string ReleasesUrl => $"{ToString()}/releases";

        public RepositoryUrl(string repositoryUrl) {

            ParseRepositoryUrl(repositoryUrl);

        }
        public RepositoryUrl(string owner, string repositoryName) {

            this.Owner = owner;
            this.RepositoryName = repositoryName;

        }

        public override string ToString() {

            return $"{GitHubUtilities.RootUrl}{Uri.EscapeUriString(Owner)}/{Uri.EscapeUriString(RepositoryName)}";

        }

        // Private members

        private void ParseRepositoryUrl(string repositoryUrl) {

            // Read the owner and repository name from the URL.

            Match ownerAndRepositoryNameMatch = Regex.Match(repositoryUrl, $@"{Regex.Escape(GitHubUtilities.RootUrl)}([^\/]+)\/([^\/]+)");

            if (!ownerAndRepositoryNameMatch.Success)
                throw new FormatException("The repository URL is not in the correct format.");

            this.Owner = Uri.UnescapeDataString(ownerAndRepositoryNameMatch.Groups[1].Value);
            this.RepositoryName = Uri.UnescapeDataString(ownerAndRepositoryNameMatch.Groups[2].Value);

            // Read any other information we can retrieve from the URL.

            Match branchNameMatch = Regex.Match(repositoryUrl, @"\/tree\/([^\/]+)");

            if (branchNameMatch.Success)
                this.BranchName = Uri.UnescapeDataString(branchNameMatch.Groups[1].Value);

        }

    }

}