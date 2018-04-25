using System;
using System.Collections;

namespace UParse
{
    public enum ConversionObjectType
    {
        Null = 0,
        Array = 1,
        Object = 2,
        String = 3,
        Number = 4,
        Boolean = 5
    }
    
    public static class ConversionObjectTypeExtensions
    {
        public static ConversionObjectType GetConversionObjectType(this Type type)
        {
            if (type == typeof(string) || type == typeof(char))
            {
                return ConversionObjectType.String;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return ConversionObjectType.Array;
            }

            if (type.IsNumber())
            {
                return ConversionObjectType.Number;
            }

            if (type == typeof(bool))
            {
                return ConversionObjectType.Boolean;
            }

            if (!type.IsPrimitive)
            {
                return ConversionObjectType.Object;
            }

            return ConversionObjectType.Null;
        }}
}