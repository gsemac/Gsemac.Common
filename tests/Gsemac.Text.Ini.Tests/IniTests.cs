﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniTests {

        // Ini()

        [TestMethod]
        public void TestConstructorRespectsKeyComparerOption() {

            IIni ini1 = new Ini(new IniOptions() {
                KeyComparer = StringComparer.InvariantCulture,
            });

            IIni ini2 = new Ini(new IniOptions() {
                KeyComparer = StringComparer.InvariantCultureIgnoreCase,
            });

            foreach (IIni ini in new[] { ini1, ini2 })
                ini.SetValue("key", "value");

            Assert.IsTrue(ini1.ContainsKey("key"));
            Assert.IsFalse(ini1.ContainsKey("KEY"));

            Assert.IsTrue(ini2.ContainsKey("key"));
            Assert.IsTrue(ini2.ContainsKey("KEY"));

        }

    }

}