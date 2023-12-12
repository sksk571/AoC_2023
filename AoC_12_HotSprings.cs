var data = File.ReadAllLines(@"c:\tmp\input.txt");

long r1 = 0;

foreach (var line in data)
{
    string[] parts = line.Split(' ');
    var pattern = parts[0];
    var checks = parts[1].Split(",").Select(int.Parse).ToArray();
    r1 += Configurations(pattern.ToCharArray(), checks);
}

Console.WriteLine(r1);

long r2 = 0;
foreach (var line in data)
{
    string[] parts = line.Split(' ');
    var pattern = string.Join("?", Enumerable.Range(0, 5).Select(_ => parts[0]));
    var checks = Enumerable.Range(0, 5).SelectMany(_ => parts[1].Split(",").Select(int.Parse)).ToArray();
    r2 += Configurations(pattern.ToCharArray(), checks);
}
Console.WriteLine(r2);

long Configurations(char[] pattern, int[] checks)
{
    var (p, n) = Parse(pattern);
    return GenerateNomograms(pattern.Length, checks, p, n, 0);
}

(Int128 positive, Int128 negative) Parse(char[] pattern)
{
    Int128 p = 0, n = 0, one = 1;
    for (int i = 0; i < pattern.Length; ++i)
    {
        if (pattern[i] == '#')
            p |= one << i;
        else if (pattern[i] == '.')
        {
            n |= one << i;
        }
    }
    return (p, n);
}

long GenerateNomograms(int length, int[] bits, Int128 p, Int128 n, Int128 wl, int j = 0, int i = 0)
{
    if (j >= bits.Length)
    {
        if ((wl & p) == p && (wl & n) == 0)
            return 1;
        return 0;
    }
    if (i >= length || (wl & n) != 0)
        return 0;

    int bit = bits[j];
    long r = 0;
    Int128 one = 1;
    for (; i < length; ++i)
    {
        Int128 bitMask = ((one << bit) - 1) << i;
        // is there a room to place this bit?
        if (i + bit <= length)
        {
            // place bit
            wl |= bitMask;
            var pMask = p & ((one << (i + bit)) - 1);
            if ((wl & pMask) == pMask)
            {
                r += GenerateNomograms(length, bits, p, n, wl, j + 1, i + bit + 1);
            }
            wl &= ~bitMask;
        }
    }
    return r;
}