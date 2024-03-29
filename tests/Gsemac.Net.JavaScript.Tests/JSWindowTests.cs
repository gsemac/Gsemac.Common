﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.JavaScript.Tests {

    [TestClass]
    public class JSWindowTests {

        [TestMethod]
        public void TestBtoaWithAsciiString() {

            Assert.AreEqual("aGVsbG8gd29ybGQ=", new JSWindow().Btoa("hello world"));

        }
        [TestMethod]
        public void TestAtobWithAsciiString() {

            Assert.AreEqual("hello world", new JSWindow().Atob("aGVsbG8gd29ybGQ="));

        }

    }

}