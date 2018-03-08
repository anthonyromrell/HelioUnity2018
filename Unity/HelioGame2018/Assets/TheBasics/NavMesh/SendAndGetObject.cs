using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SendAndGetObject : ScriptableObject
{	
	private object ObjectType;
	
	public object Object
	{
		get
		{
			return ObjectType;
		}
		set
		{
			ObjectType = value;
			SendObject.Invoke();
		}
	}

	public UnityEvent SendObject;

	public void GetObject(object obj)
	{
		Object = obj;
	}

	private void OnEnable()
	{
		Object = null;
	}
}
