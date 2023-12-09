var data = File.ReadAllLines(@"c:\tmp\input.txt");

var pattern = @"(\d+) (red|green|blue)(;)?";

List<Game> games = new();

foreach (var line in data)
{
    //Console.WriteLine(line);
    var id = int.Parse(Regex.Match(line, @"Game (\d+)").Groups[1].ToString());
    var game = new Game(id);
    var matches = Regex.Matches(line, pattern);
    List<Game.Ball> balls = new();
    
    foreach (Match match in matches.Cast<Match>())
    {
        //Console.WriteLine(match.Groups[0]);
        var count = int.Parse(match.Groups[1].ToString());
        var color = match.Groups[2].ToString();
        var isTerminator = match.Groups[3].Success;
        balls.Add(new(count, color));
        if (isTerminator)
        {
            game.Picks.Add(new(balls));
            balls = new();
        }
    }
    game.Picks.Add(new Game.Pick(balls));
    games.Add(game);
}

var possibleGames = games.Where(g => g.IsPossible(12, 13, 14));
//possibleGames.Dump();
int sum1 = possibleGames.Sum(g => g.Id);
Console.WriteLine(sum1);
int sum2 = games.Sum(g => g.Power());
Console.WriteLine(sum2);

record Game(int Id)
{
    public List<Pick> Picks = new();
    
    public bool IsPossible(int maxRed, int maxGreen, int maxBlue)
    {
        foreach (var pick in Picks)
        {
            if (pick["red"] > maxRed || pick["green"] > maxGreen ||
                pick["blue"] > maxBlue)
                return false;
        }
        return true;
    }
    public int Power()
    {
        int red = Picks.Max(p => p["red"]);
        int green = Picks.Max(p => p["green"]);
        int blue = Picks.Max(p => p["blue"]);
        return red * green * blue;
    }

    public override string ToString()
    {
        return Id + ":" + string.Join(";", Picks);
    }
  
    public record Pick(List<Ball> Balls)
    {
        public int this[string color]
        {
            get => Balls.Where(b => b.Color == color).Sum(b => b.Count);
        }
        public override string ToString()
        {
            return string.Join(",", Balls);
        }
    }
    public record Ball(int Count, string Color)
    {
        public override string ToString()
        {
            return $"{Count} {Color}";
        }
    }
}