using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net {

    public abstract class HttpWebRequestFactoryBase :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions GetOptions() {

            return options;

        }
        public IHttpWebRequestOptions GetOptions(Uri uri) {

            HttpWebRequestOptions options = new HttpWebRequestOptions(GetOptions());
            IHttpWebRequestOptions uriOptions;

            lock (uriOptionsMutex) {

                uriOptions = FindOptionsByUri(uri.AbsoluteUri);

            }

            if (!(uriOptions is null))
                uriOptions.CopyTo(options);

            return options;

        }
        public void SetOptions(IHttpWebRequestOptions options) {

            this.options = options;

        }
        public void SetOptions(string domain, IHttpWebRequestOptions options) {

            lock (uriOptionsMutex) {

                IHttpWebRequestOptions uriOptions = FindOptionsByUri(domain);

                if (!(uriOptions is null))
                    this.uriOptions.RemoveAll(item => item.Options == uriOptions);

                // Allow the user to just clear the existing options if a null argument is given.

                if (!(options is null))
                    this.uriOptions.Add(new UriOptions(domain, options));

            }

        }

        public virtual IHttpWebRequest Create(Uri requestUri) {

            IHttpWebRequest httpWebRequest = CreateInternal(requestUri);

            GetOptions(requestUri).CopyTo(httpWebRequest);

            return httpWebRequest;

        }

        // Protected members

        protected HttpWebRequestFactoryBase() :
            this(HttpWebRequestOptions.Default) {
        }
        protected HttpWebRequestFactoryBase(IHttpWebRequestOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        protected abstract IHttpWebRequest CreateInternal(Uri requestUri);

        // Private members

        private class UriOptions {

            public string Domain { get; }
            public IHttpWebRequestOptions Options { get; }

            public UriOptions(string domain, IHttpWebRequestOptions options) {

                this.Domain = domain;
                this.Options = options;

            }

        }

        private IHttpWebRequestOptions options = HttpWebRequestOptions.Default;
        private readonly List<UriOptions> uriOptions = new List<UriOptions>();
        private readonly object uriOptionsMutex = new object();

        private IHttpWebRequestOptions FindOptionsByUri(string uri) {

            return uriOptions.Where(options => new CookieDomainPattern(options.Domain).IsMatch(uri))
                .Select(options => options.Options)
                .FirstOrDefault();

        }

    }

}