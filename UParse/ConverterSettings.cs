using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UParse
{
    public static class ConverterSettings
    {
        private static readonly Dictionary<Type, List<ConversionObjectInfo>> typeDefinitions =
            new Dictionary<Type, List<ConversionObjectInfo>>();

        private static readonly Dictionary<Type, ConversionObjectInfo> conversionInformation = 
            new Dictionary<Type, ConversionObjectInfo>();

        private static readonly Dictionary<Type, ConversionObjectNode> conversionNodes =
            new Dictionary<Type, ConversionObjectNode>();


        internal static List<ConversionObjectInfo> GetTypeDefinition(Type type)
        {
            List<ConversionObjectInfo> definition;
            if (typeDefinitions.TryGetValue(type, out definition))
            {
                return definition;
            }

            definition = Deserialize(type);
            typeDefinitions.Add(type, definition);

            return definition;
        }

        private static List<ConversionObjectInfo> Deserialize(Type type)
        {
            var definition = new List<ConversionObjectInfo>();

            definition.AddRange(type.GetProperties().Select(memberInfo => (ConversionObjectInfo) memberInfo));

            return definition;
        }
    }
}