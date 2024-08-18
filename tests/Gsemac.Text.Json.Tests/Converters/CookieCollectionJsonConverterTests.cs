using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Text.Json.Converters.Tests {

    [TestClass]
    public class CookieCollectionJsonConverterTests {

        [TestMethod]
        public void TestDeserializeEmptyCookieCollection() {

            Assert.AreEqual(0, JsonConvert.DeserializeObject<CookieCollection>("[]", new JsonSerializerSettings() {
                Converters = new List<JsonConverter> {
                    new CookieCollectionJsonConverter()
                }
            }).Count);
        }
        [TestMethod]
        public void TestDeserializeCookieCollectionWithOneCookie() {

            CookieCollection cookieCollection = JsonConvert.DeserializeObject<CookieCollection>("[{ domain: \"example.com\", name: \"hello\", value: \"world\" }]", new JsonSerializerSettings() {
                Converters = new List<JsonConverter> {
                    new CookieCollectionJsonConverter()
                }
            });

            Assert.AreEqual(1, cookieCollection.Count);
            Assert.AreEqual("example.com", cookieCollection[0].Domain);
            Assert.AreEqual("hello", cookieCollection[0].Name);
            Assert.AreEqual("world", cookieCollection[0].Value);

        }

    }

}