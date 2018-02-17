using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHitBox : MonoBehaviour {
	
	private void OnEnable()
	{
		//Enable Collider and Renderer
	}

	private void OnTriggerEnter(Collider other)
	{
		//Disable Collider and Renderer
	}
}
