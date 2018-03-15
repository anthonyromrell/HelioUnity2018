using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI Color")]
public class UIConfig : ScriptableObject
{
	public Color UiBaseColor = Color.red;
	public GameObject Base;

	public void OnEnable ()
	{
		Image[] images = Base.GetComponentsInChildren<Image>();
		foreach (var image in images)
		{
			image.color = UiBaseColor;
			Debug.Log(image.color);
			PrefabUtility.ResetToPrefabState(Base);
		}
	}
}

[CustomEditor(typeof(UIConfig))]
public class UIConfigEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
        
		UIConfig myScript = (UIConfig)target;
		if (GUILayout.Button("Click"))
		{
			myScript.OnEnable();
		}
	}
}