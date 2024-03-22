using UnityEngine;
using System.Collections;

public class SortingLayerAndOrder : MonoBehaviour 
{
	public string sortingLayerName;
	public int sortingOrderInLayer;

	// Use this for initialization
	void Start () 
	{
		// get renderer for current object
		Renderer rend = gameObject.GetComponent<Renderer> ();
		// set desired sorting order
		rend.sortingLayerName = sortingLayerName;
		rend.sortingOrder = sortingOrderInLayer;
	}
}
