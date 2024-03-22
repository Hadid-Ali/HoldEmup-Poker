using UnityEngine;
using System.Collections;

public class BetButton : GameButton
{
	public int betChangeDirection = 1;

	//------------------------------------

	public override void PressAction()
	{
		MainGame.the.ChangeBet ( betChangeDirection );
		// play sound
		SoundsManager.the.buttonsSound.Play ();
	}
}
