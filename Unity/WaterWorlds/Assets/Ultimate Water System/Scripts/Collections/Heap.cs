namespace UltimateWater.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Max-heap
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Heap<T> : IEnumerable<T> where T : IComparable<T>
    {
        #region Public Variables
        public int Count
        {
            get { return _NumElements; }
        }

        public T Max
        {
            get { return _Elements[0]; }
        }
        #endregion Public Variables

        #region Public Methods
        public Heap() : this(8)
        {
        }

        public Heap(int capacity)
        {
            _Elements = new T[capacity];
        }

        public T ExtractMax()
        {
            if (_NumElements == 0)
                throw new InvalidOperationException("Heap is empty.");

            var max = _Elements[0];

            _Elements[0] = _Elements[--_NumElements];
            _Elements[_NumElements] = default(T);
            BubbleDown(0);

            return max;
        }

        public void Insert(T element)
        {
            if (_Elements.Length == _NumElements)
                Resize(_Elements.Length * 2);

            _Elements[_NumElements++] = element;
            BubbleUp(_NumElements - 1, element);
        }

        public void Remove(T element)
        {
            for (int i = 0; i < _NumElements; ++i)
            {
                if (_Elements[i].Equals(element))
                {
                    _Elements[i] = _Elements[--_NumElements];
                    _Elements[_NumElements] = default(T);
                    BubbleDown(i);

                    return;
                }
            }
        }

        public void Clear()
        {
            _NumElements = 0;
        }

        public T[] GetUnderlyingArray()
        {
            return _Elements;
        }

        public void Resize(int len)
        {
            Array.Resize(ref _Elements, len);
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_Elements.Length != _NumElements)
                Array.Resize(ref _Elements, _NumElements);

            return ((IEnumerable<T>)_Elements).GetEnumerator();
        }
        #endregion Public Methods

        #region Private Variables
        private T[] _Elements;
        private int _NumElements;
        #endregion Private Variables

        #region Private Methods
        private void BubbleUp(int index, T element)
        {
            while (index != 0)
            {
                int parentIndex = (index - 1) >> 1;
                var parent = _Elements[parentIndex];

                if (element.CompareTo(parent) <= 0)
                    return;

                _Elements[index] = _Elements[parentIndex];
                _Elements[parentIndex] = element;

                index = parentIndex;
            }
        }

        private void BubbleDown(int index)
        {
            var element = _Elements[index];
            int childrenIndex = (index << 1) + 1;

            while (childrenIndex < _NumElements)
            {
                int maxChildrenIndex;
                var a = _Elements[childrenIndex];

                if (childrenIndex + 1 < _NumElements)
                {
                    var b = _Elements[childrenIndex + 1];

                    if (a.CompareTo(b) > 0)
                        maxChildrenIndex = childrenIndex;
                    else
                    {
                        a = b;
                        maxChildrenIndex = childrenIndex + 1;
                    }
                }
                else
                    maxChildrenIndex = childrenIndex;

                if (element.CompareTo(a) >= 0)
                    return;

                _Elements[maxChildrenIndex] = element;
                _Elements[index] = a;

                index = maxChildrenIndex;
                childrenIndex = (index << 1) + 1;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_Elements.Length != _NumElements)
                Array.Resize(ref _Elements, _NumElements);

            return _Elements.GetEnumerator();
        }
        #endregion Private Methods
    }
}