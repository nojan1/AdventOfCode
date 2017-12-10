using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CircularList<T> : List<T>, IEnumerable<T>
    {
        public new T this[int index]
        {
            get => base[index % Count];
            set
            {
                base[index % Count] = value;
            }
        }

        public CircularList()
        {
        }

        public CircularList(IEnumerable<T> collection) : base(collection)
        {
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return new CircularEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CircularEnumerator<T>(this);
        }

        public void ReplaceRange(IList<T> other, int offset)
        {
            for (int i = 0; i < other.Count; i++)
            {
                this[i + offset] = other[i];
            }
        }
    }

    public class CircularEnumerator<T> : IEnumerator<T>
    {
        private IList<T> _list;
        private int _currentElement = -1;

        public CircularEnumerator(IList<T> list)
        {
            _list = list;
        }

        public T Current => _currentElement != -1 ?_list[_currentElement] : default(T);

        object IEnumerator.Current => _currentElement != -1 ? _list[_currentElement] : default(T);

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (++_currentElement > _list.Count - 1)
                _currentElement = 0;

            return true;
        }

        public void Reset()
        {
            _currentElement = -1;
        }
    }
}
