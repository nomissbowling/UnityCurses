// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.Scripts.Engine.Utility
{
    public static class CommonFunctions
    {
        /// <summary>
        ///     Generates a percentage, formatted with "places" decimal places.
        /// </summary>
        /// <param name="value">Value for which a percentage is needed.</param>
        /// <param name="total">Total from which to generate a percentage.</param>
        /// <param name="places">Decimal places to return in the percentage string.</param>
        /// <returns>string with the percentage value</returns>
        public static string GetPercentage(int value, int total, int places)
        {
            decimal percent;
            var retval = string.Empty;
            var strplaces = new string('0', places);

            if ((value == 0) || (total == 0))
            {
                percent = 0;
            }

            else
            {
                percent = decimal.Divide(value, total)*100;

                if (places > 0)
                    strplaces = "." + strplaces;
            }

            retval = percent.ToString("#" + strplaces);

            return retval;
        }

        /// <summary>
        ///     Strip illegal chars and reserved words from a candidate filename (should not include the directory path)
        /// </summary>
        /// <remarks> http://stackoverflow.com/questions/309485/c-sharp-sanitize-file-name </remarks>
        public static string GetValidFilename(string filename)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidReStr = string.Format(@"[{0}]+", invalidChars);

            var reservedWords = new[]
            {
                "CON", "PRN", "AUX", "CLOCK$", "NUL", "COM0", "COM1", "COM2", "COM3", "COM4",
                "COM5", "COM6", "COM7", "COM8", "COM9", "LPT0", "LPT1", "LPT2", "LPT3", "LPT4",
                "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
            };

            var sanitisedNamePart = Regex.Replace(filename, invalidReStr, "_");
            foreach (var reservedWord in reservedWords)
            {
                var reservedWordPattern = string.Format("^{0}\\.", reservedWord);
                sanitisedNamePart = Regex.Replace(sanitisedNamePart, reservedWordPattern, "_reservedWord_.",
                    RegexOptions.IgnoreCase);
            }

            // Removes any whitespace from the filename.
            return Regex.Replace(sanitisedNamePart, @"\s+", "_");
        }

        /// <summary>
        ///     encodes two ushorts into one signed integer
        /// </summary>
        public static int UShortsToInt(ushort high, ushort low)
        {
            return (high << 16) | low;
        }

        /// <summary>
        ///     decodes the high ushort from encoded signed integer, low ushort is get by casting int to ushort "ushort u =
        ///     (ushort)i"
        /// </summary>
        public static ushort HighUShortFromInt(int i)
        {
            return (ushort) (i >> 16);
        }

        /// <summary>
        ///     http://stackoverflow.com/a/8776650
        /// </summary>
        public static bool InRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return (value.CompareTo(from) >= 1) && (value.CompareTo(to) <= -1);
        }

        /// <summary>
        ///     takes 8 booleans and encodes them to byte
        /// </summary>
        public static byte EncodeBooleans(bool[] array)
        {
            byte value = 0;

            for (var i = 0; i < 8; i++)
            {
                value <<= 1;
                if (array[i])
                    value |= 1;
            }

            return value;
        }

        /// <summary>
        ///     decodes byte into 8 booleans
        /// </summary>
        public static bool[] DecodeByte(byte b)
        {
            var value = new bool[8];

            for (var i = 0; i < 8; i++)
                value[7 - i] = (b & (1 << i)) != 0;

            return value;
        }

        /// <summary>
        ///     linear interpolation
        /// </summary>
        public static int Lerp(int a, int b, float s)
        {
            return (int) ((a + (b - a))*s);
        }

        /// <summary>
        ///     swap two numbers
        /// </summary>
        public static void Swap(ref int x, ref int y)
        {
            x = x + y;
            y = x - y;
            x = x - y;
        }

        /// <summary>
        ///     Create an md5 sum string of this string
        /// </summary>
        public static string GetMD5Sum(string str)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            var enc = Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            var unicodeText = new byte[str.Length*2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("X2"));

            // And return it
            return sb.ToString();
        }
    }
}