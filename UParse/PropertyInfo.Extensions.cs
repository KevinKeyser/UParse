using System.Collections;
using System.Reflection;

namespace UParse
{
    public static class PropertyInfoExtensions
    {
        public static IEnumerable IndexerToEnumerable(this PropertyInfo propertyInfo, object obj)
        {
            var count = 0;
            while (true)
            {
                object value;
                try
                {
                    value = propertyInfo.GetValue(obj, new object[] {count});
                    count++;
                }
                catch (TargetInvocationException)
                {
                    break;
                }

                yield return value;
            }
        }
    }
}