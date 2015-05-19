using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamlPreprocessor;

namespace TestProject
{
    [TestClass]
    public class DirectivesProcTest
    {
        [TestMethod]
        public void TestExtractAttributeValue_1()
        {
            string tested = "Text=\"version wp7\"";
            string expected = "version wp7";
            string result = DirectiveATTR_ADD.ExtractAttributeValue(tested);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestExtractAttributeValue_2()
        {
            string tested = "Foreground=\"{Binding MoreInfoColor, ElementName=MaPage}\"";
            string expected = "{Binding MoreInfoColor, ElementName=MaPage}";
            string result = DirectiveATTR_ADD.ExtractAttributeValue(tested);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestExtractAttributeValue_3()
        {
            string tested = "test:Foreground=\"{Binding MoreInfoColor, ElementName=MaPage}\"";
            string expected = "{Binding MoreInfoColor, ElementName=MaPage}";
            string result = DirectiveATTR_ADD.ExtractAttributeValue(tested);
            Assert.AreEqual(expected, result);
        }

    }
}
