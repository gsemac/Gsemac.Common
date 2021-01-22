using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class PropertyDictionaryTests {

        // this[index]

        [TestMethod]
        public void TestSetPropertyValueWithClassWithRecursiveProperties() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), PropertyDictionaryOptions.IncludeNestedProperties);

            dict["A.X"] = 5;

            Assert.AreEqual(5, dict["A.X"]);

        }
        [TestMethod]
        public void TestUpdateObjectPropertyValue() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), PropertyDictionaryOptions.IncludeNestedProperties);

            dict["B"] = new ClassWithRecursiveProperties() {
                A = new ClassWithProperties() {
                    X = 5
                }
            };

            Assert.AreEqual(5, dict["B.A.X"]);

        }

        // ContainsKey

        [TestMethod]
        public void TestContainsKeyWithClassWithRecursiveProperties() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), PropertyDictionaryOptions.IncludeNestedProperties);

            Assert.IsTrue(dict.ContainsKey("B.B.B"));

        }


    }

}