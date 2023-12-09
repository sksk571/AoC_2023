var data = File.ReadAllLines(@"c:\tmp\input.txt");

List<(string number, int y, int begin, int end)> numbers = new();

foreach (var (line, y) in data.Select((l, i) => (l, i)))
{
    int i = 0;
    for (; i < line.Length; ++i)
    {
        string number = "";
        int begin = i, end;
        while (i < line.Length && char.IsDigit(line[i]))
        {
            number += line[i];
            i++;
        }
        end = i;
        if (!string.IsNullOrEmpty(number))
        {
            if (begin > 0) begin--;
            if (end >= line.Length) end--;
            
            numbers.Add((number, y, begin , end));
           
        }
    }
}

int sum = 0;


foreach (var (number, y, begin, end) in numbers)
{
    for (int x = begin; x <= end; ++x)
    {
        if (new[] { -1, 0, +1 }.Select(d => d + y).Where(row => row >= 0 && row < data.Length)
            .Any(row => data[row][x] != '.' && !char.IsDigit(data[row][x])))
        {
            sum += int.Parse(number);
            break;
        }
    }
}

int sum2 = 0;

foreach (var (line, y) in data.Select((l, i) => (l, i)))
{
    for (int x = 0; x < line.Length; ++x)
    {
        if (line[x] == '*')
        {
            var adj = numbers.Where(n => (n.y == y || n.y == y - 1 || n.y == y + 1) && (x >= n.begin && x <= n.end)).ToList();
            if (adj.Count == 2)
            {
                sum2 += int.Parse(adj[0].number) * int.Parse(adj[1].number);
            }
        }
    }
}
Console.WriteLine(sum);Console.WriteLine(sum2);