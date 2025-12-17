using System;
using System.Text;

public class Fenwick3D
{
    private readonly long[,,] tree;
    private readonly int n;

    public Fenwick3D(int size)
    {
        n = size;
        tree = new long[n + 2, n + 2, n + 2];
    }

    private void UpdateInternal(int x, int y, int z, long delta)
    {
        x++; y++; z++;
        for (int i = x; i <= n; i += i & -i)
            for (int j = y; j <= n; j += j & -j)
                for (int k = z; k <= n; k += k & -k)
                    tree[i, j, k] += delta;
    }

    public void Update(int x, int y, int z, long delta)
    {
        UpdateInternal(x, y, z, delta);
    }

    private long QueryInternal(int x, int y, int z)
    {

        x++; y++; z++;
        long result = 0;
        for (int i = x; i > 0; i -= i & -i)
            for (int j = y; j > 0; j -= j & -j)
                for (int k = z; k > 0; k -= k & -k)
                    result += tree[i, j, k];
        return result;
    }

    public long Query(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        long result = QueryInternal(x2, y2, z2);
        result -= QueryInternal(x1 - 1, y2, z2);
        result -= QueryInternal(x2, y1 - 1, z2);
        result -= QueryInternal(x2, y2, z1 - 1);
        result += QueryInternal(x1 - 1, y1 - 1, z2);
        result += QueryInternal(x1 - 1, y2, z1 - 1);
        result += QueryInternal(x2, y1 - 1, z1 - 1);
        result -= QueryInternal(x1 - 1, y1 - 1, z1 - 1);
        return result;
    }
}

public class Program
{
    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        Fenwick3D fenwick = new Fenwick3D(n);
        StringBuilder output = new StringBuilder();

        while (true)
        {
            string line = Console.ReadLine();
            if (line == null) break;

            string[] parts = line.Split();
            int type = int.Parse(parts[0]);

            if (type == 3) 
                break;

            if (type == 1) 
            {
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);
                int z = int.Parse(parts[3]);
                long k = long.Parse(parts[4]);
                fenwick.Update(x, y, z, k);
            }
            else if (type == 2) 
            {
                int x1 = int.Parse(parts[1]);
                int y1 = int.Parse(parts[2]);
                int z1 = int.Parse(parts[3]);
                int x2 = int.Parse(parts[4]);
                int y2 = int.Parse(parts[5]);
                int z2 = int.Parse(parts[6]);

                long sum = fenwick.Query(x1, y1, z1, x2, y2, z2);
                output.AppendLine(sum.ToString());
            }
        }

        Console.Write(output.ToString());
    }
}