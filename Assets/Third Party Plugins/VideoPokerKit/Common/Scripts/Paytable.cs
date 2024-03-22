using UnityEngine;
using System.Collections;

//-------------------------------------------------------------------

// all wins, listed from lower to bigger
public enum Wins
{
	WIN_JACKS_OR_BETTER,
	WIN_TWO_PAIR,
	WIN_THREE_OF_A_KIND,
	WIN_STRAIGHT,
	WIN_FLUSH,
	WIN_FULL_HOUSE,
	WIN_FOUR_OF_A_KIND,
	WIN_STRAIGHT_FLUSH,
	WIN_ROYAL_FLUSH,
	WINS_NO
};

//-------------------------------------------------------------------

public class Paytable : MonoBehaviour 
{
	// static reference to the object instance (set in Awake)
	public static Paytable the;

	// set this in the Inspector to the desired value
	public bool flashWinnerLabel = true;

	// set this in the Inspector to the desired value
	public bool appendMultiplierToName = true;

	// please assign this array in the inspector
	// using the order of Wins enum above
	[Header("Prize labels")]
	public TextMesh [] entries;

	// the index of the current win
	int currentWinIndex = -1;

	// wins names
	string [] prizeTitle = new string[] {
								"Jacks or Better", 
								"Two Pairs", 
								"Three of a Kind", 
								"Straight",
								"Flush",
								"Full House",
								"Four of a Kind",
								"Straight Flush",
								"Royal Flush"};
	// multipliers for each win
	int [] prizeMultipliers = new int[] {
								1, 
								2, 
								3, 
								5,
								8,
								10,
								50,
								100,
								500};

	//-------------------------------------

	// Use this for initialization
	void Awake () 
	{	
		Paytable.the = this;
		Init ();
	}

	//-------------------------------------
	
	// Update is called once per frame
	void Update () 
	{	
		// check if we have a win
		if( currentWinIndex >= 0)
		{
			// flash the current win 2 times per second
			if(flashWinnerLabel)
				entries[ currentWinIndex ].gameObject.SetActive( Time.time % 0.5f > 0.25f ? true : false );
		}
	}

	//-------------------------------------

	public int [] GetMultipliers()
	{
		// returns the vector with all the multipliers
		return prizeMultipliers;
	}

	//-------------------------------------

	public void Init()
	{
		// setup visual paytable
		for(int i=0; i<entries.Length; i++)
		{
			// set prize name
			entries[i].text = prizeTitle[i];
			// append the multiplier with a different colour
			if( appendMultiplierToName )
				entries[i].text += "  <color=white>" + prizeMultipliers[i] + "X</color>";
		}
	}

	//-------------------------------------

	public void SetCurrentWin(Wins currWin)
	{
		currentWinIndex = (int) currWin;	
	}

	//-------------------------------------

	public Wins GetCurrentWinIndex()
	{
		return (Wins)currentWinIndex;
	}

	//-------------------------------------

	public int GetCurrentWinMultiplier()
	{
		if(currentWinIndex >= 0)
			return prizeMultipliers[ currentWinIndex ];
		return 0;
	}

	//-------------------------------------

	public void ResetWins()
	{
		// because the win is flashing, we have to
		// make sure we turn ON the latest win object
		if( currentWinIndex >= 0)
			entries[ currentWinIndex ].gameObject.SetActive( true );
		// set no current win index
		currentWinIndex = -1;
	}

	//-------------------------------------
}
