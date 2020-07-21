using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class DateUtilitiesTests {

        [TestMethod]
        public void TestToUnixTimeSecondsWithUtcDateTime() {

            Assert.AreEqual(1595358202, DateUtilities.ToUnixTimeSeconds(new DateTime(2020, 7, 21, 19, 3, 22, DateTimeKind.Utc)));

        }
        [TestMethod]
        public void TestToUnixTimeSecondsWithMinimumUnixDateTime() {

            Assert.AreEqual(0, DateUtilities.ToUnixTimeSeconds(DateUtilities.MinimumUnixDate));

        }

    }

}