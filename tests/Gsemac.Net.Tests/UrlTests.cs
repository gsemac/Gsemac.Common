using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class UrlTests {

        // Scheme

        [TestMethod]
        public void TestGetScheme() {

            Assert.AreEqual("https", new Url("https://stackoverflow.com/").Scheme);

        }
        [TestMethod]
        public void TestGetSchemeWithNoScheme() {

            Assert.AreEqual(string.Empty, new Url("//stackoverflow.com/").Scheme);

        }
        [TestMethod]
        public void TestGetSchemeWithNoSchemeAndNoForwardSlashes() {

            Assert.AreEqual(string.Empty, new Url("stackoverflow.com/").Scheme);

        }
        [TestMethod]
        public void TestToStringAfterSetSchemeWithColon() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Scheme = "http:",
            };

            Assert.AreEqual("http://stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetSchemeWithoutColon() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Scheme = "http",
            };

            Assert.AreEqual("http://stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringWithNoScheme() {

            Assert.AreEqual("//stackoverflow.com/", new Url("//stackoverflow.com/").ToString());

        }
        [TestMethod]
        public void TestSetSchemeWithInvalidCharacters() {

            Assert.ThrowsException<ArgumentException>(() => new Url("https://stackoverflow.com/").Scheme = "ht%$tps:");

        }

        // Username

        [TestMethod]
        public void TestGetUsername() {

            Assert.AreEqual("username", new Url("https://username:password@stackoverflow.com/").UserName);

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithoutPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                UserName = "username",
            };

            Assert.AreEqual("https://username@stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                UserName = "username",
                Password = "password",
            };

            Assert.AreEqual("https://username:password@stackoverflow.com/", url.ToString());

        }

        // Password

        [TestMethod]
        public void TestGetPassword() {

            Assert.AreEqual("password", new Url("https://username:password@stackoverflow.com/").Password);

        }
        [TestMethod]
        public void TestToStringAfterSetPasswordWithoutUsername() {

            // A username is required.

            IUrl url = new Url("https://stackoverflow.com/") {
                Password = "password",
            };

            Assert.AreEqual("https://stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetPasswordToEmptyString() {

            // A username is required.

            IUrl url = new Url("https://stackoverflow.com/") {
                UserName = "username",
                Password = "",
            };

            Assert.AreEqual("https://username:@stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetPasswordToNull() {

            // A username is required.

            IUrl url = new Url("https://stackoverflow.com/") {
                UserName = "username",
                Password = null,
            };

            Assert.AreEqual("https://username@stackoverflow.com/", url.ToString());

        }

        // Host

        [TestMethod]
        public void TestHost() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com/").Host);

        }
        [TestMethod]
        public void TestHostWithSubdomain() {

            Assert.AreEqual("subdomain.stackoverflow.com", new Url("https://subdomain.stackoverflow.com/").Host);

        }
        [TestMethod]
        public void TestHostWithPort() {

            Assert.AreEqual("stackoverflow.com:443", new Url("https://stackoverflow.com:443/").Host);

        }
        [TestMethod]
        public void TestToStringAfterSetHost() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Host = "stackexchange.com",
            };

            Assert.AreEqual("https://stackexchange.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetHostWithPort() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Host = "stackexchange.com:8080",
            };

            Assert.AreEqual("https://stackexchange.com:8080/", url.ToString());

        }

        // Hostname

        [TestMethod]
        public void TestHostname() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com/").Hostname);

        }
        [TestMethod]
        public void TestHostnameWithSubdomain() {

            Assert.AreEqual("subdomain.stackoverflow.com", new Url("https://subdomain.stackoverflow.com/").Hostname);

        }
        [TestMethod]
        public void TestHostnameWithPort() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com:443/").Hostname);

        }
        [TestMethod]
        public void TestHostnameWithFullyQualifiedDomainName() {

            Assert.AreEqual("stackoverflow.com.", new Url("https://stackoverflow.com./").Hostname);

        }
        [TestMethod]
        public void TestToStringAfterSetHostname() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Hostname = "stackexchange.com",
            };

            Assert.AreEqual("https://stackexchange.com:443/", url.ToString());

        }

        // Port

        [TestMethod]
        public void TestGetPort() {

            Assert.AreEqual(80, new Url("https://stackoverflow.com:80/").Port);

        }
        [TestMethod]
        public void TestGetPortWithoutPort() {

            Assert.AreEqual(null, new Url("https://stackoverflow.com/").Port);

        }
        [TestMethod]
        public void TestGetPortAfterSetHostWithPort() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Host = "stackoverflow.com:8080",
            };

            Assert.AreEqual(8080, url.Port);

        }
        [TestMethod]
        public void TestToStringAfterSetPort() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Port = 8080,
            };

            Assert.AreEqual("https://stackoverflow.com:8080/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetPortToNull() {

            IUrl url = new Url("https://stackoverflow.com:443/") {
                Port = null,
            };

            Assert.AreEqual("https://stackoverflow.com/", url.ToString());

        }

        // QueryParameters

        [TestMethod]
        public void TestGetFirstQueryParameter() {

            Assert.AreEqual("value1", new Url("https://example.com/?name1=value1&name2=value2").QueryParameters["name1"]);

        }
        [TestMethod]
        public void TestGetLastQueryParameter() {

            Assert.AreEqual("value2", new Url("https://example.com/?name1=value1&name2=value2").QueryParameters["name2"]);

        }
        [TestMethod]
        public void TestGetQueryParameterWithoutValueIsAdded() {

            Assert.IsTrue(new Url("https://example.com/?name1").QueryParameters.ContainsKey("name1"));

        }
        [TestMethod]
        public void TestGetQueryParameterWithoutValueHasNoValue() {

            Assert.AreEqual(string.Empty, new Url("https://example.com/?name1").QueryParameters["name1"]);

        }
        [TestMethod]
        public void TestSetExistingQueryParameter() {

            Url url = new("https://example.com/?name1=value1&name2=value2");

            url.QueryParameters["name1"] = "newValue";

            Assert.AreEqual("https://example.com/?name1=newValue&name2=value2", url.ToString());

        }
        [TestMethod]
        public void TestSetNewQueryParameter() {

            Url url = new("https://example.com/?name1=value1&name2=value2");

            url.QueryParameters["name3"] = "value3";

            Assert.AreEqual("https://example.com/?name1=value1&name2=value2&name3=value3", url.ToString());

        }
        [TestMethod]
        public void TestUrlDecodesQueryParameters() {

            Assert.AreEqual("!@#$%^&", new Url("https://example.com/?test=%21%40%23%24%25%5E%26").QueryParameters["test"]);

        }
        [TestMethod]
        public void TestUrlEncodesQueryParameters() {

            Url url = new("https://example.com/");

            url.QueryParameters["test"] = "!@#$%^&";

            Assert.AreEqual("https://example.com/?test=%21%40%23%24%25%5E%26", url.ToString());

        }

        // ToString

        [TestMethod]
        public void TestToStringPreservesQueryParameterOrdering() {

            // In an ideal world, it wouldn't matter what order the query parameters appear in. However, this is up to the server we're communicating with.
            // To avoid problems, we'll maintain the query parameter ordering.

            Assert.AreEqual("https://example.com/?a&b&c&d&e&f&g&h&i&j&k&l&m&n&o&p&q&r&s&t&u&v&w&x&y&z",
                new Url("https://example.com/?a&b&c&d&e&f&g&h&i&j&k&l&m&n&o&p&q&r&s&t&u&v&w&x&y&z").ToString());

        }
        [TestMethod]
        public void TestToStringReturnsEmptyStringForEmptyUrl() {

            Assert.AreEqual(string.Empty, new Url().ToString());

        }

        // GetDomainName

        [TestMethod]
        public void TestGetDomainNameWithSubdomain() {

            Url url = new("https://www.stackoverflow.com/");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithoutScheme() {

            Url url = new("www.stackoverflow.com");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithPath() {

            Url url = new("https://codegolf.stackexchange.com/questions/198550/simple-circular-words");

            Assert.AreEqual("stackexchange.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithoutSubdomain() {

            Url url = new("https://stackoverflow.com/");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithMultipartTld() {

            Url url = new("https://website.co.jp/");

            Assert.AreEqual("website.co.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameEndingWithMultipartTld() {

            // "websiteco.jp" ends with a multipart TLD ("co.jp"), but this should not be detected as such (because there is no dot separator).

            Url url = new("https://websiteco.jp/");

            Assert.AreEqual("websiteco.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithSubdomainAndMultipartTld() {

            Url url = new("https://subdomain.website.co.jp/");

            Assert.AreEqual("website.co.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithIpAddress() {

            Url url = new("https://192.168.1.1/");

            Assert.AreEqual("192.168.1.1", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithLocalhost() {

            Url url = new("http://localhost");

            Assert.AreEqual("localhost", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithFullyQualifiedDomainName() {

            Url url = new("https://stackoverflow.com./");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameDomainName() {

            Url url = new("stackoverflow.com");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameDomainWithInvalidTld() {
            Assert.AreEqual("domain.faketld", Url.GetDomainName("subomain.domain.faketld"));
        }

        // GetHostname

        [TestMethod]
        public void TestGetHostnameWithPort() {

            Assert.AreEqual("stackoverflow.com",
                Url.GetHostname("https://stackoverflow.com:8080/"));

        }
        [TestMethod]
        public void TestGetHostnameWithoutPort() {

            Assert.AreEqual("stackoverflow.com",
                Url.GetHostname("https://stackoverflow.com/"));

        }

        // GetHost

        [TestMethod]
        public void TestGetHostWithSubdomain() {

            Assert.AreEqual("www.stackoverflow.com",
                Url.GetHost("https://www.stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetHostWithoutScheme() {

            Assert.AreEqual("www.stackoverflow.com",
                Url.GetHost("www.stackoverflow.com"));

        }
        [TestMethod]
        public void TestGetHostWithPath() {

            Assert.AreEqual("codegolf.stackexchange.com",
                Url.GetHost("https://codegolf.stackexchange.com/questions/198550/simple-circular-words"));

        }
        [TestMethod]
        public void TestGetHostWithoutSubdomain() {

            Assert.AreEqual("stackoverflow.com",
                Url.GetHost("https://stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetHostWithPort() {

            Assert.AreEqual("stackoverflow.com:8080",
                Url.GetHost("https://stackoverflow.com:8080/"));

        }
        [TestMethod]
        public void TestGetHostWithCredentials() {

            Assert.AreEqual("example.com:8080",
                Url.GetHost("https://username:password@example.com:8080/path"));

        }
        [TestMethod]
        public void TestGetHostWithIPAddress() {

            Assert.AreEqual("127.0.0.1:8080",
                Url.GetHost("https://127.0.0.1:8080/path"));

        }
        [TestMethod]
        public void TestGetHostWithFullyQualifiedDomainName() {

            // Preserving the dot is the same behavior seen with JavaScript's "Url.host" property.

            Assert.AreEqual("stackoverflow.com.",
                Url.GetHost("https://stackoverflow.com./"));

        }

        // GetOrigin

        [TestMethod]
        public void TestGetOriginWithTrailingDirectorySeparator() {

            Assert.AreEqual("https://www.stackoverflow.com",
                Url.GetOrigin("https://www.stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetOriginWithPath() {

            Assert.AreEqual("https://www.stackoverflow.com",
                Url.GetOrigin("https://www.stackoverflow.com/path"));

        }
        [TestMethod]
        public void TestGetOriginWithoutPath() {

            Assert.AreEqual("https://www.stackoverflow.com",
                Url.GetOrigin("https://www.stackoverflow.com"));

        }
        [TestMethod]
        public void TestGetOriginWithPort() {

            Assert.AreEqual("https://stackoverflow.com:8080",
                Url.GetOrigin("https://stackoverflow.com:8080/"));

        }
        [TestMethod]
        public void TestGetOriginWithCredentials() {

            // Credentials are not considered part of the origin.

            Assert.AreEqual("https://example.com:8080",
                Url.GetOrigin("https://username:password@example.com:8080/path"));

        }
        [TestMethod]
        public void TestGetOriginWithEmptyString() {

            Assert.AreEqual(string.Empty,
                Url.GetOrigin(string.Empty));

        }

        // GetScheme

        [TestMethod]
        public void TestGetSchemeWithScheme() {

            Assert.AreEqual("https:",
                Url.GetScheme("https://www.stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetSchemeWithoutScheme() {

            Assert.AreEqual(string.Empty,
                Url.GetScheme("www.stackoverflow.com/"));

        }

        // Combine

        [TestMethod]
        public void TestCombineWithSingleUrl() {

            // If only a single URL argument is passed, the argument itself should be returned.

            Assert.AreEqual("https://example.com", Url.Combine("https://example.com"));

        }
        [TestMethod]
        public void TestCombineWithRootedUrlAndRootedUrl() {

            // When encountering multiple rooted URLs, The last rooted URL with be used.

            Assert.AreEqual("https://website.com/", Url.Combine("https://example.com/", "https://website.com/"));

        }
        [TestMethod]
        public void TestCombineWithUrlAndEmptyString() {

            Assert.AreEqual("https://example.com", Url.Combine("https://example.com", string.Empty));

        }
        [TestMethod]
        public void TestCombineWithEmptyStringAndUrl() {

            Assert.AreEqual("https://example.com", Url.Combine(string.Empty, "https://example.com"));

        }
        [TestMethod]
        public void TestCombineWithUrlAndDirectorySeparator() {

            Assert.AreEqual("https://example.com/", Url.Combine("https://example.com", "/"));

        }

        [TestMethod]
        public void TestCombineWithSchemeAndUrlWithNoScheme() {

            // Given a scheme and a URL without any scheme, the scheme should be prepended.

            Assert.AreEqual("https://example.com", Url.Combine("https://", "example.com"));

        }
        [TestMethod]
        public void TestCombineWithSchemeAndUrlWithRelativeScheme() {

            // Given a scheme and a URL with a relative scheme, the scheme should be prepended.

            Assert.AreEqual("https://example.com", Url.Combine("https://", "//example.com"));

        }
        [TestMethod]
        public void TestCombineWithUrlWithSchemeAndUrlWithRelativeScheme() {

            // The last scheme encountered will be the sheme for any URLs with relative schemes, even if the domains don't match.

            Assert.AreEqual("https://website.com", Url.Combine("https://example.com", "//website.com"));

        }
        [TestMethod]
        public void TestCombineWithSchemeAndScheme() {

            // When encountering multiple schemes, the last scheme will be used.

            Assert.AreEqual("https://", Url.Combine("http://", "https://"));

        }
        [TestMethod]
        public void TestCombineWithUrlsWithDifferentSchemes() {

            // When encountering URLs with different schemes, the last scheme will be used.
            // Additionally, the latest root URL will always be used regardless of the scheme.

            Assert.AreEqual("http://example.com/path", Url.Combine("https://example.com/", "http://example.com/", "path"));

        }

        [TestMethod]
        public void TestCombineWithUrlAndRootedPath() {

            // The rooted path should be appended to the root of the base path, replacing the original path altogether.

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/path1", "/path2"));

        }
        [TestMethod]
        public void TestCombineWithUrlEndingWithDirectorySeparatorAndRootedPath() {

            // When the rooted path is appended to the base path, there should only be a single directory separator.
            // This is because "/" is technically a path of its own, which gets replaced by the new path.

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/", "/path2"));

        }
        [TestMethod]
        public void TestCombineWithUrlNotEndingWithDirectorySeparatorAndRootedPath() {

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com", "/path2"));

        }

        [TestMethod]
        public void TestCombineWithUrlEndingWithDirectorySeparatorAndRelativePath() {

            // When the base path ends in a directory separator, the relative path should be appended to the existing path.

            Assert.AreEqual("https://example.com/path1/path2", Url.Combine("https://example.com/path1/", "path2"));

        }
        [TestMethod]
        public void TestCombineWithUrlNotEndingWithDirectorySeparatorAndRelativePath() {

            // When the base path does not end in a directory separator, the relative path should replace the current path.

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/path1", "path2"));

        }
        [TestMethod]
        public void TestCombineWithRootedUrlWithRelativePath() {

            // Under normal circumstances, if the base path doesn't end with a directory separator, the new relative path should replace the last segment of the existing path.
            // However, the one time this rule doesn't apply is when base then URL is already rooted.

            Assert.AreEqual("https://example.com/path", Url.Combine("https://example.com", "path"));

        }
        [TestMethod]
        public void TestCombineWithUrlAndRelativePathsEndingWithDirectorySeparators() {

            // If the paths end with directory separators, an extra one should not be inserted between them.

            Assert.AreEqual("https://example.com/path1/path2/", Url.Combine("https://example.com", "path1/", "path2/"));

        }
        [TestMethod]
        public void TestCombineWithTwoRelativePaths() {

            Assert.AreEqual("path1/path2", Url.Combine("path1", "path2"));

        }

        [TestMethod]
        public void TestCombineWithUrlEndingWithDirectorySeparatorAndQueryParameter() {

            Assert.AreEqual("https://example.com/?name=value", Url.Combine("https://example.com/", "?name=value"));

        }
        [TestMethod]
        public void TestCombineWithUrlNotEndingWithDirectorySeparatorAndQueryParameter() {

            Assert.AreEqual("https://example.com?name=value", Url.Combine("https://example.com", "?name=value"));

        }
        [TestMethod]
        public void TestCombineWithUrlWithQueryParameterAndPath() {

            // The URI parameters should not be applied to the new path.

            Assert.AreEqual("https://example.com/path", Url.Combine("https://example.com/?name=value", "path"));

        }

        [TestMethod]
        public void TestCombineWithUrlAndFragment() {

            // The URI fragment should be appended to the end of the path.

            Assert.AreEqual("https://example.com/path/#fragment", Url.Combine("https://example.com/path/", "#fragment"));

        }
        [TestMethod]
        public void TestCombineWithUrlNotEndingWithDirectorySeparatorAndFragment() {

            // The URI fragment should be appended to the end of the path, even if it doesn't end with a directory separator.

            Assert.AreEqual("https://example.com/path#fragment", Url.Combine("https://example.com/path", "#fragment"));

        }
        [TestMethod]
        public void TestCombineWithUrlWithFragmentAndFragment() {

            // Given a new URI fragment and a base URL with an existing URI fragment, the existing URI fragment should be replaced.

            Assert.AreEqual("https://example.com/path#fragment2", Url.Combine("https://example.com/path#fragment1", "#fragment2"));

        }
        [TestMethod]
        public void TestCombineWithUrlWithFragmentAndPath() {

            // The fragment should be removed when setting a new path.

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/path1#fragment", "path2"));

        }

    }

}