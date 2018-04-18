using System;
using System.Collections.Generic;
using System.Text;

namespace UParse
{
    public abstract class Converter : IConverter
    {
        public string Serialize(object obj)
        {
            return "";
        }

        public object Deserialize(string serializedObject, Type type)
        {
            return null;
        }

        public abstract void WriteStartObject();
        public abstract void WriteEndObject();


        public abstract void WriteStartArray();
        public abstract void WriteEndArray();


        public abstract void WritePropertyName(string name);
    }
}
