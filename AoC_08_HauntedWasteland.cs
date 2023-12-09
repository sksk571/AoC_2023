using System.Collections.Concurrent;

var data = File.ReadAllLines(@"c:\tmp\input.txt");

var instructions = data[0];
var map = new Map();
for (int i = 2; i < data.Length; ++i)
{
    MapNode.ParseAdd(data[i], map);
}

int r1 = map.Navigate(instructions.ToCharArray());
Console.WriteLine(r1);

long r2 = map.GhostNavigate(instructions.ToCharArray());
Console.WriteLine(r2);

public class Map
{
    private readonly Dictionary<string, MapNode> _nodes = new();
    
    public MapNode? Get(string name)
    {
        _nodes.TryGetValue(name, out var node);
        return node;
    }
    
    public MapNode[] GetAll(char suffix)
    {
        return _nodes.Keys.Where(k => k.EndsWith(suffix)).Select(k => _nodes[k]).ToArray();
    }
    
    public void Add(MapNode node)
    {
        _nodes.Add(node.Name, node);
    }
    
    public int Navigate(char[] instructions)
    {
        int steps = 0;
        var node = Get("AAA");
        if (node == null) return 0;
        
        while (true)
        {
            var nextNode = node.Get(instructions[steps % instructions.Length]);
            if (nextNode == null)
                return steps;
            //Console.WriteLine(nextNode.Name);
            
            steps++;
            
            if (nextNode.Name == "ZZZ")
                return steps;
            node = nextNode;
        }
    }
    
    public long GhostNavigate(char[] instructions)
    {
        var nodes = GetAll('A');
        var deltas = nodes.Select(n => GhostNavigate(n, instructions, 0).Item2).ToArray();
        return Lcd(deltas);
    }    
    
    private static (MapNode, long) GhostNavigate(MapNode startNode, char[] instructions, long steps)
    {        
        int index = (int)(steps % instructions.Length);
        var node = startNode;
        while (true)
        {
            node = node.Get(instructions[index])!;
            steps++;index++;
            if (index >= instructions.Length)
                index = 0;
            
            if (node.IsEnd)
                return (node, steps);
        }
    }
    
    private long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }
        return a;
    }
    
    private long Lcd(long a, long b)
    {
        return a * b / Gcd(a, b);
    }

    private long Lcd(IEnumerable<long> n)
    {
        return n.Aggregate(Lcd);
    }
}

public class MapNode
{
    private MapNode? _l, _r;

    public string Name { get; }
    
    public bool IsEnd { get; }

    public MapNode(string name)
    {
        Name = name;
        IsEnd = Name.EndsWith('Z');
    }
    
    public void Add(MapNode l, MapNode r)
    {
        (_l, _r) = (l, r);
    }
    
    public MapNode? Get(char dir)
    {
        return dir == 'R' ? _r : _l;
    }
    
    public static MapNode ParseAdd(string line, Map map)
    {
        var pattern = @"(\w{3}) = \((\w{3}), (\w{3})\)";
        var m = Regex.Match(line, pattern);
        var (name, lName, rName) = (m.Groups[1].Value,m.Groups[2].Value,m.Groups[3].Value);
        var node = map.Get(name);
        if (node == null)
        {
            node = new MapNode(name);
            map.Add(node);
        }
        var l = map.Get(lName);
        if (l == null)
        {
            l = new MapNode(lName);
            map.Add(l);
        }
        var r = map.Get(rName);
        if (r == null)
        {
            r = new MapNode(rName);
            map.Add(r);

        }
        node.Add(l, r);
        return node;
    }
}