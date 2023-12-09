var data = File.ReadAllLines(@"c:\tmp\input.txt");

int sum = 0;
foreach (var line in data)
{
    int value = int.Parse(string.Concat(line.First(c => Char.IsDigit(c)), line.Last(c => Char.IsDigit(c))));
    sum += value;
}

Console.WriteLine(sum);


int sum2 = 0;

string[] digitsSpelled = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

foreach (var line in data)
{
    var digits = FindDigits(line).ToList();
    int value = int.Parse(digits[0].ToString() + digits[^1].ToString());
    sum2 += value;
}

IEnumerable<int> FindDigits(string line)
{
    for (int i = 0; i < line.Length; ++i)
    {
        if (char.IsDigit(line[i]))
            yield return (int)line[i] - '0';
        int index = 0;
        foreach (var d in digitsSpelled)
        {
            if (line.Length - i >= d.Length && line.Substring(i, d.Length) == d)
                yield return index;
            index++;
        }
    }
}

Console.WriteLine(sum2);