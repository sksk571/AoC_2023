var reader = new StreamReader(File.OpenRead(@"c:\tmp\input.txt"));

var line = reader.ReadLine();
if (line == null)
    return;
    
long[] seeds = line[7..].Split(' ').Select(long.Parse).ToArray();
reader.ReadLine();

List<Map> maps = new();

Map? map = Map.Parse(reader);
while (map != null)
{
    maps.Add(map);
    map = Map.Parse(reader);
}

long r1 = seeds.Select(s => maps.Aggregate(s, (s_prev, map) => map.Convert(s_prev))).Min();
Console.WriteLine(r1);

var seedRanges = Enumerable.Range(0, seeds.Length/2).Select(i => (Seed: seeds[i], Length: seeds[i+1])).ToArray();
Console.WriteLine($"Total seeds: {seedRanges.Sum(sr => sr.Length):0,000}");
foreach (var m in maps)
{
    seedRanges = m.Convert(seedRanges).ToArray();
}

long r2 = seedRanges.Min(r => r.Seed);
Console.WriteLine(r2);

public record Map()
{
    public List<(long Dst, long Src, long Length)> Ranges = new();
    
    public static Map? Parse(TextReader reader)
    {
        var title = reader.ReadLine();
        if (title == null)
        {
            return null;
        }
        var map = new Map();
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "")
                break;
            var numbers = line.Split(' ').Select(long.Parse).ToList();
            map.Ranges.Add((numbers[0], numbers[1], numbers[2]));
        }
        map.Ranges = map.Ranges.OrderBy(r => r.Src).ToList();
        return map;
    }

    public override string ToString()
    {
        return string.Join(", ", Ranges.Select(r => $"({r.Src},{r.Length})->({r.Dst},{r.Length})"));
    }

    public long Convert(long source)
    {
        var r = Convert(source, 1);
        return r.Single().Dst;
    }
    
    public IEnumerable<(long Dst, long Length)> Convert(long source, long length)
    {
        long s = source, end = s + length;
        foreach (var (Dst, Src, Length) in Ranges)
        {
            if (s >= Src + Length) continue;
            if (s < Src)
            {
                if (end <= Src)
                {
                    yield return (s, end - s);
                    yield break;
                }
                else
                {
                    yield return (s, Src - s);
                    s = Src;
                }
            }
            
            if (end <= Src + Length)
            {
                yield return (Dst + s - Src, end - s);
                yield break;
            }
            else
            {
                yield return (Dst + s - Src, Length - (s - Src));
                s = Src + Length;
            }
            
        }
        if (s < end)
        {
            yield return (s, end - s);
        }
    }
    
    public IEnumerable<(long Dst, long Length)> Convert(IEnumerable<(long Src, long Length)> ranges)
    {
        return ranges.SelectMany(r => Convert(r.Src, r.Length));
    }
}