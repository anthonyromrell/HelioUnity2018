using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print(gameObject.GetInstanceID());
	}
	
}
