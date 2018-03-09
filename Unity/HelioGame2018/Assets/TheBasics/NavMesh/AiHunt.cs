using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Hunt")]

public class AiHunt : AiBase
{
	public SendAndGetObject IncomingObject;
	
	private Transform Destination;

	public void GetPlayerTransform ()
	{
		Destination = IncomingObject.Object as Transform;
	}

	public override void Navigate(NavMeshAgent ai)
	{
		ai.destination = (Destination != null ? Destination.position : ai.transform.position);
	}
}