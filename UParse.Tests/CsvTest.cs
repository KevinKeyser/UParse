using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UParse.Tests
{
    [TestClass]
    public class CsvTest : BaseTest
    {
        private CsvConverter converter;
        private TestData testData;

        [TestInitialize]
        public void Initialize()
        {
            converter = new CsvConverter();
            testData = new TestData();
        }
        
        [TestMethod]
        public void GetHeaders()
        {
            Log(converter.ToCsvHeaders(testData));
        }
        
        [TestMethod]
        public void Serialize()
        {
            Log(converter.Serialize(testData));
        }

        [TestMethod]
        public void Deserialize()
        {
            var serializedData = converter.Serialize(testData);
            Log(serializedData);
            testData = converter.Deserialize(serializedData, testData.GetType()) as TestData;
            serializedData = converter.Serialize(testData);
            Log(serializedData);
        }
    }
}