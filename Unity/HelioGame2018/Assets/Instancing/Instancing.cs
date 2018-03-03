using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instancing : MonoBehaviour
{
	public CSharpInstance Csi;
	public string MyString;
	public int MyInt;

	public VCSharpObject[] Vcs;

	void Start () {
		Csi = ScriptableObject.CreateInstance<CSharpInstance>();
		Csi.InstanceWork();
		print(Csi.Health);
	}
}