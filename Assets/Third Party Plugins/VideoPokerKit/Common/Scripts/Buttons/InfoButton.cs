using UnityEngine;
using System.Collections;

public class InfoButton : GameButton 
{
	public override void PressAction()
	{
		// open info panel
		MainGame.the.OpenInfoPanel(true);
		// play sound
		SoundsManager.the.buttonsSound.Play ();
	}
}
