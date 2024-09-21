using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class PropertyDictionaryTests {

        // this[index]

        [TestMethod]
        public void TestSetPropertyValueWithClassWithRecursiveProperties() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), new PropertyDictionaryOptions {
                IncludeNestedProperties = true,
            });

            dict["A.X"] = 5;

            Assert.AreEqual(5, dict["A.X"]);

        }
        [TestMethod]
        public void TestSetPropertyValueWithCastRequired() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithProperties());

            Assert.IsTrue(dict.TrySetValue("Option", "Item3"));
            Assert.AreEqual(TestEnum.Item3, dict["Option"]);

        }
        [TestMethod]
        public void TestUpdateObjectPropertyValue() {

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), new PropertyDictionaryOptions {
                IncludeNestedProperties = true,
            });

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

            IPropertyDictionary dict = new PropertyDictionary(new ClassWithRecursiveProperties(), new PropertyDictionaryOptions {
                IncludeNestedProperties = true,
            });

            Assert.IsTrue(dict.ContainsKey("B.B.B"));

        }


    }

}