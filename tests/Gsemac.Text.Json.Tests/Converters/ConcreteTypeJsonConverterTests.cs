using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Gsemac.Text.Json.Converters.Tests {

    [TestClass]
    public class ConcreteTypeJsonConverterTests {

        [TestMethod]
        public void TestDeserializeInterfaceUsingConcreteTypeConverter() {

            TestClass obj = new() {
                Field = "hello world",
            };

            string serialized = JsonConvert.SerializeObject(obj);

            ITestInterface deserialized = JsonConvert.DeserializeObject<ITestInterface>(serialized);

            Assert.AreEqual(obj.Field, deserialized.Field);

        }

        // Test classes

        [JsonConverter(typeof(ConcreteTypeJsonConverter<TestClass>))]
        internal interface ITestInterface {

            string Field { get; set; }

        }

        [JsonConverter(typeof(NoJsonConverter))]
        internal sealed class TestClass :
            ITestInterface {

            public string Field { get; set; }

        }

    }

}