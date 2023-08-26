using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class CaseConverterTests {

        // ToProperCase

        [TestMethod]
        public void TestToProperCaseWithMixedCase() {

            Assert.AreEqual("My Title", CaseConverter.ToProper("mY tItLe"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCase() {

            Assert.AreEqual("My Title", CaseConverter.ToProper("MY TITLE"));

        }
        [TestMethod]
        public void TestToProperCaseWithUpperCaseAndPreserveAcronymsOption() {

            Assert.AreEqual("MY TITLE", CaseConverter.ToProper("MY TITLE", CasingOptions.PreserveAcronyms));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumerals() {

            Assert.AreEqual("James III Of Scotland", CaseConverter.ToProper("james iii of scotland", CasingOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithRomanNumeralsInsideOfWord() {

            Assert.AreEqual("The Liver Is An Organ", CaseConverter.ToProper("the liver is an organ", CasingOptions.CapitalizeRomanNumerals));

        }
        [TestMethod]
        public void TestToProperCaseWithPossessiveS() {

            Assert.AreEqual("John's House", CaseConverter.ToProper("john's house"));

        }

        // ToSentenceCase

        [TestMethod]
        public void TestToSentenceCaseWithMultipleSentences() {

            Assert.AreEqual("Hello! welcome to my test case.", CaseConverter.ToSentence("Hello! welcome to my test case."));

        }
        [TestMethod]
        public void TestToSentenceCaseWithLeadingWhitespace() {

            Assert.AreEqual("    Hello!", CaseConverter.ToSentence("    hello!"));

        }
        [TestMethod]
        public void TestToSentenceCaseWithMultipleSentencesAndMultiSentenceOption() {

            Assert.AreEqual("Hello! Welcome to my test case.", CaseConverter.ToSentence("hello! welcome to my test case.",
                SentenceCaseOptions.DetectMultipleSentences));

        }
        [TestMethod]
        public void TestToSentenceCaseWithPunctuationWithoutFollowingWhitespace() {

            Assert.AreEqual("Hello! Please see the attatched archive.Zip.", CaseConverter.ToSentence("hello! please see the attatched archive.zip.",
                SentenceCaseOptions.DetectMultipleSentences));

        }
        [TestMethod]
        public void TestToSentenceCaseWithPunctuationWithoutFollowingWhitespaceWithRequireWhitespaceAfterPunctuationOption() {

            Assert.AreEqual("Hello! Please see the attatched archive.zip.", CaseConverter.ToSentence("hello! please see the attatched archive.zip.",
                SentenceCaseOptions.DetectMultipleSentences | SentenceCaseOptions.RequireWhitespaceAfterPunctuation));

        }

    }

}