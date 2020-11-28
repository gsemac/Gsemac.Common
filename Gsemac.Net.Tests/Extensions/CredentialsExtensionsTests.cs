using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Extensions.Tests {

    [TestClass]
    public class CredentialsExtensionsTests {

        [TestMethod]
        public void TestToCredentialStringWithUsernameAndPassword() {

            NetworkCredential credential = new NetworkCredential("username", "password");

            Assert.AreEqual("username:password", credential.ToCredentialString());

        }
        [TestMethod]
        public void TestToCredentialStringWithUsernameAndPasswordAndDomain() {

            NetworkCredential credential = new NetworkCredential("username", "password", "example.com");

            Assert.AreEqual("username:password", credential.ToCredentialString(new System.Uri("http://example.com")));

        }
        [TestMethod]
        public void TestToCredentialStringWithUsernameAndPasswordAndDomainAndAuthType() {

            NetworkCredential credential = new NetworkCredential("username", "password", "example.com");

            Assert.AreEqual("username:password", credential.ToCredentialString(new System.Uri("http://example.com"), "Basic"));

        }
        [TestMethod]
        public void TestToCredentialStringWithUsernameAndPasswordWithReservedCharacters() {

            NetworkCredential credential = new NetworkCredential("user:name", "pass:word");

            Assert.AreEqual("user%3Aname:pass%3Aword", credential.ToCredentialString());

        }

    }

}