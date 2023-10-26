using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Dns.Tests {

    [TestClass]
    public class DnsUtilitiesTests {

        // GetReverseLookupAddress

        [TestMethod]
        public void TestGetReverseLookupAddressWithIPv4Address() {

            Assert.AreEqual("4.4.8.8.in-addr.arpa",
                DnsUtilities.GetReverseLookupAddress(IPAddress.Parse("8.8.4.4")));

        }
        [TestMethod]
        public void TestGetReverseLookupAddressWithIPv6Address() {

            Assert.AreEqual("b.a.9.8.7.6.5.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa",
                DnsUtilities.GetReverseLookupAddress(IPAddress.Parse("2001:db8::567:89ab")));

        }

    }

}