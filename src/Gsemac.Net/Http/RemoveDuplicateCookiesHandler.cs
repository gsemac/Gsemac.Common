using System;
using System.Threading;

namespace Gsemac.Net.Http {

    // There is a bug in CookieContainer where we can end up with duplicate cookies for "example.com" and ".example.com":
    // https://stackoverflow.com/q/1047669

    // This handler will remove duplicate cookies, keeping only the newest ones, before sending the request.

    public sealed class RemoveDuplicateCookiesHandler :
        HttpWebRequestHandler {

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (request.CookieContainer is object)
                CookieContainerUtilities.RemoveDuplicateCookies(request.CookieContainer);

            return base.GetResponse(request, cancellationToken);

        }

    }

}