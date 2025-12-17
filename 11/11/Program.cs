using System;
using System.Collections.Generic;
using System.Text;

public class SegmentTree
{
    class Node
    {
        public long len;          
        public int cnt;          
        public long sum;         
        public bool leftBlack;    
        public bool rightBlack; 
        public int assign;       
    }

    private readonly Node[] tree;
    private readonly int n;

    public SegmentTree(long[] lengths)
    {
        n = lengths.Length;
        tree = new Node[4 * n];
        Build(1, 0, n - 1, lengths);
    }

    private void Build(int v, int tl, int tr, long[] lengths)
    {
        if (tl == tr)
        {
            tree[v] = new Node
            {
                len = lengths[tl],
                cnt = 0,
                sum = 0,
                leftBlack = false,
                rightBlack = false,
                assign = -1
            };
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(v * 2, tl, tm, lengths);
            Build(v * 2 + 1, tm + 1, tr, lengths);
            tree[v] = new Node();
            tree[v].len = tree[v * 2].len + tree[v * 2 + 1].len;
            UpdateFromChildren(v);
        }
    }

    private void UpdateFromChildren(int v)
    {
        Node left = tree[v * 2];
        Node right = tree[v * 2 + 1];

        tree[v].cnt = left.cnt + right.cnt;
        if (left.rightBlack && right.leftBlack)
            tree[v].cnt--;

        tree[v].sum = left.sum + right.sum;
        tree[v].leftBlack = left.leftBlack;
        tree[v].rightBlack = right.rightBlack;
    }

    private void Push(int v)
    {
        if (tree[v].assign != -1)
        {
            Apply(v * 2, tree[v].assign);
            Apply(v * 2 + 1, tree[v].assign);
            tree[v].assign = -1;
        }
    }

    private void Apply(int v, int color)
    {
        if (color == 1)
        {
            tree[v].cnt = 1;
            tree[v].sum = tree[v].len;
            tree[v].leftBlack = true;
            tree[v].rightBlack = true;
        }
        else 
        {
            tree[v].cnt = 0;
            tree[v].sum = 0;
            tree[v].leftBlack = false;
            tree[v].rightBlack = false;
        }
        tree[v].assign = color;
    }

    public void Update(int l, int r, int color)
    {
        Update(1, 0, n - 1, l, r, color);
    }

    private void Update(int v, int tl, int tr, int l, int r, int color)
    {
        if (l > r)
            return;

        if (l == tl && r == tr)
        {
            Apply(v, color);
            return;
        }

        Push(v);
        int tm = (tl + tr) / 2;

        Update(v * 2, tl, tm, l, Math.Min(r, tm), color);
        Update(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r, color);

        UpdateFromChildren(v);
    }

    public (int cnt, long sum) GetAnswer()
    {
        return (tree[1].cnt, tree[1].sum);
    }
}

public class Program
{
    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        List<(char type, int x, int l)> operations = new List<(char, int, int)>();
        List<int> coords = new List<int>();

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine().Split();
            char type = parts[0][0];
            int x = int.Parse(parts[1]);
            int len = int.Parse(parts[2]);
            operations.Add((type, x, len));

            coords.Add(x);
            coords.Add(x + len);
        }
        coords.Sort();
        var uniqueCoords = new List<int>();
        foreach (int coord in coords)
        {
            if (uniqueCoords.Count == 0 || coord != uniqueCoords[uniqueCoords.Count - 1])
                uniqueCoords.Add(coord);
        }
        int m = uniqueCoords.Count - 1;
        long[] lengths = new long[m];
        for (int i = 0; i < m; i++)
            lengths[i] = uniqueCoords[i + 1] - uniqueCoords[i];

        SegmentTree segTree = new SegmentTree(lengths);
        StringBuilder output = new StringBuilder();

        foreach (var op in operations)
        {
            int l = op.x;
            int r = op.x + op.l;

            int lIdx = uniqueCoords.BinarySearch(l);
            int rIdx = uniqueCoords.BinarySearch(r);

            int color = op.type == 'B' ? 1 : 0;

            segTree.Update(lIdx, rIdx - 1, color);

            var (cnt, sum) = segTree.GetAnswer();
            output.AppendLine($"{cnt} {sum}");
        }

        Console.Write(output.ToString());
    }
}