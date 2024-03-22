using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour 
{
	// launch each template from its own Unity scene

	//-------------------------------------------

	public void LaunchClassicTemplate()
	{
		Application.LoadLevel("t1classic");
	}

	//-------------------------------------------

	public void LaunchStyleTemplate()
	{
		Application.LoadLevel("t2style");
	}
	
	//-------------------------------------------

	public void LaunchArcadeTemplate()
	{
		Application.LoadLevel("t3Arcade");
	}
	
	//-------------------------------------------
}
