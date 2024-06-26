﻿using Gsemac.Collections;
using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using Gsemac.Net.WebBrowsers;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebDrivers {

    public class EdgeWebDriverUpdater :
         WebDriverUpdaterBase {

        // Public members

        public EdgeWebDriverUpdater() :
            this(Logger.Null) {
        }
        public EdgeWebDriverUpdater(ILogger logger) :
            this(WebDriverUpdaterOptions.Default, logger) {
        }
        public EdgeWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webDriverUpdaterOptions, Logger.Null) {
        }
        public EdgeWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverUpdaterOptions, logger) {
        }
        public EdgeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webRequestFactory, webDriverUpdaterOptions, Logger.Null) {
        }
        public EdgeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            base(webRequestFactory, new WebDriverUpdaterOptions(webDriverUpdaterOptions) { WebBrowserId = WebBrowserId.MicrosoftEdge }, logger) {

            this.webRequestFactory = webRequestFactory;

        }

        // Protected members

        protected override Uri GetWebDriverUri(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            // Note that Edge and EdgeHTML use different web drivers-- MicrosoftEdgeDriver.exe and msedgedriver.exe, respectively.
            // The latest version of EdgeHTML is 18.19041, so that can be used to determine which web driver to download.
            // Currently, this method only gets web driver URIs for Edge, and not EdgeHTML.

            string edgeMajorVersionStr = webBrowserInfo.Version.Major.ToString(CultureInfo.InvariantCulture);

            // We'll get an XML document listing all web driver versions available for this version of Edge.
            // There might not be one for this specific version, so we'll pick the closest.

            using (IWebClient client = webRequestFactory.CreateWebClient()) {

                int maxResults = 999;
                string responseXml = client.DownloadString($"https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver?prefix={edgeMajorVersionStr}&delimiter=%2F&maxresults={maxResults}&restype=container&comp=list");

                string webDriverVersionStr = Regex.Matches(responseXml, @"<Name>([\d.]+)", RegexOptions.IgnoreCase).Cast<Match>()
                    .Select(m => m.Groups[1].Value) // version strings
                    .OrderByDescending(versionString => CountMatchingRevisions(versionString, edgeMajorVersionStr))
                    .ThenByDescending(versionString => versionString, new NaturalSortComparer())
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(webDriverVersionStr))
                    webDriverVersionStr = edgeMajorVersionStr;

                return new Uri($"https://msedgedriver.azureedge.net/{webDriverVersionStr}/edgedriver_{GetPlatformOS()}.zip");

            }

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.EdgeDriverExecutablePath;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private int CountMatchingRevisions(string versionString1, string versionString2) {

            return versionString1.Split('.')
                .Zip(versionString2.Split('.'), (n, m) => Tuple.Create(n, m))
                .Where(pair => pair.Item1.Equals(pair.Item2))
                .Count();

        }
        private string GetPlatformOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }

    }

}