using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;
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
            var json = converter.Serialize(data);
            Log(json);
            data = converter.Deserialize(json, data.GetType()) as TestData;
            json = converter.Serialize(data);
            Log(json);
        }
    }
}