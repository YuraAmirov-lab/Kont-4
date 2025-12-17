using System;
using System.Text;

public class SegmentTree
{
    private readonly long[] _tree;
    private readonly long[] _lazy;
    private readonly int _n;

    public SegmentTree(int size)
    {
        _n = size;
        _tree = new long[4 * _n];
        _lazy = new long[4 * _n];
    }

    private void Push(int v)
    {
        if (_lazy[v] != 0)
        {
            _tree[v * 2] += _lazy[v];
            _lazy[v * 2] += _lazy[v];

            _tree[v * 2 + 1] += _lazy[v];
            _lazy[v * 2 + 1] += _lazy[v];

            _lazy[v] = 0;
        }
    }

    public void Add(int l, int r, long v)
    {
        Add(1, 0, _n - 1, l, r - 1, v); 
    }

    private void Add(int v, int tl, int tr, int l, int r, long value)
    {
        if (l > r)
            return;

        if (l == tl && r == tr)
        {
            _tree[v] += value;
            _lazy[v] += value;
        }
        else
        {
            Push(v);
            int tm = (tl + tr) / 2;

            Add(v * 2, tl, tm, l, Math.Min(r, tm), value);
            Add(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r, value);

            _tree[v] = Math.Min(_tree[v * 2], _tree[v * 2 + 1]);
        }
    }

    public long QueryMin(int l, int r)
    {
        return QueryMin(1, 0, _n - 1, l, r - 1); 
    }

    private long QueryMin(int v, int tl, int tr, int l, int r)
    {
        if (l > r)
            return long.MaxValue;

        if (l == tl && r == tr)
        {
            return _tree[v];
        }

        Push(v);
        int tm = (tl + tr) / 2;

        long leftMin = QueryMin(v * 2, tl, tm, l, Math.Min(r, tm));
        long rightMin = QueryMin(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r);

        return Math.Min(leftMin, rightMin);
    }
}

public class Program
{
    public static void Main()
    {
        string[] firstLine = Console.ReadLine().Split();
        int n = int.Parse(firstLine[0]);
        int m = int.Parse(firstLine[1]);

        SegmentTree segmentTree = new SegmentTree(n);
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < m; i++)
        {
            string[] operation = Console.ReadLine().Split();
            int type = int.Parse(operation[0]);

            if (type == 1)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long v = long.Parse(operation[3]);
                segmentTree.Add(l, r, v);
            }
            else if (type == 2)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long min = segmentTree.QueryMin(l, r);
                result.AppendLine(min.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}