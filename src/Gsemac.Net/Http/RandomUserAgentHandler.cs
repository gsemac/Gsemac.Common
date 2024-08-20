using Gsemac.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Gsemac.Net.Http {

    public class RandomUserAgentHandler :
        HttpWebRequestHandler {

        // Public members

        public bool MaintainUserAgentPerHost { get; set; } = true;
        public bool ReplaceExistingUserAgent { get; set; } = false;

        public RandomUserAgentHandler(IEnumerable<string> userAgents) :
            base() {

            if (userAgents is null)
                throw new ArgumentNullException(nameof(userAgents));

            this.userAgents = userAgents;

        }

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.UserAgent) || ReplaceExistingUserAgent) {

                string userAgent = GetUserAgentForHost(request.RequestUri);

                if (!string.IsNullOrWhiteSpace(userAgent))
                    request.UserAgent = userAgent;

            }

            return base.GetResponse(request, cancellationToken);

        }

        // Private members

        private readonly IEnumerable<string> userAgents;
        private readonly IDictionary<string, string> hostToUserAgentDict = new Dictionary<string, string>();

        private string GetRandomUserAgent() {

            return userAgents.RandomOrDefault();

        }
        private string GetUserAgentForHost(Uri requestUri) {

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            if (MaintainUserAgentPerHost) {

                // Use the domain name instead of the hostname so subdomains use the same user agent.

                string host = Url.GetDomainName(requestUri.AbsoluteUri);

                lock (hostToUserAgentDict) {

                    if (hostToUserAgentDict.TryGetValue(host, out string userAgent))
                        return userAgent;

                    userAgent = GetRandomUserAgent();

                    hostToUserAgentDict[host] = userAgent;

                    return userAgent;

                }

            }
            else {

                return GetRandomUserAgent();

            }

        }

    }

}