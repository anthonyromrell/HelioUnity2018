using UnityEngine;

public class GameBuilder : MonoBehaviour
{
	public Game MyGame;
	private GameObject myInstance;
	
	void Start ()
	{
		myInstance  = Instantiate(MyGame.Ammo) as GameObject;
		myInstance.transform.position = MyGame.Position;
	}

	private void Update ()
	{
		MyGame.Position = myInstance.transform.position;
		SaveData();
	}
	
	void SaveData()
	{
		PlayerPrefs.SetString("GameData", JsonUtility.ToJson(MyGame));	
		
		Debug.Log(PlayerPrefs.GetString("GameData"));
	}
}