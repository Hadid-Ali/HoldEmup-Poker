using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct HighestHandOccurence
{
    public HandTypes HighestHandType;
    public List<int> handIDs;
}

public class HandsEvaluator : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.NetworkGameplayEvents.AllUserHandsReceived.Register(OnAllUserHandsReceived);
    }

    private void OnDisable()
    {
        GameEvents.NetworkGameplayEvents.AllUserHandsReceived.UnRegister(OnAllUserHandsReceived);
    }

    public void OnAllUserHandsReceived(List<NetworkDataObject> networkDataObjects)
    {
        Dictionary<int, CardData[,]> structuredData = new();
        
        int deckSize = GameData.MetaData.DeckSize;
        int deckCount = GameData.MetaData.DecksCount;
        
        for (int i = 0; i < networkDataObjects.Count; i++)
        {
            NetworkDataObject dataObject = networkDataObjects[i];

            int currentPhotonID = dataObject.PhotonViewID;
            CardData[,] cardsData = new CardData[deckCount, deckSize];

            int min = 0;
            int max = deckSize;

            for (int j = 0; j < deckCount; j++)
            {
                int x = 0;

                for (int k = min; k < max; k++)
                {
                    cardsData[j, x++] = dataObject.PlayerDecks[k];
                }

                if (max >= dataObject.PlayerDecks.Count)
                    break;

                min += deckSize;
                max += deckSize;
            }

            structuredData[currentPhotonID] = cardsData;
            
        }
        OnNetworkDataReceivedInternal(structuredData);
    }

    private void OnNetworkDataReceivedInternal(Dictionary<int, CardData[,]> cardsData)
    {
        int deckSize = GameData.MetaData.DeckSize;
        int deckCount = GameData.MetaData.DecksCount;
        
        Dictionary<int, PlayerScoreObject> userScores = new();
        
        foreach (KeyValuePair<int, CardData[,]> kvp in cardsData)
        {
            userScores[kvp.Key] = new PlayerScoreObject(kvp.Key);
        }
        
        for (int i = 0; i < deckCount; i++)
        {
            Dictionary<int, CardData[]> currentHand = new();
            List<Hand> hands = new();
            
            foreach (KeyValuePair<int, CardData[,]> kvp in cardsData)
            {
                int photonID = kvp.Key;
                CardData[,] cards = kvp.Value;

                CardData[] deck = new CardData[deckSize];

                for (int j = 0; j < deckSize; j++)
                {
                    deck[j] = cards[i, j];  
                }

                Hand hand = new Hand(deck, HandTypes.HighCard, kvp.Key);
                hands.Add(hand);
                
                currentHand[photonID] = deck;
            }

            CompareHand(hands, out int winner);
            userScores[winner].AddScore(GameData.MetaData.HandWinReward, i);
        }

        GameEvents.GameplayEvents.UserHandsEvaluated.Raise(userScores);
    }

    private static void CompareHand(List<Hand> hands, out int Winner)
    {
        foreach (var v in hands)
        {
            CardData[] cards = v.CardData;

            HandEvaluator.Evaluate(cards, out HandTypes handType);
            v._HandType = handType;
        }

        //Check if theres a tie
        HighestHandOccurence highestHandOccurence = GetHighestHandOccurence(hands);

        if (highestHandOccurence.handIDs.Count > 1)
        {
            List<Hand> winners = new List<Hand>();
            

            for (int i = 0; i < highestHandOccurence.handIDs.Count - 1; i += 2)
            {
                Hand firstValue = hands.Find(x => x.photonID == highestHandOccurence.handIDs[i]);
                Hand secondValue = hands.Find(x => x.photonID == highestHandOccurence.handIDs[i + 1]);
                int winner = TieBreakerComponent.DeepEvaluate(firstValue, secondValue);


                switch (winner)
                {
                    case 0:
                        var sortedHands = hands.OrderBy(x => (int)x._HandType)
                            .ToDictionary(x => x.photonID, x => x._HandType);
                        List<KeyValuePair<int, HandTypes>> userHandsList = sortedHands.ToList();
                        Winner = userHandsList[^1].Key;
                        break;
                    case 1:
                        winners.Add(firstValue);
                        break;
                    case 2:
                        winners.Add(secondValue);
                        break;
                }
            }
            
            Hand finalWinner = null;
            
            if(winners.Count > 0)
            {
                finalWinner = winners[^1];
                Winner = finalWinner.photonID;
            }
            else
            {
                var sortedHands = hands.OrderBy(x => (int)x._HandType).ToDictionary(x => x.photonID, x => x._HandType);
                List<KeyValuePair<int, HandTypes>> userHandsList = sortedHands.ToList();

                Winner = userHandsList[^1].Key;
            }

        }
        else
        {
            var sortedHands = hands.OrderBy(x => (int)x._HandType).ToDictionary(x => x.photonID, x => x._HandType);
            List<KeyValuePair<int, HandTypes>> userHandsList = sortedHands.ToList();

            Winner = userHandsList[^1].Key;
        }
    }

    private static HighestHandOccurence GetHighestHandOccurence(List<Hand> hands)
    {
        HandTypes highestHand = HandTypes.HighCard;
        List<int> handsIDs = new List<int>();
        
        HighestHandOccurence highestHandOccurence = new HighestHandOccurence();
        foreach (var v in hands)
        {
            if (v._HandType > highestHand)
            {
                highestHand = v._HandType;
                handsIDs.Clear();
                
                handsIDs.Add(v.photonID);
            }
            else if (highestHand == v._HandType)
            {
                handsIDs.Add(v.photonID);
            }
        }

        highestHandOccurence.HighestHandType = highestHand;
        highestHandOccurence.handIDs = handsIDs;

        return highestHandOccurence;
    }
}
