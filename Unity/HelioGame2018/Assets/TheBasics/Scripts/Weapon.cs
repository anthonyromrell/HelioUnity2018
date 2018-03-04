using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject Ammo;
	public GameObject CurrentAmmo;

	private void OnMouseDown()
	{
		CurrentAmmo  = Instantiate(Ammo) as GameObject;
		CurrentAmmo.transform.position = Vector3.zero;
	}
}
