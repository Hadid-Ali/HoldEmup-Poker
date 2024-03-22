using UnityEngine;
using System.Collections;

public class WinMessage : MonoBehaviour 
{
	Animator messageAnim;

	//*************
	// NOTE 
	// This class is for defining a win message. Each win type (one pair, two pairs, etc) 
	// has an animation ID, a Wins enum and a setting for shine (to be used or not)
	// The attached Animator has more animations, so the animation ID selects the animation
	// for the current win message
	//*************

	// change the value in Inspector for a different animation
	public int winAnimType = 1; // the animation ID that is passed to the Animator
	public Wins winName = Wins.WIN_JACKS_OR_BETTER;

	// set this reference if shine effect 
	// is wanted to be used with this win type
	[Header("Shine effect")]
	public GameObject shineObj;

	//------------------------------------------------------

	// Use this for initialization
	void Awake () 
	{
		// get a reference to the local animator (the one with the message)
		messageAnim = gameObject.GetComponent<Animator>();
		// register to new win events
		MainGame.newWinType += NewWin;
		// hide object
		gameObject.SetActive(false);
	}

	//------------------------------------------------------

	void OnDestroy()
	{
		MainGame.newWinType -= NewWin;
	}

	//------------------------------------------------------

	public void NewWin(Wins newWinName)
	{
		// check if the last win matches the local win name
		if(newWinName == winName)
		{
			// show object
			gameObject.SetActive(true);
			// make it invisible in first frame (until the Animator updates it)
			gameObject.transform.localScale = Vector3.zero;
			// when enabling the object, set the correct animation ID value
			messageAnim.SetInteger("type", winAnimType);
			// show shine effect if available
			if(shineObj)
				shineObj.SetActive(true);
		}
	}

	//------------------------------------------------------

	public void EndAnimation()
	{
		// hide object when animation is over
		gameObject.SetActive(false);
		// hide shine if available
		if(shineObj)
			shineObj.SetActive(false);
	}

	//------------------------------------------------------
}
