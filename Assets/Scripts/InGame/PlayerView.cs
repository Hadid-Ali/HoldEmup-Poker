using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI credit;
    [SerializeField] private TextMeshProUGUI action;
    [SerializeField] private TextMeshProUGUI winCredit;
    [SerializeField] private GameObject winner;
    [SerializeField] private Image pocketCard1;
    [SerializeField] private Image pocketCard2;
    [SerializeField] private Image turn;
    
    public int playerID;
    public string playerName;
    public BetAction lastAction;
    public int playerCredit;
    public bool isOnTurn;
    
    public void UpdateView()
    {
        Name.SetText(playerName);
        id.SetText( $" Seat no : {playerID}");
        credit.SetText(playerCredit.ToString());
        action.SetText( lastAction != BetAction.UnSelected ? lastAction.ToString() : "");
        turn.gameObject.SetActive(isOnTurn);
    }

    public void UpdateWinnerView(bool val, int amount)
    {
        winner.SetActive(val);
        winCredit.SetText(amount.ToString());
    }

    public void UpdateView(int not)
    {
        Name.SetText("Not Joined");
        id.SetText( "");
        credit.SetText("");
        action.SetText("");
        turn.gameObject.SetActive(false);
    }

    public void UpdateCardView(CardData card1, CardData card2)
    {
        pocketCard1.sprite = CardsRegistery.Instance.GetCardSprite(card1.type, card1.value);
        pocketCard2.sprite = CardsRegistery.Instance.GetCardSprite(card2.type, card2.value);
    }
}
