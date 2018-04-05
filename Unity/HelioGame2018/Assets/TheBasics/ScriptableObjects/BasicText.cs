using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BasicText")]
public class BasicText : ScriptableObject 
{
	public string MyText;
	public Text MyUIText;

	private void OnEnable()
	{
		MyUIText.text = MyText;
		Debug.Log(MyText);
	}

	public string MyTextProperty
	{
		get
		{
			MyUIText.text = MyText;
			return MyText; 	
		}
		set
		{
			MyText = value; 
			MyUIText.text = MyText;
		}
	}
}