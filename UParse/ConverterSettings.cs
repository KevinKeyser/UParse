using System;
using System.Collections.Generic;
using System.Linq;

namespace UParse
{
    public static class ConverterSettings
    {
        private static readonly Dictionary<Type, List<ConversionObjectInfo>> typeDefinitions =
            new Dictionary<Type, List<ConversionObjectInfo>>();

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