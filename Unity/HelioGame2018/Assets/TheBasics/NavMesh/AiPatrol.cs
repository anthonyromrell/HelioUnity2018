using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Patrol")]

public class AiPatrol : AiBase
{
	private int i = 0;
	
	public List<Transform> PatrolPoints { get ; set; }
	

	public override void Navigate(NavMeshAgent ai)
	{	
		if (ai.remainingDistance < 1)
		{
			ChangePatrolPoint();
		}
		
		ai.destination = PatrolPoints[i].position;
	}

	private void ChangePatrolPoint()
	{
		if (i < PatrolPoints.Count-1)
			i++;
		else
			i = 0;
	}
}