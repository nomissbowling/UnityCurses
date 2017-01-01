// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@3:31 PM

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Scripts.Engine.Utility
{
    /// <summary>
    ///     Helper class that assists with converting engine type data into strings, compressing them, returning the bytes, and
    ///     decoding them back into valid JSON.
    /// </summary>
    public static class JSONExtensions
    {
        /// <summary>
        ///     Converts an object into a JSON string.
        /// </summary>
        /// <param name="value">Object that needs to be converted into a string.</param>
        /// <param name="compress">Determines if the JSON string should be compressed before being returned as a string.</param>
        public static string Serialize(object value, bool compress = true)
        {
            var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});

            if (!compress)
            {
                // Standard JSON serialization with no compression (easy to read with indents).
                jsonSettings.Formatting = Formatting.Indented;
                var result = JsonConvert.SerializeObject(value, Formatting.Indented, jsonSettings);
                return result;
            }

            // Advanced JSON string compression.
            jsonSettings.Formatting = Formatting.None;
            var jsonResult = JsonConvert.SerializeObject(value, Formatting.None, jsonSettings);
            var compressedResult = Compress(jsonResult);
            return compressedResult;
        }

        /// <summary>
        ///     Converts a JSON string back into object by casting it using generics.
        /// </summary>
        /// <typeparam name="T">Object the JSON string will be casted as.</typeparam>
        /// <param name="value">JSON string that needs to be decoded.</param>
        /// <param name="decompress">
        ///     Determines if the JSON string was compressed before being sent and should be decompressed
        ///     before deserialization.
        /// </param>
        public static T Deserialize<T>(string value, bool decompress = true)
        {
            var jsonSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});

            if (!decompress)
            {
                // Standard JSON string deserialization (easier for user to read with indents).
                jsonSettings.Formatting = Formatting.Indented;
                var result = JsonConvert.DeserializeObject<T>(value, jsonSettings);
                return result;
            }
            else
            {
                // Advanced JSON string decompression.
                var jsonDecompressed = Decompress(value);
                jsonSettings.Formatting = Formatting.None;
                var result = JsonConvert.DeserializeObject<T>(jsonDecompressed, jsonSettings);
                return result;
            }
        }

        /// <summary>
        ///     Compresses a string into a byte representation of itself in an effort to compress it.
        /// </summary>
        /// <returns>Compressed string of bytes.</returns>
        private static string Compress(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        CopyTo(msi, gs);
                    }
                    return Convert.ToBase64String(mso.ToArray());
                }
            }
        }

        /// <summary>
        ///     Decompresses a string byte representation back into original string.
        /// </summary>
        /// <returns>Decompressed string.</returns>
        private static string Decompress(string str)
        {
            var bytes = Convert.FromBase64String(str);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }

        /// <summary>
        ///     Used during compression and decompression of string into bytes and back into string.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
                dest.Write(bytes, 0, cnt);
        }
    }
}