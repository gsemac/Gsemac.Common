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

            Assert.AreEqual("username", new Url("https://username:password@stackoverflow.com/").Username);

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithoutPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Username = "username",
            };

            Assert.AreEqual("https://username@stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Username = "username",
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
                Username = "username",
                Password = "",
            };

            Assert.AreEqual("https://username:@stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetPasswordToNull() {

            // A username is required.

            IUrl url = new Url("https://stackoverflow.com/") {
                Username = "username",
                Password = null,
            };

            Assert.AreEqual("https://username@stackoverflow.com/", url.ToString());

        }

        // Host

        [TestMethod]
        public void TestGetHost() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com/").Host);

        }
        [TestMethod]
        public void TestGetHostWithSubdomain() {

            Assert.AreEqual("subdomain.stackoverflow.com", new Url("https://subdomain.stackoverflow.com/").Host);

        }
        [TestMethod]
        public void TestGetHostWithPort() {

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
        public void TestGetHostname() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com/").Hostname);

        }
        [TestMethod]
        public void TestGetHostnameWithSubdomain() {

            Assert.AreEqual("subdomain.stackoverflow.com", new Url("https://subdomain.stackoverflow.com/").Hostname);

        }
        [TestMethod]
        public void TestGetHostnameWithPort() {

            Assert.AreEqual("stackoverflow.com", new Url("https://stackoverflow.com:443/").Hostname);

        }
        [TestMethod]
        public void TestGetHostnameWithFullyQualifiedDomainName() {

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

        // GetDomainName

        [TestMethod]
        public void TestGetDomainNameWithSubdomain() {

            Url url = new Url("https://www.stackoverflow.com/");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithoutScheme() {

            Url url = new Url("www.stackoverflow.com");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithPath() {

            Url url = new Url("https://codegolf.stackexchange.com/questions/198550/simple-circular-words");

            Assert.AreEqual("stackexchange.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithoutSubdomain() {

            Url url = new Url("https://stackoverflow.com/");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithMultipartTld() {

            Url url = new Url("https://website.co.jp/");

            Assert.AreEqual("website.co.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameEndingWithMultipartTld() {

            // "websiteco.jp" ends with a multipart TLD ("co.jp"), but this should not be detected as such (because there is no dot separator).

            Url url = new Url("https://websiteco.jp/");

            Assert.AreEqual("websiteco.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithSubdomainAndMultipartTld() {

            Url url = new Url("https://subdomain.website.co.jp/");

            Assert.AreEqual("website.co.jp", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithIpAddress() {

            Url url = new Url("https://192.168.1.1/");

            Assert.AreEqual("192.168.1.1", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithFullyQualifiedDomainName() {

            Url url = new Url("https://stackoverflow.com./");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameDomainName() {

            Url url = new Url("stackoverflow.com");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }

        // Combine

        [TestMethod]
        public void TestCombineWithSingleArgument() {

            Assert.AreEqual("https://example.com", Url.Combine("https://example.com"));

        }
        [TestMethod]
        public void TestCombineWithSchemeAndNoScheme() {

            Assert.AreEqual("https://example.com", Url.Combine("https://", "example.com"));

        }
        [TestMethod]
        public void TestCombineWithSchemeAndRelativeScheme() {

            Assert.AreEqual("https://example.com", Url.Combine("https://", "//example.com"));

        }
        [TestMethod]
        public void TestCombineWithUrlAndRelativeScheme() {

            Assert.AreEqual("https://website.com", Url.Combine("https://example.com", "//website.com"));

        }
        [TestMethod]
        public void TestCombineWithOnlySchemes() {

            Assert.AreEqual("https://", Url.Combine("http://", "https://"));

        }
        [TestMethod]
        public void TestCombineWithDifferentSchemes() {

            Assert.AreEqual("http://example.com/path", Url.Combine("https://example.com", "http://example.com", "path"));

        }
        [TestMethod]
        public void TestCombineWithRootedPath() {

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/path1", "/path2"));

        }
        [TestMethod]
        public void TestCombineWithRootedPathEndingWithDirectorySeparator() {

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com/", "/path2"));

        }
        [TestMethod]
        public void TestCombineWithRootedPathNotEndingWithDirectorySeparator() {

            Assert.AreEqual("https://example.com/path2", Url.Combine("https://example.com", "/path2"));

        }
        [TestMethod]
        public void TestCombineWithRelativePath() {

            Assert.AreEqual("https://example.com/path1/path2", Url.Combine("https://example.com/path1", "path2"));

        }
        [TestMethod]
        public void TestCombineWithRelativePathsEndingWithDirectorySeparators() {

            Assert.AreEqual("https://example.com/path1/path2/", Url.Combine("https://example.com", "path1/", "path2/"));

        }
        [TestMethod]
        public void TestCombineWithTwoRootedPaths() {

            Assert.AreEqual("https://website.com/", Url.Combine("https://example.com/", "https://website.com/"));

        }
        [TestMethod]
        public void TestCombineWithTwoRelativePaths() {

            Assert.AreEqual("path1/path2", Url.Combine("path1", "path2"));

        }
        [TestMethod]
        public void TestCombineWithUriParameter() {

            Assert.AreEqual("https://example.com/?name=value", Url.Combine("https://example.com/", "?name=value"));

        }
        [TestMethod]
        public void TestCombineWithMultipleUriParameters() {

            Assert.AreEqual("https://example.com/?name=value&name2=value2", Url.Combine("https://example.com/", "?name=value", "name2=value2"));

        }
        [TestMethod]
        public void TestCombineWithDirectorySeparator() {

            Assert.AreEqual("https://example.com/", Url.Combine("https://example.com", "/"));

        }
        [TestMethod]
        public void TestCombineWithMultipleDirectorySeparators() {

            Assert.AreEqual("https://example.com/path///path2///", Url.Combine("https://example.com", "///path2///"));

        }
        [TestMethod]
        public void TestCombineWithMultipleDirectorySeparatorsInMultipleParts() {

            Assert.AreEqual("https://example.com/path/////path2//", Url.Combine("https://example.com", "/", "//", "///", "path2", "//"));

        }

    }

}