using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshBehaviour : MonoBehaviour
{
	private NavMeshAgent NavMeshAgent;
	public AiBase AiBase;
    
	void Start ()
	{
		NavMeshAgent = GetComponent<NavMeshAgent>();
		NavMeshAgent.autoBraking = false;
	}

	void Update () {

		AiBase.Navigate(NavMeshAgent);
		
	}
}