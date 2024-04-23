using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class TieBreakerComponent 
{
    public static int DeepEvaluate(Hand hand1, Hand hand2)
    {
        HandEvaluator.Evaluate(hand1.CardData, out hand1._HandType);
        HandTypes rank1 = hand1._HandType;
        HandEvaluator.Evaluate(hand2.CardData, out hand2._HandType);
        HandTypes rank2 = hand2._HandType;

        Array.Reverse(hand1.CardData); //Reverse so it compares the bigger cards first where it matters
        Array.Reverse(hand2.CardData);

        if (rank1 > rank2)
        {
            return 1;
        }

        if (rank1 < rank2)
        {
            return 2;
        }

        // If both hands have the same rank, additional criteria can be used
        switch (rank1)
        {
            case HandTypes.Flush:
            case HandTypes.HighCard:
                // Compare the highest cards in the pairs or three-of-a-kind
                CardValue highestCard1 = GetHighestCardValue(hand1);
                CardValue highestCard2 = GetHighestCardValue(hand2);

                if (highestCard1 > highestCard2)
                {
                    return 1;
                }

                if (highestCard1 < highestCard2)
                {
                    return 2;
                }

                // If both hands have the same rank, compare the next highest cards
                for (int i = 0; i < hand1.CardData.Length; i++)
                {
                    if (hand1.CardData[i].value > hand2.CardData[i].value)
                    {
                        Debug.Log(hand1.CardData[i].value + " Is bigger than " + hand2.CardData[i].value);
                        return 1;
                    }

                    if (hand1.CardData[i].value < hand2.CardData[i].value)
                    {
                        Debug.Log(hand1.CardData[i].value + " Is smaller than " + hand2.CardData[i].value);
                        return 2;
                    }
                }

                // If we reach this point, it's a tie
                return 0;
            case HandTypes.Pair:
            case HandTypes.FourOfAKind:
            case HandTypes.TwoPair:
                // If both hands have the same rank (e.g., both are pairs), compare pair values first
                CardValue pairValue1 = GetPairValue(hand1);
                CardValue pairValue2 = GetPairValue(hand2);

                if (pairValue1 > pairValue2)
                {
                    return 1;
                }

                if (pairValue1 < pairValue2)
                {
                    return 2;
                }

                // If the pair values are the same, compare the non-pair values
                List<CardValue> nonPairValues1 = GetNonPairValues(hand1);
                List<CardValue> nonPairValues2 = GetNonPairValues(hand2);

                for (int i = 0; i < nonPairValues1.Count; i++)
                {
                    if (nonPairValues1[i] > nonPairValues2[i])
                    {
                        return 1;
                    }

                    if (nonPairValues1[i] < nonPairValues2[i])
                    {
                        return 2;
                    }
                }

                // If we reach this point, it's a tie
                return 0;
            case HandTypes.FullHouse:
            case HandTypes.ThreeOfAKind:

                Debug.LogError($"Rank Value A {rank1} vs Card Value B {rank2}");
                if (rank1 > rank2)
                {
                    return 1;
                }

                if (rank1 < rank2)
                {
                    return 2;
                }

                // If both hands have the same rank, compare the three of a kind values first
                CardValue threeOfAKindValue1 = GetPairOrThreeValue(hand1);
                CardValue threeOfAKindValue2 = GetPairOrThreeValue(hand2);
                Debug.LogError($"Card Value A {threeOfAKindValue1} vs Card Value B {threeOfAKindValue2}");
                if (threeOfAKindValue1 > threeOfAKindValue2)
                {
                    return 1;
                }

                if (threeOfAKindValue1 < threeOfAKindValue2)
                {
                    return 2;
                }

                // If the three of a kind values are the same, compare the non-three-of-a-kind values
                List<CardValue> nonThreeValues1 = GetNonPairValues(hand1);
                List<CardValue> nonThreeValues2 = GetNonPairValues(hand2);

                for (int i = 0; i < nonThreeValues1.Count; i++)
                {
                    if (nonThreeValues1[i] > nonThreeValues2[i])
                    {
                        return 1;
                    }

                    if (nonThreeValues1[i] < nonThreeValues2[i])
                    {
                        return 2;
                    }
                }

                // If we reach this point, it's a tie
                return 0;
            case HandTypes.Straight:
            case HandTypes.StraightFlush:
                // For straights, compare the highest card in the straight
                CardValue highestCardStraight1 = GetHighestCardValue(hand1);
                CardValue highestCardStraight2 = GetHighestCardValue(hand2);

                if (highestCardStraight1 > highestCardStraight2)
                {
                    return 1;
                }

                if (highestCardStraight1 < highestCardStraight2)
                {
                    return 2;
                }

                // If both hands have the same rank, compare the next highest cards
                for (int i = 0; i < hand1.CardData.Length; i++)
                {
                    if (hand1.CardData[i].value > hand2.CardData[i].value)
                    {
                        return 1;
                    }

                    if (hand1.CardData[i].value < hand2.CardData[i].value)
                    {
                        return 2;
                    }
                }

                // If we reach this point, it's a tie
                return 0;

            default:
                return 0; // For other cases, consider it a tie
        }
    }

    public static string DeepEvaluate(Hand hand1, Hand hand2, out HandTypes player1, out HandTypes player2)
    {
        HandEvaluator.Evaluate(hand1.CardData, out hand1._HandType);
        HandTypes rank1 = player2 = hand1._HandType;
        HandEvaluator.Evaluate(hand2.CardData, out hand2._HandType);
        HandTypes rank2 = player1 = hand2._HandType;

        Array.Reverse(hand1.CardData); //Reverse so it compares the bigger cards first where it matters
        Array.Reverse(hand2.CardData);


        if (rank1 > rank2)
        {
            return "Hand 1 wins";
        }

        if (rank1 < rank2)
        {
            return "Hand 2 wins";
        }

        // If both hands have the same rank, additional criteria can be used
        switch (rank1)
        {
            case HandTypes.Flush:
            case HandTypes.HighCard:
                // Compare the highest cards in the pairs or three-of-a-kind
                CardValue highestCard1 = GetHighestCardValue(hand1);
                CardValue highestCard2 = GetHighestCardValue(hand2);

                if (highestCard1 > highestCard2)
                {
                    return "Hand 1 wins";
                }

                if (highestCard1 < highestCard2)
                {
                    return "Hand 2 wins";
                }

                // If both hands have the same rank, compare the next highest cards
                for (int i = 0; i < hand1.CardData.Length; i++)
                {
                    if (hand1.CardData[i].value > hand2.CardData[i].value)
                    {
                        Debug.Log(hand1.CardData[i].value + " Is bigger than " + hand2.CardData[i].value);
                        return "Hand 1 wins";
                    }

                    if (hand1.CardData[i].value < hand2.CardData[i].value)
                    {
                        Debug.Log(hand1.CardData[i].value + " Is smaller than " + hand2.CardData[i].value);
                        return "Hand 2 wins";
                    }
                }

                // If we reach this point, it's a tie
                return "It's a tie";
            case HandTypes.Pair:
            case HandTypes.FourOfAKind:
            case HandTypes.TwoPair:
                // If both hands have the same rank (e.g., both are pairs), compare pair values first
                CardValue pairValue1 = GetPairValue(hand1);
                CardValue pairValue2 = GetPairValue(hand2);

                if (pairValue1 > pairValue2)
                {
                    return "Hand 1 wins";
                }

                if (pairValue1 < pairValue2)
                {
                    return "Hand 2 wins";
                }

                // If the pair values are the same, compare the non-pair values
                List<CardValue> nonPairValues1 = GetNonPairValues(hand1);
                List<CardValue> nonPairValues2 = GetNonPairValues(hand2);

                for (int i = 0; i < nonPairValues1.Count; i++)
                {
                    if (nonPairValues1[i] > nonPairValues2[i])
                    {
                        return "Hand 1 wins";
                    }

                    if (nonPairValues1[i] < nonPairValues2[i])
                    {
                        return "Hand 2 wins";
                    }
                }

                // If we reach this point, it's a tie
                return "It's a tie";
            case HandTypes.FullHouse:
            case HandTypes.ThreeOfAKind:
                if (rank1 > rank2)
                {
                    return "Hand 1 wins";
                }

                if (rank1 < rank2)
                {
                    return "Hand 2 wins";
                }

                // If both hands have the same rank, compare the three of a kind values first
                CardValue threeOfAKindValue1 = GetPairOrThreeValue(hand1);
                CardValue threeOfAKindValue2 = GetPairOrThreeValue(hand2);

                if (threeOfAKindValue1 > threeOfAKindValue2)
                {
                    return "Hand 1 wins";
                }

                if (threeOfAKindValue1 < threeOfAKindValue2)
                {
                    return "Hand 2 wins";
                }

                // If the three of a kind values are the same, compare the non-three-of-a-kind values
                List<CardValue> nonThreeValues1 = GetNonPairValues(hand1);
                List<CardValue> nonThreeValues2 = GetNonPairValues(hand2);

                for (int i = 0; i < nonThreeValues1.Count; i++)
                {
                    if (nonThreeValues1[i] > nonThreeValues2[i])
                    {
                        return "Hand 1 wins";
                    }

                    if (nonThreeValues1[i] < nonThreeValues2[i])
                    {
                        return "Hand 2 wins";
                    }
                }

                // If we reach this point, it's a tie
                return "It's a tie";
            case HandTypes.Straight:
            case HandTypes.StraightFlush:
                // For straights, compare the highest card in the straight
                CardValue highestCardStraight1 = GetHighestCardValue(hand1);
                CardValue highestCardStraight2 = GetHighestCardValue(hand2);

                if (highestCardStraight1 > highestCardStraight2)
                {
                    return "Hand 1 wins";
                }

                if (highestCardStraight1 < highestCardStraight2)
                {
                    return "Hand 2 wins";
                }

                // If both hands have the same rank, compare the next highest cards
                for (int i = 0; i < hand1.CardData.Length; i++)
                {
                    if (hand1.CardData[i].value > hand2.CardData[i].value)
                    {
                        return "Hand 1 wins";
                    }

                    if (hand1.CardData[i].value < hand2.CardData[i].value)
                    {
                        return "Hand 2 wins";
                    }
                }

                // If we reach this point, it's a tie
                return "It's a tie";

            default:
                return "It's a tie"; // For other cases, consider it a tie
        }
    }

    static CardValue GetPairOrThreeValue(Hand hand)
    {
        Dictionary<CardValue, int> valueCounts = new Dictionary<CardValue, int>();
        foreach (var card in hand.CardData)
        {
            if (!valueCounts.ContainsKey(card.value))
            {
                valueCounts[card.value] = 0;
            }

            valueCounts[card.value]++;
        }

        var keyValuePairs = valueCounts.OrderByDescending(keyValue => keyValue.Value);
        return keyValuePairs.First().Key;
    }

    static CardValue GetPairValue(Hand hand)
    {
        Dictionary<CardValue, int> valueCounts = new Dictionary<CardValue, int>();
        foreach (var card in hand.CardData)
        {
            if (!valueCounts.ContainsKey(card.value))
            {
                valueCounts[card.value] = 0;
            }

            valueCounts[card.value]++;
        }

        foreach (var kvp in valueCounts)
        {
            if (kvp.Value == 2)
            {
                return kvp.Key; // Return the value of the pair
            }
        }

        return CardValue.value_2; // Default value if no pair is found
    }

    static List<CardValue> GetNonPairValues(Hand hand)
    {
        List<CardValue> nonPairValues = new List<CardValue>();
        Dictionary<CardValue, int> valueCounts = new Dictionary<CardValue, int>();

        foreach (var card in hand.CardData)
        {
            if (!valueCounts.ContainsKey(card.value))
            {
                valueCounts[card.value] = 0;
            }

            valueCounts[card.value]++;
        }

        foreach (var card in hand.CardData)
        {
            if (valueCounts[card.value] != 2) // If the card is not part of the pair
            {
                nonPairValues.Add(card.value); // Add it to the non-pair values list
            }
        }

        // Sort the non-pair values in descending order
        nonPairValues.Sort((x, y) => y.CompareTo(x));

        return nonPairValues;
    }

    static CardValue GetThreeOfAKindValue(Hand hand)
    {
        Dictionary<CardValue, int> valueCounts = new Dictionary<CardValue, int>();
        foreach (var card in hand.CardData)
        {
            if (!valueCounts.ContainsKey(card.value))
            {
                valueCounts[card.value] = 0;
            }

            valueCounts[card.value]++;
        }

        foreach (var kvp in valueCounts)
        {
            if (kvp.Value == 3) // Check for three of a kind
            {
                return kvp.Key; // Return the value of the three of a kind
            }
        }

        return CardValue.value_2; // Default value if no three of a kind is found
    }

    static CardValue GetHighestCardValue(Hand hand)
    {
        // Helper function to get the highest card value in a hand
        CardValue highestValue = CardValue.value_2; // Start with the lowest value

        foreach (var card in hand.CardData)
        {
            if (card.value > highestValue)
            {
                highestValue = card.value;
            }
        }

        return highestValue;
    }
}