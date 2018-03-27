namespace UltimateWater.Internal
{
    using UnityEngine;

    public class Quadtree<T> where T : class, IPoint2D
    {
        #region Public Variables
        public Rect Rect
        {
            get { return _Rect; }
            set
            {
                _Rect = value;
                _Center = _Rect.center;

                _MarginRect = _Rect;
                float w = _Rect.width * 0.0025f;
                _MarginRect.xMin -= w;
                _MarginRect.yMin -= w;
                _MarginRect.xMax += w;
                _MarginRect.yMax += w;

                if (_A != null)
                {
                    float halfWidth = _Rect.width * 0.5f;
                    float halfHeight = _Rect.height * 0.5f;
                    _A.Rect = new Rect(_Rect.xMin, _Center.y, halfWidth, halfHeight);
                    _B.Rect = new Rect(_Center.x, _Center.y, halfWidth, halfHeight);
                    _C.Rect = new Rect(_Rect.xMin, _Rect.yMin, halfWidth, halfHeight);
                    _D.Rect = new Rect(_Center.x, _Rect.yMin, halfWidth, halfHeight);
                }
            }
        }

        public int Count
        {
            get { return _A != null ? _A.Count + _B.Count + _C.Count + _D.Count : _NumElements; }
        }

        public int FreeSpace
        {
            get { return _Root._FreeSpace; }
        }
        #endregion Public Variables

        #region Public Methods
        public Quadtree(Rect rect, int maxElementsPerNode, int maxTotalElements)
        {
            _Root = this;
            Rect = rect;
            _Elements = new T[maxElementsPerNode];
            _NumElements = 0;
            _FreeSpace = maxTotalElements;
        }

        public bool AddElement(T element)
        {
            Vector2 pos = element.Position;

            if (_Root._FreeSpace <= 0 || float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsInfinity(pos.x) || float.IsInfinity(pos.y))
            {
                element.Destroy();
                return false;
            }

            if (_Root == this && !_Rect.Contains(element.Position))
                ExpandToContain(element.Position);

            return AddElementInternal(element);
        }

        public void UpdateElements(Quadtree<T> root)
        {
            if (_A != null)
            {
                _A.UpdateElements(root);
                _B.UpdateElements(root);
                _C.UpdateElements(root);
                _D.UpdateElements(root);
            }

            if (_Elements != null)
            {
                for (int i = 0; i < _Elements.Length; ++i)
                {
                    var element = _Elements[i];

                    if (element != null && !_MarginRect.Contains(element.Position))
                    {
                        RemoveElementAt(i);
                        root.AddElement(element);
                    }
                }
            }
        }

        public void ExpandToContain(Vector2 point)
        {
            var rect = _Rect;

            do
            {
                float w = rect.width * 0.5f;
                rect.xMin -= w;
                rect.yMin -= w;
                rect.xMax += w;
                rect.yMax += w;
            }
            while (!rect.Contains(point));

            Rect = rect;
            UpdateElements(_Root);
        }

        public virtual void Destroy()
        {
            _Elements = null;

            if (_A != null)
            {
                _A.Destroy();
                _B.Destroy();
                _C.Destroy();
                _D.Destroy();
                _A = _B = _C = _D = null;
            }
        }
        #endregion Public Methods

        #region Private Variables

        // ReSharper disable InconsistentNaming
        protected Rect _Rect;
        protected Rect _MarginRect;
        protected Vector2 _Center;
        protected Quadtree<T> _A, _B, _C, _D;
        protected Quadtree<T> _Root;
        protected T[] _Elements;
        protected int _NumElements;
        // ReSharper restore InconsistentNaming

        private int _FreeSpace;
        private int _LastIndex;
        private int _Depth;
        #endregion Private Variables

        #region Private Methods
        private Quadtree(Quadtree<T> root, Rect rect, int maxElementsPerNode) : this(rect, maxElementsPerNode, 0)
        {
            _Root = root;
        }

        private bool AddElementInternal(T element)
        {
            if (element == null)
                throw new System.ArgumentException("Element null");

            if (_A != null)
            {
                Vector2 pos = element.Position;

                if (pos.x < _Center.x)
                {
                    if (pos.y > _Center.y)
                        return _A.AddElementInternal(element);
                    else
                        return _C.AddElementInternal(element);
                }
                else
                {
                    if (pos.y > _Center.y)
                        return _B.AddElementInternal(element);
                    else
                        return _D.AddElementInternal(element);
                }
            }
            else if (_NumElements != _Elements.Length)
            {
                for (; _LastIndex < _Elements.Length; ++_LastIndex)
                {
                    if (_Elements[_LastIndex] == null)
                    {
                        AddElementAt(element, _LastIndex);
                        return true;
                    }
                }

                for (_LastIndex = 0; _LastIndex < _Elements.Length; ++_LastIndex)
                {
                    if (_Elements[_LastIndex] == null)
                    {
                        AddElementAt(element, _LastIndex);
                        return true;
                    }
                }

                throw new System.InvalidOperationException("UltimateWater: Code supposed to be unreachable.");
            }
            else if (_Depth < 80)
            {
                var elementsLocal = _Elements;

                SpawnChildNodes();

                // ReSharper disable once PossibleNullReferenceException
                _A._Depth = _B._Depth = _C._Depth = _D._Depth = _Depth + 1;

                _Elements = null;
                _NumElements = 0;

                _Root._FreeSpace += elementsLocal.Length;

                for (int i = 0; i < elementsLocal.Length; ++i)
                    AddElementInternal(elementsLocal[i]);

                return AddElementInternal(element);
            }
            else
                throw new System.Exception("UltimateWater: Quadtree depth limit reached.");
        }

        protected virtual void AddElementAt(T element, int index)
        {
            ++_NumElements;
            --_Root._FreeSpace;
            _Elements[index] = element;
        }

        protected virtual void RemoveElementAt(int index)
        {
            --_NumElements;
            ++_Root._FreeSpace;
            _Elements[index] = null;
        }

        protected virtual void SpawnChildNodes()
        {
            var halfWidth = _Rect.width * 0.5f;
            var halfHeight = _Rect.height * 0.5f;
            _A = new Quadtree<T>(_Root, new Rect(_Rect.xMin, _Center.y, halfWidth, halfHeight), _Elements.Length);
            _B = new Quadtree<T>(_Root, new Rect(_Center.x, _Center.y, halfWidth, halfHeight), _Elements.Length);
            _C = new Quadtree<T>(_Root, new Rect(_Rect.xMin, _Rect.yMin, halfWidth, halfHeight), _Elements.Length);
            _D = new Quadtree<T>(_Root, new Rect(_Center.x, _Rect.yMin, halfWidth, halfHeight), _Elements.Length);
        }
        #endregion Private Methods
    }
}