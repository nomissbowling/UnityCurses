using System;

namespace Assets.Engine.Utility
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     Provides an instance TryParse method for .Net's System.Enum class, analogous to the non-exception-throwing TryParse
        ///     methods that other .Net built-in types provide. As opposed to those, though, this extension method is an instance
        ///     method, due to the very nature of C# extension methods themselves.
        /// </summary>
        public static bool TryParse<T>(string strType, out T result)
        {
#pragma warning disable IDE0018 // Inline variable declaration
            int number;
#pragma warning restore IDE0018 // Inline variable declaration

            if (int.TryParse(strType, out number) && Enum.IsDefined(typeof(T), number))
            {
                result = (T) Enum.ToObject(typeof(T), number);
                return true;
            }

            if (Enum.IsDefined(typeof(T), strType))
            {
                result = (T) Enum.Parse(typeof(T), strType);
                return true;
            }

            result = default(T);
            return false;
        }
    }
}