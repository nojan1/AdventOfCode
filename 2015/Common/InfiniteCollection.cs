using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class InfiniteCollectionItem<T>
    {
        public int Index { get; set; }
        public T Value { get; set; }
    }

    public class InfiniteCollection<T> : IEnumerable<InfiniteCollectionItem<T>>
    {
        private Dictionary<int, T> _values = new Dictionary<int, T>();

        public T this[int index]
        {
            get
            {
                if (_values.TryGetValue(index, out T value))
                    return value;

                return default(T);
            }
            set
            {
                _values[index] = value;
            }
        }

        public IEnumerator<InfiniteCollectionItem<T>> GetEnumerator()
        {
            return _values.Select(x => new InfiniteCollectionItem<T> { Index = x.Key, Value = x.Value }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
