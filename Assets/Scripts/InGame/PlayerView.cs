using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ID;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private Image pocketCard1;
    [SerializeField] private Image pocketCard2;
    
    public int playerID;
    public string playerName;



    public void UpdateView()
    {
        ID.SetText(playerID.ToString());
        Name.SetText(playerName);
    }

    public void UpdateCardView(CardData card1, CardData card2)
    {
        pocketCard1.sprite = CardsRegistery.Instance.GetCardSprite(card1.type, card1.value);
        pocketCard2.sprite = CardsRegistery.Instance.GetCardSprite(card2.type, card2.value);

    }
}
