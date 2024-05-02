namespace ProjectTests
{
    public class Tests
    {
        [SetUp]
        public void Setup() {}

        [Test]
        public void IsXmlFileTest()
        {
            string fileType = ".xml";

            Assert.AreEqual(true, StringCheck.IsXmlFile(fileType));
        }

        [Test]
        public void IsValidNameTest()
        {
            string name = "f2).f";

            Assert.AreEqual(false, StringCheck.IsValidName(name));
        }

        [Test]
        public void ElementWithoutSymbolsTest()
        {
            string element = "<value>";
            bool isXml = true;

            Assert.AreEqual("value", StringCheck.ElementWithoutSymbols(isXml, element));
        }
    }
}