using System;
using System.Collections.Generic;

namespace UParse.Tests
{
    public class TestData
    {
        public int[] NumberArray1 { get; set; } = {1};
        public int Number { get; set; } = 1;
        public Array NumberArray2 { get; set; } = new[] {1, 2};

        public List<string> StringList { get; set; } = new List<string>
        {
            "hello"
        };

        public bool IsBool { get; set; } = true;
    }
}