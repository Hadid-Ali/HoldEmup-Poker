using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI credit;
    [SerializeField] private Image pocketCard1;
    [SerializeField] private Image pocketCard2;
    
    public int playerID;
    public string playerName;
    public int playerCredit;
    
    public void UpdateView()
    {
        id.SetText( $" Seat no : {playerID}");
        Name.SetText(playerName);
        credit.SetText("Credits : " + playerCredit);
    }

    public void UpdateCardView(CardData card1, CardData card2)
    {
        pocketCard1.sprite = CardsRegistery.Instance.GetCardSprite(card1.type, card1.value);
        pocketCard2.sprite = CardsRegistery.Instance.GetCardSprite(card2.type, card2.value);

    }
}
