using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PA.IO
{
    public static class Serialize
    {
        public static object DeserializeFromJSON(string data, Type type)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                DataContractJsonSerializer converter = new DataContractJsonSerializer(type);
                return converter.ReadObject(stream);
            }
        }

        public static string SerializeToJson(object data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;
                DataContractJsonSerializer converter = new DataContractJsonSerializer(data.GetType());
                converter.WriteObject(stream, data);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    string parsing = reader.ReadToEnd();

                    return (parsing);
                }
            }
        }
    }
}
