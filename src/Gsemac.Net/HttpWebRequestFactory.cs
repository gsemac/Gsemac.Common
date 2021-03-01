﻿using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net {

    public class HttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public bool OverrideAutomaticRedirection { get; set; } = false;

        public HttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public HttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public HttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            if (optionsFactory is null)
                throw new ArgumentNullException(nameof(optionsFactory));

            this.optionsFactory = optionsFactory;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return new HttpWebRequestWrapper(requestUri) {
                OverrideAutomaticRedirection = OverrideAutomaticRedirection,
            }.WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}