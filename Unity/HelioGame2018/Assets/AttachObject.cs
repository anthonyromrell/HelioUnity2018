using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObject : MonoBehaviour {

	public GameObject GO;
	void OnMouseDown()
	{
		transform.parent = GO.transform;
	}

}