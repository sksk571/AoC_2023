var data = File.ReadAllLines(@"c:\tmp\input.txt");

List<Card> cards = new();

foreach (var line in data)
{
    var parts = line.Split(':', '|');
    var winningNumbers = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    var numbers = parts[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    cards.Add(new(winningNumbers, numbers));
}

int sum1 = cards.Sum(c => c.Points());
Console.WriteLine(sum1);

int[] copies = new int[cards.Count];

for (int i = 0; i < cards.Count; ++i)
{
    int matching = cards[i].MatchingNumbers();
    for (int j = i + 1, k = 0; j < copies.Length && k < matching; ++j, ++k)
    {
        copies[j] += (copies[i] + 1);
    }
}

int sum2 = copies.Sum(x => x + 1);
Console.WriteLine(sum2);

public record Card(List<int> WinningNumbers, List<int> Numbers)
{
    public int Points()
    {
        int points = 0;
        for (int k = MatchingNumbers(); k > 0; k--)
        {
            points = points == 0 ? 1 : points * 2;
        }
        return points;
    }
    
    public int MatchingNumbers()
    {
        return Numbers.Count(WinningNumbers.Contains);
    }
}