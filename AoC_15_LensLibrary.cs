var data = File.ReadAllText(@"c:\tmp\input.txt");

var steps = data.Split(',');

int r1 = 0;
foreach (var step in steps)
{
    r1 += Hash(step);
}

Console.WriteLine(r1);

LinkedList<ListValue>[] hashmap = new LinkedList<ListValue>[256];
var separators = new[] { '-', '=' };
foreach (var step in steps)
{
    int i = step.IndexOfAny(separators);
    string label = step[..i];
    int hash = Hash(label);
    var list = hashmap[hash];
    if (list == null)
    {
        list = new LinkedList<ListValue>();
        hashmap[hash] = list;
    }
    var node = list.Find(new ListValue(label, 0));
    if (step[i] == '-' && node != null)
    {
        list.Remove(node);
    }
    if (step[i] == '=')
    {
        int value = int.Parse(step[(i + 1)..]);
        if (node == null)
        {
            list.AddLast(new ListValue(label, value));
        }
        else 
            node.Value = new ListValue(label, value);
    }
}

int r2 = 0;

for (int i = 0; i < hashmap.Length; ++i)
{
    var list = hashmap[i];
    if (list == null) continue;
    var node = list.First;
    for (int j = 0; node != null; ++j)
    {
        r2 += (i + 1) * (j + 1) * node.Value.Value;
        node = node.Next;
    }
}

Console.WriteLine(r2);

int Hash(string s)
{
    int h = 0, mask = ((1 << 8) - 1);
    for (int i = 0; i < s.Length; ++i)
    {
        int c = (int)s[i];
        h += c;
        h *= 17;
        h &= mask;
    }
    return h;
}

public record ListValue(string Key, int Value)
{
    public virtual bool Equals(ListValue? other) => other != null && Key == other.Key;

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}