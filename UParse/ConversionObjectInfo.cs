using System;
using System.Reflection;

namespace UParse
{
    public class ConversionObjectInfo
    {
        public ConversionObjectInfo(MemberInfo memberInfo)
        {
            MemberInfo = memberInfo;
            UnderlyingType = memberInfo.GetUnderlyingType();
        }

        public Type UnderlyingType { get; }

        public MemberInfo MemberInfo { get; }

        public void SetValue(object obj, object value)
        {
            switch (MemberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo) MemberInfo).SetValue(obj, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo) MemberInfo).SetValue(obj, value);
                    break;
                case MemberTypes.Event:
                //return ((EventInfo)MemberInfo).AddEventHandler(obj, value);
                case MemberTypes.Method:
                //return ((MethodInfo)MemberInfo);
                default:
                    throw new ArgumentException
                    (
                        "MemberInfo must be if type FieldInfo or PropertyInfo"
                    );
            }
        }

        public void GetValue(object obj)
        {
            switch (MemberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo) MemberInfo).GetValue(obj);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo) MemberInfo).GetValue(obj);
                    break;
                case MemberTypes.Event:
                //return ((EventInfo)MemberInfo).AddEventHandler(obj, value);
                case MemberTypes.Method:
                //return ((MethodInfo)MemberInfo);
                default:
                    throw new ArgumentException
                    (
                        "MemberInfo must be if type FieldInfo or PropertyInfo"
                    );
            }
        }

        public static implicit operator ConversionObjectInfo(MemberInfo memberInfo)
        {
            return new ConversionObjectInfo(memberInfo);
        }
    }
}