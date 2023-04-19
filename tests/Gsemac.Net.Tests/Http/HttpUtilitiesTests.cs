using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class HttpUtilitiesTests {

        // ParseCookies

        [TestMethod]
        public void TestParseCookiesWithCookieWithNameAndValue() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value")
                .FirstOrDefault();

            Assert.AreEqual("Name", cookie.Name);
            Assert.AreEqual("Value", cookie.Value);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithOnlyName() {

            Cookie cookie = HttpUtilities.ParseCookies("Name")
                .FirstOrDefault();

            Assert.AreEqual("Name", cookie.Name);
            Assert.AreEqual(string.Empty, cookie.Value);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithNameAndEmptyValue() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=")
                .FirstOrDefault();

            Assert.AreEqual("Name", cookie.Name);
            Assert.AreEqual(string.Empty, cookie.Value);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithDomainAttribute() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;Domain=.example.com")
                .FirstOrDefault();

            Assert.AreEqual(".example.com", cookie.Domain);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithPathAttribute() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;path=/")
                .FirstOrDefault();

            Assert.AreEqual("/", cookie.Path);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithHttpOnlyAttribute() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;Domain=.example.com;HttpOnly")
                .FirstOrDefault();

            Assert.AreEqual(true, cookie.HttpOnly);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithSecureAttribute() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;Domain=.example.com;Secure")
                .FirstOrDefault();

            Assert.AreEqual(true, cookie.Secure);

        }
        [TestMethod]
        public void TestParseCookiesWithCookieWithExpiresAttribute() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;Domain=.example.com;expires=Sat, 29-Apr-2023 02:45:20 GMT")
                .FirstOrDefault();

            Assert.AreEqual(29, cookie.Expires.Day);
            Assert.AreEqual(4, cookie.Expires.Month);
            Assert.AreEqual(2023, cookie.Expires.Year);

        }
        [TestMethod]
        public void TestParseCookiesWithMultipleCookies() {

            Cookie[] cookies = HttpUtilities.ParseCookies("Name1=Value1, Name2,Name3=,Name4=Value4,")
                .ToArray();

            Assert.AreEqual(4, cookies.Count());

            Assert.AreEqual("Name1", cookies[0].Name);
            Assert.AreEqual("Value1", cookies[0].Value);

            Assert.AreEqual("Name2", cookies[1].Name);
            Assert.AreEqual(string.Empty, cookies[1].Value);

            Assert.AreEqual("Name3", cookies[2].Name);
            Assert.AreEqual(string.Empty, cookies[2].Value);

            Assert.AreEqual("Name4", cookies[3].Name);
            Assert.AreEqual("Value4", cookies[3].Value);

        }
        [TestMethod]
        public void TestParseCookiesWithMultipleCookiesWithAttributes() {

            Cookie[] cookies = HttpUtilities.ParseCookies("Name1=Value1; path=/,Name2=Value2; expires=Sat, 29-Apr-2023 08:39:22 GMT; path=/; domain=www.example.com,Name3=Value3; expires=Thu, 18-Apr-2024 08:39:22 GMT; path=/; domain=.example.com")
                .ToArray();

            Assert.AreEqual(3, cookies.Count());

            Assert.AreEqual("Name1", cookies[0].Name);
            Assert.AreEqual("Value1", cookies[0].Value);
            Assert.AreEqual("/", cookies[0].Path);

            Assert.AreEqual("Name2", cookies[1].Name);
            Assert.AreEqual("Value2", cookies[1].Value);
            Assert.AreEqual("/", cookies[1].Path);
            Assert.AreEqual("www.example.com", cookies[1].Domain);

            Assert.AreEqual("Name3", cookies[2].Name);
            Assert.AreEqual("Value3", cookies[2].Value);
            Assert.AreEqual("/", cookies[2].Path);
            Assert.AreEqual(".example.com", cookies[2].Domain);

        }
        [TestMethod]
        public void TestParseCookiesIgnoresWhitespaceAroundValues() {

            Cookie cookie = HttpUtilities.ParseCookies("  Name    =   Value  ")
                .FirstOrDefault();

            Assert.AreEqual("Name", cookie.Name);
            Assert.AreEqual("Value", cookie.Value);

        }
        [TestMethod]
        public void TestParseCookiesIgnoresWhitespaceAroundAttributes() {

            Cookie cookie = HttpUtilities.ParseCookies("Name=Value;Domain=   .example.com   ;  Secure")
                .FirstOrDefault();

            Assert.AreEqual(".example.com", cookie.Domain);
            Assert.AreEqual(true, cookie.Secure);

        }
        [TestMethod]
        public void TestParseCookiesDoesNotUnescapeValues() {

            // This is in line with the behavior of CookieContainer's SetCookies method.

            Cookie cookie = HttpUtilities.ParseCookies("Name=Val%20ue;")
                .FirstOrDefault();

            Assert.AreEqual("Val%20ue", cookie.Value);

        }
        [TestMethod]
        public void TestParseCookiesDoesNotStripOuterQuotes() {

            // This is in line with the behavior of CookieContainer's SetCookies method.

            Cookie cookie = HttpUtilities.ParseCookies("Name=\"Value\";")
                .FirstOrDefault();

            Assert.AreEqual("\"Value\"", cookie.Value);

        }

        [TestMethod]
        public void TestParseCookiesWithUriSetsDomain() {

            // This is in line with the behavior of CookieContainer's SetCookies method.

            Cookie cookie = HttpUtilities.ParseCookies(new Uri("https://example.com"), "Name=Value")
                .FirstOrDefault();

            Assert.AreEqual("example.com", cookie.Domain);

        }
        [TestMethod]
        public void TestParseCookiesWithUriSetsPath() {

            // This is in line with the behavior of CookieContainer's SetCookies method.

            Cookie cookie = HttpUtilities.ParseCookies(new Uri("https://example.com/hello/world/"), "Name=Value")
                .FirstOrDefault();

            Assert.AreEqual("/hello/world", cookie.Path);

        }

        // ParseDate

        [TestMethod]
        public void TestParseDateWithRfc6265Date() {

            DateTimeOffset parsedDate = HttpUtilities.ParseDate("Wed, 21 Oct 2015 07:28:00 GMT");

            Assert.AreEqual(21, parsedDate.Day);
            Assert.AreEqual(10, parsedDate.Month);
            Assert.AreEqual(2015, parsedDate.Year);

        }
        [TestMethod]
        public void TestParseDateWithRfc2109Date() {

            DateTimeOffset parsedDate = HttpUtilities.ParseDate("Sat, 29-Apr-2023 02:45:20 GMT");

            Assert.AreEqual(29, parsedDate.Day);
            Assert.AreEqual(4, parsedDate.Month);
            Assert.AreEqual(2023, parsedDate.Year);

        }

    }

}