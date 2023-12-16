var data = File.ReadAllLines(@"c:\tmp\input.txt");

var grid = data.Select(l => l.ToCharArray()).ToArray();

int r1 = Energize(grid, 0, 0, 'R');
Console.WriteLine(r1);

int r2 = 0;

for (int y = 0; y < grid.Length; ++y)
{
    int lr = Energize(grid, 0, y, 'R'); 
    int rl = Energize(grid, grid[y].Length - 1, y, 'L');
    r2 = Math.Max(r2, Math.Max(lr, rl));
}

for (int x = 0; x < grid[0].Length; ++x)
{
    int ud = Energize(grid, x, 0, 'D'); 
    int du = Energize(grid, x, grid.Length - 1, 'U');
    r2 = Math.Max(r2, Math.Max(ud, du));
}
Console.WriteLine(r2);

int Energize(char[][] grid, int x, int y, char dir)
{
    var energized = new HashSet<(int x, int y, char)>();

    Navigate(grid, x, y, dir, energized);
    
    return energized.Select(c => (c.x, c.y)).Distinct().Count();
}

void Navigate(char[][] grid, int x, int y, char dir, HashSet<(int x, int y, char)> energized)
{
    if (y < 0 || y >= grid.Length || x < 0 || x >= grid[y].Length)
        return;
    //Console.WriteLine((x, y));
    
    switch (dir)
    {
        case 'R':
            for (; x < grid[0].Length; ++x)
            {
                if (!energized.Add((x, y, 'R')))
                    return;
                if (grid[y][x] == '\\')
                {
                    Navigate(grid, x, y+1, 'D', energized);
                    break;
                }
                else if (grid[y][x] == '/')
                {
                    Navigate(grid, x, y-1, 'U', energized);
                    break;
                }
                else if (grid[y][x] == '|')
                {
                    Navigate(grid, x, y-1, 'U', energized);
                    Navigate(grid, x, y+1, 'D', energized);
                    break;
                }
            }
            break;
        case 'L':
            for (; x >= 0; --x)
            {
                if (!energized.Add((x, y, 'L')))
                    return;
                if (grid[y][x] == '\\')
                {
                    Navigate(grid, x, y-1, 'U', energized);
                    break;
                }
                else if (grid[y][x] == '/')
                {
                    Navigate(grid, x, y+1, 'D', energized);
                    break;
                }
                else if (grid[y][x] == '|')
                {
                    Navigate(grid, x, y-1, 'U', energized);
                    Navigate(grid, x, y+1, 'D', energized);
                    break;
                }
            }
            break;
        case 'U':
            for (; y >= 0; --y)
            {
                if (!energized.Add((x, y, 'U')))
                    return;
                if (grid[y][x] == '\\')
                {
                    Navigate(grid, x-1, y, 'L', energized);
                    break;
                }
                else if (grid[y][x] == '/')
                {
                    Navigate(grid, x+1, y, 'R', energized);
                    break;
                }
                else if (grid[y][x] == '-')
                {
                    Navigate(grid, x-1, y, 'L', energized);
                    Navigate(grid, x+1, y, 'R', energized);
                    break;
                }
            }
            break;
        case 'D':
            for (; y < grid.Length; ++y)
            {
                if (!energized.Add((x, y, 'D')))
                    return;
                if (grid[y][x] == '\\')
                {
                    Navigate(grid, x+1, y, 'R', energized);
                    break;
                }
                else if (grid[y][x] == '/')
                {
                    Navigate(grid, x-1, y, 'L', energized);
                    break;
                }
                else if (grid[y][x] == '-')
                {
                    Navigate(grid, x-1, y, 'L', energized);
                    Navigate(grid, x+1, y, 'R', energized);
                    break;
                }
            }
            break;
    }
}