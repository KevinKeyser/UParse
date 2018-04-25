using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UParse
{
    public class CsvConverter : IConverter
    {
        public string Serialize(object obj)
        {
            string headers = ToCsvHeaders(obj);
            return headers + Environment.NewLine + ToCsv(obj);
        }

        public object Deserialize(string serializedObject, Type type)
        {
            return null;
        }

        public string ToCsvHeaders(object obj, string prefix = "", string suffix = "")
        {
            string delimiter = ",";
            var headers = new List<string>();
            var conversionObjectType = obj.GetType().GetConversionObjectType();
            switch (conversionObjectType)
            {
                case ConversionObjectType.Array:
                    var array = (IEnumerable) obj;

                    int count = 0;
                    foreach (var item in array)
                    {
                        var itemHeaders = ToCsvHeaders(item, prefix: prefix, suffix: $"[{count++}]");
                        headers.Add(itemHeaders);
                    }
                    break;
                case ConversionObjectType.Object:
                    var innerConversionObjects = ConverterSettings.GetTypeDefinition(obj.GetType());

                    string extendedPrefix = String.IsNullOrWhiteSpace(prefix) ? prefix : $"{prefix}.";
                    foreach (var innerConversionObject in innerConversionObjects)
                    {
                        headers.Add(ToCsvHeaders(innerConversionObject.GetValue(obj),
                            prefix: $"{extendedPrefix}{innerConversionObject.Name}{suffix}"));
                    }
                    break;
                default:
                    var header = prefix + suffix;
                    if (!String.IsNullOrWhiteSpace(header))
                    {
                        headers.Add(header);
                    }
                    break;
            }

            return String.Join(delimiter, headers);
        }
        
        private string ToCsv(object obj)
        {
            string delimiter = ",";
            var values = new List<string>();
            var conversionObjectType = obj.GetType().GetConversionObjectType();
            switch (conversionObjectType)
            {
                case ConversionObjectType.Array:
                    var array = (IEnumerable) obj;
                    
                    foreach (var item in array)
                    {
                        values.Add(ToCsv(item));
                    }
                    break;
                case ConversionObjectType.Object:
                    var innerConversionObjects = ConverterSettings.GetTypeDefinition(obj.GetType());
                    
                    foreach (var innerConversionObject in innerConversionObjects)
                    {
                        values.Add(ToCsv(innerConversionObject.GetValue(obj)));
                    }
                    break;
                default:
                    values.Add(obj.ToString());
                    break;
            }

            return String.Join(delimiter, values);
        }
    }
}