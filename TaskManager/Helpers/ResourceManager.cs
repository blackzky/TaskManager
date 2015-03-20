using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TaskManager.Helpers
{
    public class ResourceManager
    {
        public static string IMAGES_BASE_PATH = @"/Resources/Images/";
        public static string PERSISTENT_DATA_PATH_FILENAME = @"PersistentData.dat";
    }

    public class Serializer
    {
        public static void SerializeObject(string filename, PersistentData objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public static PersistentData DeSerializeObject(string filename)
        {
            PersistentData objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (PersistentData)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }

    public static class StringTools
    {
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }
}
