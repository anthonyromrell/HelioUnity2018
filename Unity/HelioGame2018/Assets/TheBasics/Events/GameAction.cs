using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class GameAction : ScriptableObject
{
	public UnityAction<object> Call;
	public UnityAction CallNoArgs;
	
	public void ActionCall()
	{
		if (CallNoArgs != null) CallNoArgs();
	}
	
	
	//Overloading
	public void ActionCall(Transform obj)
	{
		if (Call != null) Call(obj);
	}
	
	public void ActionCall(int obj)
	{
		if (Call != null) Call(obj);
	}
	
	public void ActionCall(string obj)
	{
		if (Call != null) Call(obj);
	}
}