using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Text.Json.Converters.Tests {

    [TestClass]
    public class CookieCollectionJsonConverterTests {

        [TestMethod]
        public void TestDeserializeEmptyCookieCollection() {

            Assert.IsEmpty(JsonConvert.DeserializeObject<CookieCollection>("[]", new JsonSerializerSettings() {
                Converters = [
                    new CookieCollectionJsonConverter()
                ]
            }));
        }
        [TestMethod]
        public void TestDeserializeCookieCollectionWithOneCookie() {

            CookieCollection cookieCollection = JsonConvert.DeserializeObject<CookieCollection>("[{ domain: \"example.com\", name: \"hello\", value: \"world\" }]", new JsonSerializerSettings() {
                Converters = new List<JsonConverter> {
                    new CookieCollectionJsonConverter()
                }
            });

            Assert.HasCount(1, cookieCollection);
            Assert.AreEqual("example.com", cookieCollection[0].Domain);
            Assert.AreEqual("hello", cookieCollection[0].Name);
            Assert.AreEqual("world", cookieCollection[0].Value);

        }

    }

}