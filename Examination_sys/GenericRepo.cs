using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_sys
{
    public class Repository<T> where T : ICloneable, IComparable<T>
    {
        private T[] _items;
        private int _count;
        public Repository(int capacity = 4)
        {
            if(capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");
            _items = new T[capacity];
            _count = 0;
        }
        public void Add(T item)
        {
            if (_count == _items.Length)
                Resize();
            _items[_count++] = item;
        }

        public bool Remove(T item)
        {
            int index = Array.IndexOf(_items, item, 0, _count);

            if (index == -1)
                return false;

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_count - 1] = default!;
            _count--;

            return true;
        }

        public void Sort()
        {
            Array.Sort(_items, 0, _count);
        }

        public T[] GetAll()
        {
            T[] newArr = new T[_count];
            Array.Copy(_items, newArr, _count);
            return newArr;
        }
        public void Resize()
        {
            T[] newArr = new T[_items.Length * 2];
            Array.Copy(_items, newArr, _items.Length);
            _items = newArr;
        }

    }
}
