using System;
using System.Text;

public class SegmentTree
{
    private readonly int[] _tree;
    private readonly int _n;

    public SegmentTree(int[] arr)
    {
        _n = arr.Length;
        _tree = new int[4 * _n];
        Build(arr, 1, 0, _n - 1);
    }

    private void Build(int[] arr, int v, int tl, int tr)
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

    public void Toggle(int index)
    {
        Toggle(1, 0, _n - 1, index);
    }

    private void Toggle(int v, int tl, int tr, int index)
    {
        if (tl == tr)
        {
            _tree[v] = 1 - _tree[v];
        }
        else
        {
            int tm = (tl + tr) / 2;
            if (index <= tm)
                Toggle(v * 2, tl, tm, index);
            else
                Toggle(v * 2 + 1, tm + 1, tr, index);

            _tree[v] = _tree[v * 2] + _tree[v * 2 + 1];
        }
    }
    public int FindKthOne(int k)
    {
        return FindKthOne(1, 0, _n - 1, k);
    }

    private int FindKthOne(int v, int tl, int tr, int k)
    {
        if (tl == tr)
        {
            return tl;
        }

        int tm = (tl + tr) / 2;
        if (_tree[v * 2] > k)
        {
            return FindKthOne(v * 2, tl, tm, k);
        }
        else
        {
            return FindKthOne(v * 2 + 1, tm + 1, tr, k - _tree[v * 2]);
        }
    }
}

public class Program
{
    public static void Main()
    {
        string[] firstLine = Console.ReadLine().Split();
        int n = int.Parse(firstLine[0]);
        int m = int.Parse(firstLine[1]);

        int[] array = new int[n];
        string[] arrayValues = Console.ReadLine().Split();
        for (int i = 0; i < n; i++)
        {
            array[i] = int.Parse(arrayValues[i]);
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
                segmentTree.Toggle(index);
            }
            else if (type == 2)
            {
                int k = int.Parse(operation[1]);
                int index = segmentTree.FindKthOne(k);
                result.AppendLine(index.ToString());
            }
        }

        Console.Write(result.ToString());
    }
}