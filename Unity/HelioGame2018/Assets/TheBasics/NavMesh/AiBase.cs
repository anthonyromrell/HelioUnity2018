using UnityEngine;
using UnityEngine.AI;

public abstract class AiBase : ScriptableObject
{
	public abstract void Navigate(NavMeshAgent ai);
}
