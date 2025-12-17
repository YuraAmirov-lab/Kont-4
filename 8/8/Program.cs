using System;
using System.Text;

public class SegmentTree
{
    private readonly long[] _sum;
    private readonly long[] _add;
    private readonly long[] _assign;
    private readonly bool[] _hasAssign;
    private readonly int _n;

    public SegmentTree(int size)
    {
        _n = size;
        _sum = new long[4 * _n];
        _add = new long[4 * _n];
        _assign = new long[4 * _n];
        _hasAssign = new bool[4 * _n];
    }

    private void Push(int v, int tl, int tr)
    {
        if (tl == tr)
            return;

        int tm = (tl + tr) / 2;
        int left = v * 2;
        int right = v * 2 + 1;

        if (_hasAssign[v])
        {
            _sum[left] = _assign[v] * (tm - tl + 1);
            _assign[left] = _assign[v];
            _hasAssign[left] = true;
            _add[left] = 0; 

            _sum[right] = _assign[v] * (tr - (tm + 1) + 1);
            _assign[right] = _assign[v];
            _hasAssign[right] = true;
            _add[right] = 0;

            _hasAssign[v] = false;
        }

        if (_add[v] != 0)
        {
            if (_hasAssign[left])
            {
                _assign[left] += _add[v];
                _sum[left] += _add[v] * (tm - tl + 1);
            }
            else
            {
                _add[left] += _add[v];
                _sum[left] += _add[v] * (tm - tl + 1);
            }

            if (_hasAssign[right])
            {
                _assign[right] += _add[v];
                _sum[right] += _add[v] * (tr - (tm + 1) + 1);
            }
            else
            {
                _add[right] += _add[v];
                _sum[right] += _add[v] * (tr - (tm + 1) + 1);
            }

            _add[v] = 0;
        }
    }

    public void Assign(int l, int r, long v)
    {
        Assign(1, 0, _n - 1, l, r - 1, v);
    }

    private void Assign(int v, int tl, int tr, int l, int r, long value)
    {
        if (l > r)
            return;

        if (l == tl && r == tr)
        {
            _sum[v] = value * (tr - tl + 1);
            _assign[v] = value;
            _hasAssign[v] = true;
            _add[v] = 0; 
            return;
        }

        Push(v, tl, tr);
        int tm = (tl + tr) / 2;

        Assign(v * 2, tl, tm, l, Math.Min(r, tm), value);
        Assign(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r, value);

        _sum[v] = _sum[v * 2] + _sum[v * 2 + 1];
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
            if (_hasAssign[v])
            {
                _assign[v] += value;
                _sum[v] += value * (tr - tl + 1);
            }
            else
            {
                _add[v] += value;
                _sum[v] += value * (tr - tl + 1);
            }
            return;
        }

        Push(v, tl, tr);
        int tm = (tl + tr) / 2;

        Add(v * 2, tl, tm, l, Math.Min(r, tm), value);
        Add(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r, value);

        _sum[v] = _sum[v * 2] + _sum[v * 2 + 1];
    }

    public long QuerySum(int l, int r)
    {
        return QuerySum(1, 0, _n - 1, l, r - 1);
    }

    private long QuerySum(int v, int tl, int tr, int l, int r)
    {
        if (l > r)
            return 0;

        if (l == tl && r == tr)
        {
            return _sum[v];
        }

        Push(v, tl, tr);
        int tm = (tl + tr) / 2;

        long leftSum = QuerySum(v * 2, tl, tm, l, Math.Min(r, tm));
        long rightSum = QuerySum(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r);

        return leftSum + rightSum;
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
                segmentTree.Assign(l, r, v);
            }
            else if (type == 2)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long v = long.Parse(operation[3]);
                segmentTree.Add(l, r, v);
            }
            else if (type == 3)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long sum = segmentTree.QuerySum(l, r);
                result.AppendLine(sum.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}