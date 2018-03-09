using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Hunt")]
public class AiHunt : AiBase
{
	public SendAndGetObject IncomingObject;
	
	private Transform PlayerTransform;

	private void OnEnable()
	{
		IncomingObject.SendObject.AddListener(GetPlayerTransform);
	}

	private void GetPlayerTransform ()
	{
		PlayerTransform = IncomingObject.Object as Transform;
	}

	public override void Navigate(NavMeshAgent ai)
	{
		ai.SetDestination(PlayerTransform != null ? PlayerTransform.position : ai.transform.position);
	}
}