using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CalcParser : GameActionHandler
{
	public Text ObjectText;
	private string TextString;

	private void Awake()
	{
		Action.Call += Respond;
	}

	protected override void Respond (object obj)
	{
		TextString += obj as string;
		ObjectText.text = TextString ;
	}
}