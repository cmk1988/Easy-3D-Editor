using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Easy_3D_Editor.Services
{
    class Serializer
    {
        public static T DeserializeBinary<T>(string fileName)
        {
            using (var ms = File.OpenRead(fileName))
            {
                var formatter = new BinaryFormatter();
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static void SerializeBinary(string fileName, object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }
    }
}
