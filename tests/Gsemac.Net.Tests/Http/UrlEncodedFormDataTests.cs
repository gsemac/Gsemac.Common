using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class UrlEncodedFormDataTests {

        // Parse

        [TestMethod]
        public void TestParseWithValidFormDataString() {

            IUrlEncodedFormData data = UrlEncodedFormData.Parse("key=value");

            Assert.AreEqual("value", data["key"]);

        }
        [TestMethod]
        public void TestParseWithCaseSensitiveFormDataString() {

            IUrlEncodedFormData data = UrlEncodedFormData.Parse("key1=value1&KEY1=value2");

            Assert.AreEqual("value1", data["key1"]);
            Assert.AreEqual("value2", data["KEY1"]);

        }
        [TestMethod]
        public void TestParseWithFormDataStringWithEncodedValue() {

            IUrlEncodedFormData data = UrlEncodedFormData.Parse("key=%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF%E4%B8%96%E7%95%8C");

            Assert.AreEqual("こんにちは世界", data["key"]);

        }
        [TestMethod]
        public void TestParseWithEmptyFormData() {

            IUrlEncodedFormData data = UrlEncodedFormData.Parse(string.Empty);

            Assert.AreEqual(string.Empty, data.ToString());

        }
        [TestMethod]
        public void TestParsePreservesKeyOrder() {

            IUrlEncodedFormData data = new UrlEncodedFormData();
            List<string> values = new();

            for (int i = 0; i < 100; ++i) {

                data[i.ToString()] = i.ToString();

                values.Add($"{i}={i}");

            }

            Assert.AreEqual(string.Join("&", values), data.ToString());

        }

        // ToString

        [TestMethod]
        public void TestToStringWithFormDataStringWithEncodedValue() {

            IUrlEncodedFormData data = new UrlEncodedFormData {
                { "key", "こんにちは世界" }
            };

            Assert.AreEqual("key=%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF%E4%B8%96%E7%95%8C", data.ToString());

        }
        [TestMethod]
        public void TestToStringWithEmptyFormData() {

            IUrlEncodedFormData data = new UrlEncodedFormData();

            Assert.AreEqual(string.Empty, data.ToString());

        }

    }

}