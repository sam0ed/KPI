using System.Collections;

namespace Lab2;

internal class OrderedList<T> : IList<T>
{
    private readonly IComparer<T> _comparer;
    private readonly IList<T> _innerList = new List<T>();

    public OrderedList() : this(Comparer<T>.Default) { }

    public OrderedList(IComparer<T> comparer)
    {
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    }

    public T this[int index]
    {
        get => _innerList[index];
        set => throw new NotSupportedException("Cannot set an indexed item in a sorted list.");
    }

    public int Count => _innerList.Count;
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        int index = Array.BinarySearch(_innerList.ToArray(), item, _comparer);
        index = (index >= 0) ? index : ~index;
        _innerList.Insert(index, item);
    }

    public void Clear() => _innerList.Clear();
    public bool Contains(T item) => _innerList.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => _innerList.GetEnumerator();
    public int IndexOf(T item) => _innerList.IndexOf(item);
    public void Insert(int index, T item) => throw new NotSupportedException("Cannot insert an indexed item in a sorted list.");
    public bool Remove(T item) => _innerList.Remove(item);
    public void RemoveAt(int index) => _innerList.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}