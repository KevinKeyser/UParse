using System.Collections.Generic;

namespace UParse
{
    public interface IStorage<T>
    {
        IConverter Converter { get; }

        void Clear();
        void Add(IEnumerable<T> items);
    }
}