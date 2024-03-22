using UnityEngine;
using System.Collections;

public class SoundsManager : MonoBehaviour 
{
	// static reference to the object instance
	public static SoundsManager the;

	// set all sounds in Inspector into these slots, add more sounds here
	// they can be accessed via the static reference 'the'
	public AudioSource dealCardSound;
	public AudioSource buttonsSound;
	public AudioSource dealButtonSound;
	public AudioSource youWinSound;
	public AudioSource holdSound;
	public AudioSource autoHoldSound;

	//----------------------------------

	// Use this for initialization
	void Start () 
	{
		SoundsManager.the = this;
	}
}
