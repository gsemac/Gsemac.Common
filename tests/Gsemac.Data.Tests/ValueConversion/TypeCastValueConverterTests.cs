using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class TypeCastValueConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertWithValidConversion() {

            Assert.AreEqual(5, new TypeCastValueConverter<int>().Convert(5.2));

        }
        [TestMethod]
        public void TestConvertWithInvalidConversionThrowsException() {

            Assert.ThrowsException<ArgumentException>(() => new TypeCastValueConverter<int>().Convert("hello"));

        }

    }

}