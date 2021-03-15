using System;
using System.Text.RegularExpressions;

namespace Gsemac.Net.GitHub {

    public class GitHubUrl :
        IGitHubUrl {

        // Public members

        public string Owner { get; }
        public string Path { get; }
        public string RepositoryName { get; }
        public string Tree { get; }

        public static GitHubUrl Parse(string url) {

            return new GitHubUrl(url);

        }

        // Private members

        private GitHubUrl(string url) {

            // Read the owner and repository name from the URL.

            Match ownerAndRepositoryNameMatch = Regex.Match(url, $@"^(?:{Regex.Escape(GitHubUtilities.GitHubRootUrl)}|{Regex.Escape(GitHubUtilities.RawRootUrl)})(?<owner>[^\/]+)\/(?<name>[^\/]+)");

            if (!ownerAndRepositoryNameMatch.Success)
                throw new FormatException("The URL is not in the correct format.");

            Owner = Uri.UnescapeDataString(ownerAndRepositoryNameMatch.Groups["owner"].Value);
            RepositoryName = Uri.UnescapeDataString(ownerAndRepositoryNameMatch.Groups["name"].Value);

            // Read the branch name or commit hash.

            Match treeNameMatch = Regex.Match(url, $@"\/(?:tree|blob)\/(?<tree>[^\/]+)|{Regex.Escape(GitHubUtilities.RawRootUrl)}[^\/]+\/[^\/]+\/([^\/]+)");

            if (treeNameMatch.Success) {

                string tree = treeNameMatch.Groups["tree"].Value;

                Tree = tree;

                // Read the remaining path.

                Match pathMatch = Regex.Match(url, $@"{tree}(?<path>\/.+?)$");

                if (pathMatch.Success)
                    Path = Uri.UnescapeDataString(pathMatch.Groups["path"].Value);

            }

        }

    }

}