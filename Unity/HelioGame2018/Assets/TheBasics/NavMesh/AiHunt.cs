using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Hunt")]
public class AiHunt : AiBase
{
	public SendAndGetObject go;
	
	public Transform PlayerTransform;

	private void OnEnable()
	{
		go.SendObject.AddListener(GetPlayerTransform);
	}

	private void GetPlayerTransform ()
	{
		PlayerTransform = go.Object as Transform;
	}

	public override void Navigate(NavMeshAgent ai)
	{
		if (PlayerTransform != null) 
			ai.SetDestination(PlayerTransform.position);
		else
			ai.SetDestination(ai.transform.position);
	}
}