using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class UriUtilitiesTests {

        // SetFilePathCanonicalizationEnabled

        //[TestMethod]
        //public void TestAbsoluteUriWithFilePathCanonicalizationEnabled() {

        //    UriUtilities.SetFilePathCanonicalizationEnabled(true);

        //    Uri uri = new("https://example.com/file.");

        //    Assert.AreEqual("https://example.com/file", uri.AbsoluteUri);

        //}
        [TestMethod]
        public void TestAbsoluteUriWithFilePathCanonicalizationDisabled() {

            UriUtilities.SetFilePathCanonicalizationEnabled(false);

            Uri uri = new("https://example.com/file.");

            Assert.AreEqual("https://example.com/file.", uri.AbsoluteUri);

        }

        // SetPathCanonicalizationEnabled

        //[TestMethod]
        //public void TestAbsoluteUriWithPathCanonicalizationEnabled() {

        //    Uri uri = new("https://example.com/%5Cfile");

        //    UriUtilities.SetPathCanonicalizationEnabled(uri, true);

        //    Assert.AreEqual("https://example.com//file", uri.AbsoluteUri);

        //}
        [TestMethod]
        public void TestAbsoluteUriWithPathCanonicalizationDisabled() {

            Uri uri = new("https://example.com/%5Cfile");

            UriUtilities.SetPathCanonicalizationEnabled(uri, false);

            Assert.AreEqual("https://example.com/%5Cfile", uri.AbsoluteUri);

        }

    }

}