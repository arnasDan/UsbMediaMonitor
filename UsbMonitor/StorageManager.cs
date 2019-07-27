using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UsbMonitor
{
    public class StorageManager<T>
    {
        private readonly string _path;
        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        public StorageManager(string filename = null, string storageFolder = null)
        {
            filename = filename ?? typeof(T).Name.ToLower() + ".json";
            storageFolder = storageFolder ?? 
                            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/{Assembly.GetEntryAssembly()?.GetName().Name}";
            _path = $"{storageFolder}/{filename}";
            Directory.CreateDirectory(storageFolder);
        }

        public void Save(IEnumerable<T> objects)
        {
            using (var writer = new StreamWriter(_path))
            {
                _serializer.Serialize(writer, objects);
            }
        }

        public IEnumerable<T> Read()
        {
            if (!File.Exists(_path))
            {
                using (File.Create(_path))
                {
                    return Enumerable.Empty<T>();
                }
            }
            using (var reader = new StreamReader(_path))
            using (var jsonReader = new JsonTextReader(reader))
                return _serializer.Deserialize<IEnumerable<T>>(jsonReader);
        }
    }
}