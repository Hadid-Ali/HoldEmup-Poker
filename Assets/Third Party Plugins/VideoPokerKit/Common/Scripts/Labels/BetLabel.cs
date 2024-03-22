using UnityEngine;
using System.Collections;

public class BetLabel : MonoBehaviour 
{
	TextMesh betText;

	//----------------------------------------------------

	void Start () 
	{
		// get a reference to the text
		betText = GetComponent<TextMesh> ();
		// register to bet change events
		MainGame.BetUpdated += BetUpdated;
	}

	//----------------------------------------------------

	void BetUpdated(float newValue)
	{
		// update text on the screen
		betText.text = "$" + newValue.ToString("#.00");
	}

	//----------------------------------------------------

	void OnDestroy()
	{
		// make sure we unregister the callback
		MainGame.BetUpdated -= BetUpdated;
	}

	//----------------------------------------------------
}
