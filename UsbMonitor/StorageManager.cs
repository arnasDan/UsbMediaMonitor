using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace UsbMonitor
{
    public class StorageManager<T>
    {
        private readonly string _filename = typeof(T).Name.ToLower() + ".json";
        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        public void Save(IEnumerable<T> objects)
        {
            using (var writer = new StreamWriter(_filename))
            {
                _serializer.Serialize(writer, objects);
            }
        }

        public IEnumerable<T> Read()
        {
            if (!File.Exists(_filename))
            {
                using (File.Create(_filename))
                {
                    return Enumerable.Empty<T>();
                }
            }
            using (var reader = new StreamReader(_filename))
            using (var jsonReader = new JsonTextReader(reader))
                return _serializer.Deserialize<IEnumerable<T>>(jsonReader);
        }
    }
}