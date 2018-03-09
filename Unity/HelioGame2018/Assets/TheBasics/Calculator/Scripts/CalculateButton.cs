using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateButton : MonoBehaviour
{
	public InputField Input1;
	public InputField Input2;
	public Text Solution;
	
	public Calculate Calculate;

	public void RunCalculate()
	{
		Calculate.NumberParser(Input1.text, Input2.text);
		Solution.text = Calculate.RunCalculation();
	}
}
