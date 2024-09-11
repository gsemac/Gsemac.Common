using Gsemac.Polyfills.System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http {

    public static class CookieContainerUtilities {

        // Public members

        public static void RemoveDuplicateCookies(CookieContainer cookieContainer) {

            if (cookieContainer is null)
                throw new ArgumentNullException(nameof(cookieContainer));

            // CookieContainer has a bug that allows duplicate cookies to exist for "example.com" and ".example.com":
            // https://stackoverflow.com/q/1047669

            // The two domains are treated as distinct when added to the container, but both will be sent with requests.
            // This can be avoided by normalizing domains before adding them to the container, but we run into issues for cookies added automatically.

            // There are reflection-based solutions to this, but we will use a different approach to avoid relying on the class' internals too much.
            // While we can't remove cookies directly, setting them to "Expired" will cause the container to remove them automatically.
            // We'll keep the latest version of each duplicated cookie, and expire the remaining copies.

            HashSet<Cookie> cookies = new HashSet<Cookie>(new CookieComparer());

            foreach (Cookie cookie in cookieContainer.GetAllCookies().Cast<Cookie>().OrderByDescending(c => c.TimeStamp)) {

                if (cookies.Contains(cookie)) {

                    cookie.Expires = default;
                    cookie.Expired = true;

                }

                cookies.Add(cookie);

            }

        }

    }

}