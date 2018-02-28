using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
	private int _camNum = 0;
	public GameObject[] Cameras;

	private void Start()
	{
		OnMouseUp();
	}

	public void OnMouseUp()
	{

		foreach (var myCamera in Cameras)
		{
			myCamera.SetActive(false);
		}

		Cameras[_camNum].SetActive(true);
		
		if (_camNum < Cameras.Length-1)
		{
			_camNum++;
		}
		else
		{
			_camNum = 0;
		}
	}
}