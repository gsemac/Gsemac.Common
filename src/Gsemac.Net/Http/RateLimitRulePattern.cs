using Gsemac.Text.Extensions;
using Gsemac.Text.PatternMatching;
using System;

namespace Gsemac.Net.Http {

    internal sealed class RateLimitRulePattern :
        IPatternMatcher {

        // Public members

        public RateLimitRulePattern(string pattern) {
            this.pattern = pattern ?? string.Empty;
        }

        public bool IsMatch(string input) {

            // We will match all paths at the depth of the given endpoint and below.
            // The scheme is ignored unless the pattern includes a scheme.

            // "//example.com/" should match requests to "https://example.com/some/path"
            // "example.com/" should match requests to "https://example.com/some/path"
            // "example.com" should match requests to "https://example.com/some/path"
            // "https://example.com/" should ONLY match "https://example.com/some/path" but not "http://example.com/some/path"

            if (string.IsNullOrWhiteSpace(pattern) || string.IsNullOrWhiteSpace(input))
                return false;

            string endpointPattern = pattern.Trim().TrimEnd('/');
            string requestUri = input.Trim().TrimEnd('/');

            // Strip the scheme from the request URI as dictated by the endpoint pattern.

            if (endpointPattern.StartsWith("//")) {

                // Make it so both strings begin with "//".

                requestUri = requestUri.After(":");

            }
            else if (!endpointPattern.Contains("://")) {

                // If the pattern does not specify a scheme, we'll strip the scheme from the URI.

                requestUri = requestUri.After("://");

            }

            return requestUri.Equals(endpointPattern, StringComparison.OrdinalIgnoreCase) ||
                requestUri.StartsWith(endpointPattern + "/");

        }
        public IPatternMatch Match(string input) {
            throw new NotImplementedException();
        }

        public override string ToString() {
            return pattern;
        }

        // Private members

        private readonly string pattern;

    }

}