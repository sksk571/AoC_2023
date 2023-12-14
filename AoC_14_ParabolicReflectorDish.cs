var data = File.ReadAllLines(@"c:\tmp\input.txt");

char[][] grid = data.Select(l => l.ToCharArray()).ToArray();

Tilt(grid, 'N');
int r1 = CalculateLoad(grid);
Console.WriteLine(r1);

List<long> states = new();
List<int> loads = new();

int i, j = 0;

for (i = 0; ; i++)
{
    Cycle(grid);
    var h = CalculateHash(grid);
    j = states.IndexOf(h);
    if (j != -1)
        break;
    states.Add(h);
    loads.Add(CalculateLoad(grid));
}
// Loads between j and i are repeating
int r2 = loads[j + ((999999999 - j) % (i - j))];
Console.WriteLine(r2);

void Cycle(char[][] grid)
{
    Tilt(grid, 'N');
    Tilt(grid, 'W');
    Tilt(grid, 'S');
    Tilt(grid, 'E');
}

void Tilt(char[][] grid, char dir)
{
    if (dir == 'N')
    {
        for (int i = 1; i < grid.Length; ++i)
        {
            for (int j = 0; j < grid[i].Length; ++j)
            {
                if (grid[i][j] == 'O')
                {
                    for (int k = i-1; k >= 0 && grid[k][j] == '.'; --k)
                    {
                        grid[k+1][j] = '.';
                        grid[k][j] = 'O';
                    }
                }
            }
        }
    }
    else if (dir == 'S')
    {
        for (int i = grid.Length - 2; i >= 0; --i)
        {
            for (int j = 0; j < grid[i].Length; ++j)
            {
                if (grid[i][j] == 'O')
                {
                    for (int k = i+1; k < grid.Length && grid[k][j] == '.'; ++k)
                    {
                        grid[k-1][j] = '.';
                        grid[k][j] = 'O';
                    }
                }
            }
        }
    }
    else if (dir == 'W')
    {
        for (int i = 0; i < grid.Length; ++i)
        {
            for (int j = 1; j < grid[i].Length; ++j)
            {
                if (grid[i][j] == 'O')
                {
                    for (int k = j-1; k >= 0 && grid[i][k] == '.'; --k)
                    {
                        grid[i][k+1] = '.';
                        grid[i][k] = 'O';
                    }
                }
            }
        }
    }
    else if (dir == 'E')
    {
        for (int i = 0; i < grid.Length; ++i)
        {
            for (int j = grid[i].Length - 2; j >= 0; --j)
            {
                if (grid[i][j] == 'O')
                {
                    for (int k = j+1; k < grid[i].Length && grid[i][k] == '.'; ++k)
                    {
                        grid[i][k-1] = '.';
                        grid[i][k] = 'O';
                    }
                }
            }
        }
    }
}

int CalculateLoad(char[][] grid)
{
    int r = 0;
    for (int i = 0; i < grid.Length; ++i)
    {
        for (int j = 0; j < grid[i].Length; ++j)
        {
            if (grid[i][j] == 'O')
            {
                r += grid.Length - i;
            }
        }
    }
    return r;
}

long CalculateHash(char[][] grid)
{
    long h = 0;
    for (int i = 0; i < grid.Length; ++i)
    {
        for (int j = 0; j < grid[i].Length; ++j)
        {
            if (grid[i][j] == 'O')
            {
                h += i * 1000 + j;
            }
        }
    }
    return h;
}