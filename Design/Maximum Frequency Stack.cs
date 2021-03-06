public class FreqStack
{
    private readonly Dictionary<int, int> _freq = new Dictionary<int, int>();
    private readonly Dictionary<int, Stack<int>> _groups = new Dictionary<int, Stack<int>>();
    private int _maxFreq;

    public void Push(int x)
    {
        var freq = _freq.GetValueOrDefault(x, 0) + 1;
        _freq[x] = freq;
        _maxFreq = Math.Max(_maxFreq, freq);

        var stack = _groups.GetValueOrDefault(freq, new Stack<int>());
        stack.Push(x);
        _groups[freq] = stack;
    }

    public int Pop()
    {
        var group = _groups[_maxFreq];
        var res = group.Pop();

        _freq[res] -= 1;

        if (group.Count == 0)
            --_maxFreq;

        return res;
    }
}
 
public class FreqStack
{
    private readonly Dictionary<int, Entry> _valToEntry = new Dictionary<int, Entry>();
    private readonly Dictionary<int, int> _idToVal = new Dictionary<int, int>();
    private readonly SortedSet<Entry> _sorted = new SortedSet<Entry>(new EntryComparer());
    private int _id = 0;

    public FreqStack()
    {
    }

    public void Push(int x)
    {
        var id = ++_id;

        if (!_valToEntry.TryGetValue(x, out var entry))
        {
            entry = new Entry(id);
            _valToEntry.Add(x, entry);
        }
        else
        {
            _sorted.Remove(entry);
            entry.Ids.Push(id);
        }

        _idToVal.Add(id, x);
        _sorted.Add(entry);
    }

    public int Pop()
    {
        var entry = _sorted.Min;
        _sorted.Remove(entry);

        var id = entry.Ids.Pop();
        var result = _idToVal[id];

        _idToVal.Remove(id);

        if (entry.Ids.Count > 0)
            _sorted.Add(entry);
        else
            _valToEntry.Remove(result);

        return result;
    }

    private sealed class Entry : IComparable<Entry>
    {
        public readonly Stack<int> Ids = new Stack<int>();

        public Entry(int id)
        {
            Ids.Push(id);
        }

        public int CompareTo(Entry other)
        {
            if (Ids.Count == other.Ids.Count)
                return Ids.Peek() - other.Ids.Peek();

            return Ids.Count - other.Ids.Count;
        }
    }

    private sealed class EntryComparer : IComparer<Entry>
    {
        public int Compare(Entry x, Entry y)
        {
            return -1 * x.CompareTo(y);
        }
    }
}

/**
 * Your FreqStack object will be instantiated and called as such:
 * FreqStack obj = new FreqStack();
 * obj.Push(x);
 * int param_2 = obj.Pop();
 */