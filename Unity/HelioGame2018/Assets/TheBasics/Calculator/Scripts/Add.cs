using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add")]
public class Add : Calculate {
	
	public override string RunCalculation()
	{
		return (number1 + number2).ToString();
	}

	public void CalcEvent()
	{
		Event.Invoke((number1 + number2).ToString());
	}
}
