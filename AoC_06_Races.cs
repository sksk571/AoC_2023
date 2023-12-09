var data = File.ReadAllLines(@"c:\tmp\input.txt");
long[] times = Parse(data[0]), distances = Parse(data[1]);

long r1 = 1;
for (int i = 0; i < times.Length; ++i)
{
    long ways = GetWaysToWin(times[i], distances[i]);
    if (ways > 0)
        r1 *= ways;
}
Console.WriteLine(r1);

var (time, distance) = (ParseSingle(data[0]), ParseSingle(data[1]));
long r2 = GetWaysToWin(time, distance);
Console.WriteLine(r2);

long GetWaysToWin(long time, long distance)
{
    int ways = 0;
    for (int j = 0; j <= time; ++j)
    {
        var d = GetDistance(j, time);
        if (d > distance)
        {
            ways++;
        }
    }
    return ways;
}

long GetDistance(long holdingTime, long totalTime)
{
    long speed = holdingTime;
    long distance = (totalTime - holdingTime) * speed;
    return distance;
}

string[] GetNumbers(string line)
{
    var parts = line.Split(':');
    var numbers = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    return numbers;
}

long[] Parse(string line)
{
    var numbers = GetNumbers(line);
    
    return numbers.Select(long.Parse).ToArray();
}

long ParseSingle(string line)
{
    var numbers = GetNumbers(line);
    
    return long.Parse(string.Join("", numbers));
}