using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class Base64Tests {

        // IsBase64String

        [TestMethod]
        public void TestIsBase64WithPaddedBase64String() {

            Assert.IsTrue(Base64.IsBase64String("aGVsbG8gd29ybGQ="));

        }
        [TestMethod]
        public void TestIsBase64WithUnpaddedBase64String() {

            Assert.IsTrue(Base64.IsBase64String("aGVsbG8gd29ybGQ"));

        }
        [TestMethod]
        public void TestIsBase64WithUppercaseBase64String() {

            Assert.IsTrue(Base64.IsBase64String("AGVSBG8GD29YBGQ="));

        }
        [TestMethod]
        public void TestIsBase64WithExcessivelyPaddedBase64String() {

            Assert.IsFalse(Base64.IsBase64String("aGVsbG8gd29ybGQ======"));

        }
        [TestMethod]
        public void TestIsBase64WithInvalidBase64String() {

            Assert.IsFalse(Base64.IsBase64String("aGVsbG8gd29ybGQ{"));

        }
        [TestMethod]
        public void TestIsBase64WithEmptyString() {

            Assert.IsFalse(Base64.IsBase64String(string.Empty));

        }

    }

}