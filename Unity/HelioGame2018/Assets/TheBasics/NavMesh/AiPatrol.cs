using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Patrol")]

public class AiPatrol : AiBase
{
	private int i = 0;

	//public SendAndGetObject SendAndGetObject;
	
	public List<Transform> PatrolPoints { get; set; }
	
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
		if (i < PatrolPoints.Count-1)
			i++;
		else
			i = 0;
	}

	/*private void OnEnable()
	{
		SendAndGetObject.SendObject.AddListener(UpdatePatrolPoints);
	}

	private void UpdatePatrolPoints()
	{
		PatrolPoints = (List<Transform>) SendAndGetObject.Object;
	}*/
}