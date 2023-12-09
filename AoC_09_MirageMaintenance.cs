var data = File.ReadAllLines(@"c:\tmp\input.txt");

int r1 = 0, r2 = 0;

foreach (var line in data)
{
    int[] numbers = line.Split(' ').Select(int.Parse).ToArray();
    Stack<int> lastNumbers = new(), firstNumbers = new();
    
    int j = numbers.Length;
    do
    {
        firstNumbers.Push(numbers[0]);
        for (int i = 1; i < j; ++i)
        {
            numbers[i-1] = numbers[i] - numbers[i-1];
        }
        j--;
        lastNumbers.Push(numbers[j]);
    } while (j > 0);
    
    //Console.WriteLine(string.Join(", ", lastNumbers));
    r1 += lastNumbers.Sum();
    
    //Console.WriteLine(string.Join(", ", firstNumbers));
    
    int n = 0;
    while (firstNumbers.TryPop(out int tmp))
    {
        n = tmp - n;        
    }
    r2 += n;
    //Console.WriteLine(n);
}

Console.WriteLine(r1);Console.WriteLine(r2);