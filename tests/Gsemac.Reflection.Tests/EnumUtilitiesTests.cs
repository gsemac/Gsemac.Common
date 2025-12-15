using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class EnumUtilitiesTests {

        // Parse

        [TestMethod]
        public void TestParseWithNonEnumType() {

            Assert.Throws<ArgumentException>(() => EnumUtilities.Parse("blah", typeof(int)));

        }

        // TryParse

        [TestMethod]
        public void TestTryParseWithNonEnumType() {

            Assert.Throws<ArgumentException>(() => EnumUtilities.Parse("blah", typeof(int)));

        }

    }

}