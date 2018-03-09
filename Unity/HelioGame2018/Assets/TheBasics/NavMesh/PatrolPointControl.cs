using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointControl : MonoBehaviour
{
	public AiPatrol AiPatrol;

	void OnEnable ()
	{
		AiPatrol.PatrolPoints = new List<Transform>(GetComponentsInChildren<Transform>());
	}
}