var data = File.ReadAllLines(@"c:\tmp\input.txt");

char[][] grid = data.Select(s => s.ToCharArray()).ToArray();

List<((int x, int y), (int x, int y))> edges = new();
(int x, int y) s = (0, 0);

for (int y = 0; y < grid.Length; ++y)
{
    for (int x = 0; x < grid[y].Length; ++x)
    {
        if (grid[y][x] == 'S')
            s = (x, y);
        var e = Edges(x, y, grid);
        edges.AddRange(e);
    }
}

var adjList = edges.ToLookup(e => e.Item1);

var cycle = Cycle(s, adjList);

int r1 = (cycle.Count / 2 + cycle.Count % 2);
Console.WriteLine(r1);

var allPoints = cycle.SelectMany(e => new[] { e.Item1, e.Item2 }).ToHashSet();

int r2 = 0;
for (int y = 0; y < grid.Length - 1; ++y)
{
    bool inside = false;
    for (int x = 0; x < grid[y].Length; ++x)
    {
        if (cycle.Contains(((x, y), (x, y + 1))) || cycle.Contains(((x, y + 1), (x, y))))
        {
            inside = !inside;
        }
        else if (!allPoints.Contains((x, y + 1)) && inside)
        {
            r2++;
        }
    }
}

Console.WriteLine(r2);

HashSet<((int x, int y), (int x, int y))> Cycle((int, int) s, ILookup<(int, int), ((int, int), (int, int))> adjList)
{
    HashSet<(int, int)> visited = new();
    Stack<(int, int)> stack = new();
    stack.Push(s);
    HashSet<((int, int), (int, int))> path = new();
    while (stack.Count != 0)
    {
        var v = stack.Pop();
        foreach (var e in adjList[v].Where(e => visited.Add(e.Item2)))
        {
            if (!path.Contains((v, e.Item2)) && !path.Contains((e.Item2, v)))
                path.Add((v, e.Item2));
            stack.Push(e.Item2);
        }
    }
    return path;
}

IEnumerable<((int x, int y), (int x, int y))> Edges(int x, int y, char[][] grid)
{
    char c = grid[y][x];
    ((int x, int y) p1, (int x, int y) p2, var comp1, var comp2) = c switch
    {
        '|' => ((x, y - 1), (x, y + 1), ('7','|','F'), ('J','|','L')),
        '-' => ((x - 1, y), (x + 1, y), ('F','-','L'), ('J','-','7')),
        'J' => ((x, y - 1), (x - 1, y), ('F','|','7'), ('F','-','L')),
        'L' => ((x, y - 1), (x + 1, y), ('F','|','7'), ('J','-','7')),
        '7' => ((x - 1, y), (x, y + 1), ('F','-','L'), ('J','|','L')),
        'F' => ((x + 1, y), (x, y + 1), ('7','-','J'), ('J','|','L')),
        _ => ((x, y), (x, y), (c, c, c), (c, c, c))
    };
    
    if (p1 != p2)
    {
        if (p1.y >= 0 && p1.y < grid.Length && p1.x >= 0 && p1.x < grid[p1.y].Length)
        {
            char c1 = grid[p1.y][p1.x];
            if (c1 == 'S' || comp1.Item1 == c1 || comp1.Item2 == c1 || comp1.Item3 == c1)
            {
                yield return ((x, y), p1);
                yield return (p1, (x, y));
            }
        }
        if (p2.y >= 0 && p2.y < grid.Length && p2.x >= 0 && p2.x < grid[p2.y].Length)
        {
            char c2 = grid[p2.y][p2.x];
            if (c2 == 'S' || comp2.Item1 == c2 || comp2.Item2 == c2 || comp2.Item3 == c2)
            {
                yield return ((x, y), p2);
                yield return (p2, (x, y));
            }
        }
    }
}