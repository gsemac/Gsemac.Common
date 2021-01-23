using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Extensions.Tests {

    [TestClass]
    public class CredentialsExtensionsTests {

        // Public members

        [TestMethod]
        public void TestToCredentialsStringWithUsernameAndPassword() {

            NetworkCredential credential = new NetworkCredential("username", "password");

            Assert.AreEqual("username:password", credential.ToCredentialsString());

        }
        [TestMethod]
        public void TestToCredentialsStringWithUsernameAndPasswordAndDomain() {

            NetworkCredential credential = new NetworkCredential("username", "password", "example.com");

            Assert.AreEqual("username:password", credential.ToCredentialsString(new System.Uri("http://example.com")));

        }
        [TestMethod]
        public void TestToCredentialsStringWithUsernameAndPasswordAndDomainAndAuthType() {

            NetworkCredential credential = new NetworkCredential("username", "password", "example.com");

            Assert.AreEqual("username:password", credential.ToCredentialsString(new System.Uri("http://example.com"), "Basic"));

        }
        [TestMethod]
        public void TestToCredentialsStringWithUsernameAndPasswordWithReservedCharacters() {

            NetworkCredential credential = new NetworkCredential("user:name", "pass:word");

            Assert.AreEqual("user%3Aname:pass%3Aword", credential.ToCredentialsString());

        }

    }

}