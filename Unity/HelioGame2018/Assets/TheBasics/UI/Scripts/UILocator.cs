using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made By Anthony Romrell
public class UILocator : MonoBehaviour
{
	public Camera MyCamera;
	public Vector3 CameraLocation;

	private void Update()
	{
		print(MyCamera.pixelWidth);
		CameraLocation.x = MyCamera.pixelWidth * 0.5f;
		transform.localPosition = CameraLocation;
	}
}
