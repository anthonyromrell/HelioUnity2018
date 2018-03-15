using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEditor;

public class UIButton : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
        
		GameEvent myScript = (GameEvent)target;
		if(GUILayout.Button("Build Object"))
		{
			myScript.Raise();
		}
	}
	
}
