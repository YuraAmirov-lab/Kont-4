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
            _tree[v] = Math.Max(_tree[v * 2], _tree[v * 2 + 1]);
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

            _tree[v] = Math.Max(_tree[v * 2], _tree[v * 2 + 1]);
        }
    }

    public int FindFirstNotLess(long x, int l)
    {
        return FindFirstNotLess(1, 0, _n - 1, l, x);
    }

    private int FindFirstNotLess(int v, int tl, int tr, int l, long x)
    {
        if (_tree[v] < x)
            return -1;

        if (tr < l)
            return -1;

        if (tl == tr)
        {
            if (tl >= l && _tree[v] >= x)
                return tl;
            return -1;
        }

        int tm = (tl + tr) / 2;
        if (tm >= l)
        {
            int leftResult = FindFirstNotLess(v * 2, tl, tm, l, x);
            if (leftResult != -1)
                return leftResult;
        }
        return FindFirstNotLess(v * 2 + 1, tm + 1, tr, l, x);
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
                long x = long.Parse(operation[1]);
                int l = int.Parse(operation[2]);
                int index = segmentTree.FindFirstNotLess(x, l);
                result.AppendLine(index.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}