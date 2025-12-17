using System;
using System.Text;

public class SegmentTree
{
    private readonly long[] _tree;
    private readonly int _n;

    public SegmentTree(long[] arr)
    {
        _n = arr.Length;
        _tree = new long[4 * _n];
        Build(arr, 1, 0, _n - 1);
    }

    private void Build(long[] arr, int v, int tl, int tr)
    {
        if (tl == tr)
        {
            _tree[v] = arr[tl];
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(arr, v * 2, tl, tm);
            Build(arr, v * 2 + 1, tm + 1, tr);
            _tree[v] = _tree[v * 2] + _tree[v * 2 + 1];
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
            _tree[v] = value;
        }
        else
        {
            int tm = (tl + tr) / 2;
            if (index <= tm)
                Update(v * 2, tl, tm, index, value);
            else
                Update(v * 2 + 1, tm + 1, tr, index, value);
            _tree[v] = _tree[v * 2] + _tree[v * 2 + 1];
        }
    }

    public long Query(int l, int r)
    {
        return Query(1, 0, _n - 1, l, r - 1);
    }

    private long Query(int v, int tl, int tr, int l, int r)
    {
        if (l > r)
            return 0;

        if (l == tl && r == tr)
            return _tree[v];

        int tm = (tl + tr) / 2;
        return Query(v * 2, tl, tm, l, Math.Min(r, tm)) +
               Query(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r);
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

        for (int i = 0; i < m; i++)
        {
            string[] operation = Console.ReadLine().Split();
            int type = int.Parse(operation[0]);

            if (type == 1)
            {
                int index = int.Parse(operation[1]);
                long value = long.Parse(operation[2]);
                segmentTree.Update(index, value);
            }
            else if (type == 2)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long sum = segmentTree.Query(l, r);
                result.AppendLine(sum.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}