using System;
using System.Collections;
using System.Text;

namespace UParse
{
    public class CsvConverter : IConverter
    {
        public string Serialize(object obj)
        {
            return ToCsv(obj);
        }

        public object Deserialize(string serializedObject, Type type)
        {
            return null;
        }
        
        private string ToCsv(object obj)
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

                        stringBuilder.Append(ToCsv(item));
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
                            stringBuilder.Append(ToCsv(propertyInfo.IndexerToEnumerable(obj)));
                            continue;
                        }


                        stringBuilder.Append(ToCsv(propertyInfo.GetValue(obj)));
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
    }
}