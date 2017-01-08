// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@4:49 AM

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.ProjectCommon.Utility
{
    /// <summary>
    ///     Helper class that deals with activating classes without using the actual activator class because that requires and
    ///     empty parameterless constructor and we cannot always guarantee we will be able have one. Using these methods don't
    ///     require a constructor to be used and furthermore the use of expressions to generate them ensures caching so penalty
    ///     for type activation is only hit once on first instance creation.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///     FormatterServices.GetUninitializedObject(t) will fail for string. Hence special handling for string is
        ///     in place to return empty string.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>The <see cref="bool" />.</returns>
        private static bool HasDefaultConstructor(this Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        ///     Init expression is effectively cached and incurs penalty only the first time the type is loaded. Will handle
        ///     value types too in an efficient manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>http://stackoverflow.com/a/16162475</remarks>
        /// <example>MyType me = New`MyType`.Instance</example>
        public static class New<T>
        {
            /// <summary>
            ///     The instance.
            /// </summary>
            public static readonly Func<T> Instance = Creator();

            private static Func<Type, object> getUninitializedObject;

            /// <summary>
            ///     The creator.
            /// </summary>
            /// <returns>
            ///     The <see cref="Func" />.
            /// </returns>
            private static Func<T> Creator()
            {
                var t = typeof(T);
                if (t == typeof(string))
                    return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

                if (t.HasDefaultConstructor())
                    return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

                return () => (T) FormatterServices.GetUninitializedObject(t);
            }

            /// <summary>Creates a new instance of the specified type, bypassing the constructor.</summary>
            /// <param name="type">The type to create</param>
            /// <returns>The new instance</returns>
            /// <exception cref="NotSupportedException">If the platform does not support constructor-skipping</exception>
            /// <remarks>
            ///     Inspired by DCS:
            ///     https://github.com/dotnet/corefx/blob/c02d33b18398199f6acc17d375dab154e9a1df66/src/System.Private.DataContractSerialization/src/System/Runtime/Serialization/XmlFormatReaderGenerator.cs#L854-L894
            /// </remarks>
            public static object GetUninitializedObject(Type type)
            {
                //var obj = TryGetUninitializedObjectWithFormatterServices(type);
                return Activator.CreateInstance(type);
            }

            ///// <summary>Manually calls the private method which Microsoft has not marked as public yet.</summary>
            ///// <remarks> https://github.com/mgravell/protobuf-net/blob/master/protobuf-net/BclHelpers.cs#L35 </remarks>
            //internal static object TryGetUninitializedObjectWithFormatterServices(Type type)
            //{
            //    if (getUninitializedObject != null)
            //        return getUninitializedObject(type);

            //    try
            //    {
            //        var formatterServiceType = typeof(string).Assembly.GetType("System.Runtime.Serialization.FormatterServices");
            //        if (formatterServiceType != null)
            //        {
            //            var method = formatterServiceType.GetMethod("GetUninitializedObject", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            //            if (method != null)
            //            {
            //                //getUninitializedObject = (Func<Type, object>)method.CreateDelegate(typeof(Func<Type, object>));
            //                //Delegate.CreateDelegate(method, )
            //                var thing = Activator.CreateInstance(type);

            //            }
            //        }
            //    }
            //    catch
            //    {
            //        /* best efforts only */
            //    }

            //    if (getUninitializedObject == null)
            //        getUninitializedObject = x => null;
            //    return getUninitializedObject(type);
            //}
        }
    }
}