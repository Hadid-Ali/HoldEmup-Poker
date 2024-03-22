using UnityEngine;
using System.Collections;

public class CloseInfoButton : GameButton 
{
	public Animator panelAnimator;

	//------------------------------------------

	public override void PressAction()
	{
		// close info panel

		// for panels that have animations, play the close animation
		if(panelAnimator != null)
		{
			panelAnimator.Play("InfoPanelHide");
			// simulate a mouse up event
			Invoke ("ResetButton", 0.5f);
		}
		else
			ResetButton(); // for non animated panels, just close the panel

		// play sound
		SoundsManager.the.buttonsSound.Play ();
	}

	//------------------------------------------

	void ResetButton()
	{
		MainGame.the.OpenInfoPanel(false);
		normalSprite.SetActive (true);
		pressedSprite.SetActive (false);
	}
}
