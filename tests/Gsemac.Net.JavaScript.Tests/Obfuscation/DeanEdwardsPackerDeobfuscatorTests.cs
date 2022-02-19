using Gsemac.Net.JavaScript.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.Net.JavaScript.Obfuscation.Tests {

    [TestClass]
    public class DeanEdwardsPackerDeobfuscatorTests {

        [TestMethod]
        public void TestDeobfuscateWithPackedJs() {

            string sampleFilePath = Path.Combine(SamplePaths.JavaScriptSamplesDirectoryPath, "PackedWithDeanEdwardsPacker.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithAlternativeArgumentNames() {

            string sampleFilePath = Path.Combine(SamplePaths.JavaScriptSamplesDirectoryPath, "PackedWithDeanEdwardsPackerWithAlternativeArgumentNames.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithExtraWhiteSpace() {

            string sampleFilePath = Path.Combine(SamplePaths.JavaScriptSamplesDirectoryPath, "PackedWithDeanEdwardsPackerWithExtraWhiteSpace.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithFormatting() {

            string sampleFilePath = Path.Combine(SamplePaths.JavaScriptSamplesDirectoryPath, "PackedWithDeanEdwardsPackerWithFormatting.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }

    }

}