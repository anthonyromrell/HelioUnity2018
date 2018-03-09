using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtract")]
public class Subtract : Calculate {
	
	public override string RunCalculation()
	{
		return (number1 - number2).ToString();
	}
}
