using System;
using System.Text;

// Структура для хранения пары (минимум, количество)
public struct MinCount
{
    public long Min;
    public int Count;

    public MinCount(long min, int count)
    {
        Min = min;
        Count = count;
    }
}
public class SegmentTree
{
    private readonly MinCount[] _tree;
    private readonly int _n;

    public SegmentTree(long[] arr)
    {
        _n = arr.Length;
        _tree = new MinCount[4 * _n];
        Build(arr, 1, 0, _n - 1);
    }

    private void Build(long[] arr, int v, int tl, int tr)
    {
        if (tl == tr)
        {
            _tree[v] = new MinCount(arr[tl], 1);
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(arr, v * 2, tl, tm);
            Build(arr, v * 2 + 1, tm + 1, tr);
            _tree[v] = Combine(_tree[v * 2], _tree[v * 2 + 1]);
        }
    }
    private MinCount Combine(MinCount left, MinCount right)
    {
        if (left.Min < right.Min)
        {
            return left;
        }
        else if (right.Min < left.Min)
        {
            return right;
        }
        else
        {
            return new MinCount(left.Min, left.Count + right.Count);
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
            _tree[v] = new MinCount(value, 1);
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

    public MinCount Query(int l, int r)
    {
        return Query(1, 0, _n - 1, l, r - 1);
    }

    private MinCount Query(int v, int tl, int tr, int l, int r)
    {
        if (l > r)
            return new MinCount(long.MaxValue, 0);

        if (l == tl && r == tr)
            return _tree[v];

        int tm = (tl + tr) / 2;

        var leftResult = Query(v * 2, tl, tm, l, Math.Min(r, tm));
        var rightResult = Query(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r);

        return Combine(leftResult, rightResult);
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
                MinCount minCount = segmentTree.Query(l, r);
                result.AppendLine($"{minCount.Min} {minCount.Count}");
            }
        }

        Console.Write(result.ToString());
    }
}