var data = File.ReadAllLines(@"c:\tmp\input.txt");

var hands = data.Select(Hand.Parse).ToArray();
Array.Sort(hands);

int r1 = 0;
for (int i = 0; i< hands.Length; ++i)
{
    r1 += (i + 1) * hands[i].Bid;
}

Console.WriteLine(r1);

hands = data.Select(Hand.ParseCombinatoricHand).ToArray();
Array.Sort(hands);

int r2 = 0;
for (int i = 0; i< hands.Length; ++i)
{
    r2 += (i + 1) * hands[i].Bid;
}

Console.WriteLine(r2);


public class CombinatoricHand : Hand
{
    private static readonly char[] AllCards = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

    public CombinatoricHand(string cards, int bid) : base(cards, bid)
    {
    }

    protected override HandType GetHandType(IEnumerable<char> cards)
    {
        char[] newCards = cards.ToArray();
        int i = Array.IndexOf(newCards, 'J');
        if (i == -1)
            return base.GetHandType(cards);
        HandType maxType = HandType.HighCard;
        foreach (var card in AllCards)
        {
            newCards[i] = card;
            var newType = GetHandType(newCards);
            if (newType > maxType)
                maxType = newType;
        }
        return maxType;
    }

    protected override int GetCardRank(char card)
    {
        if (card == 'J') return 0;
        return base.GetCardRank(card);
    }
}

public class Hand : IComparable<Hand>
{
    private readonly string cards;

    public Hand(string cards, int bid)
    {
        this.cards = cards;
        Bid = bid;
    }
    
    public HandType Type => GetHandType(cards);
    public int Bid { get; }

    public int CompareTo(Hand? other)
    {
        if (other is null) return 0;
        
        int typeDiff = (int)Type - (int)other.Type;
        if (typeDiff != 0)
            return typeDiff;
        
        for (int i = 0; i < cards.Length; ++i)
        {
            int rankDiff = GetCardRank(cards[i]) - GetCardRank(other.cards[i]);
            if (rankDiff != 0)
                return rankDiff;
        }
        return 0;
    }

    public override string ToString()
    {
        return $"{Type} ({this.cards})";
    }
  
    public enum HandType
    {
        HighCard = 0,
        Pair = 1,
        TwoPairs = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }
    
    public static Hand Parse(string line)
    {
        var parts = line.Split(' ');
        return new Hand(parts[0], int.Parse(parts[1]));
    }
    
    public static CombinatoricHand ParseCombinatoricHand(string line)
    {
        var parts = line.Split(' ');
        return new CombinatoricHand(parts[0], int.Parse(parts[1]));
    }
    
    protected virtual HandType GetHandType(IEnumerable<char> cards)
    {
        var groups = cards.GroupBy(c => c).Select(g => g.Count()).OrderDescending().ToList();
        return groups switch
        {
            [5] => HandType.FiveOfAKind,
            [4,1] => HandType.FourOfAKind,
            [3,2] => HandType.FullHouse,
            [3,1,1] => HandType.ThreeOfAKind,
            [2,2,1] => HandType.TwoPairs,
            [2,1,1,1] => HandType.Pair,
            _ => HandType.HighCard
        };
    }
    
    protected virtual int GetCardRank(char card)
    {
        return card switch
        {
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => card - '0'
        };
    }
}