using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SimpleJSON;

namespace UParse
{
    public class JsonConverter : IConverter
    {
        public string Serialize(object obj)
        {
            return ToJson(obj);
        }

        public object Deserialize(string serializedObject, Type type)
        {
            var json = JSON.Parse(serializedObject);

            var obj = Activator.CreateInstance(type);

            var definitions = ConverterSettings.GetTypeDefinition(type);

            foreach (var definition in definitions)
            {
                var jsonnode = json[definition.MemberInfo.Name];

                if (jsonnode.Value == null)
                {
                    continue;
                }

                var value = GetValue(jsonnode, definition.UnderlyingType);

                definition.SetValue(obj, value);
            }

            return obj;
        }

        private string ToJson(object obj)
        {
            var stringBuilder = new StringBuilder();
            var ConversionObjectType = ConversionObjectTypeExtensions.GetConversionObjectType(obj.GetType());
            switch (ConversionObjectType)
            {
                case ConversionObjectType.Array:
                    stringBuilder.Append("[");
                    var array = (IEnumerable) obj;
                    
                    var firstArray = true;
                    foreach (var item in array)
                    {
                        if (!firstArray)
                        {
                            stringBuilder.Append(",");
                        }

                        stringBuilder.Append(ToJson(item));
                        firstArray = false;
                    }

                    stringBuilder.Append("]");
                    break;
                case ConversionObjectType.Object:
                    stringBuilder.Append("{");
                    var properties = obj.GetType().GetProperties();
                    var first = true;
                    foreach (var propertyInfo in properties)
                    {
                        if (propertyInfo.SetMethod == null ||
                            propertyInfo.GetMethod == null)
                        {
                            continue;
                        }

                        if (!first)
                        {
                            stringBuilder.Append(",");
                        }

                        stringBuilder.Append($"{propertyInfo.Name}:");
                        if (propertyInfo.GetIndexParameters().Length > 0)
                        {
                            stringBuilder.Append(ToJson(propertyInfo.IndexerToEnumerable(obj)));
                            continue;
                        }


                        stringBuilder.Append(ToJson(propertyInfo.GetValue(obj)));
                        first = false;
                    }

                    stringBuilder.Append("}");
                    break;
                case ConversionObjectType.String:
                    stringBuilder.Append($"\"{obj}\"");
                    break;
                default:
                    stringBuilder.Append(obj);
                    break;
            }

            return stringBuilder.ToString();
        }

        private object GetValue(JSONNode node, Type type)
        {
            if (type == typeof(string) || type == typeof(char))
            {
                return node.Value;
            }

            Type enumerableType = type.GetEnumerableType();
            if (enumerableType != null)
            {
                return GetArray(node.AsArray, type, enumerableType);
            }

            if (type.IsNumber())
            {
                return Convert.ChangeType(node.Value, type);
            }

            if (type == typeof(bool))
            {
                return Boolean.Parse(node.Value);
            }

            if (!type.IsPrimitive)
            {
                return Deserialize(node.Value, type);
            }

            return null;
        }
        
        private IEnumerable GetArray(JSONArray jsonArray, Type arrayType, Type enumerableType)
        {
            IList list;
            if (arrayType.IsArray || arrayType == typeof(Array))
            {
                list = Array.CreateInstance(enumerableType, jsonArray.Count);
            }
            else
            {
                list = (IList)Activator.CreateInstance(arrayType);
            }

            for (var i = 0; i < jsonArray.Count; i++)
            {
                if (list.IsFixedSize)
                {
                    list[i] = GetValue(jsonArray[i], enumerableType);
                    continue;
                }
                list.Add(GetValue(jsonArray[i], enumerableType));
            }
            return list;
        }

        
        #region No Schema Parsing
        private IEnumerable GetArrayFromJson(JSONArray jsonArray)
        {
            Type type = GetJsonArrayType(jsonArray);
            return GetArray(jsonArray, typeof(Array), type);
        }
        
        private Type GetJsonArrayType(JSONArray jsonArray)
        {
            Type type = null;
            if (jsonArray.Count > 0)
            {
                type = GetValueFromJson(jsonArray[0]).GetType();
            }

            for (var i = 1; i < jsonArray.Count; i++)
            {
                type = type.FindAssignableWith(GetValueFromJson(jsonArray[i]).GetType());
            }

            return type;
        }

        private object GetValueFromJson(JSONNode node)
        {
            var jsonArray = node.AsArray;
            if (jsonArray != null)
            {
                var array = GetArrayFromJson(jsonArray);
                /*for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue(GetValueFromJson(jsonArray[i]), i);
                }*/

                return array;
            }

            if (!node.Value.Contains("."))
            {
                bool boolean;
                if (bool.TryParse(node.Value, out boolean))
                {
                    return boolean;
                }

                short int16;
                if (short.TryParse(node.Value, out int16))
                {
                    return int16;
                }

                int int32;
                if (int.TryParse(node.Value, out int32))
                {
                    return int32;
                }

                long int64;
                if (long.TryParse(node.Value, out int64))
                {
                    return int64;
                }
            }

            float floatingPoint;
            if (float.TryParse(node.Value, out floatingPoint))
            {
                return floatingPoint;
            }

            double doublePrecision;
            if (double.TryParse(node.Value, out doublePrecision))
            {
                return doublePrecision;
            }

            return node.Value;
        }

        private ConversionObjectType GetConversionObjectType(Type type)
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
        }
        #endregion
    }
}