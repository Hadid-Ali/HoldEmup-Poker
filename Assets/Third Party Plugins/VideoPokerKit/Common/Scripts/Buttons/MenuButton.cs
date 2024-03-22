using UnityEngine;
using System.Collections;

public class MenuButton : GameButton 
{
	public override void PressAction()
	{
		// play sound
		SoundsManager.the.buttonsSound.Play ();

		// go to lobby
		Application.LoadLevel("lobby");
	}
}
