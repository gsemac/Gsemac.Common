using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class TypeCastConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertWithValidConversion() {

            Assert.AreEqual(5, new TypeCastConverter<int>().Convert(5.2));

        }
        [TestMethod]
        public void TestConvertWithInvalidConversionThrowsException() {

            Assert.ThrowsException<ArgumentException>(() => new TypeCastConverter<int>().Convert("hello"));

        }

    }

}