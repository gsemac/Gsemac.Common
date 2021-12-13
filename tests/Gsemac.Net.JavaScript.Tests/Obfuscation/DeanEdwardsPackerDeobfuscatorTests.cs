using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Gsemac.Net.JavaScript.Tests.Properties;

namespace Gsemac.Net.JavaScript.Obfuscation.Tests {

    [TestClass]
    public class DeanEdwardsPackerDeobfuscatorTests {

        [TestMethod]
        public void TestDeobfuscateWithPackedJs() {

            string sampleFilePath = Path.Combine(SamplePaths.DeanEdwardsPackerSamplesDirectoryPath, "Packed.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithAlternativeArgumentNames() {

            string sampleFilePath = Path.Combine(SamplePaths.DeanEdwardsPackerSamplesDirectoryPath, "PackedWithAlternativeArgumentNames.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithExtraWhiteSpace() {

            string sampleFilePath = Path.Combine(SamplePaths.DeanEdwardsPackerSamplesDirectoryPath, "PackedWithExtraWhiteSpace.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }
        [TestMethod]
        public void TestDeobfuscateWithPackedJsWithFormatting() {

            string sampleFilePath = Path.Combine(SamplePaths.DeanEdwardsPackerSamplesDirectoryPath, "PackedWithFormatting.js");

            Assert.AreEqual("alert(\"hello, world!\");",
                new DeanEdwardsPackerDeobfuscator().Deobfuscate(File.ReadAllText(sampleFilePath)));

        }

    }

}