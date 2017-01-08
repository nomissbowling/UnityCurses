﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@4:49 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Maxwolf.ProjectCommon.Utility
{
    /// <summary>
    ///     Meant for dealing with attributes and grabbing all the available classes of a given type with specified attribute
    ///     using generics.
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        ///     Find all the classes which have a custom attribute I've defined on them, and I want to be able to find them
        ///     on-the-fly when an application is using my library.
        /// </summary>
        /// <param name="inherit">The inherit.</param>
        /// <remarks>http://stackoverflow.com/a/720171</remarks>
        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var t in a.GetTypes())
                if (t.IsDefined(typeof(TAttribute), inherit))
                    yield return t;
        }

        /// <summary>Determine if a type implements a specific generic interface type.</summary>
        /// <param name="baseType">The base Type.</param>
        /// <param name="interfaceType">The interface Type.</param>
        /// <remarks>http://stackoverflow.com/a/503359</remarks>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool IsImplementationOf(this Type baseType, Type interfaceType)
        {
            return baseType.GetInterfaces().Any(interfaceType.Equals);
        }

        /// <summary>Find the fields in an enum that have a specific attribute with a specific value.</summary>
        /// <param name="source">The source.</param>
        /// <param name="inherit">The inherit.</param>
        public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider source, bool inherit)
            where T : Attribute
        {
            var attrs = source.GetCustomAttributes(typeof(T), inherit);
            return attrs != null ? (T[]) attrs : Enumerable.Empty<T>();
        }

        /// <summary>Extension method for enum's that helps with getting custom attributes off of enum values</summary>
        /// <param name="enumValue">The enum Value.</param>
        /// <returns>The <see cref="T" />.</returns>
        public static T GetEnumAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribs = field.GetCustomAttributes(typeof(T), false);
            var result = default(T);

            if (attribs.Length > 0)
                result = attribs[0] as T;

            return result;
        }

        /// <summary>Grabs first attribute from a given object and returns the first one in the enumeration.</summary>
        /// <typeparam name="T">Role of attribute that we should be looking for.</typeparam>
        /// <param name="value">Object that will have attribute tag specified in generic parameter..</param>
        /// <returns>Attribute of the specified type from inputted object.</returns>
        private static T GetAttribute<T>(this object value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var firstOrDefault = memberInfo.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var attributes = firstOrDefault.GetCustomAttributes(typeof(T), false);
                return (T) attributes.FirstOrDefault();
            }

            return null;
        }

        /// <summary>Attempts to grab description attribute from any object.</summary>
        /// <param name="value">Object that should have description attribute.</param>
        /// <returns>Description attribute text, if null then type name without name space.</returns>
        public static string ToDescriptionAttribute(this object value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
#pragma warning disable IDE0030 // Use null propagation
            return attribute == null ? value.ToString() : attribute.Description;
#pragma warning restore IDE0030 // Use null propagation
        }
    }
}