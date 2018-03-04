using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ammo : MonoBehaviour
{
	public float DestroyTime = 10;
	public Vector3 Force;
	private Rigidbody _rg;
	
	void Start ()
	{
		_rg = GetComponent<Rigidbody>();
		_rg.AddForce(Force);	
		
		Destroy(gameObject, DestroyTime);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		Destroy(gameObject);
	}
}