using System;
using System.Text;

public class SegmentTree
{
    private const long INF = long.MaxValue;
    private const long NEG_INF = long.MinValue;

    private readonly int _n;
    private readonly long[] _min;
    private readonly long[] _max;
    private readonly int[] _count;
    private readonly bool[] _clearFlag;

    public SegmentTree(int n)
    {
        _n = n;
        _min = new long[4 * n];
        _max = new long[4 * n];
        _count = new int[4 * n];
        _clearFlag = new bool[4 * n];
        Build(1, 0, n - 1);
    }

    private void Build(int v, int tl, int tr)
    {
        if (tl == tr)
        {
            _min[v] = INF;
            _max[v] = NEG_INF;
            _count[v] = 0;
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(v * 2, tl, tm);
            Build(v * 2 + 1, tm + 1, tr);
            UpdateNode(v);
        }
    }

    private void Push(int v)
    {
        if (_clearFlag[v])
        {
            _min[v * 2] = INF;
            _max[v * 2] = NEG_INF;
            _count[v * 2] = 0;
            _clearFlag[v * 2] = true;

            _min[v * 2 + 1] = INF;
            _max[v * 2 + 1] = NEG_INF;
            _count[v * 2 + 1] = 0;
            _clearFlag[v * 2 + 1] = true;

            _clearFlag[v] = false;
        }
    }

    private void UpdateNode(int v)
    {
        _min[v] = Math.Min(_min[v * 2], _min[v * 2 + 1]);
        _max[v] = Math.Max(_max[v * 2], _max[v * 2 + 1]);
        _count[v] = _count[v * 2] + _count[v * 2 + 1];
    }
    public void Update(int index, long h)
    {
        Update(1, 0, _n - 1, index, h);
    }

    private void Update(int v, int tl, int tr, int index, long h)
    {
        if (tl == tr)
        {
            _min[v] = h;
            _max[v] = h;
            _count[v] = 1;
            _clearFlag[v] = false;
        }
        else
        {
            Push(v);
            int tm = (tl + tr) / 2;

            if (index <= tm)
                Update(v * 2, tl, tm, index, h);
            else
                Update(v * 2 + 1, tm + 1, tr, index, h);

            UpdateNode(v);
        }
    }
    public int Earthquake(int l, int r, long p)
    {
        return Earthquake(1, 0, _n - 1, l, r - 1, p);
    }

    private int Earthquake(int v, int tl, int tr, int l, int r, long p)
    {
        if (l > r)
            return 0;

        if (tr < l || r < tl)
            return 0;
        if (_min[v] > p)
            return 0;

        if (l <= tl && tr <= r && _max[v] <= p)
        {
            int destroyed = _count[v];
            _min[v] = INF;
            _max[v] = NEG_INF;
            _count[v] = 0;
            _clearFlag[v] = true;
            return destroyed;
        }

        if (tl == tr)
        {
            int destroyed = _count[v];
            _min[v] = INF;
            _max[v] = NEG_INF;
            _count[v] = 0;
            return destroyed;
        }

        Push(v);
        int tm = (tl + tr) / 2;

        int leftDestroyed = Earthquake(v * 2, tl, tm, l, r, p);
        int rightDestroyed = Earthquake(v * 2 + 1, tm + 1, tr, l, r, p);

        UpdateNode(v);

        return leftDestroyed + rightDestroyed;
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
                int index = int.Parse(operation[1]);
                long h = long.Parse(operation[2]);
                segmentTree.Update(index, h);
            }
            else if (type == 2)
            {
                int l = int.Parse(operation[1]);
                int r = int.Parse(operation[2]);
                long p = long.Parse(operation[3]);
                int destroyed = segmentTree.Earthquake(l, r, p);
                result.AppendLine(destroyed.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}