using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TestScript : MonoBehaviour
{
    //[Serializable]
    
    [SerializeField] private Hand[] player1Deck;
    [SerializeField] private Hand[] player2Deck;
    [SerializeField] private Hand[] player3Deck;
    
    [SerializeField] [ReadOnlyInspector] private string Winner;


    
    

    [ContextMenu("Compare Hands")]
    public void EvaluateHands()
    {
        // Player1Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player1Handtype1 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player1Handtype2 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player2Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player2Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player2Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player3Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player3Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player3Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player4Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player4Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player4Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player5Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player5Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player5Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player6Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player6Handtype0 = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        // Player6Handtype0 = HandTypes.HighCard;
        //
        // HandEvaluator.Evaluate(cards1, out Player1Handtype0);
        //
        // Player2Handtype = HandTypes.HighCard;
        // HandEvaluator.Evaluate(cards2, out Player2Handtype);
        //
        // hand1 = new Hand(cards1, Player1Handtype0, 1);
        // hand2 = new Hand(cards2, Player1Handtype1, 1);
        // hand3 = new Hand(cards3, Player1Handtype2, 1);
        //
        // hand4 = new Hand(cards4, Player2Handtype0, 2);
        // hand5 = new Hand(cards5, Player2Handtype1, 2);
        // hand6 = new Hand(cards6, Player2Handtype2, 2);
        //
        // hand7 = new Hand(cards7, Player3Handtype0, 3);
        // hand8 = new Hand(cards8, Player3Handtype1, 3);
        // hand9 = new Hand(cards9, Player3Handtype2, 3);
        //
        // hand10 = new Hand(cards4, Player4Handtype0, 4);
        // hand11 = new Hand(cards5, Player4Handtype1, 4);
        // hand12 = new Hand(cards6, Player4Handtype2, 4);
        //
        // hand13 = new Hand(cards4, Player5Handtype0, 5);
        // hand14 = new Hand(cards5, Player5Handtype1, 5);
        // hand15 = new Hand(cards6, Player5Handtype2, 5);
        //
        // hand16 = new Hand(cards4, Player6Handtype0, 6);
        // hand17 = new Hand(cards5, Player5Handtype1, 6);
        // hand18 = new Hand(cards6, Player5Handtype2, 6);
        //
        //
        // List<CardData[]> deck1 = new List<CardData[]>();
        // deck1.Add(hand1.CardData);
        //
        //
        //
        // NetworkDataObject dataobjects = new NetworkDataObject(hand1.CardData, hand1.photonID);
        // dataobjects.Add();
        //
        //
        //
        // dataobjects.Add(new NetworkDataObject(hand1.CardData, hand1.photonID));
        // String winneray;
        //
        // HandsEvaluator.OnTestingDataReceivedInternal( , out winneray);
        //
        // Player1Handtype = hand1._HandType;
        // Player2Handtype = hand2._HandType;
        // Player3Handtype = hand3._HandType;
        // Player4Handtype = hand4._HandType;
        // Player5Handtype = hand5._HandType;
        // Player6Handtype = hand6._HandType;
    }
    // public void EvaluateHands()
    // {
    //     Player1Handtype = HandTypes.HighCard;
    //     HandEvaluator.Evaluate(cards, out Player1Handtype);
    //     
    //     Player2Handtype = HandTypes.HighCard;
    //     HandEvaluator.Evaluate(cards2, out Player2Handtype);
    //     
    //     hand1 = new Hand(cards, Player1Handtype);
    //     hand2 = new Hand(cards2, Player2Handtype);
    //
    //     Winner = DeepEvaluateDrawedDecks.ComparePokerHands(hand1, hand2, out Player1Handtype, out Player2Handtype);
    //
    //     Player1Handtype = hand1._HandType;
    //     Player2Handtype = hand2._HandType;
    // }
    
    
}
