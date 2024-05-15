using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI credit;
    [SerializeField] private TextMeshProUGUI winCredit;
    [SerializeField] private GameObject winner;
    [SerializeField] private Image pocketCard1;
    [SerializeField] private Image pocketCard2;
    [SerializeField] private Image turnImage;
    [SerializeField] public SlideAnimation animationSlide;
    [SerializeField] public PlayerDialogueBox dialogueBox;
    
    private bool _alreadySlide;
    public int playerID;
    public bool IsOccupied { get; private set; }

    public void Initialize(string playerName, int _playerID, int playerCredit)
    {
        Name.SetText(playerName);
        id.SetText( $" Seat no : {playerID}");
        credit.SetText(playerCredit.ToString());

        playerID = _playerID;
        animationSlide.Slide();

        IsOccupied = true;
    }

    public void UpdateTurnView(bool turn) => turnImage.gameObject.SetActive(turn);
    public void UpdateCreditView(int credits) => credit.SetText(credits.ToString());

    public void UpdateCardsView(CardData card1, CardData card2)
    {
        pocketCard1.sprite = CardsRegistery.Instance.GetCardSprite(card1.type, card1.value);
        pocketCard2.sprite = CardsRegistery.Instance.GetCardSprite(card2.type, card2.value);
    }

    public void PopDialog(string dialog, float duration)
    {
        dialogueBox.Pop(dialog, duration);
    }

    public void UpdateWinnerView(bool val, int amount)
    {
        winner.SetActive(val);
        winCredit.SetText(amount.ToString());
    }




}
