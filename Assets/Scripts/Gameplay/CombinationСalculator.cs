// ReSharper disable IdentifierTypo
/// <summary>
/// Source https://github.com/ccqi/TexasHoldem
/// </summary>
public static class CombinationСalculator
{
    public static Handd GetBestHandd(Handd Handd)
    {
        if (Handd.Count() < 5)
        {
            Handd.Clear();
            return Handd;
        }
        if (IsRoyalFlush(Handd))
            return GetRoyalFlush(Handd);
        if (IsStraightFlush(Handd))
            return GetStraightFlush(Handd);
        if (IsFourOfAKind(Handd))
            return GetFourOfAKind(Handd);
        if (IsFullHouse(Handd))
            return GetFullHouse(Handd);
        if (IsFlush(Handd))
            return GetFlush(Handd);
        if (IsStraight(Handd))
            return GetStraight(Handd);
        if (IsThreeOfAKind(Handd))
            return GetThreeOfAKind(Handd);
        if (IsTwoPair(Handd))
            return GetTwoPair(Handd);
        if (IsOnePair(Handd))
            return GetOnePair(Handd);
        return GetHighCard(Handd);
    }
    
    //get best class without running isRoyalFlush, since straightflush covers the royal flush
    public static Handd GetBestHanddEfficiently(Handd Handd)
    {
        if (Handd.Count() < 5)
        {
            Handd.Clear();
            return Handd;
        }
        if (IsStraightFlush(Handd))
            return GetStraightFlush(Handd);
        if (IsFourOfAKind(Handd))
            return GetFourOfAKind(Handd);
        if (IsFullHouse(Handd))
            return GetFullHouse(Handd);
        if (IsFlush(Handd))
            return GetFlush(Handd);
        if (IsStraight(Handd))
            return GetStraight(Handd);
        if (IsThreeOfAKind(Handd))
            return GetThreeOfAKind(Handd);
        if (IsTwoPair(Handd))
            return GetTwoPair(Handd);
        if (IsOnePair(Handd))
            return GetOnePair(Handd);
        return GetHighCard(Handd);
    }
    
    //look for royal flush, removing pair using recursion
    private static bool IsRoyalFlush(Handd Handd)
    {
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd[i] != Handd[i + 1])
            {
                continue;
            }

            Handd simplifiedHandd1 = new(Handd);
            simplifiedHandd1.RemoveAt(i);
            Handd simplifiedHandd2 = new(Handd);
            simplifiedHandd2.RemoveAt(i + 1);
            if (IsRoyalFlush(simplifiedHandd1))
                return true;
            if (IsRoyalFlush(simplifiedHandd2))
                return true;
        }
        
        CardType currenttype = Handd.GetCard(0).type;
        if (Handd.GetCard(0).GetRank() == 14 && Handd.GetCard(1).GetRank() == 13 && Handd.GetCard(2).GetRank() == 12 && Handd.GetCard(3).GetRank() == 11 && Handd.GetCard(4).GetRank() == 10 && Handd.GetCard(1).type == currenttype && Handd.GetCard(2).type == currenttype && Handd.GetCard(3).type == currenttype && Handd.GetCard(4).type == currenttype)
            return true;
        return false;
    }
    
    //get royal flush using recursion
    private static Handd GetRoyalFlush(Handd Handd)
    {
        Handd.SortByRank();
        Handd straightflush = new(GetStraightFlush(Handd));
        straightflush.AddCombinationId(10);
        if (straightflush.GetCard(0).GetRank() == 14)
            return straightflush;
        straightflush.Clear();
        return straightflush;
    }
    
    //use recursion to get rid of pairs, then evaluate straight flush
    private static bool IsStraightFlush(Handd Handd)
    {
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1))
            {
                continue;
            }

            Handd simplifiedHandd1 = new(Handd);
            simplifiedHandd1.RemoveAt(i);
            Handd simplifiedHandd2 = new(Handd);
            simplifiedHandd2.RemoveAt(i + 1);
            if (IsStraightFlush(simplifiedHandd1))
                return true;
            if (IsStraightFlush(simplifiedHandd2))
                return true;
        }
        for (var i = 0; i <= Handd.Count() - 5; i++)
        {
            int currentrank = Handd.GetCard(i).GetRank();
            CardType currenttype = Handd.GetCard(i).type;
            if (currentrank == Handd.GetCard(i + 1).GetRank() + 1 && currentrank == Handd.GetCard(i + 2).GetRank() + 2 && currentrank == Handd.GetCard(i + 3).GetRank() + 3 && currentrank == Handd.GetCard(i + 4).GetRank() + 4 && currenttype == Handd.GetCard(i + 1).type && currenttype == Handd.GetCard(i + 2).type && currenttype == Handd.GetCard(i + 3).type && currenttype == Handd.GetCard(i + 4).type)
                return true;
            
        }
        for (var i = 0; i <= Handd.Count() - 4; i++)
        {
            int currentrank = Handd.GetCard(i).GetRank();
            CardType currenttype = Handd.GetCard(i).type;
            
            if (currentrank == 5 && Handd.GetCard(i + 1).GetRank() == 4 && Handd.GetCard(i + 2).GetRank() == 3 && Handd.GetCard(i + 3).GetRank() == 2 && Handd.GetCard(0).GetRank() == 14 && currenttype == Handd.GetCard(i + 1).type && currenttype == Handd.GetCard(i + 2).type && currenttype == Handd.GetCard(i + 3).type && currenttype == Handd.GetCard(0).type)
                return true;
        }
        return false;
    }
    
    //get straight flush using two pointer variable and taking care of all cases
    private static Handd GetStraightFlush(Handd Handd)
    {
        Handd.SortByRank();
        Handd straightflush = new();
        straightflush.AddCombinationId(9);
        if (Handd.GetCard(0).GetRank() == 14)
            Handd.Add(new CardData( Handd.GetCard(0).type, CardValue.value_A));

        straightflush.Add(Handd.GetCard(0));
        int ptr1=0, ptr2=1;
        while (ptr1 < Handd.Count() - 2 || ptr2 < Handd.Count())
        {
            if (straightflush.Count() >= 5)
                break;
            int rank1=Handd.GetCard(ptr1).GetRank(), rank2=Handd.GetCard(ptr2).GetRank();
            CardType type1 = Handd.GetCard(ptr1).type, type2=Handd.GetCard(ptr2).type;
            if (rank1 - rank2 == 1 && type1 == type2)
            {
                straightflush.Add(Handd.GetCard(ptr2));
                ptr1 = ptr2;
                ptr2++;
            }
            else if(rank1==2&&rank2==14&&type1==type2)
            {
                straightflush.Add(Handd.GetCard(ptr2));
                ptr1 = ptr2;
                ptr2++;
            }
            else
            {
                if (rank1 - rank2 <= 1)
                    ptr2++;
                else
                {
                    straightflush.Clear();
                    straightflush.AddCombinationId(9);
                    ptr1++;
                    ptr2=ptr1+1;
                    straightflush.Add(Handd.GetCard(ptr1));
                }
            }    
        }
        if (Handd.GetCard(0).GetRank() == 14)
            Handd.RemoveAt(Handd.Count() - 1);
        straightflush.AddCombinationId(straightflush.GetCard(0).GetRank());
        if (straightflush.Count() < 5)
            straightflush.Clear();
        return straightflush;
    }
    
    //easy algorithm to understand, just loop through the array and check for a certain amount of pairs
    //same for 3 of a kind, full house, 2 pair and 1 pair
    private static bool IsFourOfAKind(Handd Handd)
    {
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 4; i++)
        {
            if (Handd.GetCard(i) == Handd.GetCard(i + 1) && Handd.GetCard(i) == Handd.GetCard(i + 2) && Handd.GetCard(i) == Handd.GetCard(i + 3))
                return true;
        }
        return false;
    }
    
    //same as above except return the cards themselves
    private static Handd GetFourOfAKind(Handd Handd)
    {
        Handd fourofakind = new();
        fourofakind.AddCombinationId(8);
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 4; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i) != Handd.GetCard(i + 2) || Handd.GetCard(i) != Handd.GetCard(i + 3))
            {
                continue;
            }

            fourofakind.Add(Handd.GetCard(i));
            fourofakind.Add(Handd.GetCard(i + 1));
            fourofakind.Add(Handd.GetCard(i + 2));
            fourofakind.Add(Handd.GetCard(i + 3));
            fourofakind.AddCombinationId(Handd.GetCard(i).GetRank());
            break;
        }
        return GetKickers(Handd,fourofakind);
    }

    private static bool IsFullHouse(Handd Handd)
    {
        Handd.SortByRank();
        bool threeofakind = false, pair = false;
        var threeofakindRank = 0;
        
        for (var i = 0; i <= Handd.Count() - 3; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i) != Handd.GetCard(i + 2))
            {
                continue;
            }

            threeofakind = true;
            threeofakindRank = Handd.GetCard(i).GetRank();
            break;
        }
        
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i).GetRank() == threeofakindRank)
            {
                continue;
            }

            pair = true;
            break;
        }
        
        return threeofakind == true && pair == true;
    }

    private static Handd GetFullHouse(Handd Handd)
    {
        Handd.SortByRank();
        Handd fullhouse = new();
        fullhouse.AddCombinationId(7);
        bool threeofakind = false, pair = false;
        var threeofakindRank = 0;
        
        for (var i = 0; i <= Handd.Count() - 3; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i) != Handd.GetCard(i + 2))
            {
                continue;
            }

            threeofakind = true;
            threeofakindRank = Handd.GetCard(i).GetRank();
            fullhouse.Add(Handd.GetCard(i));
            fullhouse.Add(Handd.GetCard(i + 1));
            fullhouse.Add(Handd.GetCard(i + 2));
            fullhouse.AddCombinationId(Handd.GetCard(i).GetRank());
            break;
        }
        
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i).GetRank() == threeofakindRank)
            {
                continue;
            }

            pair = true;
            fullhouse.Add(Handd.GetCard(i));
            fullhouse.Add(Handd.GetCard(i + 1));
            fullhouse.AddCombinationId(Handd.GetCard(i).GetRank());
            break;
        }
        
        if (threeofakind == true && pair == true)
            return fullhouse;
        
        fullhouse.Clear();
        return fullhouse;
    }
    
    //use a counter, if a counter reaches five, a flush is detected
    private static bool IsFlush(Handd Handd)
    {
        Handd.SortByRank();
        int diamondCount = 0, clubCount = 0, heartCount = 0, spadeCount = 0;
        for (var i = 0; i < Handd.Count(); i++)
        {
            if (Handd.GetCard(i).type == CardType.TYPE_DIAMONDS)
                diamondCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_CLUBS)
                clubCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_HEARTS)
                heartCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_SPADES)
                spadeCount++;
        }
        
        if (diamondCount >= 5)
            return true;
        if (clubCount >= 5)
            return true;
        if (heartCount >= 5)
            return true;
        if (spadeCount >= 5)
            return true;

        return false;
    }
    
    private static Handd GetFlush(Handd Handd)
    {
        Handd.SortByRank();
        Handd flush = new();
        flush.AddCombinationId(6);
        int diamondCount = 0, clubCount = 0, heartCount = 0, spadeCount = 0;
        for (var i = 0; i < Handd.Count(); i++)
        {
            if (Handd.GetCard(i).type == CardType.TYPE_DIAMONDS)
                diamondCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_CLUBS)
                clubCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_HEARTS)
                heartCount++;
            else if (Handd.GetCard(i).type == CardType.TYPE_SPADES)
                spadeCount++;
        }

        CardType type = 0;
        if (diamondCount >= 5)
        {
            type = CardType.TYPE_DIAMONDS;
        }
        else if (clubCount >= 5)
        {
            type = CardType.TYPE_CLUBS;

        }
        else if (heartCount >= 5)
        {
            type = CardType.TYPE_HEARTS;

        }
        else if (spadeCount >= 5)
        {
            type = CardType.TYPE_SPADES;
        }
        
        for (var i = 0; i <= Handd.Count(); i++)
        {
            if (Handd.GetCard(i).type == type)
            {
                flush.Add(Handd.GetCard(i));
                flush.AddCombinationId(Handd.GetCard(i).GetRank());
            }
            if (flush.Count() == 5)
                break;
        }
        
        return flush;
    }

    private static bool IsStraight(Handd Handd)
    {
        Handd.SortByRank();
        if(Handd.GetCard(0).GetRank()==14)
            Handd.Add(new CardData(Handd.GetCard(0).type, CardValue.value_A));
        var straightCount=1;
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            //if 5 cards are found to be straights, break out of the loop
            if (straightCount == 5)
                break;
            int currentrank = Handd.GetCard(i).GetRank();
            //if cards type differ by 1, increment straight
            if (currentrank - Handd.GetCard(i + 1).GetRank() == 1)
                straightCount++;
            //specific condition for 2-A
            else if (currentrank == 2 && Handd.GetCard(i + 1).GetRank() == 14)
                straightCount++;
            //if cards type differ by more than 1, reset straight to 1
            else if (currentrank - Handd.GetCard(i + 1).GetRank() > 1)
                straightCount = 1;
            //if card types does not differ, do nothing
        }
        if (Handd.GetCard(0).GetRank() == 14)
            Handd.RemoveAt(Handd.Count() - 1);
        //depending on the straight count, return true or false
        return straightCount == 5;
    }

    private static Handd GetStraight(Handd Handd)
    {
        Handd.SortByRank();
        Handd straight = new();
        straight.AddCombinationId(5);
        if (Handd.GetCard(0).GetRank() == 14)
            Handd.Add(new CardData(Handd.GetCard(0).type, CardValue.value_A));
        var straightCount = 1;
        straight.Add(Handd.GetCard(0));
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            //if 5 cards are found to be straights, break out of the loop
            if (straightCount == 5)
                break;
            int currentrank = Handd.GetCard(i).GetRank();
            //if cards type differ by 1, increment straight
            if (currentrank - Handd.GetCard(i + 1).GetRank() == 1)
            {
                straightCount++;
                straight.Add(Handd.GetCard(i+1));
            }
            //specific condition for 2-A
            else if (currentrank == 2 && Handd.GetCard(i + 1).GetRank() == 14)
            {
                straightCount++;
                straight.Add(Handd.GetCard(i+1));
            }
            //if cards type differ by more than 1, reset straight to 1
            else if (currentrank - Handd.GetCard(i + 1).GetRank() > 1)
            {
                straightCount = 1;
                straight.Clear();
                straight.AddCombinationId(5);
                straight.Add(Handd.GetCard(i+1));
            }
            //if card types does not differ, do nothing
        }
        //depending on the straight count, return true or false
        if (Handd.GetCard(0).GetRank() == 14)
            Handd.RemoveAt(Handd.Count() - 1);
        if (straightCount != 5)
            straight.Clear();
        straight.AddCombinationId(straight.GetCard(0).GetRank());
        return straight;
    }

    private static bool IsThreeOfAKind(Handd Handd)
    {
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 3; i++)
        {
            if (Handd.GetCard(i) == Handd.GetCard(i + 1) && Handd.GetCard(i) == Handd.GetCard(i + 2))
                return true;
        }
        return false;
    }

    private static Handd GetThreeOfAKind(Handd Handd)
    {
        Handd.SortByRank();
        Handd threeofakind = new();
        threeofakind.AddCombinationId(4);
        for (var i = 0; i <= Handd.Count() - 3; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1) || Handd.GetCard(i) != Handd.GetCard(i + 2))
            {
                continue;
            }

            threeofakind.AddCombinationId(Handd.GetCard(i).GetRank());
            threeofakind.Add(Handd.GetCard(i));
            threeofakind.Add(Handd.GetCard(i + 1));
            threeofakind.Add(Handd.GetCard(i + 2));
            break;
        }
        return GetKickers(Handd, threeofakind);
    }

    private static bool IsTwoPair(Handd Handd)
    {
        Handd.SortByRank();
        var pairCount = 0;
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1))
            {
                continue;
            }

            pairCount++;
            i++;
        }
        return pairCount >= 2;
    }

    private static Handd GetTwoPair(Handd Handd)
    {
        Handd.SortByRank();
        Handd twopair = new();
        twopair.AddCombinationId(3);
        var pairCount = 0;
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) != Handd.GetCard(i + 1))
            {
                continue;
            }

            twopair.AddCombinationId(Handd.GetCard(i).GetRank());
            twopair.Add(Handd.GetCard(i));
            twopair.Add(Handd.GetCard(i+1));
            pairCount++;
            if (pairCount == 2)
                break;
            i++;
        }
        if (pairCount == 2)
            return GetKickers(Handd,twopair);
        
        twopair.Clear();
        return twopair;
    }

    private static bool IsOnePair(Handd Handd)
    {
        Handd.SortByRank();
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) == Handd.GetCard(i + 1))
                return true;
        }
        return false;
    }

    private static Handd GetOnePair(Handd Handd)
    {
        Handd.SortByRank();
        Handd onepair = new();
        onepair.AddCombinationId(2);
        for (var i = 0; i <= Handd.Count() - 2; i++)
        {
            if (Handd.GetCard(i) == Handd.GetCard(i + 1))
            {
                onepair.AddCombinationId(Handd.GetCard(i).GetRank());
                onepair.Add(Handd.GetCard(i));
                onepair.Add(Handd.GetCard(i + 1));
                break;
            }
        }
        return GetKickers(Handd, onepair);
    }

    private static Handd GetHighCard(Handd Handd)
    {
        Handd.SortByRank();
        Handd highcard = new();
        highcard.AddCombinationId(1);
        highcard.Add(Handd.GetCard(0));
        highcard.AddCombinationId(Handd.GetCard(0).GetRank());
        return GetKickers(Handd, highcard);
    }

    private static Handd GetKickers(Handd Handd, Handd specialCards)
    {
        if (specialCards.Count() == 0)
            return specialCards;
        for (var i = 0; i < specialCards.Count(); i++)
        {
            Handd.Remove(specialCards.GetCard(i));
        }
        for (var i = 0; i < Handd.Count();i++)
        {
            if (specialCards.Count() >= 5)
                break;
            specialCards.Add(Handd.GetCard(i));
            specialCards.AddCombinationId(Handd.GetCard(i).GetRank());
        }
        return specialCards;
    }
}