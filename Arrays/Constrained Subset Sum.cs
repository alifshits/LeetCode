public class Solution 
{
    public int ConstrainedSubsetSum(int[] nums, int k) 
    {
        var res = int.MinValue;
        var heap = new Heap<(int sum, int idx)>(new Comparer());
        
        for (var i = 0; i < nums.Length; ++i)
        {            
            while (heap.Count > 0 && i - heap.Peek().idx > k)
                heap.Pop();
            
            var newSum = 0;
            if (heap.Count > 0)
                newSum = Math.Max(0, heap.Peek().sum);
            
            newSum += nums[i];
            res = Math.Max(res, newSum);
            heap.Add((newSum, i));
        }
        
        return res;
    }
    
    private sealed class Comparer : IComparer<(int sum, int idx)>
    {
        public int Compare((int sum, int idx) x, (int sum, int idx) y)
        {
            return y.sum - x.sum;
        }
    }
    
    private sealed class Heap<TItem>
    {
        private readonly IComparer<TItem> _comparer;
        private TItem[] _arr;
        private int _size;

        public Heap(IComparer<TItem> comparer, int capacity = 2048)
        {
            _comparer = comparer;
            _arr = new TItem[capacity];
            _size = 0;
        }

        public void Add(TItem item)
        {
            if (_size == _arr.Length)
            {
                Resize();
            }

            ++_size;
            var i = _size - 1;
            _arr[i] = item;

            while (i != 0 && _comparer.Compare(_arr[Parent(i)], _arr[i]) > 0)
            {
                Swap(i, Parent(i));
                i = Parent(i); 
            } 
        }

        public TItem Peek() => _arr[0];

        public TItem Pop()
        {
            if (_size == 1)
            {
                --_size;
                return _arr[0];
            }

            var root = _arr[0]; 
            _arr[0] = _arr[_size - 1]; 
            --_size;
            Heapify(0); 

            return root; 
        }

        public int Count => _size;

        private void Resize()
        {
            var arr = new TItem[_arr.Length * 2];
            Array.Copy(_arr, arr, _arr.Length);
            _arr = arr;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Swap(int i , int j)
        {
            var tmp = _arr[i];
            _arr[i] = _arr[j];
            _arr[j] = tmp;
        }

        private void Heapify(int i) 
        { 
            var l = Left(i); 
            var r = Right(i); 
            var smallest = i; 

            if (l < _size && _comparer.Compare(_arr[l], _arr[i]) < 0)
                smallest = l; 

            if (r < _size && _comparer.Compare(_arr[r], _arr[smallest]) < 0)
                smallest = r; 

            if (smallest != i) 
            { 
                Swap(i, smallest); 
                Heapify(smallest); 
            } 
        } 

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int Parent(int i) => (i - 1) / 2;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int Left(int i) => 2 * i + 1;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int Right(int i) => 2 * i + 2;
    }
}