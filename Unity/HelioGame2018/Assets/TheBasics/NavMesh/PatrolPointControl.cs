using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointControl : MonoBehaviour
{
	public AiPatrol AiPatrol;

	void OnEnable ()
	{
		AiPatrol.PatrolPoints = new List<Transform>(GetComponentsInChildren<Transform>());
		for (var index = 0; index < AiPatrol.PatrolPoints.Count; index++)
		{
			AiPatrol.PatrolPoints.Remove(transform);
		}
	}
	/*public SendAndGetObject SendAndGetObject;
	private List<Transform> PatrolPoints;

	void OnEnable ()
	{
		PatrolPoints = new List<Transform>(GetComponentsInChildren<Transform>());
		for (var index = 0; index < PatrolPoints.Count; index++)
		{
			PatrolPoints.Remove(transform);
		}
		SendAndGetObject.GetObject(PatrolPoints);
	}*/
}