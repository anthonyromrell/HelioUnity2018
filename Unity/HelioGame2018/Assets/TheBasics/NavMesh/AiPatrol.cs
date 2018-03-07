using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Patrol")]

public class AiPatrol : AiBase
{
	private int i = 0;
	
	public Transform[] PatrolPoints;
	private int destPoint;

	public override void Navigate(NavMeshAgent ai)
	{	
		if (ai.remainingDistance < 1)
		{
			ChangePotrolPoint();
		}
		
		ai.destination = PatrolPoints[i].position;
	}

	private void ChangePotrolPoint()
	{
		if (i < PatrolPoints.Length-1)
			i++;
		else
			i = 0;
	}
}