using System;
using System.Collections.Generic;
using System.Text;

public class Solution
{
    class Event
    {
        public long x;
        public int type;
        public int rectIndex;
    }

    class Rectangle
    {
        public long x1, y1, x2, y2;
    }

    class Node
    {
        public int max;
        public int pos;  
        public int add;
    }

    static Node[] tree;

    static Node Combine(Node left, Node right)
    {
        Node res = new Node();
        if (left.max > right.max)
        {
            res.max = left.max;
            res.pos = left.pos;
        }
        else
        {
            res.max = right.max;
            res.pos = right.pos;
        }
        return res;
    }

    static void Push(int v)
    {
        if (tree[v].add != 0)
        {
            tree[v * 2].max += tree[v].add;
            tree[v * 2].add += tree[v].add;
            tree[v * 2 + 1].max += tree[v].add;
            tree[v * 2 + 1].add += tree[v].add;
            tree[v].add = 0;
        }
    }

    static void Update(int v, int tl, int tr, int l, int r, int add)
    {
        if (l > r) return;
        if (tl == l && tr == r)
        {
            tree[v].max += add;
            tree[v].add += add;
            return;
        }
        Push(v);
        int tm = (tl + tr) / 2;
        Update(v * 2, tl, tm, l, Math.Min(r, tm), add);
        Update(v * 2 + 1, tm + 1, tr, Math.Max(l, tm + 1), r, add);
        tree[v] = Combine(tree[v * 2], tree[v * 2 + 1]);
    }

    static void Build(int v, int tl, int tr)
    {
        if (tl == tr)
        {
            tree[v] = new Node { max = 0, pos = tl, add = 0 };
        }
        else
        {
            int tm = (tl + tr) / 2;
            Build(v * 2, tl, tm);
            Build(v * 2 + 1, tm + 1, tr);
            tree[v] = Combine(tree[v * 2], tree[v * 2 + 1]);
        }
    }

    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        List<Rectangle> rects = new List<Rectangle>();
        List<long> ys = new List<long>();

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine().Split();
            long x1 = long.Parse(parts[0]);
            long y1 = long.Parse(parts[1]);
            long x2 = long.Parse(parts[2]);
            long y2 = long.Parse(parts[3]);
            if (x1 > x2)
            {
                long temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                long temp = y1;
                y1 = y2;
                y2 = temp;
            }

            rects.Add(new Rectangle { x1 = x1, y1 = y1, x2 = x2, y2 = y2 });
            ys.Add(y1);
            ys.Add(y2);
        }

        long minY = long.MaxValue, maxY = long.MinValue;
        foreach (long y in ys)
        {
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }
        int L = (int)(maxY - minY + 2);
        tree = new Node[4 * (L - 1)];
        Build(1, 0, L - 2);
        List<Event> events = new List<Event>();
        for (int i = 0; i < rects.Count; i++)
        {
            Rectangle rect = rects[i];
            events.Add(new Event { x = rect.x1, type = 1, rectIndex = i });
            events.Add(new Event { x = rect.x2 + 1, type = -1, rectIndex = i });
        }
        events.Sort((a, b) => a.x.CompareTo(b.x));

        int maxCover = 0;
        long bestX = 0, bestY = 0;

        int eventIndex = 0;
        while (eventIndex < events.Count)
        {
            long currentX = events[eventIndex].x;

            while (eventIndex < events.Count && events[eventIndex].x == currentX)
            {
                Event e = events[eventIndex];
                Rectangle rect = rects[e.rectIndex];
                int l = (int)(rect.y1 - minY);
                int r = (int)(rect.y2 - minY);
                Update(1, 0, L - 2, l, r, e.type);
                eventIndex++;
            }

            if (tree[1].max > maxCover)
            {
                maxCover = tree[1].max;
                bestX = currentX;
                bestY = minY + tree[1].pos;
            }
        }

        Console.WriteLine(maxCover);
        Console.WriteLine(bestX + " " + bestY);
    }
}