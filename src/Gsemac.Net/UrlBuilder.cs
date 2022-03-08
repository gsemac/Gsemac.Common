using System;

namespace Gsemac.Net {

    public class UrlBuilder :
        IUrlBuilder {

        // Public members

        public UrlBuilder() {

            url = new Url();

        }
        public UrlBuilder(string url) {

            this.url = new Url(url);

        }
        public UrlBuilder(IUrl url) {

            if (url is null)
                throw new ArgumentNullException(nameof(url));

            this.url = new Url(url);

        }

        public IUrlBuilder WithFragment(string fragment) {

            url.Fragment = fragment;

            return this;

        }
        public IUrlBuilder WithHostname(string hostname) {

            url.Hostname = hostname;

            return this;

        }
        public IUrlBuilder WithPassword(string password) {

            url.Password = password;

            return this;

        }
        public IUrlBuilder WithPath(string path) {

            url.Path = path;

            return this;

        }
        public IUrlBuilder WithPort(int port) {

            url.Port = port;

            return this;

        }
        public IUrlBuilder WithQueryParameter(string name, string value) {

            url.QueryParameters[name] = value;

            return this;

        }
        public IUrlBuilder WithScheme(string scheme) {

            url.Scheme = scheme;

            return this;

        }
        public IUrlBuilder WithUserName(string userName) {

            url.UserName = userName;

            return this;

        }

        public IUrl Build() {

            return url;

        }

        public override string ToString() {

            return url.ToString();

        }

        // Private members

        private readonly Url url;

    }

}