var data = File.ReadAllLines(@"c:\tmp\input.txt");

List<(long[], int)> fields = new();
List<long> field = new();
int length = 0;

foreach (var line in data)
{
    if (string.IsNullOrEmpty(line))
    {
        fields.Add((field.ToArray(), length));
        field = new();
        continue;
    }
    var pattern = Parse(line);
    field.Add(pattern);
    length = line.Length;
}
fields.Add((field.ToArray(), length));

int r1 = 0;

foreach (var (f, len) in fields)
{
    int r = FindMirror(f);
    int c = FindMirror(Invert(f, len));
    if (r != -1)
        r1 += r * 100;
    if (c != -1)
        r1 += c;
}

Console.WriteLine(r1);


int r2 = 0;

foreach (var (f, len) in fields)
{
    int r = FindMirror(f, 1);
    int c = FindMirror(Invert(f, len), 1);
    if (r != -1)
        r2 += r * 100;
    if (c != -1)
        r2 += c;
}

Console.WriteLine(r2);

int FindMirror(long[] field, int diff = 0)
{
    for (int i = 1; i < field.Length; ++i)
    {
        int bits = 0;
        for (int j = i - 1, k = i; j >= 0 && k < field.Length; --j,++k)
        {
            bits += Difference(field[j], field[k]);
        }
        if (bits == diff)
        {
            return i;
        }
    }
    return -1;
}

int Difference(long a, long b)
{
    long xor = a ^ b;
    int count = 0;
    while (xor != 0)
    {
        xor &= (xor - 1);
        count++;
    }
    return count;
}

long Parse(string pattern)
{
    long r = 0;
    for (int i = 0; i < pattern.Length; ++i)
    {
        if (pattern[i] == '#')
            r |= 1L << i;
    }
    return r;
}

long[] Invert(long[] field, int length)
{
    long[] r = new long[length];
    for (int i = 0; i < field.Length; ++i)
    {
        long pattern = field[i];
        for (int j = 0; j < length; ++j)
        {
            if ((pattern & (1L << j)) != 0)
                r[j] |= 1L << i;
        }
    }
    return r;
}