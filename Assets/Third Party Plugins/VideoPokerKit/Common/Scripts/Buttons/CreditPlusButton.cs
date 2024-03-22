using UnityEngine;
using System.Collections;

public class CreditPlusButton : GameButton 
{
	//------------------------------------
	
	public override void PressAction()
	{
		//----------------------------------------------------
		// HERE YOU CAN TRIGGER OTHER METHODS OF ADDING CREDIT

		// add credit
		MainGame.the.ChangeCredit (20);
		// play sound
		SoundsManager.the.buttonsSound.Play ();
	}
}
