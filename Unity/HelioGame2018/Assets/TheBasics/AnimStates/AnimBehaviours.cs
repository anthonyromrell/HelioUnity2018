using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class AnimBehaviours : StateMachineBehaviour
{

	public GameEvent Event;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName("A"))
		{
			Event.Raise();
		}
	}
}
