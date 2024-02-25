using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.WebBrowsers.Tests {

    [TestClass]
    public class WebBrowserUtilitiesTests {

        // EscapeUriString

        [TestMethod]
        public void TestEscapeUriStringWithUriWithPathAndQueryContainingReservedCharactersAndChrome() {

            Assert.AreEqual("https://example.com/any%EC%9D%B4%EC%9C%A4%ED%9D%AC%20%22%3C%3E%%7C!*'();:@$[]thing?id=%EC%9D%B4%EC%9C%A4%ED%9D%AC%20%22%3C%3E%|!*%27();:@$[]",
                WebBrowserUtilities.EscapeUriString("https://example.com/any이윤희 \"<>%|!*'();:@$[]thing?id=이윤희 \"<>%|!*'();:@$[]", WebBrowserId.Chrome));

        }
        [TestMethod]
        public void TestEscapeUriStringWithUriWithPathAndQueryContainingReservedCharactersAndFirefox() {

            Assert.AreEqual("https://example.com/any%EC%9D%B4%EC%9C%A4%ED%9D%AC%20%22%3C%3E%|!*'();:@$[]thing?id=%EC%9D%B4%EC%9C%A4%ED%9D%AC%20%22%3C%3E%|!*%27();:@$[]",
                WebBrowserUtilities.EscapeUriString("https://example.com/any이윤희 \"<>%|!*'();:@$[]thing?id=이윤희 \"<>%|!*'();:@$[]", WebBrowserId.Firefox));

        }

    }

}