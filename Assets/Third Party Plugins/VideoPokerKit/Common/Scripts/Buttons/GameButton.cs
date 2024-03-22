using UnityEngine;
using System.Collections;

public class GameButton : MonoBehaviour 
{
	[Header("Sprites")]
	public GameObject normalSprite;
	public GameObject pressedSprite;

	[Header("Highlight state")]
	public SpriteRenderer highlightSprite;
	[HideInInspector]
	public bool highlightEnabled = false;

	protected BoxCollider2D buttonCollider;

	//---------------------------------------------------------------------------------

	public void Awake()
	{
		// get a reference to the collider
		buttonCollider = gameObject.GetComponent<BoxCollider2D>();
	}

	//---------------------------------------------------------------------------------

	public void Update()
	{
		// update highlight
		if (highlightEnabled) 
		{
			if(highlightSprite != null)
			{
				if(!highlightSprite.gameObject.activeSelf)
					highlightSprite.gameObject.SetActive(true);
			}

			Color col = highlightSprite.color;
			col.a = Mathf.PingPong(Time.time*1.5f, 1.0f);
			highlightSprite.color = col;
		}
	}

	//---------------------------------------------------------------------------------

	void OnMouseDown()
	{
		// hide normal state and show pressed state
		normalSprite.SetActive (false);
		pressedSprite.SetActive (true);

		// call the PressAction method which has to be implemented
		// in any class that overrides the GameButton class
		PressAction ();

		if(highlightSprite != null)
			highlightSprite.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------

	void OnMouseUp()
	{
		// hide pressed state and show the normal button state
		normalSprite.SetActive (true);
		pressedSprite.SetActive (false);
	}

	//---------------------------------------------------------------------------------

	public virtual void PressAction()	
	{
		Debug.Log ("Override this function for button action");
	}

	//---------------------------------------------------------------------------------
}
