using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made By Anthony Romrell
public class MeshInfo : MonoBehaviour 
{
	private void Start()
	{
		print(gameObject.GetComponent<MeshFilter>().mesh.triangles.Length);
	}
}
