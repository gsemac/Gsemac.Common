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

        // GetDomainName

        [TestMethod]
        public void TestGetDomainNameWithSubdomain() {

            Url url = new Url("https://www.stackoverflow.com/");

            Assert.AreEqual("stackoverflow.com", Url.GetDomainName(url.ToString()));

        }
        [TestMethod]
        public void TestGetDomainNameWithoutProtocol() {

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

    }

}