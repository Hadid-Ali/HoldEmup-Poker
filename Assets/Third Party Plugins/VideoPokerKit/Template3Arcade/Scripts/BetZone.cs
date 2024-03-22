using UnityEngine;
using System.Collections;

public class BetZone : MonoBehaviour 
{
	//*************
	// NOTE
	// this class is used to define a paytable zone that holds all the
	// prizes computed for a specific bet value
	//*************

	// set the bet value of the block in Inspector 
	public int betValue = 1;

	public GameObject selectionObj;
	public TextMesh [] multipliers;

	//-----------------------------------------

	// Use this for initialization
	void Start () 
	{
		// set prizes on the screen
		Init ();

		// register to bet change events
		MainGame.BetUpdated += BetUpdated;
	}

	//-----------------------------------------

	void OnDestroy()
	{
		// make sure we unregister the callback
		MainGame.BetUpdated -= BetUpdated;
	}

	//-----------------------------------------

	void Init()
	{
		// compute the prizes and updates them on the screen
		int [] paytableMultipliers = Paytable.the.GetMultipliers();
		for(int i=0; i<multipliers.Length; i++)
		{
			float prize = betValue * paytableMultipliers[i];
			multipliers[i].text = "$" + prize.ToString("#");
		}
	}

	//-----------------------------------------
	
	// Update is called once per frame
	void Update () {
	
	}

	//-----------------------------------------

	public void BetUpdated(float newValue)
	{
		// enable the zone if the current game bet matches the one
		// defined in the Inspector for this object
		if((int)newValue == betValue)
			selectionObj.SetActive(true);
		else
			selectionObj.SetActive(false);
	}

	//-----------------------------------------
}
