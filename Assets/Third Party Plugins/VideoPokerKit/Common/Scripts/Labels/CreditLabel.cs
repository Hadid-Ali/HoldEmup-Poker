using UnityEngine;
using System.Collections;

public class CreditLabel : MonoBehaviour 
{
	TextMesh creditText;
	
	//----------------------------------------------------
	
	void Start () 
	{
		// get a reference to the text
		creditText = GetComponent<TextMesh> ();
		// register to credit change events
		MainGame.CreditUpdated += CreditUpdated;
	}
	
	//----------------------------------------------------
	
	void CreditUpdated(float newValue)
	{
		// update text on the screen
		creditText.text = "$" + newValue.ToString("#.00");
	}

	//----------------------------------------------------
	
	void OnDestroy()
	{
		// make sure we unregister the callback
		MainGame.CreditUpdated -= CreditUpdated;
	}
	
	//----------------------------------------------------
}
