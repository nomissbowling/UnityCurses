using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Engine.Utility
{
    public class Serializer
    {
        public static BinaryFormatter binarizer = new BinaryFormatter();

        public static string SerializeObject(object obj)
        {
            var memoryStream = new MemoryStream();
            binarizer.Serialize(memoryStream, obj);

            binarizer.Binder = new VersionDeserializationBinder();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static object DeserializeObject(string obj)
        {
            var memoryStream = new MemoryStream(Convert.FromBase64String(obj));

            binarizer.Binder = new VersionDeserializationBinder();

            return binarizer.Deserialize(memoryStream);
        }
    }

    public sealed class VersionDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
            {
                assemblyName = Assembly.GetExecutingAssembly().FullName;

                //The following line of code returns the type.
                var typeToDeserialize = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));

                return typeToDeserialize;
            }

            return null;
        }
    }
}