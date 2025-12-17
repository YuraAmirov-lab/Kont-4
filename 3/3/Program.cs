using System;
using System.Text;

public struct Node
{
    public long Sum;   
    public long MaxPrefix; 
    public long MaxSuffix;
    public long MaxSubarray; 

    public Node(long sum, long maxPrefix, long maxSuffix, long maxSubarray)
    {
        Sum = sum;
        MaxPrefix = maxPrefix;
        MaxSuffix = maxSuffix;
        MaxSubarray = maxSubarray;
    }
}

public class SegmentTree
{
    private readonly Node[] _tree;
    private readonly int _n;

    public SegmentTree(long[] arr)
    {
        _n = arr.Length;
        _tree = new Node[4 * _n];
        Build(arr, 1, 0, _n - 1);
    }
    private Node MakeNode(long value)
    {
        return new Node(
            value,
            Math.Max(value, 0), 
            Math.Max(value, 0), 
            Math.Max(value, 0)   
        );
    }
    private Node Combine(Node left, Node right)
    {
        return new Node(
            left.Sum + right.Sum,
            Math.Max(left.MaxPrefix, left.Sum + right.MaxPrefix),
            Math.Max(right.MaxSuffix, right.Sum + left.MaxSuffix),
            Math.Max(Math.Max(left.MaxSubarray, right.MaxSubarray),
                    left.MaxSuffix + right.MaxPrefix)
        );
    }

    private void Build(long[] arr, int v, int tl, int tr)
    {
        if (tl == tr)
        {
            _tree[v] = MakeNode(arr[tl]);
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(arr, v * 2, tl, tm);
            Build(arr, v * 2 + 1, tm + 1, tr);
            _tree[v] = Combine(_tree[v * 2], _tree[v * 2 + 1]);
        }
    }

    public void Update(int index, long value)
    {
        Update(1, 0, _n - 1, index, value);
    }

    private void Update(int v, int tl, int tr, int index, long value)
    {
        if (tl == tr)
        {
            _tree[v] = MakeNode(value);
        }
        else
        {
            int tm = (tl + tr) / 2;
            if (index <= tm)
                Update(v * 2, tl, tm, index, value);
            else
                Update(v * 2 + 1, tm + 1, tr, index, value);

            _tree[v] = Combine(_tree[v * 2], _tree[v * 2 + 1]);
        }
    }

    public long GetMaxSubarraySum()
    {
        return Math.Max(0, _tree[1].MaxSubarray);
    }
}

public class Program
{
    public static void Main()
    {
        string[] firstLine = Console.ReadLine().Split();
        int n = int.Parse(firstLine[0]);
        int m = int.Parse(firstLine[1]);

        long[] array = new long[n];
        string[] arrayValues = Console.ReadLine().Split();
        for (int i = 0; i < n; i++)
        {
            array[i] = long.Parse(arrayValues[i]);
        }

        SegmentTree segmentTree = new SegmentTree(array);
        StringBuilder result = new StringBuilder();

        result.AppendLine(segmentTree.GetMaxSubarraySum().ToString());

        for (int i = 0; i < m; i++)
        {
            string[] operation = Console.ReadLine().Split();
            int index = int.Parse(operation[0]);
            long value = long.Parse(operation[1]);

            segmentTree.Update(index, value);
            result.AppendLine(segmentTree.GetMaxSubarraySum().ToString());
        }

        Console.Write(result.ToString());
    }
}