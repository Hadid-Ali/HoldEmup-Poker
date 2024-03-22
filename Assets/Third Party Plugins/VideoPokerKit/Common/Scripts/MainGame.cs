using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public class MainGame : MonoBehaviour 
{
	// reference for use in static calls
	public static MainGame the;

	// game states
	[HideInInspector]
	public byte gameState = STATE_IDLE;
	public static byte STATE_IDLE = 0;	// idle state: waiting for player to press DEAL
	public static byte STATE_DEALING = 1; // first hand deal is animating
	public static byte STATE_WAIT_HOLD = 2; // waiting for the player to hold cards
	public static byte STATE_DEALING2 = 3; // second hand deal is animating
	public static byte STATE_SHOW_RESULTS = 4; // compute & show results

	// starting value for the credit
	[HideInInspector]
	public float playerCredit = 100;	

	// current bet used by the player
	[HideInInspector]
	public float playerBet = 1;

	// value to hold the win from the current game
	[HideInInspector]
	public float currentWin = 0;

	// object that flashes showing the current win amount
	[Header("Win amount message")]
	bool showWinAmount = false;
	public GameObject winAmountObj;

	// reference to win amount text
	public TextMesh winAmountText;

	// bet settings, set them in the Inspector
	[Header("Bet settings")]
	public float betStep = 1;
	public float minBet = 1;
	public float maxBet = 10;

	// bet changes callback
	public delegate void BetUpdatedCallback(float newValue);
	public static BetUpdatedCallback BetUpdated;

	// credit changes callback
	public delegate void CreditUpdatedCallback(float newValue);
	public static CreditUpdatedCallback CreditUpdated;

	// win amount changes callback
	public delegate void WinAmountUpdatedCallback(float newValue);
	public static WinAmountUpdatedCallback WinAmountUpdated;

	// win messages callback
	public delegate void NewWinTypeCallback(Wins newWinName);
	public static NewWinTypeCallback newWinType;

	public CardsManager cardsManager;

	int cardsToDeal = 0;

	[Header("Info panel")]
	public GameObject infoPanelObj;

	
	//--------------------------------------------

	// Use this for initialization
	void Start () 
	{
		// save script reference in a static field so 
		// we can access the class from anywhere in the project
		MainGame.the = this;

		// set starting bet at 5
		playerBet = 5;
		// notify bet listeners that the bet has changed
		if (BetUpdated != null)
			BetUpdated (playerBet);
	}

	//--------------------------------------------

	public void Update()
	{
		// flash the win amount if needed
		if(showWinAmount)
			winAmountObj.SetActive( Time.time % 0.5f > 0.25f ? true : false );
	}

	//--------------------------------------------

	// 'direction' can be positive or negative to increase/decrease the bet
	public void ChangeBet(int direction)
	{
		// we are allowed to change the bet only in the IDLE state, between games
		if(gameState == STATE_SHOW_RESULTS)
			ResetGame();
		if(gameState != STATE_IDLE)
			return;

		// update bet to new value using the current bet step
		playerBet += betStep * direction;

		// check bet limits
		if (playerBet > maxBet)
			playerBet = maxBet;
		if (playerBet < minBet)
			playerBet = minBet;

		// notify bet listeners that the bet has changed
		if (BetUpdated != null)
			BetUpdated (playerBet);
	}
	
	//--------------------------------------------

	public void ChangeCredit(float amount)
	{
		// changing credit not allowed in other states different than IDLE
		if(gameState == STATE_SHOW_RESULTS)
			ResetGame();
		if(gameState != STATE_IDLE)
			return;

		//*********
		// NOTE
		// Instead of adding new credit, you can open a panel with: buy chips/credit/coins, in-app purchase, etc
		//*********

		// credit is updated with the new amount
		playerCredit += amount;

		// notify credit listeners that the credit has changed
		if (CreditUpdated != null)
			CreditUpdated (playerCredit);
	}

	//--------------------------------------------

	// the ResetGame function is clearing the old game: removes cards, hides win meesage, resets the deci
	public void ResetGame()
	{
		// we are allowed to reset the game's state only from RESULTS stage
		if(gameState != STATE_SHOW_RESULTS)
			return;

		// go to IDLE state
		gameState = STATE_IDLE;

		// clear the drawn cards, reset the deck and hide wins
		cardsManager.ResetDeck();
		cardsManager.ClearHand();
		Paytable.the.ResetWins();
		// set no current win
		currentWin = 0;
		showWinAmount = false;
		winAmountObj.SetActive(false);
	}

	//--------------------------------------------

	public void DealCardsSimple()
	{
		//cardsToDeal = cardsManager.DealCards();
	}

	[Button("Deal Cards")]
	public void DealCards()
	{
		//cardsToDeal = cardsManager.DealCards();
	}

	//--------------------------------------------

	// this function is called everytime a card finished the deal animation
	public void CardAnimationFinished()
	{
		// count a new card that has finished the deal animation
		cardsToDeal--;
		// check if all cards were dealt
		if(cardsToDeal == 0)
		{
		//	cardsManager.EvaluateHand();
			CheckResults();
		}

	}

	//--------------------------------------------

	public void CheckResults()
	{
		Debug.LogError("Checking Result");
		// change state to RESULTS
		gameState = STATE_SHOW_RESULTS;
		// search for wins
		cardsManager.EvaluateHand();

		// send the new win to win messages listeners (even if it's -1)
		// this triggers the overlay win messages
		if(newWinType != null)
			newWinType( Paytable.the.GetCurrentWinIndex() );

		// get current win
		int currWinMultiplier = Paytable.the.GetCurrentWinMultiplier();
		if( currWinMultiplier > 0 )
		{
			// compute the current win amount
			currentWin = playerBet * currWinMultiplier;
			// add it to the player's credit
			playerCredit += currentWin;
			// notify credit listeners that the credit has changed
			if (CreditUpdated != null)
				CreditUpdated (playerCredit);

			// update win amount text on the screen
			winAmountText.text = "$" + currentWin.ToString("#.00");
			showWinAmount = true;

			// play win sound
			SoundsManager.the.youWinSound.Play();
		}
	}

	//--------------------------------------------

	public void OpenInfoPanel(bool on)
	{
		// show/hide info panel
		infoPanelObj.SetActive(on);
	}

	//--------------------------------------------
}
