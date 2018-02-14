using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{

	public PowerUpData ThisPowerUpData;
	
	private void OnTriggerEnter(Collider other)
	{
		print("This has " + ThisPowerUpData.PowerLevel);
	}
}
