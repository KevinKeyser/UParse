using System;
using System.Collections.Generic;

namespace UParse.Tests
{
    public class TestData
    {
        public int[] NumberArray1 { get; set; } = {1,3};
        //public int Number { get; set; } = 1;
        
        //public List<string> StringList { get; set; } = new List<string>
        //{
        //    "hello"
        //};

        //public bool IsBool { get; set; } = true;
        
        //TODO: Need To Add Conditions
        
        //Non Generic Object Array
        public Array NumberArray2 { get; set; } = new[] {1, 2};
        
        //ICollection (Non IList)
        //public Dictionary<string, int> Dict { get; set; } = new Dictionary<string, int>() { { "1", 1 } };
    }
}