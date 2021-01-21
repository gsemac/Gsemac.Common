using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class CaseConverterTests {

        // ToProperCase

        [TestMethod]
        public void TestToProperCaseWithMixedCase() {

            Assert.AreEqual("My Title", CaseConverter.ToProperCase("mY tItLe"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCase() {

            Assert.AreEqual("My Title", CaseConverter.ToProperCase("MY TITLE"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCaseAndPreserveAcronymsOption() {

            Assert.AreEqual("MY TITLE", CaseConverter.ToProperCase("MY TITLE", CasingOptions.PreserveAcronyms));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumerals() {

            Assert.AreEqual("James III Of Scotland", CaseConverter.ToProperCase("james iii of scotland", CasingOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumeralsInsideOfWord() {

            Assert.AreEqual("The Liver Is An Organ", CaseConverter.ToProperCase("the liver is an organ", CasingOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithPossessiveS() {

            Assert.AreEqual("John's House", CaseConverter.ToProperCase("john's house"));

        }

        // ToSentenceCase

        [TestMethod]
        public void TestToSentenceCaseWithMultipleSentences() {

            Assert.AreEqual("Hello! welcome to my test case.", CaseConverter.ToSentenceCase("Hello! welcome to my test case."));

        }
        [TestMethod]
        public void TestToSentenceCaseWithLeadingWhitespace() {

            Assert.AreEqual("    Hello!", CaseConverter.ToSentenceCase("    hello!"));

        }
        [TestMethod]
        public void TestToSentenceCaseWithMultipleSentencesAndMultiSentenceOption() {

            Assert.AreEqual("Hello! Welcome to my test case.", CaseConverter.ToSentenceCase("hello! welcome to my test case.",
                SentenceCasingOptions.DetectMultipleSentences));

        }
        [TestMethod]
        public void TestToSentenceCaseWithPunctuationWithoutFollowingWhitespace() {

            Assert.AreEqual("Hello! Please see the attatched archive.Zip.", CaseConverter.ToSentenceCase("hello! please see the attatched archive.zip.",
                SentenceCasingOptions.DetectMultipleSentences));

        }
        [TestMethod]
        public void TestToSentenceCaseWithPunctuationWithoutFollowingWhitespaceWithRequireWhitespaceAfterPunctuationOption() {

            Assert.AreEqual("Hello! Please see the attatched archive.zip.", CaseConverter.ToSentenceCase("hello! please see the attatched archive.zip.",
                SentenceCasingOptions.DetectMultipleSentences | SentenceCasingOptions.RequireWhitespaceAfterPunctuation));

        }

    }

}