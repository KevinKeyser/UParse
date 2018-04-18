using System;

namespace UParse
{
    public interface IConverter
    {
        string Serialize(object obj);
        object Deserialize(string serializedObject, Type type);
    }
}