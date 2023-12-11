var data = File.ReadAllLines(@"c:\tmp\input.txt");

var hands = data.Select(Hand.Parse).ToArray();
Array.Sort(hands);

int r1 = 0;
for (int i = 0; i< hands.Length; ++i)
{
    r1 += (i + 1) * hands[i].Bid;
}

Console.WriteLine(r1);

var combinatoricHands = hands.Select(h => new CombinatoricHand(h)).ToArray();
Array.Sort(combinatoricHands);

int r2 = 0;
for (int i = 0; i< combinatoricHands.Length; ++i)
{
    r2 += (i + 1) * combinatoricHands[i].Bid;
}

Console.WriteLine(r2);

public class CombinatoricHand : IComparable<CombinatoricHand>
{
    private static readonly char[] AllCards = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    
    private readonly Hand hand;

    public CombinatoricHand(Hand h)
    {
        this.hand = h;
        Bid = h.Bid;
        Type = GetHandType(Cards);
    }
    
    public char[] Cards => hand.Cards;
    public HandType Type { get; }
    public int Bid { get; }

    public int CompareTo(CombinatoricHand? other)
    {
        if (other is null) return 0;
        
        int typeDiff = (int)Type - (int)other.Type;
        if (typeDiff != 0)
            return typeDiff;
            
        var cards = Cards;
        var otherCards = other.Cards;
        
        for (int i = 0; i < cards.Length; ++i)
        {
            int rankDiff = GetCardRank(cards[i]) - GetCardRank(otherCards[i]);
            if (rankDiff != 0)
                return rankDiff;
        }
        return 0;
    }

    private static HandType GetHandType(char[] cards)
    {
        char[] newCards = cards.ToArray();
        int i = Array.IndexOf(newCards, 'J');
        if (i == -1)
            return new Hand(cards, 0).Type;
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

    private static int GetCardRank(char card)
    {
        return card switch
        {
            'T' => 10,
            'J' => 0,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => card - '0'
        };
    }
}

public class Hand : IComparable<Hand>
{
    private readonly char[] cards;
    
    public Hand(string cards, int bid)
        : this(cards.ToCharArray(), bid)
    {
    }

    public Hand(char[] cards, int bid)
    {
        this.cards = cards;
        Bid = bid;
        Type = GetHandType(cards);
    }
    
    public char[] Cards => cards;
    public HandType Type { get; }
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
    
    public static Hand Parse(string line)
    {
        var parts = line.Split(' ');
        return new Hand(parts[0], int.Parse(parts[1]));
    }
    
    private static HandType GetHandType(IEnumerable<char> cards)
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
    
    private static int GetCardRank(char card)
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