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
	}

	void Update () {
		AiBase.Navigate(NavMeshAgent);
	}
}