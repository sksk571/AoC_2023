var data = File.ReadAllLines(@"c:\tmp\input.txt");

List<(int x, int y)> galaxies = new();
List<int> emptyRows = new(), emptyColumns = new();

for (int y = 0; y < data.Length; ++y)
{
    bool emptyRow = true, emptyColumn = true;
    for (int x = 0; x < data[y].Length; ++x)
    {
        if (data[x][y] == '#')
        {
            galaxies.Add((x, y));
            emptyRow = false;
        }
        if (data[y][x] == '#')
        {
            emptyColumn = false;
        }
    }
    if (emptyRow)
        emptyRows.Add(y);
    if (emptyColumn)
        emptyColumns.Add(y);
}

long r1 = ShortestPaths(galaxies, 2);

Console.WriteLine(r1);

long r2 = ShortestPaths(galaxies, 1000000);

Console.WriteLine(r2);


long ShortestPaths(List<(int x, int y)> galaxies, int expansionFactor)
{
    var q = new Queue<(int x, int y)>(galaxies);
    long r = 0;
    while (q.Count != 0)
    {
        var g1 = q.Dequeue();
        foreach (var g2 in q)
        {
            int minX = Math.Min(g1.x, g2.x), maxX = Math.Max(g1.x, g2.x);
            int minY = Math.Min(g1.y, g2.y), maxY = Math.Max(g1.y, g2.y);
            long d = maxX - minX + maxY - minY + emptyRows.Where(r => r >= minY && r <= maxY).Count() * (expansionFactor-1) + emptyColumns.Where(c => c >= minX && c <= maxX).Count() * (expansionFactor-1);
            r += d;
        }
    }
    return r;
}