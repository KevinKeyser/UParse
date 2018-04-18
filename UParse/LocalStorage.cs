using System.Collections.Generic;
using System.IO;

namespace UParse
{
    public class LocalStorage<T> : IStorage<T>
    {
        private readonly string fileLocation;

        public LocalStorage(string fileLocation, IConverter converter)
        {
            this.fileLocation = fileLocation;
            Converter = converter;
        }

        public IConverter Converter { get; }

        public void Clear()
        {
            using (var fileStream = new FileStream(fileLocation, FileMode.Append, FileAccess.ReadWrite))
            {
                fileStream.SetLength(0);
                fileStream.Close();
            }
        }

        public void Add(IEnumerable<T> items)
        {
            using (var fileStream = new FileStream(fileLocation, FileMode.Append, FileAccess.ReadWrite))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (var item in items)
                    {
                        streamWriter.WriteLine(Converter.Serialize(item));
                    }
                }
            }
        }
    }
}