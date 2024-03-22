using System;
using UnityEngine;
using System.Collections;

public class DealButton : GameButton 
{
	private void Start()
	{
		Invoke(nameof(PressAction), 0.1f);
	}

	public override void PressAction()
	{
		MainGame.the.DealCardsSimple();
		SoundsManager.the.dealButtonSound.Play ();
	}
}
