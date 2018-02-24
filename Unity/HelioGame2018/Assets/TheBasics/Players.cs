using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Players : ScriptableObject
{

	public List<string> PlayerNames;

	public void AddPlayers(string name)
	{
		PlayerNames.Add(name);
	}

}
