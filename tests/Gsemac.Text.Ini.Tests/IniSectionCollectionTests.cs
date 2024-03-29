﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniSectionCollectionTests {

        // this[]

        [TestMethod]
        public void TestGetSectionWithInvalidSectionNameReturnsObject() {

            // Attempting to get a section using an invalid name should return a transient section object.

            IIniSection section = new IniSectionCollection()[string.Empty];

            Assert.IsTrue(section is object);

        }
        [TestMethod]
        public void TestGetSectionWithNullSectionNameReturnsObject() {

            IIniSection section = new IniSectionCollection()[null];

            Assert.IsTrue(section is object);

        }
        [TestMethod]
        public void TestGetSectionWithEmptyAndNullSectionNamesReturnsSameObject() {

            IIniSectionCollection items = new IniSectionCollection();

            IIniSection section1 = items[string.Empty];
            IIniSection section2 = items[null];

            Assert.IsTrue(ReferenceEquals(section1, section2));

        }

        [TestMethod]
        public void TestGetTransientSectionDoesNotAddSectionToCollection() {

            IIniSectionCollection items = new IniSectionCollection();

            // Accessing a section that doesn't exist in the collection should create a "transient" section.
            // These sections functionally don't appear in the collection until items are added to them.
            // This is to permit the items["section"]["key"] syntax even in the case the section doesn't already exist.

            IIniSection transientSection = items["section_name"];

            Assert.AreEqual(0, items.Count);

            Assert.IsFalse(items.Contains(transientSection.Name));
            Assert.IsFalse(items.Contains(transientSection));

        }
        [TestMethod]
        public void TestAddPropertyToTransientSectionAddsSectionToCollection() {

            IIniSectionCollection items = new IniSectionCollection();

            // Adding a property to a transient section should cause it to no longer be transient.

            items["section"]["key"] = "value";

            Assert.AreEqual(1, items.Count);

            Assert.IsTrue(items["section"] is object);

            Assert.IsTrue(items.Contains("section"));

        }

        [TestMethod]
        public void TestGettingMissingPropertyIsNotNull() {

            IIniSectionCollection items = new IniSectionCollection();

            string value = items["section"]["key"];

            Assert.AreEqual(string.Empty, value);

        }

        // Contains

        [TestMethod]
        public void TestLookupIsCaseInsensitiveByDefault() {

            IIniSectionCollection items = new IniSectionCollection {
                "section",
            };

            Assert.IsTrue(items.Contains("section"));
            Assert.IsTrue(items.Contains("SECTION"));

        }
        [TestMethod]
        public void TestLookupWithCaseSensitiveKeyComparer() {

            IIniSectionCollection items = new IniSectionCollection(StringComparer.Ordinal) {
                "section",
            };

            Assert.IsTrue(items.Contains("section"));
            Assert.IsFalse(items.Contains("SECTION"));

        }

    }

}