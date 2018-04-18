using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UParse
{
    public static class TypeExtensions
    {
        public static Type GetEnumerableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.HasElementType)
            {
                return type.GetElementType();
            }

            if (type.IsAssignableToGenericType(typeof(IEnumerable<>)))
            {
                return type.GetGenericArguments()[0];
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return typeof(object);
            }

            return null;
        }
        
        public static bool IsAssignableToGenericType(this Type type, Type genericType)
        {
            var interfaceTypes = type.GetInterfaces();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;
            
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            Type baseType = type.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
        
        public static bool IsNumber(this Type type)
        {
            return type == typeof(sbyte)
                   || type == typeof(byte)
                   || type == typeof(short)
                   || type == typeof(ushort)
                   || type == typeof(int)
                   || type == typeof(uint)
                   || type == typeof(long)
                   || type == typeof(ulong)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(decimal);
        }
        
    #region https://stackoverflow.com/questions/14107683/how-to-find-the-smallest-assignable-type-in-two-types-duplicate

        /// <summary>
        ///     Finds a common base class or implemented interface
        /// </summary>
        /// <returns>Common Type</returns>
        public static Type FindAssignableWith(this Type type1, Type type2)
        {
            if (type1 == null || type2 == null)
            {
                return null;
            }

            var commonBaseClass = type1.FindBaseClassWith(type2) ?? typeof(object);

            return commonBaseClass.Equals(typeof(object))
                ? type1.FindInterfaceWith(type2)
                : commonBaseClass;
        }

        /// <summary>
        ///     Searches for common base class (either concrete or abstract)
        /// </summary>
        /// <param name="type1">First Type to compare</param>
        /// <param name="type2">Second Type to compare</param>
        /// <returns>Common base type</returns>
        public static Type FindBaseClassWith(this Type type1, Type type2)
        {
            if (type1 == null || type2 == null)
            {
                return null;
            }

            return type1
                .GetClassHierarchy()
                .Intersect(type2.GetClassHierarchy())
                .FirstOrDefault(type => !type.IsInterface);
        }


        /// <summary>
        ///     Searches for common implemented interface
        ///     It's possible for one class to implement multiple interfaces, in this case return first common based interface
        /// </summary>
        /// <param name="type1">First Type to compare</param>
        /// <param name="type2">Second Type to compare</param>
        /// <returns>Common base type</returns>
        public static Type FindInterfaceWith(this Type type1, Type type2)
        {
            if (type1 == null || type2 == null)
            {
                return null;
            }

            return type1
                .GetInterfaceHierarchy()
                .Intersect(type2.GetInterfaceHierarchy())
                .FirstOrDefault();
        }

        /// <summary>
        ///     Iterates through interface hierarhy
        /// </summary>
        /// <param name="type">Type to get interface hierarchy from</param>
        /// <returns>Interface hierarchy</returns>
        public static IEnumerable<Type> GetInterfaceHierarchy(this Type type)
        {
            if (type.IsInterface)
            {
                return new[] {type}.AsEnumerable();
            }

            return type
                .GetInterfaces()
                .OrderByDescending(current => current.GetInterfaces().Count())
                .AsEnumerable();
        }

        /// <summary>
        ///     Iterates through class hierarhy
        /// </summary>
        /// <param name="type">Type to get class hierarchy from</param>
        /// <returns>Class hierarchy</returns>
        public static IEnumerable<Type> GetClassHierarchy(this Type type)
        {
            if (type == null)
            {
                yield break;
            }

            var typeInHierarchy = type;

            do
            {
                yield return typeInHierarchy;
                typeInHierarchy = typeInHierarchy.BaseType;
            }
            while (typeInHierarchy != null && !typeInHierarchy.IsInterface);
        }

    #endregion
    }
}