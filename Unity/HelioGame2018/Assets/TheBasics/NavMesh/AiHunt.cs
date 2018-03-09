using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Hunt")]
public class AiHunt : AiBase
{
	public SendAndGetObject IncomingObject;
	
	private Transform Destination;

	private void OnEnable()
	{
		IncomingObject.SendObject.AddListener(GetPlayerTransform);
	}

	private void GetPlayerTransform ()
	{
		Destination = IncomingObject.Object as Transform;
	}

	public override void Navigate(NavMeshAgent ai)
	{
		ai.SetDestination(Destination != null ? Destination.position : ai.transform.position);
	}
}