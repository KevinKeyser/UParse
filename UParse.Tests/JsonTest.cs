using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UParse.Tests
{
    [TestClass]
    public class JsonTest : BaseTest
    {
        [TestMethod]
        public void Serialize()
        {
            var data = new TestData();
            var converter = new JsonConverter();
            Log(converter.Serialize(data));
        }

        [TestMethod]
        public void Deserialize()
        {
            var data = new TestData();
            var converter = new JsonConverter();
            converter.Deserialize(converter.Serialize(data), data.GetType());
        }
    }
}