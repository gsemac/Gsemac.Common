using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Polyfills.System.Net.Tests {

    [TestClass]
    public class IPEndPointExTests {

        // TryParse

        [TestMethod]
        public void TestTryParseWithIPv4Address() {

            Assert.IsTrue(IPEndPointEx.TryParse("192.168.1.1", out IPEndPoint result));
            Assert.AreEqual(IPAddress.Parse("192.168.1.1"), result.Address);

        }
        [TestMethod]
        public void TestTryParseWithIPv4AddressAndPort() {

            Assert.IsTrue(IPEndPointEx.TryParse("192.168.1.1:443", out IPEndPoint result));
            Assert.AreEqual(IPAddress.Parse("192.168.1.1"), result.Address);
            Assert.AreEqual(443, result.Port);

        }
        [TestMethod]
        public void TestTryParseWithIPv6AddressWithBraces() {

            Assert.IsTrue(IPEndPointEx.TryParse("[2001:db8:85a3:8d3:1319:8a2e:370:7348]", out IPEndPoint result));
            Assert.AreEqual(IPAddress.Parse("2001:db8:85a3:8d3:1319:8a2e:370:7348"), result.Address);

        }
        [TestMethod]
        public void TestTryParseWithIPv6AddressWithBracesAndPort() {

            Assert.IsTrue(IPEndPointEx.TryParse("[2001:db8:85a3:8d3:1319:8a2e:370:7348]:443", out IPEndPoint result));
            Assert.AreEqual(IPAddress.Parse("2001:db8:85a3:8d3:1319:8a2e:370:7348"), result.Address);
            Assert.AreEqual(443, result.Port);

        }

    }

}