using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Calculate : ScriptableObject
{
	protected int number1;
	protected int number2;
	public UnityEvent<string> Event;
	
	public void NumberParser(string input1, string input2)
	{
		number1 = int.Parse(input1);
		number2 = int.Parse(input2);
	}

	
	public abstract string RunCalculation();
}
