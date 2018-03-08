using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SendAndGetTransform : SendAndGetObject {


	public void GetObject(Transform obj)
	{
		Object = obj;
	}
}
