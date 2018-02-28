using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instancing : MonoBehaviour
{
	public CSharpInstance Csi;

	void Start () {
		Csi = ScriptableObject.CreateInstance<CSharpInstance>();
		Csi.InstanceWork();
		print(Csi.Health);
	}
}